(* Lambda-interpreter *)

type id = string
type expr = 
 | Var of id
 | Lam of id*expr
 | App of expr*expr
 | Int of int
 | Cond of expr*expr*expr
 | Let of id*expr*expr
 | LetRec of id*expr*expr
 | PFunc of id
 | Op of id*int*expr list
 | Closure of expr*env
 | RClosure of expr*env*id
and
 env = Map<id,expr>

let lx = App(App(PFunc("+"),Int(8)),Int(2));;

let arity = function
| "sin" -> 1
| otherwise -> 2


let funof = function
| "+" -> (function [Int(a);Int(b)] -> Int(a+b))
| "-" -> (function [Int(a);Int(b)] -> Int(a-b))
| "*" -> (function [Int(a);Int(b)] -> Int(a*b))
| "/" -> (function [Int(a);Int(b)] -> Int(a/b))
| "=" -> (function [Int(a);Int(b)] -> if a=b then Int(1) else Int(0))
| ">" -> (function [Int(a);Int(b)] -> if a>b then Int(1) else Int(0))
| "<" -> (function [Int(a);Int(b)] -> if a<b then Int(1) else Int(0))
| "<=" -> (function [Int(a);Int(b)] -> if a<=b then Int(1) else Int(0))


let rec eval exp env =
  let _ = printfn "eval %A where %A" exp env in
  match exp with
  | App(e1,e2) -> apply (eval e1 env) (eval e2 env)
  | Int(n) -> Int(n)
  | Var(x) -> Map.find x env
  | PFunc(f) -> Op(f,arity f,[])
  | Op(id,n,el) -> Op(id,n,el)
  | Cond(e0,e1,e2) -> 
     if Int(1)=eval e0 env then eval e1 env else eval e2 env
  | Let(id,e1,e2) -> 
    let r = eval e1 env in
      eval e2 (Map.add id r env)
  | LetRec(id,e1,e2) ->
      eval e2 (Map.add id (RClosure(e1,env,id)) env)
  | Lam(id,ex) -> Closure(exp,env)
  | Closure(exp,env) -> exp
and apply e1 e2 =
  let _ = printfn "app (%A) (%A)" e1 e2 in
  match e1 with
   | Closure(Lam(v,e),env) -> eval e (Map.add v e2 env)
   | RClosure(Lam(v,e),env,id) -> eval e (Map.add v e2 (Map.add id e1 env))
   | Op(id,n,args) ->
      if n=1 then (funof id)(args@[e2])
      else Op(id,n-1,args@[e2])


let E exp = eval exp Map.empty;;

E (App(App(PFunc("+"),Int(8)),Int(2)));;
E (App(Lam("x",Var("x")),Int(5)));;
E (Let("id",Lam("x",Var("x")),
     Let("sq",Lam("z",App(App(PFunc("*"),Var("z")),Var("z"))),
       App(Var("sq"),App(Var("id"),Int(5)))
    )));;

E (LetRec("fact",Lam("x",
Cond(App(App(PFunc("<="),Var("x")),Int(1)),Int(1),App(App(PFunc("*"),Var("x")),App(Var("fact"),App(App(PFunc("-"),Var("x")),Int(1)))))
),App(Var("fact"),Int(5))));;
