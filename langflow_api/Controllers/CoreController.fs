namespace langflow_api.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open langflow_api
open System.Text.Json
open System.Text.Json.Serialization
open System.Net
open System.IO
open System.Diagnostics;

[<ApiController>]
[<Route("[controller]")>]
type CoreController (logger : ILogger<CoreController>) =
    inherit ControllerBase()

    let convertToString result  = 
        result |> Seq.concat |> Seq.toArray |> System.String

    let readBody (body : Stream) : string  =
        
        let res = seq {
            use sr = new StreamReader(body)
            while sr.EndOfStream <> true do
                yield sr.ReadLine
        }

        Seq.fold(fun str x -> str + " " + (string x)) " " res
                

    [<HttpGet>]
    member x.Get() =
          //let mutable dictionary = new Dictionary<string, string>()
          //dictionary.Add("Response", "Hello World!")
          //dictionary.Add("Version", "1.0.0")
          //dictionary.Add("Environment", "Development")
          //dictionary.Add("Date", DateTime.Now.ToString())

          //let json = JsonSerializer.Serialize(dictionary)

          //x.Response.Headers.Add("Content-Type", "application/json; charset=utf-8")

          //json

          
           { Response = "Hi Mom!"}

    [<HttpPost>]
    member x.Post() =

        Debug.WriteLine "data recivied - parsing..."

        
        let mutable body_result = "n/a"
        
        if x.Request.Body <> null then 
           body_result <- readBody x.Request.Body
            
        

        Debug.WriteLine "finished reading"
        
        //let str = body |> Array.fold(fun str n -> str + " " + (string n)) " "

        

        Debug.WriteLine "data parsed..."

        

        //let data = Seq.toList body

        //Debug.WriteLine "converted to a list..."

        //let result = String.Join(",", data)

        //Debug.WriteLine ("Result: " + result)

        { Response = body_result }
          