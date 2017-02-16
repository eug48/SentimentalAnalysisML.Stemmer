namespace SentimentalAnalysisML.Stemmer

module Stemmer =
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Utils.Text
    open SentimentalAnalysisML.Utils
    open SentimentalAnalysisML.Stemmer
    
    [<CompiledName("Stem")>]
    let rec stem =    
        let localstem = fun (word: string) ->
            if word.Length <= 2 then 
                word
            else 
                word |> toLower |> SpecialWord.apply |> PostSpecialWord.apply
        Memonization.memoize localstem