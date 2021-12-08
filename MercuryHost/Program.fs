open System
open System.Net.Http
open System.Threading
open System.Xml.Serialization
open MercuryLibrary
open MercuryLibrary.Models

let log (message: string) : unit =
    let timestamp = DateTime.Now.ToString("hh:mm:ss.fff")
    printfn $"{timestamp}: {message}"

let getWhoisResponse (apiUrlFormat: string) (domain: string) : Async<WhoisResponse option> =
    InputValidation.whoisInputValidation apiUrlFormat domain

    let apiUrl = String.Format(apiUrlFormat, domain)

    let cancellationTokenSource =
        new CancellationTokenSource(TimeSpan.FromSeconds(3.0))

    let cancellationToken = cancellationTokenSource.Token

    let client = new HttpClient()

    async {
        // call GetAsync
        // translate Task to Async
        // execute the asynchronous expression
        // bind its result to apiResponse
        let! apiResponse =
            client.GetAsync(apiUrl, cancellationToken)
            |> Async.AwaitTask

        if apiResponse.IsSuccessStatusCode then
            let serializer = XmlSerializer(typeof<WhoisRecord>)

            // call ReadAsStreamAsync
            // translate Task to Async
            // execute the asynchronous expression
            // bind its result to stream
            let! stream =
                apiResponse.Content.ReadAsStreamAsync(cancellationToken)
                |> Async.AwaitTask

            // deserialize stream to obj
            // downcast to WhoisRecord
            let whoisRecord =
                serializer.Deserialize(stream) :?> WhoisRecord

            return Mappers.toWhoisResponse DateTime.Now domain whoisRecord
        else
            return Option.None
    }

[<EntryPoint>]
let main argv : int =
    log "function is starting..."

    let apiUrlFormat = argv.[0]

    let domain = argv.[1]

    let job =
        async { return! getWhoisResponse apiUrlFormat domain }

    let response = Async.RunSynchronously(job)

    printfn $"{response.ToString()}"

    log "function execution finished"

    0