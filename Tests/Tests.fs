module Tests

open System
open Xunit
open langflow_api.Controllers
open langflow_api
open langflow.Core 


[<Fact>]
let ``My test`` () =
    Assert.True(true)

[<Fact>]
let ``new test`` () =
    let res = testExamples.runTest
    assert(res)


[<Fact>]
let ``explode line test`` () =
    let res = ExplodeLine("3 + 3")
    printfn "%A" res
    assert(res = [['3'; '+'; '3']])