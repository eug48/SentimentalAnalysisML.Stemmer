namespace SentimentalAnalysisML.Stemmer.Utils 

module StepsUtils =
    open SentimentalAnalysisML.Stemmer.Dto
    open System.Text.RegularExpressions

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

    let (|SuffixMatch|_|) pattern input =
        let m = Regex.Match(input, pattern + "$")
        if m.Success then 
            Some(m.Value)
        else 
            None


    let log _s1 _s2 =
        //printfn "%s %s" _s1 _s2
        ()