open System

// Создаем запись
type ContactCard = {
    Name : string
    Tg : string
    mutable Verified : bool
}

// Как создать объект: record expression
let contact1 = {
    Name = "Evgen"
    Tg = "eshlykov"
    Verified = true
}

// Второй способ
let contactOnSameLine = { Name = "Evgen"; Tg = "eshlykov"; Verified = true }

// Можно создать новый контакт, взяв часть полей от старого
let contact2 = {
    contact1 with
        Tg = "e_shlykov"
        Verified = false
}

// Обратите внимание, что мы нигде не указали сам тип записи, он выводился
// автоматически. Отсюда сразу вопрос - а что будет, если завести второй тип
// записи с теми же полями?
type DuplicatedContactCard = {
    Name : string
    Tg : string
    mutable Verified : bool
}

let concat3 = {
    Name = "Evgen"
    Tg = "eshlykov"
    Verified = true
}

// Неожиданно, тип contact3 - DuplicatedContactCard! Теперь, чтобы явно создать
// ContactCard, надо писать так:
let concat4 = {
    ContactCard.Name = "Evgen"
    Tg = "eshlykov"
    Verified = true
}

// Как использовать записи с функциями
let showContactCard (c: ContactCard) =
        c.Name + " Tg: " + c.Tg + (if not c.Verified
                                   then " (unverified)" else "")

printfn "%A" (showContactCard contact1)
printfn "%A" (showContactCard contact2)

// Для записей работает сопоставление с образцом
type Point = { X: float; Y: float }
let evaluatePoint (point: Point) =
    match point with
    | { X = 0.; Y = 0. } -> printfn "Point is at the origin."
    | { X = xVal; Y = 0. } -> printfn "Point is on the Ox: %A." xVal
    | { X = 0.; Y = yVal } -> printfn "Point is on the Oy: %A." yVal
    | { X = xVal; Y = yVal } -> printfn "Point is at (%A, %A)." xVal yVal

evaluatePoint { X = 0.; Y = 0. }
evaluatePoint { X = 100.; Y = 0. }
evaluatePoint { X = 0.; Y = 10. }
evaluatePoint { X = -4.; Y = 10. }

////////////////////////////////////////////////////////////////////////////////

// Значительная часть объектно-ориентированного кода может быть смоделирована
// через замыкания, как обычно делают в классическом функциональном
// программировании.

// Можно описывать методы в виде записей с полями, тип которых - функции.
type Shape = {
    Draw : unit -> unit
    Area : unit -> float
    SetCenter : Point -> unit
}

// Напишем теперь функции конструкторы
let newCircle center radius =
    let mutable c, r = center, radius  // Можно использовать ref-ссылки
    { Draw = fun () -> printfn "Circle ((%A, %A), %A)" c.X c.Y r
      Area = fun () -> Math.PI * r * r / 2.
      SetCenter = fun center -> c <- center }

let newSquare center length =
    let mutable c, l = center, length
    { Draw = fun() -> printfn "Square ((%A, %A), %A)" c.X c.Y l
      Area = fun () -> l * l
      SetCenter = fun center -> c <- center }

let shapes = [
    newCircle { X = 1.0; Y = 2.0 } 10.0
    newSquare { X = 10.0; Y = 3.0} 2.0
]
shapes |> List.map (fun shape -> shape.Area()) |> printfn "%A"

shapes |> List.iter (fun shape -> shape.Draw())
shapes |> List.iter (fun shape -> shape.SetCenter { X = 0.; Y = 0. })
shapes |> List.iter (fun shape -> shape.Draw())

// У объявляемых типов могут быть методы. Мы их уже видели у монад, сейчас
// посмотрим у записей, но вообще они могут быть у всех объявляемых типов
// данных, даже у размеченных объединений
type AdvancedPoint =
    { X : float ; Y : float }
    member this.Draw () = printfn "%O" this
    static member Zero = { X = 0.; Y = 0. }
    static member Distance (p1, p2) =
        let sqr x = x * x
        Math.Sqrt (sqr (p1.X - p2.X) + sqr (p1.Y - p2.Y))
    member this.Distance p = AdvancedPoint.Distance (this, p)
    static member ( + ) (p1 : Point, p2 : Point) =
        { X = p1.X + p2.X ; Y = p1.Y + p2.Y }
    override this.ToString() = sprintf "Point (%A, %A)" this.X this.Y

