// Монады (в F# - Computation Expressions) - это вычислительный design pattern. Зачем они нужны?
// Во-первых, они используются в чистом функциональном программировании для задания явной последовательности
// вычислений. Это важно для ленивых языков, так как там порядок вычислений непредсказуем. Монады позволяют эти
// вычисления упорядочить.
// Во-вторых, в чистом функциональном программировании отсутствуют побочные эффекты. Но в реальности, конечно,
// побочные эффекты есть. Тот же ввод-вывод, например, изменяет состояние консоли. Монады позволяют реализовать
// императивный подход внутри функционального. (Например, в Haskell ввод-вывод сделан через монады.)

// В F# есть только три встроенных монады (seq, async, query), но можно определять собственные.
// Заметьте, что seq - наши ленивые последовательсности - на самом деле также монады!

// Синтаксис для монадических вычислений:
// builderExpr { body }
// Например,
let monad = seq { 1 .. 10 }

// Внутри монад можно использовать несколько дополнительных операторов:
// let!, do!, yield, yield!, return, return!
// Некоторые из них определены не для всех монад

// С yield и yield! мы уже знакомы:
// * yield возвращает наружу один элемент, но не завершает вычисление
// * yield! встраивает значения, полученные из монады-аргумента, в текущую
// Вспомним пример со случайным блужданием
let rand = System.Random()

let rec randomWalk2 x = seq {
    yield x
    yield! randomWalk2 (x + rand.NextDouble() - 0.5)
}

let seq1 = seq { 1 .. 6 }
let seq2 = seq [ 7; 8; 9; 10; 11; 12 ]
let concatSeqs = seq {
    yield 0
    yield! seq1
    yield 13
    yield! seq2
    yield 14
}
printfn "%A" (Seq.toList concatSeqs)

let rec ones = seq {
    yield 1
    yield! ones
}

let rec twos = seq {
    yield 2
    yield! twos
}

let twiceOnes = seq { yield! ones; yield! twos }

// Для знакомства с остальными методами рассмотрим монаду async. Для начала рассмотрим класс Async, который содержит
// методы для работы с асинхронными вычислениями

// return возвращает значение, как yield, но останавливает вычисление. Его больше нельзя продолжить
let getNum (num : int) = async { return num }  // int -> Async<int>

// let! связывает контсанту с вызовом другой монады. Если написать без восклицательного знака, то вычисление не будет
// запущено
let asyncPrinters = async {
    let! one = getNum 1
    return one
}

// return! запускает другое вычисление и возвращает его результат
let some = async {
    return! asyncPrinters
}

// do! будет дожидаться выполнения другого вычисления, которое ничего не возвращает
let print x = async { printfn "%A" x }  // -> Async<unit>
let waiter = async { do! print 1 }

// Для запуска асинхронных вычислений используется функция Async.RunSynchronously. Она запускает вычисление в отдельном
// потоке и дожидается его окончания. Если результат ждать не нужно, то есть Async.Start
waiter |> Async.RunSynchronously

// Создали два потока, каждый из которых выдал единицу, и дождались обоих
[ waiter; waiter ] |> Async.Parallel |> Async.RunSynchronously |> ignore

// Для того, чтобы не дожидаться выполнения этого потока, используестся
// Async.Start
waiter |> Async.Start

////////////////////////////////////////////////////////////////////////////////

// Монада недетерминированных вычислений

type 'T NonDet = 'T list

type NonDetBuilder () =
    // M<'T> * ('T -> M<'U>) -> M<'U>
    member this.Bind (comp, func) = List.distinct (List.collect func comp)
    // 'T -> M<'T>
    member this.Return value = [value]
    
// let!    = Bind(comp, func) : M<'T> * ('T -> M<'U>) -> M<'U>, 'U <> unit
// do!     = Bind(comp, func) : M<'T> * ('T -> M<unit>) -> M<unit>
// return  = Return(value) : 'T -> M<'T>
// return! = ReturnFrom(value) : M<'T> -> M<'T>
// yield   = Yield : 'T -> M<'T>
// yield!  = YieldFrom : M<'T> -> M<'T>

