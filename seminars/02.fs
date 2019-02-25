// 1. Объявление унарных операторов
let ( ~& ) x = -x
printfn "%A" &7
// Не все операторы могут быть унарными

// 2. Перегружать функции нельзя, но можно методы классов (об этом в будущем)

// 3. В F# есть исключения двух видов: обычные и от .NET
// Объявление обычных исключений
exception DivisionByIZero of string

let ( &!@ ) (x : float) (y : float) =
    if y = 0. then raise (DivisionByIZero "divided by zero")
    else x / y

// Для обработки исключений используетя try..with или try..finally
let example (x : float) =
    try
        try
            let y = 1.0 &!@ x
            ()
        with
        | DivisionByIZero s -> printfn "%A" s
    finally
        printfn "%A" "printed"
example 1.0
example 0.0

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Для объявления рекурсивной функции нужно указать ключевое слово rec
let rec factorial n =
    if n <= 0 then 1
    else n * factorial (n - 1)

printfn "%A" (factorial 4)

// С помощью рекурсии можно реализовать цикл for со счетчиком
let rec forLoop counter condition increment body =
    if condition counter then
        body counter
        forLoop (increment counter) condition increment body

forLoop 0 (( > ) 10) (( + ) 1) (printfn "%A")

// В F# также есть конструкция и для обычного цикла for
let uglyFor start finish action =
    match start, finish with
    | (a, b) when a <= b - 5 -> for i in a .. b do action i
    | (a, b) when a <= b ->  for i = a to b do action (i * 2)
    | (a, b) when a > b -> for i = a downto b do action (i * 2)
    | _ -> printfn ":("
 
uglyFor 3 10 (printf "%A ")
printfn ""
uglyFor 3 5 (printf "%A ")
printfn ""
uglyFor 8 4 (printf "%A ")
printfn ""

// Списки определяются, используя квадратный скобки
// Внутри через ; перечисляются элементы
let emptyList = []
let listOf123 = [ 1; 2; 3; ]
let listOf123Multiline = [
    1
    2
    3
]
printfn "%A\n%A\n%A" emptyList listOf123 listOf123Multiline

// Еше один способ - испольщзуя конструктор списков
printfn "%A" (1 :: 2 :: 3 :: [])

// Списки можно задавать также диапазоном
let range = [ 1 .. 10 ]
let rangeEmpty = [ 10 .. 1 ]
printfn "%A\n%A" range rangeEmpty

// Наконец, списки можно задавать генераторами, комбинируя цикл for и yield
let squares =
    [ for i in 1 .. 10 do
          yield i * i ]
printfn "%A" squares

// Два списка можно конкатенировать в один через @
printfn "%A" ([1; 2; 3] @ [4; 5; 6])

// Добавить элемент в начало списка можно через
let appbegin list x = x :: list
printfn "%A" (appbegin [ 0; 2; 3 ] 1)

// В языке F# есть два синтаксиса для лямбда-выражений:
let lambda1 = fun x -> x * 2
let lambda2 = function x -> x * 2

// Вторая запись поддержитвает сопоставление с образцом, а первая нет
let tryPopFront = function
| [] -> (None, [])
| x :: xs -> (Some x, xs)

// Это эквивалентно
let tryPopFront' y =
    match y with
    | [] -> (None, [])
    | x :: xs -> (Some x, xs)

// Если функция имеет аргументы, то аргумент function - последние
let pushBackEvenAndPushFrontOdd list = function
| x when x % 2 = 0 -> list @ [x]
| x -> x :: list

printfn "%A" (pushBackEvenAndPushFrontOdd [ 1; 2 ] 1)
printfn "%A" (pushBackEvenAndPushFrontOdd [ 1; 2 ] 2)

// Функции для работы со спискам лежат в модуле List
// Вот некоторые из них
printfn "%A" (List.empty)
printfn "%A" (List.append [ 1; 2; 3 ] [4; 5; 6])
printfn "%A" (List.average [ 1.; 2.; 3. ])
printfn "%A" (List.chunkBySize 2 [ 1; 2; 3; 4; 5 ])
printfn "%A" (List.distinct [ 1; 2; 2; 3; 4; 4 ])
printfn "%A" (List.head [ 1; 2; 3 ])
printfn "%A" (List.tail [ 1; 2; 3 ])
printfn "%A" (List.length [ 1; 2; 3 ])
printfn "%A" (List.item 1 [ 1; 2; 3 ])
printfn "%A" (List.contains 1 [ 1; 2; 3 ])
printfn "%A" (List.isEmpty [ 1; 2; 3 ])
printfn "%A" (List.max [ 1; 2; 3 ])
printfn "%A" (List.min [ 1; 2; 3 ])
printfn "%A" (List.rev [ 1; 2; 3 ])
printfn "%A" (List.sort [ 3; 2; 1 ])
printfn "%A" (List.sum [ 1; 2; 3 ])
printfn "%A" (List.truncate 2 [ 1; 2; 3 ])
printfn "%A" (List.zip [ 1; 2; 3 ] [ 'a'; 'b'; 'c' ])
printfn "%A" (List.unzip [ (1, 'a'); (2, 'b'); (3, 'c') ])

// Быстрая сортировка списка из различных элементов
let rec quickSort list =
    match list with
    | [] -> []
    | _ -> let left = [ for x in list do if x < (List.head list) then yield x ]
           let right = [ for x in list do if x > (List.head list) then yield x ]
           (quickSort left) @ ((List.head list) :: (quickSort right))

printfn "%A" (quickSort [ 10; 5; 3; 7; 4; 2; 1 ])

