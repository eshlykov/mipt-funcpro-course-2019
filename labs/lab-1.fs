open System

type 'a MySet = | MySet of 'a list

// При решении запрещено пользоваться функциями из модуля List, оператором ( @ ) и циклом for
// Каждый пункт без звездочки стоит 1 балл, каждый пункт со звездочкой - 2 балла,
// причем второй балл добавляется при использовании хвостовой рекурсии
// (в том числе во всех используемых рекурсивных функциях)
// При значительном несоблюдении код-стайла итоговый балл за всю работу понижается на 1

// Проверить, является ли множество пустым
let isEmpty set = raise (NotImplementedException ())

// Проверить, лежит ли элемент во множестве
let isMember element set = raise (NotImplementedException ())

// Проверить, что первое множество является подмножеством второго множества
let isSubset set1 set2 = raise (NotImplementedException ())

// Проверить, равны ли два множества
let isEqual set1 set2 = raise (NotImplementedException ())

// * Получить мощность множества
let cardinality set = raise (NotImplementedException ())

// * Найти объединение двух множеств
let unite set1 set2 = raise (NotImplementedException ())

// * Найти пересечение двух множеств
let intersect set1 set2 = raise (NotImplementedException ())

// * Найти декартово произведение двух множеств
let product set1 set2 = raise (NotImplementedException ())

// * Сконструировать множество из списка
let mySet list = raise (NotImplementedException ())

// * Найти множество из всех подмножеств данного множества
let powerSet set = raise (NotImplementedException ())

////////////////////////////////////////////////////////////////////////////////

let set0 = MySet []
let set1 = MySet [ 0; 2; 3; 4; 7; 8; 10; 21 ]
let set2 = MySet [ 0; 1; 2; 4; 5; 10; 11; 12; 14 ]

printfn "%A" (isEmpty set0)
printfn "%A" (isEmpty set1)
printfn "%A" (isEmpty set2)

printfn "%A" (isMember 1 set0)
printfn "%A" (isMember 2 set1)
printfn "%A" (isMember 3 set2)

printfn "%A" (isSubset set0 set0)
printfn "%A" (isSubset set0 set1)
printfn "%A" (isSubset set0 set2)
printfn "%A" (isSubset set1 set0)
printfn "%A" (isSubset set1 set1)
printfn "%A" (isSubset set1 set2)
printfn "%A" (isSubset set2 set0)
printfn "%A" (isSubset set2 set1)
printfn "%A" (isSubset set2 set2)

printfn "%A" (isEqual set0 set0)
printfn "%A" (isEqual set0 set1)
printfn "%A" (isEqual set0 set2)
printfn "%A" (isEqual set1 set0)
printfn "%A" (isEqual set1 set1)
printfn "%A" (isEqual set1 set2)
printfn "%A" (isEqual set2 set0)
printfn "%A" (isEqual set2 set1)
printfn "%A" (isEqual set2 set2)

printfn "%A" (cardinality set0)
printfn "%A" (cardinality set1)
printfn "%A" (cardinality set2)

printfn "%A" (unite set0 set0)
printfn "%A" (unite set0 set1)
printfn "%A" (unite set0 set2)
printfn "%A" (unite set1 set0)
printfn "%A" (unite set1 set1)
printfn "%A" (unite set1 set2)
printfn "%A" (unite set2 set0)
printfn "%A" (unite set2 set1)
printfn "%A" (unite set2 set2)

printfn "%A" (intersect set0 set0)
printfn "%A" (intersect set0 set1)
printfn "%A" (intersect set0 set2)
printfn "%A" (intersect set1 set0)
printfn "%A" (intersect set1 set1)
printfn "%A" (intersect set1 set2)
printfn "%A" (intersect set2 set0)
printfn "%A" (intersect set2 set1)
printfn "%A" (intersect set2 set2)

printfn "%A" (product set0 set0)
printfn "%A" (product set0 set1)
printfn "%A" (product set0 set2)
printfn "%A" (product set1 set0)
printfn "%A" (product set1 set1)
printfn "%A" (product set1 set2)
printfn "%A" (product set2 set0)
printfn "%A" (product set2 set1)
printfn "%A" (product set2 set2)

printfn "%A" (mySet [])
printfn "%A" (mySet [ 4; 5; 3; 1; 2 ])
printfn "%A" (mySet [ 3; 3; 1; 2; 2; 1 ])
printfn "%A" (mySet [ 1; 2; 3; 4 ])

printfn "%A" (powerSet set0)
printfn "%A" (powerSet (MySet [ 0; 1; 2; 3 ]))
