[<AutoOpen>]
module Interop

open Fable.Core.JsInterop
open Fable.Core.JS
open Browser.Types

type Songs() =

    static member SaveFiles
        (files: FileList)
        : Promise<{| id: string; ok: bool; rev: string |} array> =
        importMember "./Interop.js"

    static member LoadSongs<'T>() : Promise<{| count: int; docs: 'T array |}> =
        importMember "./Interop.js"
