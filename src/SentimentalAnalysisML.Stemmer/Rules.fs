namespace SentimentalAnalysisML.Stemmer

module Rules = 
    open System.Text.RegularExpressions
    open Microsoft.FSharpLu.Text
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentFS.Text.Regex
    open SentimentFS.Text

    [<Literal>]
    let V = "aeiouy"
    let vowels = sprintf "[%s]" V
    let consonant  = sprintf "[^%s]" V
    let nonVowelWXY = sprintf "[^%swxY]" V
    let doubles = [|"bb"; "dd"; "ff"; "gg"; "mm"; "nn"; "pp"; "rr"; "tt"|]
    let shortSyllable = sprintf "((%s%s%s)|(^%s%s))" consonant vowels nonVowelWXY vowels consonant

    let liEndings = [|"cli"; "dli"; "eli"; "gli"; "hli"; "kli"; "mli"; "nli"; "rli"; "tli"|]

    let rVc = sprintf "^%s*%s+%s" consonant vowels consonant

    /// Returns string
    ///
    /// ## Parameters
    ///  - `word` - string
    /// ## Description
    /// R1 is the region after the first non-vowel following a vowel, or is the null
    /// region at the end of the word if there is no such non-vowel.
    [<CompiledName("R1")>]
    let r1(word: string) =
        match word with
        | FirstMatch "^(gener|commun|arsen)" matched -> 
            word |> Text.replacePrefix (matched, "")
        | FirstMatch rVc matched  -> 
            word |> Text.replacePrefix (matched, "")
        | _ -> ""

    [<CompiledName("IsShort")>]
    let isShort(word: string) =
        r1(word) = "" && Regex.IsMatch(word, shortSyllable)
    
    let private foundSuffixInR1(suffix: string) (replacement: string) (word: string) =
        match word with
        | FirstMatch suffix _ -> 
            Found(word |> Text.replaceSuffix(suffix, replacement))
        | _ ->  Found(word)

    [<CompiledName("ReplaceR1Suffix")>]
    let replaceR1Suffix (suffix: string) (replacement: string) (word: string) =
        match word with
        | FirstMatch suffix _ ->
            word |> foundSuffixInR1 suffix replacement
        | _ -> Next(word)

    [<CompiledName("Invariant")>]
    let invariant(word:string) = 
        (word, [| "inning"; "outing"; "canning"; "herring"; "earring"; "proceed"; "exceed"; "succeed" |] |> Array.exists(fun x -> x = word))

    let private normalR1(word: string) =
        match word with 
        | FirstMatch rVc result -> 
            word |> Text.replacePrefix (result, "")
        | _ -> ""

    [<CompiledName("R2")>]
    let r2(word: string) = 
        word |> r1 |> normalR1


