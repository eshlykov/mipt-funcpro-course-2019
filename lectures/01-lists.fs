let f x = x*2
let f = fun x y -> x*2
let f = function 1 -> 2 | _ -> 3 



[1;2;3]

1::2::3::[]

let rec len = function
 | [] -> 0
 | x::xs -> 1+len xs

List.map ((*)2) [1;2;3]

List.filter (fun x->x%2=0) [1..100]

List.fold (fun acc x ->acc+x) 0 [1..100]
List.reduce (fun x y -> x+y) [1..100]

let L = [5;7;8;45;78;99;34;56;4]

List.reduce (max) L
List.reduce (min) L

List.fold (fun (mi,ma) x -> (min x mi,max x ma)) (List.head L,List.head L) L

List.append [1;2;3] [4;5;6]
[1;2;3]@[4;5;6]

let rec app A B =
  match A with
  | [] -> B
  | x::xs -> x::(app xs B)

let rec rev = function
 | [] -> []
 | x::xs -> (rev xs)@[x]


let rec rev L = 
 match L with
 | [] -> []
 | x::xs -> (rev xs)@[x]


rev [1..100]

let rev L = 
  let rec rev' acc = function
  | [] -> acc
  | x::xs -> rev' (x::acc) xs
  rev' [] L

rev [1..100]

let rec ins x = function
 | [] -> [[x]]
 | y::ys ->
    let u = ins x ys |> List.map (fun t -> y::t)
    (x::y::ys)::u

let rec perm = function
 | [] -> [[]]
 | x::xs ->
     List.collect (ins x) (perm xs)

perm [1;2;3]
