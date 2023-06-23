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

let rec ExplodeLine (line: char list) (result: char list) : string =
    match line with
    | head :: tail ->
        let appendedResult = List.append result [head]
        ExplodeLine tail result
    | _ -> 
    result 
    |> List.toArray 
    |> System.String

let rec ExplodeDoument (data: string list) (result : string list) : string list = 
    match data with
    | head :: tail -> 
        let explodedLine = ExplodeLine (Seq.toList head) [] 
        ExplodeDoument tail (List.append result [explodedLine])
    | _ -> result


let ReadFile (filePath: string) =
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

let doc = ReadFile(fileLocation + @"\ts.ev") |> ExplodeDoument [] |> printfn "\tResult:\n\n%A"

printfn "\n\tDone."

