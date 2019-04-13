// Learn more about F# at http://fsharp.org

open System.Threading
open Akkling

module StatefulActor =
    // Messages in Akkling (and using the Akka.NET F# API) are typically defined using a discriminated union.
    // (See https://fsharpforfunandprofit.com/posts/discriminated-unions/.)
    type Message =
        | Hi
        | Greet of string
        
    // This function defines the behavior of our actor. It is a recursive function that never finishes but is
    // exectude for its "side effects." (In this example, "real" side-effects like printing to the console or
    // changing behavior.
    //
    // The variable. `lastKnown`, contains the state of this actor; specifically, the name of last person who
    // greeted this actor. Note particularly the expression `become (greeter who)`. The result of this
    // expression is for the actor to change its behavior to incorporate the new state.
    let rec greeter lastKnown = function
        | Hi -> printfn "Who sent Hi? %s?" lastKnown |> ignored
        | Greet (who) ->
            printfn "%s sends greetings..." who
            become (greeter who)
        
[<EntryPoint>]
let main argv =
    // Create the actor system for this application. The returned actor system is an instance of `IDisposable`.
    // The `use` keyword automatically disposes of this instance when this expression goes out of scope.
    // (See https://fsharpforfunandprofit.com/posts/let-use-do/#use-bindings for details.)
    //
    // Note that one can use three **different** expressions to configure the actor system using the
    // `Configuration` module:
    //
    // - `defaultConfig ()` returns the default F# Akka configuration
    // - `parse (hoconString : string)` returns the configuration specified by a HOCON string
    // (https://en.wikipedia.org/wiki/HOCON)
    // - `load ()` loads an Akka configuration found inside the current projects `.config` file
    //
    use system = System.create "my-system" <| Configuration.defaultConfig ()
    
    // This `let` binding creates an actor with the name "greeter."
    let aref = spawn system "greeter" <| props(actorOf (StatefulActor.greeter "Unknown"))
    
    // Send greetings from two people, Tom and Jane. However, sending `Hi` only greets Jane (the last person
    // who sent greetings to the actor).
    aref <! StatefulActor.Greet "Tom"
    aref <! StatefulActor.Greet "Jane"
    aref <! StatefulActor.Hi
    
    // The following call to sleep the current thread, allowing the actor to execute, was **not** present in the
    // example. However, without it, the program exits printing **no** message.
    Thread.Sleep (100)
    
    0 // return an integer exit code
