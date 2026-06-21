# Inkling + WordWeaver — Master Build Specification (v1.0 summary)

**Canonical spec (use for all implementation):** **[master_spec_expanded.md](./master_spec_expanded.md)** — Version 2.0, full production architecture (§1–30).

This file is a **short summary** of §1–10 only. For component trees, data flow, event bus, AI pipelines, rendering, testing, and roadmap, read the expanded spec.

---

## Quick reference (§1–10)

- **Modes:** 3D default, 2D toggle, Inkling + Alerts as overlays (`calendarMode.js`)
- **Data:** Unified `Event` type; `timelineModel.js` is SSOT; no direct `localStorage` from UI
- **3D:** 12-month ring, glass `DayBlock3D`, no fracture visuals, `focusOnDay` / `focusOnMonth`
- **2D:** Year / month / week / day — keep in sync with 3D; do not delete when changing 3D
- **Inkling:** `classifyEvent`, system events, timeline APIs only
- **Guidelines:** Minimal scope, event bus for cross-module comms (v2 §10, §13)

See **[master_spec_expanded.md](./master_spec_expanded.md)** for full detail.
