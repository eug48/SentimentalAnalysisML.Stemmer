namespace SentimentalAnalysisML.Stemmer.Utils 

module StepsUtils =
    open SentimentalAnalysisML.Stemmer.Dto

    [<CompiledName("ActionReducer")>]
    let rec actionReducer word actions = 
        match actions with
            | [] -> word
            | head::tail -> 
                match head(word) with
                | Found(result) -> result
                | Next(_) -> actionReducer word tail
    
    [<CompiledName("RemoveSuffix")>]
    let removeSuffix (filter: string -> bool) (replaceF: string -> string) (word: string) =
        if filter(word) then
            Found(replaceF(word))
        else
            Next(word)