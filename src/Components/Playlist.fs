module Components.Playlist

open Browser.Types
open Fable.Core

open Sutil
open Sutil.Attr
open Sutil.Bindings

open Types
open Stores

type PlaylistProps =
    { currentSong: Song option
      playlist: Song array }

let private songTemplate (song: Song) = Html.li [ Html.text song.name ]

let private emptyList (isEmpty) =
    if isEmpty then
        Html.p "There's nothing here, Add some music :)"
    else
        Html.p "Select a song to play :)"

let onFilesSelected (ev: Event) =
    let target = (ev.target :?> HTMLInputElement)

    if target.files.length > 0 then
        Pl.SaveSongs target.files
        |> Promise.map (fun _ -> Pl.LoadSongs())
        |> Promise.map (fun _ -> printfn "Songs Loaded")
        |> Promise.map (fun _ -> target.value <- "")
        |> Promise.catch (fun err -> JS.console.log (err))
        |> Promise.start
    else
        target.value <- ""


let private Playlist (props: Store<PlaylistProps>) (host: Browser.Types.Node) =
    let isEmptyList =
        props .> (fun props -> props.playlist.Length = 0)

    let files =
        props
        .> (fun props -> props.playlist |> Array.toList)

    Html.aside [
        Html.h3 "Perla Playlist!"
        Html.ul [
            Bind.each (files, songTemplate)
        ]
        Html.section [
            Html.section [
                Bind.fragment isEmptyList emptyList
            ]
            Html.input [
                Attr.typeFile
                Attr.multiple true
                on "change" onFilesSelected []
            ]
        ]
    ]

let register () =
    WebComponent.Register(
        "perla-playlist",
        Playlist,
        { currentSong = None
          playlist = Array.empty }
    )
