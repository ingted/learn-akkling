// Learn more about F# at http://fsharp.org

open System.Threading
open Akkling

[<EntryPoint>]
let main argv =
    use system = System.create "my-system" (Configuration.defaultConfig())
    
    // The ignore behavior "eats" all messages sent to it.
    let blackhole = spawnAnonymous system <| props Behaviors.ignore
    blackhole <! "Tom"
    
    // The printf behavior prints all incoming messages to standard out using the specified format.
    let printer = spawnAnonymous system <| props (Behaviors.printf "%s")
    printer <! "Dick"
    
    // The echo behavior simply sends any message received back to the sender.
    // I'm uncertain how to test this behavior. :(
    let echo = spawnAnonymous system <| props Behaviors.echo
                                               
    // Wait to finish handling all the messages
    Thread.Sleep(100)
    
    0 // return an integer exit code
