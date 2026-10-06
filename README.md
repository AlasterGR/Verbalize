![Verbalize logo2](https://github.com/AlasterGR/Verbalize/assets/35509561/64d0719e-65bf-4204-aed4-5233fe056c12)

Verbalize is an app which utilizes Microsoft's Azure Cognitive Services to deliver text-to-speech transformation in a realistic and nuanced manner.


## Setting up your Azure key

Verbalize needs an [Azure AI Speech](https://learn.microsoft.com/azure/ai-services/speech-service/) key. The key is not stored in the code, so set it in one of two ways:

- **Environment variables:** set `VERBALIZE_SPEECH_KEY` to your key, and optionally `VERBALIZE_SPEECH_REGION` to your resource's region (the default is `westeurope`).
- **Settings file:** create `%APPDATA%\Verbalize\settings.json` containing:
  ```json
  { "SpeechKey": "your key", "SpeechRegion": "westeurope" }
  ```

Environment variables take priority over the settings file. Without a key, the app still opens but tells you that speaking, exporting sound and downloading voices will not work yet.

## Building and testing

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download). The app runs on Windows; building and testing also work on Linux and macOS.

```sh
dotnet build Verbalize.sln
dotnet test Verbalize.Core.Tests
dotnet run --project 2022TextToSpeech   # Windows only
```
