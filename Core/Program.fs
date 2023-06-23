module langflow.Core

open System
open System.IO
open System.Text


//// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

let fileLocation = @"C:\Users\WorkPC\Documents\Software Developement\Programming Language Development\code files"

//type token =
//    | Plus
//    | Minus
//    | Times
//    | Divide
//    | Mod
//    | Num of int

////[<RequireQualifiedAccess>]
//type tokenResult =
//    | Tok of token
//    | Null

//let isWhiteSpace c = c = " " || c = "\t" || c = "\n"

//let isNumber c = c >= '0' && c <= '9'

///// convert a character to integar
//let charToInt (c: char) = Int32.Parse(c.ToString())

///// convert an integar to decimal
//let intToDecimal i = Decimal.Parse(i.ToString())

///// convert the character to a token if possible
//let convertCharToToken (c: char) : bool * tokenResult =
//    match c with
//    | '+' -> (true, Tok token.Plus)
//    | '-' -> (true, Tok token.Minus)
//    | '*' -> (true, Tok token.Times)
//    | '/' -> (true, Tok token.Divide)
//    | '%' -> (true, Tok token.Mod)
//    | x ->
//        (if isNumber (x) = true then
//             (true, (Tok(Num(charToInt x))))
//         else
//             (false, Null)) // does not handle whitespace ....

//// let handleCombinedToken (value: char list list) (res: char list list) (currentType: string) =
////     match value with
////     | head :: tail -> (
////         if head = ' ' then
////             res
////         else (
////             match currentType with
////             | "number" -> (
////                 if isNumber(head) = true then
////                     handleCombinedToken tail (res @ [head]) currentType

////             )
////             | "operator" -> (
////                 if isNumber(head) = false then
////                     handleCombinedToken tail (res @ [head]) currentType
////                 else
////                     res
////             )
////             | _ -> res
////         )
////     )


//let handleSmashedContext token lastValue result =
//    match token with
//    | head :: tail ->
//        if isNumber head = true then
//            (handleSmashedContext tail "number" (result @ [head]))

/////parse the line
//let ExplodeLine (line: string) : char list list =
//    line.Split(" ")
//    |> Array.toList
//    |> List.filter (fun x -> x.Trim().Length <> 0)
//    |> List.map (fun x -> x.ToCharArray() |> Array.toList)

/////parse the document
//let ExplodeDocument (text: string) : char list list list =
//    text.Split("\n")
//    |> Array.toList
//    |> List.filter (fun x -> x.Trim().Length <> 0)
//    |> List.filter (fun x -> x.StartsWith("//") = false)
//    |> List.map (fun x -> ExplodeLine x)


///// lex the document (convert a list of list of list of characters into tokenResults)
//let lex (doc: char list list list) : (bool * tokenResult) list list list =
//    List.map (fun z -> List.map (fun x -> List.map (fun y -> convertCharToToken y) x) z) doc

///// run the programming language
//let run (txt: string) =
//    ExplodeDocument txt |> Seq.toList |> lex

//// File.ReadAllText(fileLocation) //read the file
//// |> run //anaylize the file
//// |> printfn "Result: %A" // print results

////
//// Tests
////

//let test1 = ExplodeLine("1 + 2")
//printfn ("test1: %A") test1

//let test2 = ExplodeLine("1+2  ")
//printfn ("test2: %A") test2

//let test3 = ExplodeDocument("1 + 2\n3 + 4\n5 + 6")
//printfn ("test3: %A") test3

exception ValueNotUnderstoodException of string

type ExplodeStatus =
    | Start
    | Semicolon
    | Token
    | ReadString
    | Number
    | Add
    | Mult
    | Div
    | Subtract
    | Letter
    | Let
    | Equal

let rec ReadBack (data: char list) (result: char list) (amtLeft: int) : string =

    //printfn "\t\t\trev: %A" data

    match (data, amtLeft) with
    | ([], x) -> result |> List.toArray |> System.String
    | (h::t, x) -> 
        //printfn "%A" h
        if x >= 0 then ReadBack t (List.append result [h]) (amtLeft - 1)
        else result |> List.toArray |> System.String