// Что здесь произошло:
// - методы объявляются с ключевым словом member
// - перед именем метода пишется имя, которое играет роль самого объекта в теле
//   метода. Я называл его this, но можно брать любой идентификатор
// - все объекты неявно унаследованы от общего объекта Object, у которого есть
//   метод ToString. Мы его переопределлили словом override
// - функций Distance две: одна как метод класса, возвращающий расстояние от
//   данной точки до другой, другая - как статическая функция расстояния между
//   двумя точками
// - мы перегрузили оператор ( + ), сделав его статическим методом класса
// - Zero - это не метод, а поле, у него нет аргументов
// - все поля и методы здесь публичные

let aPoint = { X = 4. ; Y = 5. }
let bPoint = { X = 1. ; Y = 0. }

aPoint.Draw()
printfn "%A" (aPoint.Distance bPoint)
printfn "%A" (AdvancedPoint.Distance (aPoint, bPoint))
printfn "%O" aPoint  // Вызов ToString
printfn "%O" AdvancedPoint.Zero

////////////////////////////////////////////////////////////////////////////////

// Интерфейс - это шаблон с описанием функциональности объекта, которую потом
// можно воплотить в конкретных объектах

// С помощью интерфейсов перепишем нашу структуру Shape
type IShape =
    abstract Draw : unit -> unit
    abstract Area : float

// Здесь два метода и одно поле, но все они не содержат реализацию.
// Мы просто указали тип

// Перейдем теперь к функциям-конструкторам
let newCircle1 center radius =
    { new IShape with
        member this.Draw () =
            printfn "Circle ((%A, %A), %A)" center.X center.Y radius
        member this.Area = Math.PI * radius * radius / 2. }
  
let newSquare1 center size =
    { new IShape with
        member x.Draw() = printfn "Square (%A,%A), %A" center.X center.Y size
        member x.Area = size * size }

// Такой синтаксис называется объектным выражением. Эти выражения создают
// конкретные экземпляры классов и даже интерфейсов (как здесь) с помощью
// указания конкретных полей и переопределения / доопределения методов.

// В данном случае с их помощью мы сделали экземпляр интерфейса с необходимой
// нам функциональностью

let circle = newCircle1 { X = 0.; Y = 2. } 3.
circle.Draw ()
printfn "%A" circle.Area

////////////////////////////////////////////////////////////////////////////////

// Что делать, если мы хотим рисовать геометрические фигуры по-разному?
// Мы бы завели общий базовый класс для каждой фигуры с абстрактным методом
// Draw, а затем создать наследников, реализуюзих этот метод

// Но можно и по-другому: сделаем не класс, а функцию, и эту функцию будем
// кидать фигурам в конструктор

type Drawer = float * float * float -> unit  // Алиас для функции рисования

let newCircle2 draw center radius =
    { new IShape with
        member this.Draw () = draw center.X center.Y radius
        member this.Area = Math.PI * radius * radius / 2. }

let usualCircle center radius =
    newCircle2 (fun x y r ->
        printfn "Circle (%A, %A), %A)" x y r) center radius

let circle2 = usualCircle { X = 0.; Y = 2. } 3.

circle2.Draw()

////////////////////////////////////////////////////////////////////////////////

// Если мы хотим наследование... Рассмотрим иерархию геометрических фигур.
// У каждой фигуры будет возможность модифицировать координаты. Базовый класс
// для всех фигур - Point2D.

// Объявление базового класса. Это уже не запись!

type Point2D (initX, initY) =  // 1
    let mutable x = initX  // 2
    let mutable y = initY
    
    new () = Point2D (0., 0.)  // 3
    
    abstract MoveTo : Point2D -> unit  // 4
    default this.MoveTo point = this.Coords <- point.Coords
    
    member this.Coords  // 5
        with get () = (x, y)
        and set newCenter =
            let (newX, newY) = newCenter
            x <- newX
            y <- newY
    
    interface IShape with  // 6
        override this.Draw () = printfn "Point %A" this.Coords
        override this.Area = 0.
    
    static member Zero = new Point2D ()  // 7

