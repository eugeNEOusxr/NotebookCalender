# Meshy.ai worlds in WordWeaver

[Meshy](https://www.meshy.ai/) generates 3D models (GLB, FBX, OBJ). Inkling treats them as **world props** or full uploads inside WordWeaver’s **World scene** layout.

## In the app

1. Open **WordWeaver** (🪡).
2. In the top bar, choose a **world**:
   - **Starfield** — original grid (default)
   - **House** — room with desk / window anchors
   - **Park** — trees, bench, path (gentle sway)
   - **Beach** — sand + animated water
   - **Meshy upload** — your GLB
3. Layout switches to **World scene**; each thought is placed at a scene **anchor** with a neon time label above the text.
4. Tap **GLB** and pick a `.glb` / `.gltf` exported from Meshy (or any tool).

Uploads are stored under `public/environments/uploads/` when using `npm run dev`, or kept in memory on device if offline.

## Meshy workflow (recommended)

1. On [meshy.ai](https://www.meshy.ai/), create assets with **Text to 3D** or **Image to 3D**.
2. In the task result, download **GLB** (`model_urls.glb` in the API).
3. For a **single hero prop** (bench, house, tree): upload via **GLB** in WordWeaver.
4. For a **full environment**: combine several GLBs in a DCC, or upload one large scene GLB; adjust anchors in code in `src/wordweaver/environments/weaveEnvironments.js` under the `meshy` preset.

## API (optional)

Meshy’s REST API can automate generation. Set `MESHY_API_KEY` on the server and extend `server/lib/meshy/` (not wired by default). See [Meshy API docs](https://docs.meshy.ai/).

Example request shape:

```bash
curl https://api.meshy.ai/openapi/v1/image-to-3d \
  -H "Authorization: Bearer $MESHY_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{"image_url":"https://…","target_formats":["glb"]}'
```

Poll the task until `status` is `SUCCEEDED`, then download `model_urls.glb`.

## Fluid motion

Built-in presets use light animation (swaying trees, water vertices, drifting stars). Uploaded GLBs are static unless you add animated glTF clips in a future pass.

## Files

| Path | Role |
|------|------|
| `src/wordweaver/environments/weaveEnvironments.js` | Preset catalog + anchors |
| `src/wordweaver/environments/WeaveEnvironment.js` | Loader + procedural worlds |
| `src/wordweaver/layoutModes.js` | `scene` layout places notes on anchors |
| `POST /api/environments/upload` | Saves GLB for dev/PWA |
