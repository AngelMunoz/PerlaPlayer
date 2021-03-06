module Pages.Home

open Sutil
open Sutil.Styling
open Components.Counter

let Home () =
    Html.article [
        Html.h1 "Home Page"
        Html.section [
            Html.h3 "Counter Starts at 0"
            Counter None
        ]
        Html.section [
            Html.h3 "Counter Starts at 100"
            Counter(Some 100)
        ]
    ]
    |> withStyle [
        rule
            "article"
            [ Css.displayFlex
              Css.flexDirectionColumn
              Css.alignContentCenter
              Css.alignItemsCenter ]
       ]
