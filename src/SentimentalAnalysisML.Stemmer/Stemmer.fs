namespace SentimentalAnalysisML.Stemmer

module Stemmer =
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentFS.Text.Text
    open StemmerFS
    open StemmerFS.Utils
    
    [<CompiledName("Stem")>]
    let rec stem =    
        let localstem (word: string) : string =
            if word.Length <= 2 then 
                word
            else 
                word |> toLower |> SpecialWord.apply |> PostSpecialWord.apply
        Memonization.memoize localstem