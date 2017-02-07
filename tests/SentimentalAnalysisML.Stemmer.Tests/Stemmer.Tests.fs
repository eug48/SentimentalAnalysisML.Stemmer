namespace SentimentalAnalysisML.Stemmer.Tests

module Stemmer =
    open SentimentalAnalysisML.Stemmer
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer.Dto.Either
    open Expecto

    [<Tests>]
    let tests = 
        testList "stem" [
            testCase "Hello" <| fun _ -> 
                let subject = Stemmer.stem("Hello")
                Expect.equal subject ("hello") "should equal hello"
            testCase "hElLo" <| fun _ -> 
                let subject = Stemmer.stem("hElLo")
                Expect.equal subject ("hello") "should equal hello"
            testCase "capabilities" <| fun _ -> 
                let subject = Stemmer.stem("capabilities")
                Expect.equal subject ("capabl") "should equal capabl"
        ]