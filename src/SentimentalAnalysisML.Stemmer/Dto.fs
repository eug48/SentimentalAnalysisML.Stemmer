namespace SentimentalAnalysisML.Stemmer.Dto

type WordStatus =
| Found of string
| Next of string

type Either<'a,'b> = Choice<'a, 'b>

module Either = 
    let Left x: Either<'a,'b> = Choice1Of2 x
    let Right x: Either<'a,'b> = Choice2Of2 x
    let (|Left|Right|) = function Choice1Of2 x -> Left x | Choice2Of2 x -> Right x