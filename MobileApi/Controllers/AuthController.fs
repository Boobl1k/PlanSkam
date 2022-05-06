namespace MobileApi.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MobileApi

[<ApiController>]
[<Route("[controller]")>]
type AuthController() =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =
        let rng = Random()

        [| for index in 0..4 ->
               { Date = DateTime.Now.AddDays(float index)
                 TemperatureC = rng.Next(-20, 55)
                 Summary = "asd" } |]
