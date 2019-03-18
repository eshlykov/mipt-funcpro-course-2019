open System

// Задача - написать парсер строки в Json-объект
// http://www.json.org
// Нельзя использовать библиотеки для работы с Json

type Json =
    | Null
    | Boolean of bool
    | Number of int
    | String of string
    | Array of (Json list)
    | Object of ((string * Json) list)

// Ограничения:
// 1. Все подаваемые строки корректны и не содержат пробельных символов 
// 2. Литералы типов Null, Boolean в нижнем регистре (null, false, true)
// 3. Литералы типа Number - неорицательные десятичные числа без ведущих нулей, они укладываются в int
// 4. Литералы типа String не содержат спецсимволов и написаны в двойных кавычках
// 5. Все ключи в Object различны

// Также см. тесты
// Если что-то пошло не так, вызывайте failwith -https://docs.microsoft.com/ru-ru/dotnet/fsharp/language-reference/exception-handling/the-failwith-function

// Шаг 1 (1 балл). Преобразование из строки в список символов

let explode (str : string) : char list = raise (NotImplementedException ())

// Шаг 2 (3 балла). Написать токенизатор - функцию, которая принимает список символов и возвращает список токенов

type Token =
    | OpenBrace'
    | CloseBrace'
    | Colon'
    | OpenBracket'
    | CloseBracket'
    | Comma'
    | Null'
    | Boolean' of bool
    | Number' of int
    | String' of string

let rec tokenize (chars : char list) : Token list =
    // Возможно, вам понадобятся вспомогательные функции, которые принимают список символов
    // и возврашают пару из токена и остатка списка
    let takeNull (chars : char list) : (Token * char list) = raise (NotImplementedException ())
    let takeBoolean (chars : char list) : (Token * char list) = raise (NotImplementedException ())
    let takeNumber (chars : char list) : (Token * char list) = raise (NotImplementedException ())
    let takeString (chars : char list) : (Token * char list) = raise (NotImplementedException ())
    raise (NotImplementedException ())

// Шаг 3 (3 балла). Распарсить токены в Json-объект

let parse (tokens : Token list) : Json = raise (NotImplementedException ())

// Шаг 4 (9 баллов). Прохождение тестов

let jsonize (str : string) : Json = str |> explode |> tokenize |> parse

let rec stringify = function
  | Object list ->
    "{" + (String.concat "," (List.map (fun (key, value) -> "\"" + key + "\":" + stringify value) list)) + "}"
  | Array array  -> "[" + (String.concat "," (List.map stringify array)) + "]"
  | String value -> "\"" + value + "\""
  | Number value -> string value
  | Boolean true -> "true"
  | Boolean false -> "false"
  | Null -> "null"

// Группа тестов на +1 балл
let jsonNull = "null"
let jsonBoolean2 = "false"
let jsonBoolean3 = "true"
let jsonString1 = "\"\""
let jsonString2 = "\"string\""
let jsonNumber1 = "0"
let jsonNumber2 = "1024"
// Группа тестов на +2 балла
let jsonArray1 = "[]"
let jsonArray2 = "[null,true,\"string\",1024]"
// Группа тестов на +2 балла
let jsonObject1 = "{}"
let jsonObject2 = "{\"null\":null,\"bool\":true,\"number\":1024,\"string\":\"string\"}"
// Группа тестов на +2 балла
let jsonArray3 = "[[],{}]"
let jsonArray4 = "[" + jsonArray2 + "," + jsonObject2 + "]"
// Групап тестов на +2 балла
let jsonObject3 = "{\"jsonArray1\":[],\"jsonObject1\":{}}"
let jsonObject4 = "{\"jsonArray2\":" + jsonArray2 + ",\"jsonObject2\":" + jsonObject2 + "}"

// Здесь можно закомментировать строчку, и тогда тест не добавится в список
let jsons = [
    jsonNull
    jsonBoolean2
    jsonBoolean3
    jsonString1
    jsonString2
    jsonNumber1
    jsonNumber2
    jsonArray1
    jsonArray2
    jsonObject1
    jsonObject2
    jsonArray3
    jsonArray4
    jsonObject3
    jsonObject4
]

let convetedJsons = (List.map (fun str -> stringify (jsonize str)) jsons)
let isPassed = List.map2 ( = ) jsons convetedJsons

printfn "%A" (List.zip jsons convetedJsons)
printfn "%A" isPassed

if List.contains false isPassed then
    failwith "Tests are not passed"
