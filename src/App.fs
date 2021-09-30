module App

open Fable.Core
open Sutil
open Sutil.Attr
open Types
open Pages.Home
open Pages.Notes
open Stores

Pl.LoadSongs()
|> Promise.map (fun _ -> printfn "Songs Loaded")
|> Promise.catch (fun err -> JS.console.log (err))
|> Promise.start

let private app () =
    let page = Store.make (Page.Home)

    let goToPage newPage _ = page <~ newPage

    let getPage page =
        match page with
        | Page.Home -> Home()
        | Page.Notes -> Notes()

    Html.app [
        Html.main [ Bind.fragment page getPage ]
        Html.custom (
            "perla-playlist",
            [ Bind.attr ("currentSong", CurrentSong)
              Bind.attr ("playlist", Playlist) ]
        )
        Html.custom (
            "perla-player",
            [ Bind.attr ("currentSong", CurrentSong)
              Bind.attr ("playlist", Playlist) ]
        )
    ]

let start () =
    Program.mountElement "sutil-app" (app ())
