module Components.Player

open Browser.Types
open Browser.Url
open Sutil
open Sutil.Attr
open Sutil.Styling

open type Feliz.length
open Types

type PlayerProps =
    { currentSong: Song option
      playlist: Song seq }

let private revokeUrl (ev: Event) =
    let audio = (ev.target :?> HTMLAudioElement)
    URL.revokeObjectURL audio.src

let styles =
    [ rule
        ":host"
        [ Css.displayFlex
          Css.alignContentStretch
          Css.alignItemsStretch ]
      rule
          "article"
          [ Css.width (percent 100)
            Css.displayFlex
            Css.alignContentCenter ]
      rule
          "nav"
          [ Css.displayFlex
            Css.width (percent 25)
            Css.custom ("justify-content", "space-evenly") ]
      rule "audio" [ Css.width (percent 100) ] ]

let private Player (props: Store<PlayerProps>) (host: Browser.Types.Node) =
    let songSrc =
        props
        |> Store.map (fun props -> props.currentSong)
        |> Store.filter (fun song -> song.IsSome)
        |> Store.map (fun song -> URL.createObjectURL song.Value.song)

    Html.article [
        adoptStyleSheet styles
        Html.nav [
            Html.button [ Html.text "shuffle" ]
            Html.button [ Html.text "repeat" ]
        ]
        Html.audio [
            Attr.autoPlay true
            Attr.controls true
            Bind.attr ("src", songSrc)
            on "canplay" revokeUrl []
        ]
    ]

let register () =
    WebComponent.Register(
        "perla-player",
        Player,
        { currentSong = None
          playlist = Seq.empty }
    )
