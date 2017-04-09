namespace SentimentalAnalysisML.Stemmer.Tests.Steps

module Step0 = 
    open Expecto
    open SentimentalAnalysisML.Stemmer.Steps
    open SentimentalAnalysisML.Stemmer.Dto

    [<Tests>]
    let test = 
        testList "Step0" [
            testList "markConsonantY" [
                testCase "when word has y at first position" <| fun _ ->
                    let subject = Step0.markConsonantY "youth"
                    Expect.equal subject "Youth" "should equal Youth"
                testCase "when word has y after vowel" <| fun _ ->
                    let subject = Step0.markConsonantY "boyish"
                    Expect.equal subject "boYish" "should equal boYish"
                testCase "when word has no y after vowel" <| fun _ ->
                    let subject = Step0.markConsonantY "flying"
                    Expect.equal subject "flying" "should equal flying"
                testCase "when word has no y" <| fun _ ->
                    let subject = Step0.markConsonantY "test"
                    Expect.equal subject "test" "should equal test"
            ]
            testList "removeSApostrophe" [
                testCase "when word end with 's" <| fun _ ->
                    let subject = Step0.removeSApostrophe "test's"
                    Expect.equal subject "test" "should equal test"
                testCase "when word no end with 's" <| fun _ ->
                    let subject = Step0.removeSApostrophe "test"
                    Expect.equal subject "test" "should equal test"
            ]
            testList "TrimApostrophes" [
                testCase "when word has no Apostrophes at middle position" <| fun _ ->
                    let subject = "te'st" |> Step0.trimEndApostrophe |> Step0.trimStartApostrophe
                    Expect.equal subject "te'st" "should equal te'st"
            ]
            testList "Apply" [
                testCase "youth's'" <| fun _ ->
                    let subject = Step0.apply "youth's'"
                    Expect.equal subject "Youth" "should equal Youth"
            ]
        ]
    [<Tests>]
    let test2 = 
        testList "Step1" [
            testList "removeS" [
                testCase "gas" <| fun _ ->
                    let subject = Step1.removeS "gas"
                    Expect.equal subject (Next("gas")) "should equal gas"
                testCase "this" <| fun _ ->
                    let subject = Step1.removeS "this"
                    Expect.equal subject (Next("this")) "should equal this"
                testCase "gaps" <| fun _ ->
                    let subject = Step1.removeS "gaps"
                    Expect.equal subject (Found("gap")) "should equal gap"
                testCase "kiwis" <| fun _ ->
                    let subject = Step1.removeS "kiwis"
                    Expect.equal subject (Found("kiwi")) "should equal kiwi"
            ]
            testList "leaveUSandSS" [
                testCase "abyss" <| fun _ ->
                    let subject = Step1.leaveUSandSS "abyss"
                    Expect.equal subject (Found("abyss")) "should equal abyss"
                testCase "us" <| fun _ ->
                    let subject = Step1.leaveUSandSS "us"
                    Expect.equal subject (Found("us")) "should equal us"
                testCase "gaps" <| fun _ ->
                    let subject = Step1.leaveUSandSS "gap"
                    Expect.equal subject (Next("gap")) "should equal gap"
            ]
            testList "replaceIedAndIes" [
                testCase "tied" <| fun _ ->
                    let subject = Step1.replaceIedAndIes "tied"
                    Expect.equal subject (Found("tie")) "should equal tie"
                testCase "ties" <| fun _ ->
                    let subject = Step1.replaceIedAndIes "ties"
                    Expect.equal subject (Found("tie")) "should equal tie"
                testCase "cries" <| fun _ ->
                    let subject = Step1.replaceIedAndIes "cries"
                    Expect.equal subject (Found("cri")) "should equal cri"
                testCase "test" <| fun _ ->
                    let subject = Step1.replaceIedAndIes "test"
                    Expect.equal subject (Next("test")) "should equal test"
            ]
            testList "replaceSses" [
                testCase "actresses" <| fun _ ->
                    let subject = Step1.replaceSses "actresses"
                    Expect.equal subject (Found("actress")) "should equal actress"
                testCase "test" <| fun _ ->
                    let subject = Step1.replaceSses "test"
                    Expect.equal subject (Next("test")) "should equal test"
            ]
            testList "replaceSuffix" [
                testCase "abyss" <| fun _ ->
                    let subject = Step1.replaceSuffix "abyss"
                    Expect.equal subject "abyss" "should equal abyss"
            ]
        ]

    [<Tests>]
    let test3 = 
        testList "Step2" [
            testList "postRemoveEdEdlyIngIngly" [
                testCase "when word end with at or bi or iz" <| fun _ ->
                    let subject = Step2.postRemoveEdEdlyIngIngly "plat"
                    Expect.equal subject ("plate") "should equal plate"
                testCase "when word has doubles" <| fun _ ->
                    let subject = Step2.postRemoveEdEdlyIngIngly "add"
                    Expect.equal subject ("ad") "should equal ad"
                testCase "when word is short" <| fun _ -> 
                    let subject = Step2.postRemoveEdEdlyIngIngly "on"
                    Expect.equal subject "one" "should equal one"
                testCase "when word is other" <| fun _ -> 
                    let subject = Step2.postRemoveEdEdlyIngIngly "awesome"
                    Expect.equal subject "awesome" "should equal awesome"
            ]
            testList "removeEdEdlyIngIngly" [
                testCase "luxuriating" <| fun _ ->
                    let subject = Step2.removeEdEdlyIngIngly "luxuriating"
                    Expect.equal subject (Found("luxuriate")) "should be Found and equal luxuriate"
                testCase "hopping" <| fun _ ->
                    let subject = Step2.removeEdEdlyIngIngly "hopping"
                    Expect.equal subject (Found("hop")) "should be Found and equal hop"
                testCase "hoping" <| fun _ -> 
                    let subject = Step2.removeEdEdlyIngIngly "hoping"
                    Expect.equal subject (Found("hope")) "should be Found and equal hope"
                testCase "add" <| fun _ ->
                    let subject = Step2.removeEdEdlyIngIngly "add"
                    Expect.equal subject (Next("add")) "should be Next and equal add"
                testCase "on" <| fun _ -> 
                    let subject = Step2.removeEdEdlyIngIngly "on"
                    Expect.equal subject (Next("on")) "should be Next and equal on"
            ]
            testList "replaceEedEddlyInR1" [
                testCase "proceed" <| fun _ ->
                    let subject = Step2.replaceEedEddlyInR1 "proceed"
                    Expect.equal subject (Found("procee")) "should be Found and equal procee"
                testCase "proceedly" <| fun _ ->
                    let subject = Step2.replaceEedEddlyInR1 "proceedly"
                    Expect.equal subject (Found("procee")) "should be Found and equal procee"
                testCase "need" <| fun _ -> 
                    let subject = Step2.replaceEedEddlyInR1 "need"
                    Expect.equal subject (Found("need")) "should be Found and equal need"
            ]
            testList "replaceEedEedly" [
                testCase "proceed" <| fun _ ->
                    let subject = Step2.replaceEedEedly "proceed"
                    Expect.equal subject (Found("procee")) "should be Found and equal procee"
                testCase "proceedly" <| fun _ ->
                    let subject = Step2.replaceEedEedly "proceedly"
                    Expect.equal subject (Found("procee")) "should be Found and equal procee"
                testCase "need" <| fun _ -> 
                    let subject = Step2.replaceEedEedly "need"
                    Expect.equal subject (Found("need")) "should be Found and equal need"
            ]
            testList "replaceSuffix" [
                testCase "bleed" <| fun _ ->
                    let subject = Step2.replaceSuffix "bleed"
                    Expect.equal subject "bleed" "should be bleed"
                testCase "proceedly" <| fun _ ->
                    let subject = Step2.replaceSuffix "proceedly"
                    Expect.equal subject "procee" "should be procee"
            ]
            testList "replaceSuffixY" [
                testCase "cry" <| fun _ -> 
                    let subject = Step2.replaceSuffixY "cry"
                    Expect.equal subject "cri" "should equal cri"
                testCase "say" <| fun _ -> 
                    let subject = Step2.replaceSuffixY "say"
                    Expect.equal subject "say" "should equal say"
                testCase "by" <| fun _ -> 
                    let subject = Step2.replaceSuffixY "by"
                    Expect.equal subject "by" "should equal by"
            ]
        ]
    [<Tests>]
    let test4 = 
        testList "Step3" [
            testList "replaceSuffixInR1" [
                testCase "sensational" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "sensational"
                    Expect.equal subject "sensate" "should equal sensate"
                testCase "sentenci" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "sentenci"
                    Expect.equal subject "sentence" "should equal sentence"
                testCase "entranci" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "entranci"
                    Expect.equal subject "entrance" "should equal entrance"
                testCase "pocketabli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "pocketabli"
                    Expect.equal subject "pocketable" "should equal pocketable"
                testCase "momentli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "momentli"
                    Expect.equal subject "moment" "should equal moment"
                testCase "nationalization" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "nationalization"
                    Expect.equal subject "nationalize" "should equal nationalize"
                testCase "acceleration" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "acceleration"
                    Expect.equal subject "accelerate" "should equal accelerate"
                testCase "accelerator" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "accelerator"
                    Expect.equal subject "accelerate" "should equal accelerate"
                testCase "usefulness" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "usefulness"
                    Expect.equal subject "useful" "should equal useful"
                testCase "mopli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "mopli"
                    Expect.equal subject "mopli" "should equal mopli"
                testCase "geologi" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "geologi"
                    Expect.equal subject "geolog" "should equal geologi"
                testCase "greatli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "greatli"
                    Expect.equal subject "great" "should equal great"
                testCase "masterfulli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "masterfulli"
                    Expect.equal subject "masterful" "should equal masterful"
                testCase "generousli" <| fun _ -> 
                    let subject = Step3.replaceSuffixInR1 "generousli"
                    Expect.equal subject "generous" "should equal generous"
            ]
        ]
    
    [<Tests>]
    let tests5 = 
        testList "Step4" [
            testCase "proportional" <| fun _ -> 
                let subject = Step4.apply "proportional"
                Expect.equal subject "proportion" "should equal proportion"
            testCase "duplicate" <| fun _ -> 
                let subject = Step4.apply "duplicate"
                Expect.equal subject "duplic" "should equal duplic"
            testCase "dupliciti" <| fun _ -> 
                let subject = Step4.apply "dupliciti"
                Expect.equal subject "duplic" "should equal duplic"
            testCase "duplical" <| fun _ -> 
                let subject = Step4.apply "duplical"
                Expect.equal subject "duplic" "should equal duplic"
            testCase "colourful" <| fun _ -> 
                let subject = Step4.apply "colourful"
                Expect.equal subject "colour" "should equal colour"
            testCase "eagerness" <| fun _ -> 
                let subject = Step4.apply "eagerness"
                Expect.equal subject "eager" "should equal eager"
            testCase "negative" <| fun _ -> 
                let subject = Step4.apply "negative"
                Expect.equal subject "negative" "should equal negative"
            testCase "imaginative" <| fun _ -> 
                let subject = Step4.apply "imaginative"
                Expect.equal subject "imagin" "should equal imagin"
        ]

    [<Tests>]
    let tests6 = 
        testList "Step5" [
            testCase "national" <| fun _ -> 
                let subject = Step5.apply "national"
                Expect.equal subject "nation" "should equal nation"
            testCase "association" <| fun _ -> 
                let subject = Step5.apply "association"
                Expect.equal subject "associat" "should equal associat"
            testCase "apprehension" <| fun _ -> 
                let subject = Step5.apply "apprehension"
                Expect.equal subject "apprehens" "should equal apprehens"
            testCase "concepcion" <| fun _ -> 
                let subject = Step5.apply "concepcion"
                Expect.equal subject "concepcion" "should equal concepcion"
            testCase "addition" <| fun _ -> 
                let subject = Step5.apply "addition"
                Expect.equal subject "addit" "should equal addit"
            testCase "agreement" <| fun _ -> 
                let subject = Step5.apply "agreement"
                Expect.equal subject "agreement" "should equal agreement"
        ]

    [<Tests>]
    let tests7 = 
        testList "Step6" [
            testCase "conceive" <| fun _ -> 
                let subject = Step6.apply "conceive"
                Expect.equal subject "conceiv" "should equal conceiv"
            testCase "move" <| fun _ -> 
                let subject = Step6.apply "move"
                Expect.equal subject "move" "should equal move"
            testCase "momoie" <| fun _ -> 
                let subject = Step6.apply "momoie"
                Expect.equal subject "momoi" "should equal momoi"
            testCase "moe" <| fun _ -> 
                let subject = Step6.apply "moe"
                Expect.equal subject "moe" "should equal moe"
            testCase "daniell" <| fun _ -> 
                let subject = Step6.apply "daniell"
                Expect.equal subject "daniel" "should equal daniel"
            testCase "doll" <| fun _ -> 
                let subject = Step6.apply "doll"
                Expect.equal subject "doll" "should equal doll"
            testCase "mail" <| fun _ -> 
                let subject = Step6.apply "mail"
                Expect.equal subject "mail" "should equal mail"
        ]