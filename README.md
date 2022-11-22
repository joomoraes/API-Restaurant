# API Restaurant .NET C#

C# library project template

[![super linter][super-linter-img]][super-linter-url]
[![test][test-img]][test-url]
[![semantic][semantic-img]][semantic-url]
<p align="center">
  <img alt="GitHub language count" src="https://img.shields.io/github/languages/count/joomoraes/API-Restaurant">

  <img alt="Repository size" src="https://img.shields.io/github/repo-size/joomoraes/API-Restaurant">
  
  <a href="https://github.com/joomoraes/API-Restaurant/commits/main">
    <img alt="GitHub last commit" src="https://img.shields.io/github/last-commit/joomoraes/API-Restaurant">
  </a>

  <img alt="Packagist" src="https://img.shields.io/badge/License-MIT-green.svg">
</p>



This package publishes all versions of the [CloudEvents][] Schema JSON spec

    .
    ├── LICENSE
    ├── program.cs
    └── schemas
        ├── latest
        │   └── appsettings.json
        └── 1.0
            └── appsettings.json

## How

``` js
// HTTP REQUESTS
var GET = require('https://localhost:44372/Restaurant/Text?text=Restaurant')
var DELETE = require('https://localhost:44372/Restaurant/636d18bdb8688cbef378c0ab')
var GET = require('https://localhost:44372/Restaurant/Top3')
var PATCH = require('https://localhost:44372/Restaurant/6375535d29545307a4e663c0/review')
var GET = require('https://localhost:44372/Restaurant?name=Dec')
var PUT = require('https://localhost:44372/Restaurant/636d18bdb8688cbef378c0ab')
var GET = require('https://localhost:44372/Restaurant/636d18bdb8688cbef378c0ab')
var GET = require('https://localhost:44372/Restaurant/All')
var POST = require('https://localhost:44372/Restaurant')
/** schemas is an object with the following shape:
  {
    latest: ...
    1.0: ...
  }
*/
```

  [CloudEvents]: https://cloudevents.io/

----
> Author: [João Pedro](https://github.com/joomoraes) &bull;
