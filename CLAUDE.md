# Verbalize

Windows Forms desktop app (C#, .NET 10) that turns text into speech with Azure AI Speech.

## Projects

- `2022TextToSpeech/` — the Windows Forms app (`net10.0-windows`). Runs on Windows only.
- `Verbalize.Core/` — logic with no Windows or UI dependency (`net10.0`): building and reading SSML, converting and querying the voice list. Put new non-UI logic here so it can be unit tested on any machine.
- `Verbalize.Core.Tests/` — xUnit tests for `Verbalize.Core`.

## Build and test

```sh
dotnet build Verbalize.sln
dotnet test Verbalize.Core.Tests
```

Both work on Linux; the app sets `EnableWindowsTargeting`. Cloud sessions install the .NET 10 SDK automatically through `.claude/hooks/session-start.sh`. The app itself can only be run on Windows.

## Coding conventions

These apply to all code written or changed in this repo, tests included. Generated files (`*.Designer.cs`) are exempt.

- **Comment every logic block.** Put a single-line comment above each logic block that a non-technical reader could follow. It must be concise, plain-language and free of jargon, written exactly as `//  <Sentence>.`: two slashes, two spaces, one sentence starting with a capital and ending with a full stop.
  ```csharp
  //  Ask the user to pick a file, and stop if they cancel.
  if (openFileDialog.ShowDialog() != DialogResult.OK) { return; }
  ```
- **Summarise every method.** Every method, whatever its visibility, has an XML `/// <summary>` saying in plain language what it does, plus `<param>` and `<returns>` where they apply.
- **Refactor conservatively.** Refactors must not change behaviour. Keep side effects, ordering and the existing quirks unless the change is explicitly a bug fix. Put behaviour fixes in separate commits from refactors, and call out any intended behaviour change.
- **Test with unit tests.** Cover new or changed logic with unit tests, even simple checks, and verify by running `dotnet test` rather than ad-hoc checks. When a test pins a known limitation, say so in its summary.
- Follow standard .NET practices, and match the naming and style of the surrounding code.

## Notes

- `Class_SSML_Builder.cs` is unfinished work and is excluded from the build in the `.csproj`.
- The Azure key and region are read from the `VERBALIZE_SPEECH_KEY` / `VERBALIZE_SPEECH_REGION` environment variables or `%APPDATA%\Verbalize\settings.json` (see `SpeechCredentialsResolver` and the README). Never put keys in the code. The keys that were once committed are still in git history and are due to be rotated.
