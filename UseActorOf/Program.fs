// Create an actor using the `actorOf` shortcut

open System.Threading
open Akkling

let helloBehavior = function
    | "stop" -> Stop
    | "unhandle" -> Unhandled
    | x ->
        printfn "%s" x
        Ignore
    

[<EntryPoint>]
let main argv =
    use system = System.create "my-system" (Configuration.defaultConfig())
    let helloRef = spawn system "hello-greeter" <| props (actorOf helloBehavior)
    
    // Three "typical messages"
    helloRef <! "Tom"
    helloRef <! "Dick"
    helloRef <! "Harry"
    
    // An "unhandled" message
    helloRef <! "unhandle"
    
    // Finally, stop the actor
    helloRef <! "stop"
    
    // Wait to finish handling all the messages
    Thread.Sleep(100)
    
    0 // return an integer exit code