let nonDet = new NonDetBuilder ()

let nd = nonDet {
    let! start = [ 1; 2; 3 ]  // int
    // Смысл в том, что мы пишем справа M<'T>, а слева получаем 'T и работаем с
    // с ним как с 'T
    let! next = [ start ; start * 2; start * 3 ]
    let final = next + 1
    return final  // Возвращаться должен тип 'T
}

printfn "%A" nd

// Монада Maybe
    
type MaybeBuilder () =
    member this.Bind (comp, func) =
        match comp with
        | None -> None
        | Some value -> Some (func value)
    member this.Return value  = Some value
    member this.ReturnFrom value = value
    
let mayBe = MaybeBuilder ()

let mb init = mayBe {
    let! index = List.tryFindIndex (fun x -> x = init) [ 1; 2; 3; 4; 5; 6; 7 ]
    return! index
}

printfn "%A" (mb 10)

// Монада продолжения

type ContBuilder () =
    // M<T> = (T -> U) -> U
    member this.Bind (comp, func) = fun cont -> comp (fun x -> func x cont)
    member this.Return value = fun cont -> cont value

let cont = new ContBuilder ()

let fact number =
    let rec fact' number = cont {
        if number = 0 then return 1
        else
            let! prev = fact' (number - 1)
            return number * prev
    }
    // fact' number = cont { ... }, но cont возвраащет функцию
    // в качестве этой функции подадим на вход тождественную функцию
    fact' number (fun x -> x)
    
printfn "%A" (fact 0)
printfn "%A" (fact 3)
printfn "%A" (fact 5)


let quickSort list =
    let rec sort' list = cont {
        match list with
        | [] -> return []
        | head :: tail ->
            let left, right = List.partition (( > ) head) tail
            let! leftSorted = sort' left
            let! rightSorted = sort' right
            return leftSorted @ (head :: rightSorted)
    }
    sort' list (fun x -> x)
    
printfn "%A" (quickSort [ 1; 4; 2; 6; 5; 2; 6; 7; 3; 1; 4; 4; 7; 9; 5; 2; 4 ])

// Другие методы для монад

// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions#creating-a-new-type-of-computation-expression

type TraceBuilder () =
    member this.Bind (comp, func) =
        match comp with
        | None ->
            printfn "binding with None"
            None
        | Some value ->
            printfn "binding with Some %A" value
            Some (func value)
    member this.Return value =
        printfn "returning %A" value
        Some value
    member this.ReturnFrom comp =
        printfn "returning from %A" comp
        comp
    member this.Yield value =
        printfn "yielding %A" value
        Some value
    member this.YieldFrom comp =
        printfn "yielding from %A" comp
        comp
    // { other-expr }          = expr; this.Zero ()
    // { if expr then cexpr0 } = if expr then { cexpr0 } else this.Zero ()
    member this.Zero () =
        printfn "zeroing"
        None
    // { for p in e do ce }       = this.For (enum, (fun p -> { ce }))
    // { for i = e1 to e2 do ce } = this.For (enum, (fun i -> { ce }))
    member this.For (comp, func) =
        printfn "foring %A" comp
        this.Bind (comp, func)
    // { ce1; ce2 } = this.Combine ({ ce1 }, { ce2 })
    member this.Combine (comp, func) =
        let value = func ()
        printfn "combining %A and %A" comp value
        match value with
        | None -> comp
        | Some _ -> value
    // this.Run(this.Delay(fun () -> { ce }))
    member this.Delay func =
        let delayedValue = fun() ->
            printfn "delaying"
            let value = func ()
            printfn "delay value is %A" value
            value
        delayedValue
    member this.Run func =
        let runValue =
            printfn "running"
            let value = func ()
            printfn "run value is %A" value
            value
        runValue

 
let trace = new TraceBuilder ()

let value = trace {
    return 1
    return 2
}

printfn "%A" (value)
