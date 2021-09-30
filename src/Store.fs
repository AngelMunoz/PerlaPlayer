module Stores

open Sutil
open Types
open Browser.Types
open Fable.Core

let Playlist: IStore<Song array> = Store.make [||]
let CurrentSong: IStore<Song option> = Store.make None

module Pl =
    let LoadSongs () =
        Songs.LoadSongs()
        |> Promise.map (fun songs -> Playlist <~ songs.docs)

    let SaveSongs = Songs.SaveFiles

    let SelectSong (_id: string) =
        CurrentSong
        <~ (Playlist
            |-> (fun songs ->
                songs
                |> Array.tryFind (fun song -> song._id = _id)))
