let rec fix f x = f (fix f) x

let s f g x = f x (g x)
let k x y = x
let c f a b = f b a
let cond p f g x = if p x then f x else g x

let rec fact n = if n=1 then 1 else n*fact(n-1)

fact 5

let fact = fix (fun f->fun n-> if n=1 then 1 else n*f(n-1))

let fact = fix (fun f-> cond ((=)1) (k 1) (fun n->n*f(n-1)))

let fact = fix (cond ((=)1) (k 1) >> (fun f->fun n->n*f(n-1)))

let fact = fix(cond((=)1)(k 1)>>((s(*))<<(c(<<)(c(-)1))))


fact 5
