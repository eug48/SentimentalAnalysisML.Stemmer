namespace SentimentalAnalysisML.Stemmer.Tests


module Program =
    open Expecto
    
    [<EntryPoint>]
    let main argv =
        let ret = Tests.runTestsInAssembly defaultConfig argv
        ret