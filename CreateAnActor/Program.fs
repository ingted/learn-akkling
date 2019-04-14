// Learn more about F# at http://fsharp.org

open System
open System.Threading
open Akkling

[<EntryPoint>]
let main argv =
    use system = System.create "my-system" (Configuration.defaultConfig())
    let helloRef =
        spawn system "hello-actor"
        <| props (fun context ->
            let rec loop () = actor {
                let! msg = context.Receive()
                match msg with
                | "stop" ->
                    printfn "Received a stop message"
                    // Returns the stop effect. This effect stops this actor.
                    return Stop 
                | "unhandle" ->
                    printfn "Received an unhandled message"
                    // Returns the unhandeld effect. This effect reports an unhandled message to the actor system.
                    return Unhandled 
                | x ->
                    printfn "%s" x
                    return! loop()
            }
            loop ())
   
    // Three "typical messages"
    helloRef <! "Tom"
    helloRef <! "Dick"
    helloRef <! "Harry"
    
    // An "unhandled" message
    helloRef <! "unhandle"
    
    // Finally, stop the actor
    helloRef <! "stop"
    
    // What if I try to send another message?
    // Then the system reports a dead letter.
    // helloRef <! "Jane"
    
    // Wait to finish handling all the messages
    Thread.Sleep(100)
    // Console.ReadKey() |> ignore
    
    0 // return an integer exit code