let IsLet result : bool =
    printfn "\t\tres: %A" result
    let str = ReadBack result [] 4
    printfn "\t\tstr: %A" str
    if str = "let" then true 
    else false

///<summary>clean line and convert into tokens</summary>
let rec ExplodeLine (line: char list) (result: char list) (status : ExplodeStatus) (lineNumber: int) (characterNumber: int) : string =
    match line with
    | head :: tail ->
        if head = ' ' && status <> ExplodeStatus.ReadString then
            if (IsLet result) then 
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Let lineNumber (characterNumber + 1) //keep the whitespace - we're now looking at some sort of value store
            else
                ExplodeLine tail result status lineNumber (characterNumber + 1)
        elif Char.IsDigit head then
            let appendedResult = List.append result [head]
            //printfn "\tappended Result: %A" appendedResult
            ExplodeLine tail appendedResult status lineNumber (characterNumber + 1)
        elif status <> ExplodeStatus.ReadString then
            if head = '+' then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Add lineNumber (characterNumber + 1)
            elif head = '-' then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Subtract lineNumber (characterNumber + 1)
            elif head = '*' then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Mult lineNumber (characterNumber + 1)
            elif head = '/' then
                if status = Div then //not reading a string but the second time we see a dividing line...
                    List.removeAt ((List.length result) - 1) result |> List.toArray |> System.String //it's a comment... exit and remove the previous divide character
                else
                    ExplodeLine tail (List.append result [head]) ExplodeStatus.Div lineNumber (characterNumber + 1) // it's could be a divide operator 

            elif head = ';' then
                ExplodeLine tail result ExplodeStatus.Semicolon lineNumber (characterNumber + 1)

            elif head = '=' then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Equal lineNumber (characterNumber + 1)

            elif Char.IsLetter head then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Letter lineNumber (characterNumber + 1)
            else 
                //ExplodeLine tail result status
                raise (ValueNotUnderstoodException($"Cannot parse the unknown value in the document {head} at {lineNumber}:{characterNumber}")) // oh poop
        elif status = ExplodeStatus.ReadString && Char.IsAscii head then
                ExplodeLine tail (List.append result [head]) ExplodeStatus.Letter lineNumber (characterNumber + 1)
        else
            raise (ValueNotUnderstoodException($"Cannot parse the unknown value in the document {head} at {lineNumber}:{characterNumber}")) // oh poop
    | _ -> 
    result 
    |> List.toArray 
    |> System.String

///<summary>Clean the doc and convert into tokens</summary>
let rec ExplodeDocument (data: string list) (result : string list) (lineNumber: int) : string list = 
    match data with
    | head :: tail ->
        //printfn "head: %A" head
        match head.Trim() with 
        | "" -> ExplodeDocument tail result (lineNumber + 1) // line does not contain anything
        | x -> 
            if x.StartsWith("//") then 
                ExplodeDocument tail result (lineNumber + 1) // remove lines that start with comments
            else
                //printfn "reading line: %A" x
                let explodedLine = ExplodeLine (Seq.toList x) [] ExplodeStatus.Start lineNumber 0
                ExplodeDocument tail (List.append result [explodedLine]) (lineNumber + 1)
    | _ -> result

///<summary>Read the file and return a list of file lines</summary>
let ReadFile (filePath: string) : string list =
    use fileReader = File.OpenRead(filePath)
    use reader = new StreamReader(fileReader)
    
    let mutable completed = false

    seq{

        while completed = false do
            let line = reader.ReadLine()

            if line = null then
                completed <- true
            else
                line
    }
    |> Seq.toList
            

printfn "\tStarted:\n"

let doc = ReadFile(fileLocation + @"\ts.ev") 

ExplodeDocument doc [] 0 |> printfn "\tResult:\n\n%A"

printfn "\n\tDone."

