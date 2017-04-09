namespace SentimentalAnalysisML.Stemmer.Steps

module Step0 = 
    open System.Text.RegularExpressions
    open SentimentalAnalysisML.Stemmer.Rules
    open SentimentalAnalysisML.Stemmer.Utils

    [<CompiledName("TrimEndApostrophe")>]
    let rec trimEndApostrophe(word:string) =  
        if word.EndsWith("'") then
            word.Substring(0, word.Length - 1)
        else
            word
    
    [<CompiledName("TrimStartApostrophe")>]
    let trimStartApostrophe(word:string) =
        if word.StartsWith("'") then
            word.Substring(1)
        else
            word


    [<CompiledName("RemoveSApostrophe")>]
    let removeSApostrophe(word: string) =
        if word.EndsWith("'s") then
            word.Substring(0, word.Length - 2)
        else
            word

    [<CompiledName("MarkConsonantY")>]
    let markConsonantY(word: string) =
        if word.Contains("y") then
            Regex.Replace(word, (sprintf "^y|(%s)y" vowels), "$1Y")
        else 
            word

    [<CompiledName("Apply")>]
    let apply word = 
        let result = word |> trimStartApostrophe |> trimEndApostrophe |> removeSApostrophe |> markConsonantY
        StepsUtils.log "Step 0: %s" result
        result

module Step1 =
    open SentimentalAnalysisML.Stemmer.Dto
    open System.Text.RegularExpressions
    open SentimentalAnalysisML.Stemmer.Rules
    open SentimentalAnalysisML.Stemmer.Utils

    
    [<CompiledName("ReplaceSses")>]
    let replaceSses(word: string) = 
        if word.EndsWith("sses") then
            Found(word.Substring(0, word.Length - 2))
        else 
            Next(word)

    [<CompiledName("ReplaceIedAndIes")>]
    let replaceIedAndIes(word: string) = 
        if word.EndsWith("ied") || word.EndsWith("ies") then
            let result = if word.Length > 4 then 
                            word.Substring(0, word.Length - 2)
                         else 
                            word.Substring(0, word.Length - 1)
            Found(result)
        else 
            Next(word)
        

    [<CompiledName("RemoveS")>]
    let removeS(word: string) =
        if Regex.IsMatch(word, (sprintf "(%s).+s$" vowels)) then
            Found(word.Substring(0, word.Length - 1))
        else
            Next(word)

    [<CompiledName("LeaveUSandSS")>]
    let leaveUSandSS(word: string) =
        if word.EndsWith("ss") then
            Found(word)
        elif word.EndsWith("us") then
            Found(word)
        else 
            Next(word)
    
    [<CompiledName("ReplaceSuffix")>]
    let replaceSuffix(word: string) = StepsUtils.actionReducer word [replaceSses; replaceIedAndIes; leaveUSandSS; removeS]
     
    [<CompiledName("Apply")>]
    let apply(word: string) =
        let result = word |> replaceSuffix
        StepsUtils.log "Step 1: %s" result
        result

module Step2 =
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentalAnalysisML.Stemmer.Utils.StepsUtils
    open SentimentalAnalysisML.Stemmer.Rules
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer
    open System.Text.RegularExpressions
    open SentimentalAnalysisML.Utils
    open SentimentalAnalysisML.Utils.Regex
   
    [<CompiledName("postRemoveEdEdlyIngIngly")>]
    let postRemoveEdEdlyIngIngly(word: string) = 
        if Text.endsWith (word) ([|"at"; "bi"; "iz"|]) then
            word + "e"
        elif Text.endsWith (word) (doubles) then
            word.Substring(0, word.Length - 1)
        elif Rules.isShort(word) then
            word + "e"
        else
            word
    
    [<CompiledName("RemoveEdEdlyIngIngly")>]
    let removeEdEdlyIngIngly(word: string) =
        let rule = sprintf "(%s.*)(ingly|edly|ing|ed)$" Rules.vowels;
        if Regex.IsMatch(word, rule) then
            Found(Regex.Replace(word, rule,"$1") |> postRemoveEdEdlyIngIngly)
        else 
            Next(word)

    [<CompiledName("ReplaceEedEddlyInR1")>]
    let replaceEedEddlyInR1(word: string) = 
        if Text.endsWith(Rules.r1(word))([|"eedly"; "eed"|]) then
            Found((word |> Text.replaceSuffix ("eedly", "ee") |> Text.replaceSuffix ("eed", "ee")))
        else 
            Found(word)

    [<CompiledName("ReplaceEedEedly")>]
    let replaceEedEedly(word: string) = 
        if Text.endsWith(word)([|"eedly"; "eed"|]) then  
            word |> replaceEedEddlyInR1
        else
            Next(word)

    [<CompiledName("ReplaceSuffix")>]
    let replaceSuffix(word: string) =
        match word |> replaceEedEedly with
        | Next(word) ->
            match word |> removeEdEdlyIngIngly with
            | Next(word) -> word
            | Found(word) -> word
        | Found(word) -> word

    [<CompiledName("ReplaceSuffixY")>]
    let replaceSuffixY(word: string) =
        match word.ToLower() with
        | SuffixMatch (sprintf ".+%sy" Rules.consonant) _ ->
            word.Substring(0, word.Length - 1) + "i"
        | _ -> word

    [<CompiledName("Apply")>]
    let apply(word: string) = 
        let result = word |> replaceSuffix |> replaceSuffixY
        log "Step 2: %s" result
        result

