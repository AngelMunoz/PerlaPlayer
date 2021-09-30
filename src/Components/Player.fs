module Components.Player

open Sutil

open Types

type PlayerProps =
    { currentSong: Song option
      playlist: Song seq }

let private Player (props: Store<PlayerProps>) (host: Browser.Types.Node) =
    Html.article [
        Html.text "Perla player!"
    ]

let register () =
    WebComponent.Register(
        "perla-player",
        Player,
        { currentSong = None
          playlist = Seq.empty }
    )
