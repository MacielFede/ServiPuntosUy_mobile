# ServiPuntos.uy Mobile App

## First setup

Execute the file `./setup-hooks.sh` to setup the git pre-commit hook

## Google Maps API Key Configuration

This project requires a Google Maps API key to be injected into the Android build process.

### Where to include the API key

You must provide the Google Maps API key as an Android manifest placeholder named `GEO_API_KEY`.
Go into the `AndroidManifest.xml` file and search for `${GEO_API_KEY}` and replace it with the api key.

### !Important!
Before commiting to the repo, revert your changes to the `AndroidManifest.xml` file, otherwise you would likely not be able to commit.


## How to build the project

```bash
dotnet build -p:Configuration=Debug -p:TargetFramework=net9.0-android
```