module Types

[<RequireQualifiedAccess>]
type Page =
    | Home
    | Notes

type PouchCoreReponse = {| id: string; ok: bool; rev: string |}

type Song =
    {| _id: string
       _rev: string option
       name: string
       song: Browser.Types.Blob |}
