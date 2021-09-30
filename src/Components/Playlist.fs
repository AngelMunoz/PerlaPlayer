module Components.Playlist

open Browser.Types
open Fable.Core

open Sutil
open Sutil.Attr
open Sutil.Styling

open type Feliz.length

open Types
open Stores

type PlaylistProps =
    { currentSong: Song option
      playlist: Song array }

let private songTemplate (host: Node) (song: Song) =
    let onSelected _ =
        host.dispatchCustom ("on-selected-song", song)
        |> ignore


    Html.li [
        Html.text song.name
        on "dblclick" onSelected []
    ]

let private emptyList (isEmpty) =
    let cleanPlaylist _ =
        Pl.CleanPlaylist()
        |> Promise.map (fun () -> printfn "Songs Deleted")
        |> Promise.start

    if isEmpty then
        Html.p "There's nothing here, Add some music :)"
    else
        Html.div [
            Html.p "Select a song to play 🎧"
            Html.button [
                Html.text "Or start from scratch 🧺"
                on "click" cleanPlaylist []
            ]
        ]

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

let private styles =
    [ rule
        ":host"
        [ Css.displayFlex
          Css.flexDirectionColumn ]
      rule
          "ul"
          [ Css.maxHeight (vh 58)
            Css.overflowYAuto
            Css.paddingBottom (em 5) ]
      rule
          "footer"
          [ Css.padding (em 0.5)
            Css.displayFlex
            Css.flexDirectionColumn
            Css.justifyContentCenter
            Css.alignItemsCenter
            Css.positionSticky
            Css.bottom 0
            Css.backgroundColor "var(--playlist-footer-background, #302f2f)"
            Css.borderRadius (px 4)
            Css.opacity 0.8 ]
      rule
          "header"
          [ Css.padding (em 0.5)
            Css.textAlignCenter
            Css.positionSticky
            Css.top 0
            Css.backgroundColor "var(--playlist-footer-background, #302f2f)"
            Css.borderRadius (px 4)
            Css.opacity 0.8 ] ]

let private Playlist (props: Store<PlaylistProps>) (host: Browser.Types.Node) =
    let isEmptyList =
        props .> (fun props -> props.playlist.Length = 0)

    let files =
        props
        .> (fun props -> props.playlist |> Array.toList)

    Html.aside [
        adoptStyleSheet styles
        Html.header [
            Html.h3 "Perla Playlist!"
        ]
        Html.ul [
            Bind.each (files, songTemplate host)
        ]
        Html.footer [
            Bind.fragment isEmptyList emptyList
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
