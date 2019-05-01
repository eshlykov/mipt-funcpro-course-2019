#load @"d:\winapp\lib\Fsharp.Charting\FSharp.Charting.fsx"
open FSharp.Charting

[1;2;3] |> Chart.Bar 


[1..100] |> List.filter (fun x->x%2=0) |> List.fold (+) 0

let generator f n =
   let mutable c = n
   fun () ->
      c <- f c
      c

let fibg = generator (fun (u,v) -> (u+v,u)) (1I,1I)

let map f g =
    fun () ->
       f(g())

let rec filter f g =
    fun () ->
       let x = g()
       if f x then x
       else (filter f g)()

let f = fibg |> map fst |> filter (fun x->x%256I=0I)

let s = seq { 1..100 }

let fib = Seq.unfold(fun (u,v) -> Some(v,(u+v,u))) (1I,1I)
fib |> Seq.filter (fun x->x%256I=0I) |> Seq.take(1) |> Seq.toList

open System.IO

let ReadLines fn =
  seq { use inp = File.OpenText fn in
        while not(inp.EndOfStream) do
            yield (inp.ReadLine())
      }

let book = ReadLines @"c:\books\Anna_kar.txt"

book
|> Seq.map (fun s -> s.Split([|' ';';';',';'.';'-';'!';'<';'>';'=';'/'|]))
|> Seq.concat
|> Seq.map (fun x->x.ToLower())
|> Seq.filter (fun x -> x.Length>3)
|> Seq.fold (fun ht s ->
              if Map.containsKey s ht then Map.add  s ((Map.find s ht)+1) ht
              else Map.add s 1 ht) Map.empty
|> Map.toSeq
|> Seq.sortBy (fun (w,n) -> -n)
|> Seq.take 7
|> Chart.Bar

book
|> Seq.map (fun s -> s.Split([|' ';';';',';'.';'-';'!';'<';'>';'=';'/'|]))
|> Seq.concat
|> Seq.map (fun x->x.ToLower())
|> Seq.filter (fun x -> x.Length>3)
|> Seq.groupBy (fun x->x.Length)
|> Seq.map (fun (n,l) -> (n,Seq.length l))
|> Seq.sortBy fst
|> Chart.Bar

book
|> Seq.map (fun s -> s.Split([|' ';';';',';'.';'-';'!';'<';'>';'=';'/'|]))
|> Seq.concat
|> Seq.map (fun x->x.ToLower())
|> Seq.filter (fun x -> x.Length>3)
|> Seq.groupBy (fun x->x)
|> Seq.map (fun (n,l) -> (n,Seq.length l))
|> Seq.sortBy (fun (w,n) -> -n)
|> Seq.take 7
|> Chart.Bar

let readset fn = 
    ReadLines fn
    |> Seq.map (fun x->x.[1..])
    |> Set.ofSeq

let pos = readset @"c:\books\positive.txt"
let neg = readset @"c:\books\negative.txt"

let w x = if Set.contains x pos then 1 elif Set.contains x neg then -1 else 0


ReadLines @"c:\books\wap_1.txt"
|> Seq.map (fun s -> s.Split([|' ';';';',';'.';'-';'!';'<';'>';'=';'/'|]))
|> Seq.concat
|> Seq.map (fun x->x.ToLower())
|> Seq.filter (fun x -> x.Length>0)
|> Seq.map w
|> Seq.scan (+) 0
|> Seq.toList
|> Chart.Bar


let rec ones = seq {
    yield 1
    yield! ones
}

let rec nat = seq {
    yield 1
    yield! Seq.map ((+)1) nat
}

let facts = nat |> Seq.scan (*) 1
Seq.nth 5 facts

{ 1..10 }
(..) 1 10

let fact = (..) 1 >> Seq.fold (*) 1
fact 5