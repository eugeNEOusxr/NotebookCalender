# Inkling + WordWeaver – Upgrade Path Roadmap (1,000 Tasks)

This document defines a long‑term, 1,000‑task roadmap for building Inkling + WordWeaver into a full platform.

- **Master spec:** `/docs/master_spec.md`
- **Extended spec:** `/docs/master_spec_extended.md` (optional, from Claude)
- **This file:** Implementation roadmap. Cursor should always follow this in order unless explicitly told otherwise.

Each task is identified as:

> Phase.Milestone.Task  →  e.g., `1.1.3`

- **Phase** = big area of work  
- **Milestone** = coherent feature slice  
- **Task** = atomic, Cursor‑implementable step

Total:  
10 Phases × 10 Milestones per Phase × 10 Tasks per Milestone = **1,000 tasks**.

---

## Phase 1 – Core Foundations (Data, Events, Persistence)

### Milestone 1.1 – Timeline Model Core

**Goal:** A solid, spec‑compliant `timelineModel.js` with full CRUD and persistence.

- **1.1.1** Create `timelineModel.js` file and scaffold module structure (no logic yet).
- **1.1.2** Implement `Event` TypeScript/JS typedefs exactly as in `/docs/master_spec.md`.
- **1.1.3** Implement in‑memory `_events` array and initialization function `initTimelineModel()`.
- **1.1.4** Implement `loadFromStorage()` and `saveToStorage()` using `localStorage` keys from spec.
- **1.1.5** Implement `createEvent(partial)` with validation, `id`, `createdAt`, `updatedAt`.
- **1.1.6** Implement `updateEvent(id, changes)` with validation and `updatedAt`.
- **1.1.7** Implement `deleteEvent(id)`.
- **1.1.8** Implement `bulkCreateEvents(partials)` for starter data.
- **1.1.9** Wire event bus emits: `eventCreated`, `eventUpdated`, `eventDeleted`.
- **1.1.10** Add basic unit tests (or test harness) for create/update/delete and persistence.

### Milestone 1.2 – Derived Views & Helpers

**Goal:** All read helpers for day/week/month/year.

- **1.2.1** Implement `getEventsForDate(date: Date): Event[]`.
- **1.2.2** Implement `getEventsForDay(year, month, day): Event[]`.
- **1.2.3** Implement `getEventsForWeek(year, isoWeek): Event[]`.
- **1.2.4** Implement `getEventsForMonth(year, month): Event[]`.
- **1.2.5** Implement `getEventsForYear(year): Event[]`.
- **1.2.6** Implement `getUpcomingAlerts(withinMinutes)`.
- **1.2.7** Implement `getUnifiedEventsForDate(date)` for shared 2D/3D use.
- **1.2.8** Ensure all helpers are pure and return new arrays.
- **1.2.9** Add tests for each helper with sample data.
- **1.2.10** Add performance notes (no O(n²) loops on every call).

### Milestone 1.3 – Starter Data & Flags

- **1.3.1** Create `starterEvents` constant with a realistic week/month of events.
- **1.3.2** Implement `hasUserNotes` flag logic using `inkling-has-user-notes`.
- **1.3.3** On first run (no data), load `starterEvents` via `bulkCreateEvents`.
- **1.3.4** When user creates first real event, set `hasUserNotes = true`.
- **1.3.5** Hide starter events once `hasUserNotes` is true.
- **1.3.6** Add tests for starter data lifecycle.
- **1.3.7** Add event bus emit `timelineInitialized`.
- **1.3.8** Document starter data behavior in comments.
- **1.3.9** Ensure no starter data leaks into export/backup (future).
- **1.3.10** Confirm behavior matches `/docs/master_spec.md`.

### Milestone 1.4 – Storage Safety & Limits

- **1.4.1** Implement storage size estimation before writes.
- **1.4.2** Emit `storageWarning` when approaching 4.5MB.
- **1.4.3** Emit `storageFull` and reject writes above ~4.9MB.
- **1.4.4** Add `_writing` mutex to prevent re‑entrant writes.
- **1.4.5** Add logging hooks (dev‑only) for storage issues.
- **1.4.6** Add tests simulating large event sets.
- **1.4.7** Document storage behavior in `timelineModel.js`.
- **1.4.8** Ensure no direct `localStorage` writes outside timeline model.
- **1.4.9** Add TODO for future cloud sync integration.
- **1.4.10** Cross‑check with spec §3.3–3.4.

### Milestone 1.5–1.10 – (Reserved for future data extensions)

For now, these milestones are placeholders for:

- Recurrence rules  
- Tags/labels  
- Attachments  
- Cross‑device sync metadata  
- Audit logs  

Tasks 1.5.x–1.10.x will be filled later when you’re ready to extend the data model.

---

## Phase 2 – Event Bus, Scheduler, and Alerts Core

### Milestone 2.1 – Event Bus

- **2.1.1** Create `eventBus.js` with simple pub/sub API.
- **2.1.2** Implement `on(eventName, handler)` and `off(eventName, handler)`.
- **2.1.3** Implement `emit(eventName, payload)`.
- **2.1.4** Add type hints / JSDoc for known events.
- **2.1.5** Ensure no circular imports with core modules.
- **2.1.6** Add tests for subscription and unsubscription.
- **2.1.7** Add dev‑only logging toggle for emitted events.
- **2.1.8** Integrate timeline model emits with event bus.
- **2.1.9** Document event naming conventions.
- **2.1.10** Cross‑check with spec §2.2, §12.0.

