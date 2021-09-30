module Types

[<RequireQualifiedAccess>]
type Page =
    | Home
    | Notes

type Song =
    {| _id: string
       _rev: string option
       name: string
       song: Browser.Types.Blob |}
