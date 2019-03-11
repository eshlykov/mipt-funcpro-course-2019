open System

// Из простых типов в F# также есть перечисления. Они очень похожи на
// размеченные объединения, но к типам привязан не другой тип, а
// целочисленное значеие.
type Color =
    | White = 0
    | Black = 1
    | Green = 2

// Их можно тривиально конвертировать в целые числа
let two = int Color.Green

// Кроме того, есть функция, которая создает значения из типа-перечисления
let white = enum<Color>(0)
printfn "%A" (white = Color.White)

// Так можно создавать даже новые значения
let newColor4 = enum<Color>(4)
let newColor5 = enum<Color>(5)
printfn "%A" (newColor4 = Color.White)
printfn "%A" (newColor4 = Color.Black)
printfn "%A" (newColor4 = Color.Green)
printfn "%A" (newColor4 = enum<Color>(4))
printfn "%A" (newColor4 = newColor5)

// Дерево может иметь вершины двух видов: лист и узловая вершина. Лист хранит только значение некоторого типа,
// а узловая вершина - значение и список поддеревьев. Это удобно записывается в виде размеченного объединения:
type 'T Tree =
    | Leaf of 'T
    | Node of 'T * ('T Tree list)  // Пара из значения и списка деревьев

// Обратите внимание, что определение типа рекурсивное!

// Когда мы объявили тип `'T Tree`, то это значит, что Tree - generic type, то такой тип, для конструирования объектов
// которого надо указать тип-параметр (который почти всегда выводится неявно). Мы с таким уже встречались: например,
// `int list` - это constructed type от generic type `'T list`. Другой синтаксис для generic-типов: `Tree<'T>`

// В F# нет встроенной реализации деревьев, поскольку для разных задач требуются разные деревья.
// В нашей реализации, правда, может быть узловая вершина `Node (value, [])`, что не очень удобно, ну да ладно

// Напишем обход в глубину
let rec bfs func = function
    | Leaf x -> func x
    | Node (x, list) ->
        func x
        list |> List.map (fun subtree -> bfs func subtree) |> ignore

let tree = Node (1, [Node (2, [Node (1, [Leaf 1; Node (2, [Leaf 3])])])])

bfs (printfn "%A") tree

// Напишем какие-нибудь функции для обработки деревьев
let rec treeMap func = function
    | Leaf x -> Leaf (func x)
    | Node (x, list) ->
        Node (func x, list
              |> List.map (fun subtree -> treeMap func subtree))

