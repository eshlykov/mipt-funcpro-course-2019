module Seminar01

// Основные примитивные типы: int, float, bool, char, string, unit

// Объявление значений
let intTwo = 2
let floatThree = 3.
let boolTrue, boolFalse = true, false
let charA = 'A'
let stringHello = "Hello"
let unitValue = ()

// Вывод на экран
printfn "%i %f %b %b %c %s" intTwo floatThree boolTrue boolFalse charA stringHello
printfn "%A %A %A %A %A %A %A" intTwo floatThree boolTrue boolFalse charA stringHello unitValue
printfn "%O %O %O %O %O %O %O" intTwo floatThree boolTrue boolFalse charA stringHello unitValue

// Основные операторы
// Арифметические бинарные операции: + - * / % **
// Арифметические унарные операции: + -
// Операторы сравнения: = < > >= <= <>
// Логические операторы: not || &&
// Побитовые операторы: ||| &&& ^^^ ~~~ <<< >>>

// Знак остатка от деления равен знаку делимого
let plusByPlus = 17 % 5
let plusByMinus = 17 % -5
let minusByPlus = -17 % 5
let minusByMinus = -17 % -5
printfn "%A %A %A %A" plusByPlus plusByMinus
    minusByPlus minusByMinus

// Деления на ноль
let positiveInfinity1 = 1. / 0.
let positiveInfinity2 = -1. / -0.
let negativeInfinity1 = -1. / 0.
let negativeInfinity2 = 1. / -0.
let notANumber = 0. / 0.
printfn "%A %A %A %A %A" positiveInfinity1 positiveInfinity2
    negativeInfinity1 negativeInfinity2
    notANumber

// Замечания - бесконечности и nan имеют тип float, но в языке нет неявных преобразований типов
// Поэтому при делении на ноль целых чисел будет просто ошибка, а не бесконечность

// Объявление своих бинарных операторов
let ( &*!@% ) x y = x ** 2. + x * y + y ** 2.
printfn "%A" (3. &*!@% 5.)

// Объявление функции
let plus x y = ( + ) x y
let unaryMinus x = -x
let notEqual x y = x <> y
let logicalAnd x y = x && y
let bitwiseLeftShift x y = x <<< y

printfn "%A" (plus 4 (unaryMinus 6))
printfn "%A" (bitwiseLeftShift 32 1)

// Тип функции записывается в виде 'a1 -> ... -> 'aN -> 'r
// 'aK - тип k-го аргумента (k = 1 .. n), 'r - тип результата
// Если один из типов - другая функция, то ее тип пишется в скобках
let minusInt (x : int) (y : int) : int = x - y // : int -> int -> int

// Для вызова функции аргументы перечисляются обычно перез пробел:
printfn "%A" (minusInt 4 (1 + 2))

// Каррирование - частичное применение функции
let twoMinus = minusInt 2 // : int -> int
printfn "%A" (twoMinus 7)

// Операторы - тоже функции. Их можно переписать в виде обычной функции:
printfn "%A" (( &*!@% ) 3. 5.)

// Их также можно каррировать
let threeMinus = ( - ) 3 // : int -> int
printfn "%A" (threeMinus 7)

let applyAndMinus (f : int -> int) x y = (f x) - y // : (int -> int) -> int -> int -> int
let differenceIncreadedByFour = applyAndMinus (( + ) 4) // : int -> int -> int
printfn "%A" (differenceIncreadedByFour 0 -2)

// С помощью функций int, float и др. можно делать преобразования типов
let intThree = int floatThree
printfn "%A %A" floatThree intThree

// Внутри функций можно делать вложенные выражения и объявления
// Значением будет значение последнего выражения
let getRootOfLinearEquation a b =
    let root = -b / a
    root

printfn "%A" (getRootOfLinearEquation 3. 5.)

// Строка - отдельный тип, никак не связанный с char
// Символы '\'. '\n'. '\t' и т. п. нужно экранировать
let stringWithSlash = "Hello\n\tWorld\!"
let stringWithEscapedSlash = "Hello\\n\\tWorld\\!"
printfn "%A" stringWithSlash
printfn "%A" stringWithEscapedSlash

// Либо можно сделать @-строку, в этом случае все символы записываются как есть,
// и экранировать ничего не нужно
// То же самое - записать строку в тройных кавычках
printfn "%A" @"Hello\n\tWorld\!"
printfn "%A" """Hello\n\tWorld\!"""

