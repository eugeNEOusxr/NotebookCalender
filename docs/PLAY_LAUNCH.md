# Inkling — Google Play closed-test launch kit

Goal: get the **closed testing** track running with **12 testers for 14 continuous days**, which is what unblocks publishing to production. Everything here is ready to paste/use.

---

## YOUR STEPS (in order — do the slow one first)

1. **Create a Google Play Developer account** ($25 one-time) and **start identity verification immediately.**
   → Verification can take a few days, so kick it off today. It's the slowest gate; everything else is fast.
2. **Build the Android app:** go to **pwabuilder.com** → enter `https://eugeneousxr.github.io` → Package → **Android**.
   → It generates an `.aab` (the upload file) and a **signing keystore**. **Back up that keystore** (lose it = can never update the app).
3. In Play Console: create the app → **Testing → Closed testing → create a new track/release → upload the `.aab`.**
4. **Add your 12 testers' Gmail addresses** to the track's tester list. Play gives you a **join link** — send it to them.
5. **Send me the Play App Signing SHA-256** (Play Console → Test and release → Setup → **App integrity** → "App signing key certificate"). I'll drop it into `.well-known/assetlinks.json` and deploy → the browser URL bar disappears and it looks fully native.
6. Fill in **Data safety**, **content rating**, and the **store listing** (copy below). Then the 14-day clock just needs to run.

> The 14-day clock starts once the closed test is live with testers opted in. Steps 1–4 are the critical path — do them fast.

---

## MESSAGE TO SEND TESTERS — try it now (works today, before Play is set up)

> Hey! I built a study app called **Inkling** and I'm putting it on the Play Store — I need a few people to try it for ~2 weeks. Would you help me out? 🙏
>
> It turns schoolwork into **graded flashcards** (with hints + step-by-step answers), and it's also a calendar you can just talk to.
>
> Try it right now, no account needed:
> 1. Open on your phone: **https://eugeneousxr.github.io**
> 2. Tap **"Continue without signing in"**
> 3. Tap **📇 Flashcards** and do a deck
>
> If you can open it a few times over the next two weeks and tell me anything that's confusing or broken, that's exactly what I need. Thank you!!

## MESSAGE TO SEND TESTERS — Play closed test (once you have the join link)

> Thanks for helping test **Inkling** on the Play Store! Two quick steps:
> 1. Tap this link on your phone (signed in to the Gmail you gave me): **[PASTE PLAY JOIN LINK]**
> 2. Tap **"Become a tester"**, then install from the Play Store button it shows you.
>
> Please open it a few times over the next two weeks — even just one deck each time helps. That's what lets it go live. 🙏

---

## STORE LISTING COPY

**App name** (≤30 chars): `Inkling — Flashcards & Study`
*(or just `Inkling` if you prefer the clean brand over discoverability.)*

**Short description** (≤80 chars):
`Turn your homework and textbook into graded flashcards with hints & answers.`

**Full description** (≤4000 chars):

```
Inkling turns what you're studying into practice that actually sticks.

Pick a topic and Inkling gives you graded flashcards — multiple choice, fill-in,
matching, even read-the-graph questions — with instant scoring, hints when you're
stuck, and step-by-step explanations. It's built around real coursework: OpenStax
Precalculus and Biology are ready to go.

WHY IT HELPS
• Graded practice, not just flip cards — you see exactly what you got right and
  where you slipped.
• Hints that nudge before they tell, so you still do the thinking.
• A clear explanation for every answer.
• Progress tracking, so you can see what's sticking.

ALSO A CALENDAR YOU CAN TALK TO
Tell Inkling a plan in plain words — "lunch with Sam Thursday at 1" — and it adds
it and reminds you. Your studying and your schedule live in one place.

No account required to start — just open it and study. Sign in when you want to
save your decks and progress.
```

---

## WHAT TO ASK TESTERS TO LOOK AT (so feedback is useful)

- Did they understand what the app is within 10 seconds of opening it?
- Could they find and finish a flashcard deck without help?
- Anything confusing, broken, or ugly?
- Would they actually use it before a test?

---

## NOTES / GOTCHAS

- **Use guest mode for testing.** "Continue without signing in" keeps everything on-device and needs no backend — so the (currently unreliable) account server can't ruin anyone's test. The flashcards work fully offline.
- **assetlinks fingerprint = the Play one, not the upload key.** Use the App Signing cert SHA-256 from Play Console, or the URL bar stays visible. (Send it to me and I'll handle the file.)
- **Backend to fix before *public* launch (not before the test):** the free server currently wipes accounts on restart. Testers on guest mode won't hit it, but real public users would.
