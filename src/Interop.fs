[<AutoOpen>]
module Interop

open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.JS
open Browser.Types
open Types

[<Emit("new Event($0, $1)")>]
let private createEvent (name, obj) : Event = jsNative

[<Emit("new CustomEvent($0, $1)")>]
let private createCustomEvent (name, obj) : CustomEvent<'T> = jsNative

type Songs() =

    static member SaveFiles(files: FileList) : Promise<PouchCoreReponse array> =
        importMember "./Interop.js"

    static member LoadSongs<'T>() : Promise<{| count: int; docs: 'T array |}> =
        importMember "./Interop.js"

    static member CleanPlaylist() : Promise<unit> = importMember "./Interop.js"

type Node with
    member this.dispatchSimple
        (
            event: string,
            ?bubbles: bool,
            ?composed: bool,
            ?cancelable: bool
        ) : bool =
        let evt =
            createEvent (
                event,
                {| bubbles = defaultArg bubbles true
                   composed = defaultArg composed true
                   cancelable = defaultArg cancelable true |}
            )

        this.dispatchEvent evt

    member this.dispatchCustom<'T>
        (
            event: string,
            detail: 'T,
            ?bubbles: bool,
            ?composed: bool,
            ?cancelable: bool
        ) : bool =
        let evt =
            createCustomEvent (
                event,
                {| bubbles = defaultArg bubbles true
                   composed = defaultArg composed true
                   cancelable = defaultArg cancelable true
                   detail = detail |}
            )

        this.dispatchEvent evt