// Но в этом случае есть проблема с каычками
// Чтобы их написать, нужно писать две кавычки вместо одной
// При этом тройные кавычки использовать не стоит
// Например, в примере ниже будут напечатаны еще и внешние кавычки
printfn "%A" @"""Text, ""quoted text"", text"""

// Строки можно конкатенировать с помощью оператора +
printfn "%A" ("Hi" + " & Bye")

// Для строк есть доступ по индексу
// Индексы должны быть неотртцательными, шаг указать нельзя
// Обе границы включаются
printfn "%A" stringWithSlash.[0..2]

// Если взять один символ, то тип будет char
// Если взять диапазон, то будет string
let onlyChar (c : char) = c
let onlyString (s : string) = s
let acceptChar = onlyChar stringWithSlash.[0]
let acceptString = onlyString stringWithSlash.[1..1]

// Кортежи обозначаются 'a1 * ... * 'aN, где 'aK - тип k-го элемента
let tuple2 = (unitValue, charA) // : unit * char
let tuple3 = (boolFalse, intThree, floatThree) // : bool * int * float

printfn "%A" tuple2
printfn "%A" tuple3

// Как достать элементы
let first2 = fst tuple2 // Только для пар
let second2 = snd tuple2 // Только для пар
let first3, second3, third3 = tuple3 // Распаковка значений
let _, second, _ = tuple3 // Распаковка значений с отбрасыванием ненужных

// Опциональный тип: обозначается 'T option
let optionalEmpty : int option  = Some 1
let optionalNotEmpty = Some 5
printfn "%A" optionalEmpty
printfn "%A" optionalNotEmpty

// Достать значение
let five : int = optionalNotEmpty.Value // Если на самом деле None, то выпадет исключение
printfn "%A" five

// Проверки на пустоту
printfn "%A" optionalEmpty.IsNone
printfn "%A" optionalEmpty.IsSome

// Условный оператор
let findFirstIndexOf x (tuple : int * int) =
    if fst tuple = x then Some 0
    elif snd tuple = x then Some 1
    else None

printfn "%A" (findFirstIndexOf 3 (3, 3))
printfn "%A" (findFirstIndexOf 3 (3, 4))
printfn "%A" (findFirstIndexOf 3 (5, 3))
printfn "%A" (findFirstIndexOf 3 (5, 4))

// Как видно, ветка else должна быть обязательной, чтобы оператор возвращал значение
// Это не так, если результат - unit
let tryPrint (x : int option) = if x.IsSome then printfn "%A" x.Value

tryPrint (Some 10)
tryPrint None

// Объявление алиаса
type OptionInt = int option

// Объявление размеченного объединение
type MaybeInt =
    | NotInt
    | Int of int

// Размеченное объединение из одной ветки можно записать так
// Имя ветки может совпадать с именем типа
type Name = Name of string
    
printfn "%A" NotInt
printfn "%A" (Int 5)
    
// Сопоставление с образцом
let castToOption (x : MaybeInt ) : int option =
    match x with
    | NotInt -> None
    | Int y -> Some y

printfn "%A" (castToOption (Int 4))

// Очень многие структуры можно сопоставлять с образцом
let matchTuple x =
    match x with
    | ('+', a, b) -> a + b
    | ('-', a, b) -> a - b
    | ('*', a, b) -> a * b
    | _ -> 0
    
// В последнем случае использована анонимная переменная
// Если не все случаи разобраны, то компилятор выдаст ошибку
// Сопоставление происходит с первой подошедшей ветвью
    
printfn "%A" (matchTuple ('*', 3, 4))
printfn "%A" (matchTuple ('@', 3, 4))

let invertPair x =
    match x with
    | (a, b) -> (b, a)
    
printfn "%A" (invertPair (3, 4))

// Сопоставление можно проводить с учетом условий
let matchTuple2 x =
    match x with
    | ('/', a, b) when b <> 0. -> Some (a / b)
    | _ -> None


printfn "%A" (matchTuple2 ('/', 4., 2.))
printfn "%A" (matchTuple2 ('/', 4., 0.))

// Также могут бысть условия на сами паттерны
let matchConmplex x =
    match x with
    | (3, 5) & (5, 3) -> "three and five" // Никогда не сработает
    | (3, _) | (_, 5) | (_, 3) | (5, _) -> "three or five"
    | _ -> "other pair"
    
printfn "%A" (matchConmplex (2, 1))
printfn "%A" (matchConmplex (3, 5))
printfn "%A" (matchConmplex (5, 1))
printfn "%A" (matchConmplex (2, 3))