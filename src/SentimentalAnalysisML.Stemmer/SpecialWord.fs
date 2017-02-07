namespace StemmerFS

module SpecialWord =
    open StemmerFS.Dto

    let specialWord(word: string) = 
        let map = [ ("skis", "ski")
                    ("skies" ,"sky")
                    ("dying" ,"die")
                    ("lying" ,"lie")
                    ("tying" ,"tie")
                    ("idly"  ,"idl")
                    ("gently","gentl")
                    ("ugly"  ,"ugli")
                    ("early" ,"earli")
                    ("only"  ,"onli")
                    ("singly","singl")] |> Map.ofList
        map.TryFind(word)
    
    [<CompiledName("Appy")>]
    let apply(word: string) =    
        match (word |> specialWord) with
        | Some w -> Found(w)
        | None -> Next(word)

module PostSpecialWord =
    open StemmerFS.Dto
    open StemmerFS.Steps

    let private postInvariant(invariant: (string * bool)) =
        match invariant with
        | (word, true) -> word
        | (word, false) -> 
            word |> Step2.apply |> Step3.apply |> Step4.apply |> Step5.apply |> Step6.apply

    let apply(word: WordStatus) =
        match word with
        | Found(word) -> word
        | Next(word) -> word |> Step0.apply |> Step1.apply |> Rules.invariant |> postInvariant