// Что здесь произошло:
// 1. В скобках аргументы конструктора
// 2. Приватные поля класса, можно считать их просто переменными. Поля могут
//    статическими (static перед let) и иммутабельными (без mutable), слова
//    static и mutable комбинируются как угодно
// 3. Дополнительный конструктор. В данном случае он просто вызывает первый,
//    передаваю параметры по умолчанию
// 4. Дальше идет абстрактный метод и его дефолтная реализация. Абстрактность
//    означает, что метод могут переопределить наследники, если они не
//    переопределяются, то используется дефолтный метод
//    (а также в самом Point2D). Если класс не имеет дефолтной реализации, то
//    объект нельзя создать, кроме как через объектное выражение, которое
//    было выше. Слова default и override - синониммы
// 5. Публичное поля класса с геттером и сеттером для чтения и записи.
// 6. Ниже мы говорим, что класс реализует интерфейс IShape и предоставляем
//    реализацию методов
// 7. Наконец, мы заводим статическое поле - нулевую точку

// Конструируем объект
let point = Point2D (2., 4.)
printfn "%A" point.Coords

let defaultPoint = Point2D ()
printfn "%A" defaultPoint.Coords

// Стоит обратить внимание, что несмотря на то, что класс реализует интерфейс
// IShape, напрямую вызвать эти методы нельзя! Для этого надо сначала выполнить
// приведение объектного типа с помощью оператора ( :> )
(point :> IShape).Draw()

// Второй способ - написать функцию, делающую это автоматически
let draw (p : #IShape) = p.Draw()
draw point
// В функцию можно передавать все объекты, реализующие интерфейс IShape

// Наконец, объявим наследников

type Circle (centerX, centerY, initRadius) =
    inherit Point2D (centerX, centerY)  // 1
    
    let mutable radius = initRadius
    
    new () = new Circle (0., 0., 1.)
    
    member this.Radius
        with get () = radius
        and set newRadius = radius <- newRadius
    
    interface IShape with
        override this.Draw () =
            printfn "Cicle ((%A), %A)" base.Coords radius  // 2
        override this.Area = Math.PI * radius * radius / 2.

// 1. При помощи ключевого слова inherit гуказываем бащовый класс и аргументы
//    конструктора.
// 2. Ссылаемся на базовый класс через слово base

// Помимо этого, мы добавили конструктор, приватное поле, публичное поле. Тут
// как и в прошлый раз

// Объявим аналогично второго наследника
type Square (centerX, centerY, initLength) =
    inherit Point2D (centerX, centerY)
    
    let mutable length = initLength
    
    new () = new Square (0., 0., 1.)
    
    member this.Length
        with get () = length
        and set newLength = length <- newLength
    
    interface IShape with
        override this.Draw () =
            printfn "Square ((%A), %A)" base.Coords length
        override this.Area = length * length
   
// Проверим
draw (new Square (2., 4., 0.5))

// Можно делать полиморфизм!
let points = [
    new Point2D ()
    new Square () :> Point2D
    new Circle (1., 1., 5.) :> Point2D
]

printfn "%A" (points |> List.map (fun x -> (x.Coords, (x :> IShape).Area)))

points |> List.iter draw

points |> List.iter (fun p -> p.MoveTo (Point2D.Zero) )

points |> List.iter draw

// Оператор ( :> ) работает в сторону более абстрактного класса, то есть это
// только upcasting.

// Для downcasting'а используется оператор ( :?> ). Но
// на этапе компиляции проверить типы не всегда возможно, поэтому он кидает
// исключение в рантайме. Так что вместо него рекомендуется использовать
// сопоставление с образцом:
let area (p : Object) =
    match p with
    | :? IShape as s -> Some s.Area
    | _ -> None
    
printfn "%A" (area 4)
printfn "%A" (area (new Square (1., 1., 4.)))

// На самом деле ( :? ) - оператор, возвращающий bool. Таким образом, можно и
// не делать сопоставление с образцом, а просто проверять перед downcasting'ом 
let area2 (p : Object) =
    if p :? Shape then Some (p :?> Shape).Area
    else None
