module Tests.InputValidationTests

open System
open Xunit
open Swensen.Unquote

[<Theory>]
[<InlineData("", "")>]
[<InlineData("http://www.sample.url", "")>]
[<InlineData("", "sample.domain")>]
let ``whoisInputValidation negative`` (apiUrlFormat: string, domain: string) =
    raises<ArgumentException> <@ MercuryLibrary.InputValidation.whoisInputValidation apiUrlFormat domain @>