// Свертка
let rec treeFold func acc = function
    | Leaf x -> func acc x
    | Node (x, list) ->
        list |> List.fold (fun acc' subtree
                            -> treeFold func acc' subtree) (func acc x)

printfn "%A" (treeFold ( + ) 0 tree)

// Если мы знаем, что дерево двоичное, то его удобнее записать в виде тройки
type 'T BinaryTree =
    | Nil
    | Node of 'T * 'T BinaryTree * 'T BinaryTree

// Обходить бинарные деревья можно тремя способами: инфиксным, префиксным, постфиксным
let prefix root leftChild rightChild =
    root ()
    leftChild ()
    rightChild ()
let infix root leftChild rightChild =
    leftChild ()
    root ()
    rightChild ()
let postfix root leftChild rightChild =
    leftChild ()
    rightChild ()
    root ()

let rec traverse order func = function
    | Nil -> ()
    | Node (x, leftChild, rightChild) ->
        order (fun () -> func x) (fun () -> traverse order func leftChild) (fun () -> traverse order func rightChild)

let binaryTree = Node(1,
                      Node(4, Nil, Node (2, Node (8, Nil, Nil), Nil)),
                      Node (6, Node (1, Nil, Nil), Node (3, Nil, Nil)))

traverse prefix (printfn "%A") binaryTree

// Реализуем теперь бинарное дерево поиска. Для этого нам потребуются операции вставки, проверки и удаления
let rec insert elem = function
    | Nil -> Node (elem, Nil, Nil)
    | Node (x, leftSubtree, rightSubtree) ->
        if elem < x then Node (x, insert elem leftSubtree, rightSubtree)
        else Node (x, leftSubtree, insert elem rightSubtree)

// Добавление сразу нескольких элементов можно сделать через свертку
let insertMany list tree = List.fold (fun tree elem -> insert elem tree) tree list

let rec bst = insertMany [ 4; 2; 2; 4; 3; 6; 5; 3; 2 ] Nil
printfn "%A" bst

// Попробуем тут удалить аргумент list слева и справа от функции. Тогда, казалось бы, произойдет каррирование,
// аргумент останется неявным, и функция не поменяется. Но код не будет компилироваться. Поблема в том, что list -
// это generic type, а тип неявных аргументов не может быть дженериком из-за ограничений компилятора (он не может
// правильно определить обобщенный тип)

let rec contains elem = function
    | Nil -> false
    | Node (x, _, _) when x = elem -> true
    | Node (_, leftSubtree, rightSubtree) ->
        contains elem leftSubtree || contains elem rightSubtree

printfn "%A" (contains 1 bst)
printfn "%A" (contains 2 bst)
printfn "%A" (contains 3 bst)

// Подсчитаем размер дерева
let rec treeSize = function
    | Nil -> 0
    | Node (_, leftSubtree, rightSubtee) ->
        1 + treeSize leftSubtree + treeSize rightSubtee

// Хочется сделать рекурсию хвостовой, однако как это сделать, если здесь два рекурсивных вызова?
// Нужно вспомнить продолжения
let treeSizeCont tree =
    let rec size tree' cont =
        match tree' with
        | Nil -> cont 0
        | Node (_, leftSubtree, rightSubtree) ->
            size leftSubtree (fun leftSubtreeSize ->
                size rightSubtree (fun rightSubtreeSize ->
                    cont (1 + leftSubtreeSize + rightSubtreeSize)))
    size tree (fun x -> x)

// Идейное объяснение: продолжение - это функция, которая выполняется после рекурсивного вызова. В данном случае
// сначала вычислится размер левого поддерева, после чего будет продолжение - оно вычислит размер правого поддерева,
// после чего сложит все значения

printfn "%A" (treeSize binaryTree)
printfn "%A" (treeSizeCont binaryTree)

// Что такое абстрактное синтаксическое дерево? Текст программы на большинстве языков программирования записывается в
// виде контекстно-свободной грамматики. Сама грамматика тривиальным образом преобразуется в дерево:
// - левая честь, то есть нетерминалы, превращаются в узлы
// - правая часть, то есть последовательность терминалов и нетерминалов, в список узлов

// Пример грамматкии для языка
// ClassDeclaration ::= "class" Identifier "{" ( VarDeclaration )* ( MethodDeclaration )* "}"
// MethodDeclaration ::= Type Identifier "(" AgrumentsList ")" "{" ( VarDeclaration )* ( Statement )* "return" Expression ";" "}"
// VarDeclaration ::= Type Identifier ";"
// AgrumentsList ::= ( Type Identifier ( "," Type Identifier )* )
// Type ::= "boolean"
// | "int"
// | Identifier
// Statement ::= "{" ( Statement )* "}"
// | "if" "(" Expression ")" Statement "else" Statement
// | "while" "(" Expression ")" Statement
// | Identifier "=" Expression ";"
// Expression ::= Expression ( "+" | "-" | "*" | "||" | "&&" | "==" ) Expression
// | Expression "." Identifier "(" ( Expression ( "," Expression )* )? ")"
// | <INTEGER_LITERAL>
// | "true"
// | "false"
// | Identifier
// | "this"
// | "new" Identifier "(" ")"
// | "!" Expression
// | "(" Expression ")"
// Identifier ::= <IDENTIFIER>

// Как теперь записать это в виде абстрактного синтаксического дерева
type Expression =
    | Plus of Expression * Expression
    | Minus of Expression * Expression
    | Star of Expression * Expression
    | Or of Expression * Expression
    | And of Expression * Expression
    | Equal of Expression * Expression
    | MethodCall of Expression * string * Expression list
    | Number of int
    | LogicConstant of bool
    | Identifier of string
    | This
    | New of string
    | Not of Expression
type Statement =
    | List of Statement list
    | IfElse of Expression * Statement * Statement
    | While of Expression * Statement
    | Assignment of string * Expression
type Type =
    | Custom of string
    | Int
    | Boolean
type ArgumentList = (Type * string) list
type VarDeclaration = Type * string
type MethodDeclaration = Type * string * ArgumentList * (VarDeclaration list) * (Statement list) * Expression
type ClassDeclaration = string * (VarDeclaration list) * (MethodDeclaration list)

// Тогда каждый класс описывается просто элементом типа ClassDeclaration. Полученное дерево называется абстрактным
// синтаксическим деревом. Заметим, что здесь мы избавились от ключевых слов и служебных символов. Деревья, в которых
// они оставлены, называются просто синтаксическими деревьями

 // Записав текст программы в таком виде, его становится удобно интерпретировать
 // Например, вычислим Expression для бинарных операторов
let rec compute = function
    | Plus (lhs, rhs) -> compute lhs + compute rhs
    | Minus (lhs, rhs) -> compute lhs - compute rhs
    | Star (lhs, rhs) -> compute lhs * compute rhs
    | Number x -> x
    | _ -> raise (NotImplementedException ())

printfn "%A" (compute (Plus (Minus (Number 4, Number 1), Star (Number 2, Number 3))))

// В F# есть не только константные значения, но и переменные. Для того, чтобы значение было мутабельным, нужно
// написать ключевое слово mutable при объявлении
let mutable number = 2
number <- 3
printfn "%A" number

// Маccивы - коллекция постоянной длины элементов одного типа. В целом массивы похожи на списки, в отличие от них,
// массивы в целом быстрее и поддерживают быстрый доступ по индексу. Функции для работы с массивами находятся в модуле
// Array. Массивы - мутабельная структура данных!
// Тип массива обозначается 'a []. Например, int []
let emptyArray = [||]
let enumArray = [| 1; 2; 3 |]
let lineArray = [|
    1
    2
    3
|]
let rangeArray = [| 0 .. 1000 |]
let forArray = [| for i in 1 .. 20 -> i * i |]

// Есть стандартные функции для создания массивов
let arrayZeroCreate : int array = Array.zeroCreate 10
let initArray = Array.init 100 (fun x -> x * 2)

// Доступ к элементам
let four = initArray.[ 2 ]
let slice = initArray.[ 2 .. 10 ]

// Изменение значения
initArray.[0] <- 4

// По массивам можно также итерироваться в цикле for
// Для массивов реализованы многие функции, аналогичные List
for x in initArray.[ 0 .. 4 ] do
    printfn "%A" x

slice
|> Array.map (fun x -> x * x)
|> Array.filter (fun x -> x < 40)
|> printfn "%A"

// Массивы могут быть многомерными. Есть конструктор двумерного массива от списка списков. Функции для работы
// с двумерными массивами лежат в модуле Array2D. Его тип - 'a [,]. Для трехмерных - 'a [,,] и так далее
let my2dArray = array2D [ [ 1; 2; 3 ]; [ 3; 4; 5 ] ]

// Заметим, что массив массивов - это не двумерный массив!
let arrayOFArrays = [| [| 1; 2; 3 |]; [| 3; 5; 7 |] |]

// Доступ по индексам и слайсы поддерживаются вплоть до 4-мерных массивов
printfn "%A" my2dArray.[ 0 .. 0, 0 .. 1 ]

// Для взятия всей размерности используется звездочка
printfn "%A" my2dArray.[ 0, * ]

// Для массивов есть модуль Array.Parallel, который позволяет обрабатывать массив параллельно
Array.iter (printfn "%A") forArray
Array.Parallel.iter (printfn "%A") forArray

// В стандартной библиотеке есть также и более интересные структуры данных, например, множества и словари
// Тип множесва записывается Set<'a>, это тоже generic type. Множества реализованы на деревьях.
// Множества имеют свои методы, например, Add, Contains, Count, IsEmpty и так далее
// Множества иммутабельны, поэтому каждый мутирующий метод возвращает новое множество
let set1 = Set.empty.Add(1).Add(2).Add(3).Add(4).Add(1)
let set2 = Set.ofList [ 1; 2; 3; 2; 1; 5 ]
printfn "%A" set1
printfn "%A" set2

// Для множетсв определены операции ( + ) и ( - )
printfn "%A" (set1 + set2)
printfn "%A" (set1 - set2)

// Также для есть модуль Set, который реализует много функций для работы с множествами (подоьные Array и List).
// Часть из них дублирует методы, часть нет
Set.iter (printfn "%A") set1

// Очень похожий тип Map имеет два параметра-типа, для ключа и значения. У значений такого типа есть методы
// Add, ContainsKey, Count, IsEmpty, Item (если значения нет, будет исключение), Remove, TryFind (возвращает option)
let map1 = Map.empty.Add('c', 0).Add('d', 1).Add('e', 2)
printfn "%A" map1

// Что произойдет, если добавим второй раз один и тот же ключ?
let map2 = map1.Add('c', 2);
// При повторном добавлении значение перезаписывается

// Аналогично библиотека Map содержит много функций для работы со словарями
Map.map (fun _ value -> value * 2) map1 |> printfn "%A"
// _ здесь - это ключ