// Функция, которая принимает на вход другую функцию, называется функцией высшего порядка
// Такие функции нужны чаще всего для обработки структур данных или некоторых проверок
// Основных функций три
printfn "%A" (List.map (fun x -> x * x) [ 1; 2; 3 ])
printfn "%A" (List.filter (fun x -> x % 2 <> 0) [ 1; 2; 3 ])
printfn "%A" (List.fold (fun acc x -> acc - x) 0 [ 1; 2; 3 ])

// Есть также функция reduce, которая делает то же, что и fold, но в качестве аккумулятора
// берет первый элемент списка
// Это делает ее не способной работать с пустым списком, поэтому, надо убеждаться
// что список не пустой
// Полезно, когда для функции нет нейтрального элемента
printfn "%A" (List.reduce (fun acc x -> acc + x) [ 1; 2; 3 ])

// Другие функции высших порядков
printfn "%A" (List.averageBy (fun x -> x * x) [ 1.; 2.; 3. ])
printfn "%A" (List.collect (fun x -> [ x; x - 1 ]) [ 1; 2; 3 ])
printfn "%A" (List.countBy (fun x -> x % 2) [ 1; 2; 3 ])
printfn "%A" (List.distinctBy (fun x -> x * x) [ 1; -1; 2; -2; 3 ])
printfn "%A" (List.exists (fun x -> x % 2 = 0) [ 1; 2; 3 ])
printfn "%A" (List.find (fun x -> x % 2 = 0) [ 1; 2; 3 ])
printfn "%A" (List.findIndex (fun x -> x % 2 = 0) [ 1; 2; 3 ])
printfn "%A" (List.forall (fun x -> x % 2 = 0) [ 1; 2; 3 ])
printfn "%A" (List.maxBy (fun x -> x * x) [ 1; 2; 3 ])
printfn "%A" (List.minBy (fun x -> x * x) [ 1; 2; 3 ])
printfn "%A" (List.init 3 (fun x -> x * x))
printfn "%A" (List.partition (fun x -> x % 2 = 0) [ 1; 2; 3; 4; 5 ])
printfn "%A" (List.sortBy (fun x -> -x) [ 1; 2; 3 ])
printfn "%A" (List.sumBy (fun x -> -x) [ 1; 2; 3 ])
printfn "%A" (List.tryFind (fun x -> x % 4 = 0) [ 1; 2; 3 ])

// Для удобства есть оператор конвейера |>, который берет левую часть и кладет ее последним аргументом в правую часть
// Если справа написать ignore, то значение будет проигнорировано
[ 1; 2; 3 ] |> List.map (fun x -> x * 2) |> printfn "%A"
[ 1; 2; 3 ] |> ignore

let ignore' _ = ()

[ 1; 2; 3; 4; 5 ]
|> List.map (( - ) 3)
|> List.filter (( <> ) 0)
|> List.reduce ( + )
|> ignore

// Есть вариант конвейера в обратную сторону, что позволяет не писать скобок
printfn "%A" (( + ) 2 <| ( * ) 2 2)
printfn "%A %A %A" <| 2 + 2 <| 3 + 3 <| 4 + 4

// По аналогии есть операторы композиции: >> и <<. Оператор |> принимает функцию и значение (которое, конечно,
// может быть функцией), а композиция - принимает две функции
// |> : a -> (a -> b) -> b
// <| : (a -> b) -> a -> b
// >> : (a -> b) -> (b -> c) -> a -> c
// << : (a -> b) -> (c -> a) -> c -> b
printfn "%A" (((( + ) 2) >> (( * ) 3)) 4) // (4 + 2) * 3
printfn "%A" (((( + ) 2) << (( * ) 3)) 4) // 4 * 3 + 2

// Асимптотитка методов списка
// Операция                            Асимптотика
// Случайный доступ                    O(n)
// Поиск                               O(n)
// Вставка в начало / удаление головы  O(1)
// Вставка / удаление в общем случае   O(n)
// Реверсирование                      O(n)

// С памятью все сложнее. Рассмотрим, например, получение длины
let rec length = function
    | [] -> 0
    | _ :: xs -> 1 + (length xs)

// На каждом уровне рекурсии потребуется хранить адрес возврата, аргументы и возвращаемый результат, то есть всего
// нужно хранить O(n) ячеек памяти. А на императивном языке программирования можно обойтись константой.
// К счастью, компилятор умеет оптимизировать рекурсивные функции, если они реализованы при помощи хвостовой рекурсии.
// Рекурсия называется хвостовой, если рекурсивный вызов является последней операцией, например, вот исправленная
// версия:
let lengthTail list =
    let rec lengthTail' acc = function
        | [] -> acc
        | _ :: xs -> lengthTail' (acc + 1) xs
    lengthTail' 0 list

// Иногда не получается сразу свести рекурсию к хвостовой. В этом случае можно воспользоваться продолжением.
// Напишем с ее помошью реверсирование списка.
let reverseContinuation list =
    let rec reverseContinuation' list continuation =
        match list with
        | [] -> (continuation [])
        | x :: xs ->
            reverseContinuation' xs (continuation >> (fun y -> x :: y))
    reverseContinuation' list (fun x -> x)

reverseContinuation [ 1; 2; 3 ] |> ignore

// На самом деле это просто демонстрация подхода, в данном соучае можно и бех них
// Заметим, что это выглядит, как будто мы сразу подставили функцию-продолжение
let reverseTail list =
    let rec reverseTail' list acc =
        match list with
        | [] -> acc
        | x :: xs -> reverseTail' xs (x :: acc)
    reverseTail' list []
    
reverseTail [ 1; 2; 3 ] |> ignore
