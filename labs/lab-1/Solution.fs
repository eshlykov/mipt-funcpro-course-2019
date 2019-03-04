module Solution

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
