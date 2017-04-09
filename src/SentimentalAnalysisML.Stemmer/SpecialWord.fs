namespace  SentimentalAnalysisML.Stemmer

module SpecialWord =
    open  SentimentalAnalysisML.Stemmer.Dto

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
                    ("singly","singl")
                    ("sky"   ,"sky")
                    ("news"  ,"news")
                    ("howe"  ,"howe")
                    ("atlas" ,"atlas")
                    ("cosmos","cosmos")
                    ("bias"  ,"bias")
                    ("andes" ,"andes")
                  ] |> Map.ofList
        map.TryFind(word)
    
    [<CompiledName("Appy")>]
    let apply(word: string) =    
        match (word |> specialWord) with
        | Some w -> Found(w)
        | None -> Next(word)

module PostSpecialWord =
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer.Steps

    let private postInvariant(invariant: (string * bool)) =

        let capitalYbackToLowercase (str:string) =
            str.Replace("Y", "y")

        match invariant with
        | (word, true) -> word
        | (word, false) -> 
            word |> Step2.apply |> Step3.apply |> Step4.apply |> Step5.apply |> Step6.apply |> capitalYbackToLowercase

    let apply(word: WordStatus) =
        match word with
        | Found(word) -> word
        | Next(word) -> word |> Step0.apply |> Step1.apply |> Rules.invariant |> postInvariant


