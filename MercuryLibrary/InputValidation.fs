module MercuryLibrary.InputValidation

open System

let whoisInputValidation (apiUrlFormat: string) (domain: string) =
    if String.IsNullOrWhiteSpace apiUrlFormat then invalidArg (nameof apiUrlFormat) ""
    if String.IsNullOrWhiteSpace domain then invalidArg (nameof domain) ""