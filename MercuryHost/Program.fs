open System
open System.Net.Http
open System.Threading
open System.Xml.Serialization
open MercuryLibrary
open MercuryLibrary.Models

let log (message: string) =
    let timestamp = DateTime.Now.ToString("hh:mm:ss.fff")
    printfn $"{timestamp}: {message}"

let getWhoisResponse (apiUrlFormat: string) (domain: string) =
    InputValidation.whoisInputValidation apiUrlFormat domain

    let apiUrl = String.Format(apiUrlFormat, domain)

    let cancellationTokenSource =
        new CancellationTokenSource(TimeSpan.FromSeconds(3.0))

    let cancellationToken = cancellationTokenSource.Token

    let client = new HttpClient()

    async {
        let! apiResponse =
            client.GetAsync(apiUrl, cancellationToken)
            |> Async.AwaitTask

        if apiResponse.IsSuccessStatusCode then
            let serializer = XmlSerializer(typeof<WhoisRecord>)

            let! stream =
                apiResponse.Content.ReadAsStreamAsync(cancellationToken)
                |> Async.AwaitTask

            let whoisRecord =
                serializer.Deserialize(stream) :?> WhoisRecord

            return Mappers.toWhoisResponse DateTime.Now domain whoisRecord
        else
            return Option.None
    }

[<EntryPoint>]
let main argv =
    log "function is starting..."

    let apiUrlFormat = argv.[0]

    let domain = argv.[1]

    let job =
        async { return! getWhoisResponse apiUrlFormat domain }

    let response = Async.RunSynchronously(job)

    printfn $"{response.ToString()}"

    log "function execution finished"

    0