module MercuryLibrary.BusinessLogic

open System

let differenceInDays (start: DateTime) (finish: DateTime) = (finish - start).TotalDays

let differenceInDaysOptionalEnd (now: DateTime) (date: Option<DateTime>) =
    if date.IsSome then
        differenceInDays now date.Value |> Option.Some
    else
        Option.None

let differenceInDaysOptionalStart (date: Option<DateTime>) (now: DateTime) =
    if date.IsSome then
        differenceInDays date.Value now |> Option.Some
    else
        Option.None