[![Issue Stats](http://issuestats.com/github/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer/badge/issue)](http://issuestats.com/github/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer)
[![Issue Stats](http://issuestats.com/github/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer/badge/pr)](http://issuestats.com/github/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer)

# SentimentalAnalysisML.Stemmer

An English ([Porter2](http://snowballstem.org/algorithms/english/stemmer.html)) stemming implementation in F#.

> In linguistic morphology and information retrieval, __stemming__ is the process of reducing inflected (or sometimes derived) words to their word stem, base or root form—generally a written word form. The stem need not be identical to the morphological root of the word; it is usually sufficient that related words map to the same stem, even if this stem is not in itself a valid root. - [Wikipedia](https://en.wikipedia.org/wiki/Stemming)

## Based on elixir implementation 

[stemmer](https://github.com/fredwu/stemmer)

## Requirements

SentimentalAnalysisML.Stemmer requires .Net 4.6.1

## Usage

```fsharp
    open SentimentalAnalysisML.Stemmer
    let result = Stemmer.stem "capabilities" // "capabl"

```  

## Build Status

Mono | .NET | MyGet
---- | ---- | ----
[![Build Status](https://travis-ci.org/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer.svg?branch=master)](https://travis-ci.org/SentimentalAnalysisML/SentimentalAnalysisML.Stemmer)| [![Build status](https://ci.appveyor.com/api/projects/status/d7lk960dgq7gpvqa?svg=true)](https://ci.appveyor.com/project/dominikus1993/sentimentalanalysisml-stemmer) | [![sentimentalanalysisml MyGet Build Status](https://www.myget.org/BuildSource/Badge/sentimentalanalysisml?identifier=45a497f1-9d16-4476-9f21-741ccf803606)](https://www.myget.org/)

## BuildHistory
[![Build history](https://buildstats.info/appveyor/chart/dominikus1993/sentimentalanalysisml-stemmer)](https://ci.appveyor.com/project/dominikus1993/sentimentalanalysisml-stemmer/history)

## Maintainer(s)

- [@dominikus1993](https://github.com/dominikus1993)