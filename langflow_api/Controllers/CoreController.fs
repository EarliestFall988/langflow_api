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

[<ApiController>]
[<Route("[controller]")>]
type CoreController (logger : ILogger<CoreController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =       
       """
       {
            "response" : "Hi Mom!"
       }
        """