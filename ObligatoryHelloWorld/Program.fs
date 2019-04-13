// Learn more about F# at http://fsharp.org

open System.Threading
open Akkling

[<EntryPoint>]
let main argv =
    // Create the actor system for this application. The returned actor system is an instance of `IDisposable`.
    // The `use` keyword automatically disposes of this instance when this expression goes out of scope.
    // (See https://fsharpforfunandprofit.com/posts/let-use-do/#use-bindings for details.)
    use system = System.create "my-system" (Configuration.defaultConfig())
    
    // This `let` binding creates an actor with **no** name.
    let aref = spawnAnonymous system <| props(actorOf (fun m -> printfn "%s" m |> ignored))
    
    aref <! "Hello, Akkling World!"
    // Uncommenting the following line results in **compile-time** error because, by default, Akkling actors are
    // **statically** typed. (That is, since the call to `printfn` expects a string argument, F# will not even
    // compile sending a message that is **not** of type `string`.
    // aref <! 1 
    
    // The following call to sleep the current thread, allowing the actor to execute, was **not** present in the
    // example. However, without it, the program exits printing **no** message.
    Thread.Sleep(100)
    
    0 // return an integer exit code
