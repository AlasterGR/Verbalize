# UI plan

A plan for reworking Verbalize's window around Joe Natoli's UX principles. It uses the same nine principles as BlackwoodManor (`BlackwoodManor.Wasm/orchestration/NATOLI_PRINCIPLES.md`), adapted to a Windows desktop app. Work through the steps in order, one small step per commit, and check the real app after each one.

## The user's goal

> Turn my text into natural speech, listen to it, and save it as a sound file.

Every element on screen is judged against that sentence.

## The nine principles, and how to check them here

Each principle has a check that passes or fails. A UI change is not done while it fails a principle it touches.

| | Principle | Check in Verbalize |
|---|---|---|
| P1 | **Progressive disclosure:** show only what is needed now. | In each situation (no text, offline, no key, speaking), every control that cannot work is either hidden or disabled *with a visible reason*. |
| P2 | **Reduce the cost of use.** | Speaking the text takes one click. The main window at rest can be scanned in about 3 seconds. |
| P3 | **Forgiveness:** prevent, then warn, then undo. | Every action that discards something (Clear, Load over unsaved text, overwriting a file) can be undone, or is confirmed first. |
| P4 | **Feedback and trust:** tell the truth about what happened. | Every action shows that it started and how it ended. Cutting the internet mid-action gives a clear message, never silence. |
| P5 | **Context of use.** | The window works from small laptop screens to 4K, at 100–200% scaling. Closing mid-task doesn't lose the text. |
| P6 | **Consistency:** one term means one thing. | One word per action ("Save", not a mix of "Export" and "Save"). Buttons, fonts and spacing follow one style. |
| P7 | **Content is the interface.** | Labels make sense without knowing the code ("Populate the combo boxes" fails). No unexplained codes such as "af-ZA". |
| P8 | **Authorised affordances.** | Every control maps to something the app can really do right now (see `ActionAvailability`). |
| P9 | **One source of truth.** | The form holds no rules or message wording. Those live in `Verbalize.Core` (`ActionAvailability`, `UserMessages`), where they are unit tested. |

Grades, as in BlackwoodManor: **A** exemplary, **B** compliant, **C** acceptable debt (listed below), **D** defective, which blocks the step.

Natoli's own pages: [givegoodux.com](https://givegoodux.com/), [books](https://givegoodux.com/books/).

## Done

- Messages appear in a quiet bar instead of pop-up windows with a sound (P4).
- Speak and Save are disabled while there is no text, while offline, or without a key, and the reason is shown (P1, P8).
- Message wording and action rules live in `Verbalize.Core` (P9).
- Messages go where they belong (P4):
  - The bar above the text box appears only for warnings and errors.
  - The status bar's left side shows the app's state ("Speaking…", Online/Offline, key).
  - The status bar's right side shows the outcome of the last action ("Saved …").
  - The main buttons have tooltips.

## Decided

- **Keep the app's own title bar**, like Visual Studio's, instead of the Windows one. Finish it so it behaves like a real title bar: drag to move, double-click to maximise, Windows snap layouts, resizable edges. "?" opens help.
- **Fill in the unfinished buttons and menu items gradually** (Update this file, Colour Scheme and others). Until each works, it stays visibly disabled with "Coming later" as its hover text (P1, P8).
- **Polish all developer wording** (P7).
- **Merge Mute and Speak** into one button that reads "Speak" and turns into "Stop" while speaking (P2, P6).
- **Start the language list at the user's language:** the last one used, or Windows' own language the first time (P2).

## Decided and built: where messages and hints go

The window has two spaces for messages: the bar above the text box, and the "System messages / User messages" strip at the bottom. The split:

- **Bottom strip as a status bar**, like Visual Studio's:
  - *Left, system:* the app's lasting state, such as Online/Offline, Speech key OK, "Speaking…", "Saving…", and the voice list's date.
  - *Right, to the user:* the outcome of their last action, such as "Saved speech.mp3" or "Voice list downloaded".
- **Bar above the text box only for what needs attention**, such as errors and warnings like "offline" or "no key". It appears only then, so it is noticed, since people look where they act (P4). A failure shown only in a bottom corner is easily missed.
- **Hover hints as normal tooltips next to the pointer**, not in the space above the text box. Hints there would appear and disappear as the mouse moves and push the text box up and down. The left of the status bar can also show the hovered control's hint, which can explain disabled buttons, since Windows shows no tooltips on disabled controls.

## Audit findings so far

| Element | Principle | Finding | Plan |
|---|---|---|---|
| Clear | P3 | Wipes the text with no undo or warning | Make it undoable (Ctrl+Z), or offer "Undo" in the status bar |
| Load text | P3 | Replaces unsaved text without warning | Warn, or offer undo |
| Mute + Speak | P2, P6 | Two buttons for one job; "Mute" actually stops | Merge (decided) |
| "Populate the combo boxes" | P7 | Developer wording; the lists already fill themselves | Done: renamed to "Refresh voices" (button and Options menu) |
| "Download Voices list" | P7 | Breaks mid-word ("Downloa d") | Done: renamed to "Get voices" (button and Options menu) |
| Locale code ("af-ZA") | P7 | Code shown to users | Done: hidden; the code is the language list's tooltip |
| "Export Sound", "Export Text", "Export audio from a local file", "Soundfile type :", "Textfile type :" | P6, P7 | "Export" and "Save" mixed; file-system wording | Done: one verb, "Save sound", "Save text", "Turn a file into sound…", "Save sound as", "Save text as"; the Export menu is "Save as" |
| Language list | P2 | Starts at Afrikaans | Start at the user's language (decided) |
| Rate/Pitch | P2, P6 | A list and a slider for the same setting | One control each, with "Reset" |
| Panel colours | P6 | Green, blue and peach panels with no meaning | One calm palette and one accent for the main action |
| Unfinished menu items | P1, P8 | Look usable but do nothing | Disabled with "Coming later" (decided) |

## Steps

1. ~~Message placement (P4).~~ Done. Still to do: show the hovered control's hint on the left of the status bar, including for disabled buttons.
2. ~~Merge Mute and Speak into Speak/Stop, with Ctrl+Enter to speak and Esc to stop (P2).~~ Done.
3. ~~Undo for Clear and for Load over existing text (P3).~~ Done. Known debt (grade C): Undo after Load brings back the text and file name, but not voice settings the loaded SSML file changed.
4. ~~Plain labels throughout, one word per action (P6, P7).~~ Done. Hints in `ControlHints` match the new names.
5. Language list starts at the user's language; remember the last voice and settings (P2, P5).
6. Finish the custom title bar (P5, P6).
7. Progressive disclosure: put Rate, Pitch and Volume under a collapsible "Fine-tune" section, one control each (P1, P2).
8. One calm palette, a clear main action, consistent spacing (P6).
9. Fill in the unfinished features one by one (P8).
10. Check with a real task: someone new turns a paragraph into an mp3 while you watch where they hesitate. Grade each principle (P1–P9) and fix the lowest grade first.
