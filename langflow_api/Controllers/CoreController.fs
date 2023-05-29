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
                yield sr.ReadLine()
        }

        Seq.fold(fun str x -> str + " " + (string x)) " " res
                
    
    let v1Path (path: string) : string = @"/api/v1" + path


    [<HttpGet>]
    member x.Get() =
          
           { Response = "Hi Mom!"}

    [<HttpPost>]
    member x.Post() =
            
        let mutable body_result = "n/a"
        
        if x.Request.Body <> null then 
           body_result <- readBody x.Request.Body

        { Response = body_result }

    [<HttpPost("/api/v1/instructions")>]
    member x.HandleIncomingInstructions() =
        
        let body = readBody x.Request.Body

        let (res, instructions) = Core.parse_instructions_request body

        {
            Status = (if res = true then 200 else 400)
            Message = instructions
            Values = []
        }