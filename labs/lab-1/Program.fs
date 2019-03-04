open Solution

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
