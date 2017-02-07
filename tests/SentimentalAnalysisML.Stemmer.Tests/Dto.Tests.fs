namespace StemmerFS.Tests

module Dto =
    open Expecto
    open SentimentalAnalysisML.Stemmer.Dto
    open SentimentalAnalysisML.Stemmer.Dto.Either
    [<Tests>]
    let either =
        testList "Dto" [
            testList "Either" [
                testCase "Left" <| fun _ -> 
                    let subject: Either<int, string> = Choice1Of2(2)
                    Expect.equal subject (Left 2) "should be Left"
                testCase "Right" <| fun _ -> 
                    let subject: Either<int, string> = Choice2Of2("2")
                    Expect.equal subject (Right "2") "should be Right"
            ]
        ]