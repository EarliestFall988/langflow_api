namespace langflow_api
#nowarn "20"
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http.Features
open Microsoft.AspNetCore.Http


module Program =
    let exitCode = 0

    //let fixSynchronousIO (context: HttpContext) (next: RequestDelegate)  = 
    //    if context.Request.Path.StartsWithSegments("/core") then
    //        let syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();

    //        if syncIOFeature <> null then
    //            syncIOFeature.AllowSynchronousIO <- true

    //    next

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()

        //fixing wierd synchronous IO problem
        builder.WebHost.ConfigureKestrel(fun serverOptions -> 
        (serverOptions.AllowSynchronousIO <- true))

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        //app.Use(fun (context: HttpContext) (next: RequestDelegate) -> fixSynchronousIO context next )

        let port = System.Environment.GetEnvironmentVariable("PORT");
         
        if (app.Environment.IsDevelopment()) then
            app.Run()
        else
            app.Run($"http://0.0.0.0:{port}") //now we can talk to the outside world via railway

        exitCode

