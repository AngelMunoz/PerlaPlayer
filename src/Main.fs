module Maon

open Fable.Core

JsInterop.importSideEffects "./styles.css"


Components.Player.register ()
Components.Playlist.register ()
App.start ()
