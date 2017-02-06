module Tests

open Expecto
open SentimentalAnalysisML.Stemmer
[<Tests>]
let tests =
  testList "samples" [
    testCase "universe exists" <| fun _ ->
      let subject = Library.hello 42
      Expect.equal subject 42 "should equal 42"
  ]