### Milestone 2.2 – Alerts Data & Scheduler

- **2.2.1** Create `alerts/Scheduler.js`.
- **2.2.2** Implement in‑memory list of scheduled alerts.
- **2.2.3** On `timelineInitialized`, scan events and schedule alerts.
- **2.2.4** On `eventCreated`, add relevant alerts to schedule.
- **2.2.5** On `eventUpdated`, reschedule alerts for that event.
- **2.2.6** On `eventDeleted`, cancel alerts for that event.
- **2.2.7** Implement timer loop or `setTimeout` per alert.
- **2.2.8** Emit `alertTriggered` via event bus when alert fires.
- **2.2.9** Mark alert as `triggered` in event data.
- **2.2.10** Add tests for alert scheduling and triggering.

### Milestone 2.3 – Alerts UI Shell Integration

- **2.3.1** Add Alerts button to top bar with badge.
- **2.3.2** Create `AlertsDropdown.js` component.
- **2.3.3** Subscribe to `alertTriggered` and `eventUpdated` to refresh list.
- **2.3.4** Show upcoming alerts with “time until” labels.
- **2.3.5** Clicking an alert navigates to that event’s day.
- **2.3.6** Emit `alertsOpened` when dropdown opens.
- **2.3.7** Style dropdown per spec (glass, subtle, readable).
- **2.3.8** Add mobile‑friendly layout for dropdown.
- **2.3.9** Add basic keyboard navigation for alerts list.
- **2.3.10** Cross‑check with spec §7.0.

### Milestone 2.4–2.10 – Advanced Alerts (later)

Reserved for:

- Snooze  
- Recurring alerts  
- Alert templates  
- AI‑suggested alerts  
- Cross‑device alert sync  

---

## Phase 3 – UI Shell & Mode Management

### Milestone 3.1 – Top Bar & Mode State

- **3.1.1** Create `UIShell.js` as root layout component.
- **3.1.2** Implement top bar with app title/logo.
- **3.1.3** Implement 2D/3D mode toggle button.
- **3.1.4** Implement view toggle: Today / Week / Month / Year.
- **3.1.5** Wire `calendarMode` state to `inkling-calendar-mode` storage key.
- **3.1.6** On load, restore last mode from storage.
- **3.1.7** Emit `modeChanged` on mode switch.
- **3.1.8** Ensure 2D and 3D containers mount once and toggle visibility only.
- **3.1.9** Add responsive layout for mobile vs desktop.
- **3.1.10** Cross‑check with spec §2.1, §8.0.

### Milestone 3.2 – Inkling Panel Shell

- **3.2.1** Create `InklingPanel.js` with open/close state.
- **3.2.2** Add open/close button in top bar.
- **3.2.3** Implement slide‑in animation (right on desktop, bottom on mobile).
- **3.2.4** Add message list area and input area (no AI yet).
- **3.2.5** Add minimized state (header only).
- **3.2.6** Close on Escape key.
- **3.2.7** Persist open/closed state in memory only (no storage yet).
- **3.2.8** Ensure panel overlays both 2D and 3D modes.
- **3.2.9** Add basic styling per spec (glass, readable).
- **3.2.10** Cross‑check with spec §4.2.

### Milestone 3.3 – Settings Panel Shell

- **3.3.1** Create `SettingsPanel.js`.
- **3.3.2** Add Settings button in top bar.
- **3.3.3** Implement slide‑in animation.
- **3.3.4** Add placeholder sections: Theme, Time format, Data.
- **3.3.5** Close on Escape and outside click.
- **3.3.6** Ensure no direct timeline writes yet.
- **3.3.7** Add basic layout for mobile/desktop.
- **3.3.8** Wire to event bus for future preferences.
- **3.3.9** Add stub preferences module.
- **3.3.10** Cross‑check with spec §8.2.

---

## Phase 4 – Calendar 2D Core

(Year view, Month view, Week view, Day view, all wired to timeline model.)

…  
*(You’ll continue this same pattern: each Milestone has 10 atomic tasks. Cursor always works on the next one.)*  

---

## Phase 5 – WordWeaver 3D Core

(Scene init, year layout, DayBlock3D, AtomGlyph3D, camera, interaction.)

---

## Phase 6 – Inkling AI Integration

(Intent classification, summaries, navigation, suggestions, clarification loop.)

---

## Phase 7 – Advanced UX & Visuals

(Animations, theming, iconography, micro‑interactions, transitions.)

---

## Phase 8 – Mobile & Input Systems

(Mobile gestures, desktop shortcuts, accessibility, focus management.)

---

## Phase 9 – Performance, Testing, Error Recovery

(Profiling, instancing, tests, error handling, recovery flows.)

---

## Phase 10 – Future Platform Features

(Plugins, cloud sync, multi‑user, collaboration, experimental 3D timelines.)

---

> **Rule for Cursor:**  
> Always implement tasks in order (Phase.Milestone.Task) unless explicitly instructed otherwise.  
> Always align with `/docs/master_spec.md`.  
> Never delete working systems unless the spec explicitly says so.