module Step3 =
    open SentimentalAnalysisML.Stemmer
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentalAnalysisML.Utils
    open SentimentalAnalysisML.Utils.Regex
   
    let private replaceSuffixOgi(word: string) =
        if (Text.endsWith word [|(Rules.r1(word))|]) && (Text.endsWith word [|"logi"|]) then
            Found(word |> Text.replaceSuffix("ogi", "og"))
        else
            Next(word)
    
    let private replaceSuffixLi(word: string) = 
        if (Text.endsWith (Rules.r1(word)) [|"li"|]) && (Text.endsWith word (Rules.liEndings)) then
            Found(word |> Text.replaceSuffix("li", "") )
        else 
            Next(word)

    [<CompiledName("ReplaceSuffixInR1")>]
    let replaceSuffixInR1 =
        let actions = [ Rules.replaceR1Suffix ("ization") ("ize")
                        Rules.replaceR1Suffix ("ational") ("ate")
                        Rules.replaceR1Suffix ("fulness") ("ful") 
                        Rules.replaceR1Suffix ("ousness") ("ous") 
                        Rules.replaceR1Suffix ("iveness") ("ive") 
                        Rules.replaceR1Suffix ("tional") ("tion") 
                        Rules.replaceR1Suffix ("biliti") ("ble") 
                        Rules.replaceR1Suffix ("lessli") ("less") 
                        Rules.replaceR1Suffix ("entli") ("ent")
                        Rules.replaceR1Suffix ("ation") ("ate") 
                        Rules.replaceR1Suffix ("alism") ("al") 
                        Rules.replaceR1Suffix ("aliti") ("al") 
                        Rules.replaceR1Suffix ("ousli") ("ous") 
                        Rules.replaceR1Suffix ("iviti") ("ive") 
                        Rules.replaceR1Suffix ("fulli") ("ful") 
                        Rules.replaceR1Suffix ("enci") ("ence") 
                        Rules.replaceR1Suffix ("anci") ("ance") 
                        Rules.replaceR1Suffix ("abli") ("able") 
                        Rules.replaceR1Suffix ("izer") ("ize") 
                        Rules.replaceR1Suffix ("ator") ("ate") 
                        Rules.replaceR1Suffix ("alli") ("al") 
                        Rules.replaceR1Suffix ("bli") ("ble")
                        replaceSuffixOgi
                        replaceSuffixLi ]

        fun word -> StepsUtils.actionReducer word actions

    [<CompiledName("Apply")>]
    let apply(word: string) = 
        let result = word |> replaceSuffixInR1
        StepsUtils.log "Step 3: %s" result
        result

module Step4 =
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer
    open SentimentalAnalysisML.Utils
    let private replaceSuffixAtiveInR2(word: string) =
        if (Text.endsWith (Rules.r2(word)) ([|"ative"|])) then
            Found((word |> Text.replaceSuffix ("ative", "") ))
        else
            Next(word)
    
    [<CompiledName("Apply")>]
    let apply = 
        let actions = [
            Rules.replaceR1Suffix "ational" "ate"
            Rules.replaceR1Suffix "tional" "tion"
            Rules.replaceR1Suffix "alize" "al"
            Rules.replaceR1Suffix "icate" "ic"
            Rules.replaceR1Suffix "iciti" "ic"
            Rules.replaceR1Suffix "ical" "ic"
            Rules.replaceR1Suffix "ness" ""
            Rules.replaceR1Suffix "ful" ""
            replaceSuffixAtiveInR2
        ]
        fun word ->
            let result = StepsUtils.actionReducer word actions
            StepsUtils.log "Step 4: %s" result
            result

module Step5 = 
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentalAnalysisML.Stemmer.Utils.StepsUtils
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer
    open SentimentalAnalysisML.Utils
    open SentimentalAnalysisML.Utils.Regex
    
    let private removeSuffixIon(word:string) =  
        word |> StepsUtils.removeSuffix 
            (fun w -> (Text.endsWith(Rules.r2(w))([|"ion"|])) && (Text.endsWith(word)([|"sion"; "tion"|])))
            (fun w -> w |> Text.replaceSuffix("ion", ""))

    let private removeSuffixinR2(word: string)(suffix: string) = 
        word |> StepsUtils.removeSuffix 
            (fun w -> Text.endsWith(Rules.r2(w))([|suffix|]))
            (fun w -> w |> Text.replaceSuffix(suffix, ""))
    
    let private removeSuffix(word: string) = 
        match word with
        | SuffixMatch "(al|ance|ence|er|ic|able|ible|ant|ement|ment|ent|ism|ate|iti|ous|ive|ize)" result -> 
            removeSuffixinR2 word result
        | _ -> Next(word)
    
    [<CompiledName("Apply")>]
    let apply = 
        let actions = [
            removeSuffix
            removeSuffixIon
        ]
        fun (word: string) ->
            let result = StepsUtils.actionReducer word actions
            log "Step 5: %s" result
            result

module Step6 =
    open SentimentalAnalysisML.Stemmer.Utils
    open SentimentalAnalysisML.Utils
    open SentimentalAnalysisML.Utils.Regex
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer
    open System.Text.RegularExpressions
    
    // TODO: should be refactored
    let private removeSuffixInR2(word: string) = 
        let r2 = Rules.r2 word
        if (Text.endsWith(r2)([|"e"|])) then
            word |> Text.replaceSuffix("e", "")
        elif (Text.endsWith(Rules.r1 word)([|"e"|])) && not (Regex.IsMatch(word, (sprintf "%se$" Rules.shortSyllable))) then
            word |> Text.replaceSuffix("e", "")
        elif (Text.endsWith(r2)([|"l"|])) && (Text.endsWith(word)([|"ll"|])) then
            word |> Text.replaceSuffix("l", "")
        else
            word

    [<CompiledName("Apply")>]
    let apply(word: string) = 
        let result = word |> removeSuffixInR2
        StepsUtils.log "Step 6: %s" result
        result
        