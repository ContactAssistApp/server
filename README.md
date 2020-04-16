# CovidSafe Backend API

Contains source code for the backend service API, which provides data to CovidSafe client applications.

## Build Status

Build + Unit Test Pipeline:

[![Build status](https://dev.azure.com/msresearch/CovidSafe/_apis/build/status/Builds/CovidSafe-BuildTests)](https://dev.azure.com/msresearch/CovidSafe/_build/latest?definitionId=2388)

Compliance Pipeline:

[![Build status](https://dev.azure.com/msresearch/CovidSafe/_apis/build/status/Compliance/Backend-Compliance%20Assessment)](https://dev.azure.com/msresearch/CovidSafe/_build/latest?definitionId=2384)

Compilance Scans include:

* [BinSkim](https://github.com/Microsoft/binskim)
* [CredScan](https://secdevtools.azurewebsites.net/helpcredscan.html)
* PoliCheck
  * Scans source code for words/phrases which may be insensitive.
* [Roslyn Analyzers](https://secdevtools.azurewebsites.net/helpRoslynAnalyzers.html)

## Organization

### CovidSafe.API

Web API project which contains controllers for each endpoint.

### CovidSafe.DAL

Data Access Layer project which has the actual database interaction (repositories, etc.) and service layer classes.

### CovidSafe.Entities

Library with all entity types used across the other projects.
