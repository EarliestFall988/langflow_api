module Tests

open System
open Xunit
open langflow_api.Controllers
open langflow_api

[<Fact>]
let ``My test`` () =
    Assert.True(true)



[<Fact>]
let ``new test`` () =
    let res = testExamples.runTest
    assert(res)
