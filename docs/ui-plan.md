# UI plan

A plan for reworking Verbalize's window, guided by Joe Natoli's principles (Give Good UX). Work through it in order, one small step per commit, and test with the real app after each step.

## Principles

Joe Natoli's own pages state these principles; the plan applies them:

- **Think first.** Audit what exists (content, labels, navigation, layout) before designing anything new.
- **Every element either helps the user reach their goal or gets in the way.** Reducing clutter is a series of small changes that clear the path to action.
- **Progressive disclosure.** Information the user doesn't need yet is noise: keep it, hide it for later, or delete it.
- **Icons only where they are recognisable.** Choose between an icon, an icon with a label, or a label alone.
- **Timeless fundamentals over trends.** Hierarchy, alignment, contrast and consistency come from print design.
- **Practical over dogmatic.** Keep the methods simple and aim for the biggest effect.

His pages don't cover error messages or disabled buttons. For those, the plan follows common usability guidance (for example the U.S. Web Design System's research on disabled states): say *why* something can't be done, and never rely on colour alone.

Sources: [givegoodux.com](https://givegoodux.com/), [books](https://givegoodux.com/books/), [start here](https://givegoodux.com/start-here/), [USWDS disabled-states research](https://github.com/uswds/uswds/wiki/Disabled-States-Research-Findings-2023).

## The user's goal

> Turn my text into natural speech, listen to it, and save it as a sound file.

Every element on screen is judged against that sentence.

## Done

- Messages appear in a quiet bar above the text box instead of pop-up windows with a sound. Errors and warnings stay until dismissed; confirmations close by themselves.
- Speak and Export are disabled while there is no text, while offline, or without a key. The bar or the empty text box's hint says why.

## Step 1: Audit (think first)

List every element on the window and in the menus. For each one, note what it is for, whether it works, and whether to keep it, hide it for later, or delete it. Elements spotted so far:

| Element | Problem | Suggestion |
|---|---|---|
| "?", "-", "+", "X" buttons | Duplicate the Windows title bar; "?" does nothing | Delete |
| "Update this file", File → Update / Delete | Do nothing | Hide until built |
| View → Menu bar / App GUI / Colour Scheme, Options → Preset Language | Do nothing | Hide until built |
| "Populate the combo boxes" | Developer wording; the app already fills the lists itself | Delete |
| "System messages" / "User messages" strip at the bottom | Replaced by the message bar | Delete |
| "Mute" | Actually stops the speech | Merge into Speak (see step 4) |
| "Download Voices list" | Word breaks mid-word ("Downloa d"); rarely needed | Move to the Options menu |
| Locale code ("af-ZA") next to Language | Code that means little to users | Delete, or show as a tooltip |
| Language list starts at Afrikaans | Not the user's language | Default to the last used, or the Windows language |

## Step 2: Remove what gets in the way

Delete or hide everything the audit marks. Each removal is small, safe and immediately visible.

## Step 3: Plain labels

Replace developer wording with what the user is doing: "Soundfile type" becomes "Save sound as", "Textfile type" becomes "Save text as", "Export audio from a local file" becomes "Turn a file into sound…", and so on. Use one verb per action throughout (for example "Save" rather than a mix of "Export" and "Save").

## Step 4: Clear hierarchy

- Make **Speak** the most prominent button, next to the text box. While speaking, it turns into **Stop**, so there is one button instead of two.
- Group the saving actions together, after Speak.
- Use one accent colour for the primary action. Calm the green, blue and peach panels into one consistent palette.

## Step 5: Progressive disclosure

- Keep Language, Voice and Style always visible, since those are what users change most.
- Move Rate, Pitch and Volume into a "Fine-tune" section that is closed by default. Give each setting one control instead of a list plus a slider, with a "Reset" link.

## Step 6: Feedback and defaults

- Show progress while speaking or saving (for example "Speaking…" in the bar, or a busy state on the button).
- Remember the last language, voice and settings between sessions.

## Step 7: Keyboard and accessibility

- Add shortcuts: Ctrl+Enter to speak, Esc to stop, Ctrl+S to save.
- Set a logical tab order. Check contrast, and test with Narrator.

## Step 8: Check with a real task

Ask someone who hasn't seen the app to turn a paragraph into an mp3, and watch where they hesitate. Fix the biggest hesitation first, then repeat.
