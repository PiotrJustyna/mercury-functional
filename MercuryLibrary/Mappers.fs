module MercuryLibrary.Mappers

open System
open MercuryLibrary.Models

let parseDate (date: string) =
    match DateTime.TryParse date with
    | true, date -> Option.Some date
    | false, _ -> Option.None

let toWhoisResponse (now: DateTime) (domain: string) (whoisRecord: WhoisRecord) =
    if obj.ReferenceEquals(whoisRecord, null) then
        Option<WhoisResponse>.None
    else
        let createdDate = parseDate whoisRecord.createdDate

        let updatedDate = parseDate whoisRecord.updatedDate

        let expiresDate = parseDate whoisRecord.expiresDate

        if obj.ReferenceEquals(whoisRecord.audit, null) then
            let whoisResponse =
                { Domain = domain
                  DomainAgeInDays = BusinessLogic.differenceInDaysOptionalStart createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDaysOptionalStart updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDaysOptionalEnd now expiresDate
                  AuditCreated = Option.None
                  AuditUpdated = Option.None }

            Option<WhoisResponse>.Some whoisResponse
        else
            let auditCreatedDate = parseDate whoisRecord.audit.createdDate

            let auditUpdatedDate = parseDate whoisRecord.audit.updatedDate

            let whoisResponse =
                { Domain = domain
                  DomainAgeInDays = BusinessLogic.differenceInDaysOptionalStart createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDaysOptionalStart updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDaysOptionalEnd now expiresDate
                  AuditCreated = auditCreatedDate
                  AuditUpdated = auditUpdatedDate }

            Option<WhoisResponse>.Some whoisResponse
