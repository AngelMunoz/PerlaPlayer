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

    let CleanPlaylist () =
        promise {
            let! _ = Songs.CleanPlaylist()
            return! LoadSongs()
        }

    let TryNextSong () =
        let current = CurrentSong |> Store.getMap id
        let playlist = Playlist |> Store.getMap id

        let index =
            match current with
            | Some song ->
                playlist
                |> Array.tryFindIndex (fun s -> s._id = song._id)
                |> Option.map
                    (fun index ->
                        if playlist.Length = index + 1 then
                            0
                        else
                            index + 1)
                |> Option.defaultValue 0
            | None -> 0

        CurrentSong <~ (playlist |> Array.tryItem index)
