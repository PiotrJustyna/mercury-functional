module MercuryLibraryTests.BusinessLogicTests

open System
open Xunit
open Swensen.Unquote

[<Fact>]
let ``differenceInDays finish date after start date`` () =
    let now = DateTime.Now

    let start = now.AddDays(-1.0)

    let finish = now

    test <@ MercuryLibrary.BusinessLogic.differenceInDays start finish = 1.0 @>

[<Fact>]
let ``differenceInDays finish date before start date`` () =
    let now = DateTime.Now

    let finish = now.AddDays(-1.0)

    let start = now

    test <@ MercuryLibrary.BusinessLogic.differenceInDays start finish = -1.0 @>

[<Fact>]
let ``differenceInDays finish date equal to start date`` () =
    let now = DateTime.Now

    let finish = now

    let start = now

    test <@ MercuryLibrary.BusinessLogic.differenceInDays start finish = 0.0 @>

[<Fact>]
let ``differenceInDaysOptionalStart returns None if start date not provided`` () =
    test <@ MercuryLibrary.BusinessLogic.differenceInDaysOptionalStart Option.None DateTime.Now = Option.None  @>

[<Fact>]
let ``differenceInDaysOptionalEnd returns None if end date not provided`` () =
    test <@ MercuryLibrary.BusinessLogic.differenceInDaysOptionalEnd DateTime.Now Option.None = Option.None  @>