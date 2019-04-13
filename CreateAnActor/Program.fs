// Learn more about F# at http://fsharp.org

open Akkling

[<EntryPoint>]
let main argv =
    use system = System.create "my-system" (Configuration.defaultConfig())
    let helloRef = spawn system "hello-actor" <| props(fun context ->
        let rec loop () = actor {
            let! msg = context.Receive ()
            match msg with
            | "stop" -> return Stop // effect - stops current actor
            | "unhandle" -> return Unhandled // effect - marks message as unhandled
            | x -> 
                printfn "%s" x
                return! loop ()
        }
        loop ())
   
    helloRef <! "Tom"
    helloRef <! "Dick"
    
    system.Terminate().Wait()
    
    0 // return an integer exit code
