module MercuryLibraryTests.MappersTests

open System
open MercuryLibrary.Models
open Swensen.Unquote
open Xunit

[<Fact>]
let ``toWhoisResponse all properties correct`` () =
    let now = DateTime.UtcNow

    let irrelevant = String.Empty

    let domain = "google.com"

    let oneDayAgo = now.AddDays(-1.0).ToString()

    let twoDaysAgo = now.AddDays(-2.0).ToString()

    let threeDaysFromNow = now.AddDays(3.0).ToString()

    let fourDaysAgo = now.AddDays(-4.0).ToString()

    let fiveDaysAgo = now.AddDays(-5.0).ToString()

    let audit = Audit()
    audit.createdDate <- fourDaysAgo
    audit.updatedDate <- fiveDaysAgo

    let whoisRecord = WhoisRecord()
    whoisRecord.createdDate <- oneDayAgo
    whoisRecord.updatedDate <- twoDaysAgo
    whoisRecord.expiresDate <- threeDaysFromNow
    whoisRecord.status <- irrelevant
    whoisRecord.audit <- audit

    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse now domain whoisRecord

    test <@ whoisResponse.Value.Domain = domain @>

    test <@ whoisResponse.Value.DomainAgeInDays = Option.Some (now - DateTime.Parse(oneDayAgo)).TotalDays @>

    test <@ whoisResponse.Value.DomainLastUpdatedInDays = Option.Some (now - DateTime.Parse(twoDaysAgo)).TotalDays @>

    test <@ whoisResponse.Value.DomainExpirationInDays = Option.Some (DateTime.Parse(threeDaysFromNow) - now).TotalDays @>

    test <@ whoisResponse.Value.AuditCreated.Value.ToString() = fourDaysAgo @>

    test <@ whoisResponse.Value.AuditUpdated.Value.ToString() = fiveDaysAgo @>

[<Fact>]
let ``toWhoisResponse all properties bar audit correct`` () =
    let now = DateTime.UtcNow

    let irrelevant = String.Empty

    let domain = "google.com"

    let oneDayAgo = now.AddDays(-1.0).ToString()

    let twoDaysAgo = now.AddDays(-2.0).ToString()

    let threeDaysFromNow = now.AddDays(3.0).ToString()

    let whoisRecord = WhoisRecord()
    whoisRecord.createdDate <- oneDayAgo
    whoisRecord.updatedDate <- twoDaysAgo
    whoisRecord.expiresDate <- threeDaysFromNow
    whoisRecord.status <- irrelevant
    whoisRecord.audit <- null

    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse now domain whoisRecord

    test <@ whoisResponse.Value.Domain = domain @>

    test <@ whoisResponse.Value.DomainAgeInDays = Option.Some (now - DateTime.Parse(oneDayAgo)).TotalDays @>

    test <@ whoisResponse.Value.DomainLastUpdatedInDays = Option.Some (now - DateTime.Parse(twoDaysAgo)).TotalDays @>

    test <@ whoisResponse.Value.DomainExpirationInDays = Option.Some (DateTime.Parse(threeDaysFromNow) - now).TotalDays @>

    test <@ true = whoisResponse.Value.AuditCreated.IsNone @>

    test <@ true = whoisResponse.Value.AuditUpdated.IsNone @>

[<Fact>]
let ``WhoisRecord missing`` () =
    let now = DateTime.UtcNow

    let domain = "google.com"

    let whoisRecord = null
    
    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse now domain whoisRecord

    test <@ true = whoisResponse.IsNone @>