# Inkling — Android (Tauri) & Google Play

This project can ship as an **Android App Bundle (`.aab`)** for Google Play and **APK (`.apk`)** for sideloading. The mobile app is a **static WebView shell**; your **Node API** must be hosted separately.

## Architecture

| Piece | Location | In the APK? |
|-------|----------|-------------|
| UI (HTML/JS/CSS/Three.js) | `dist/` after `npm run build:web` | Yes |
| Tauri / WebView shell | `src-tauri/` | Yes |
| Auth, sync, LLM, email | `server/` | **No** — deploy to HTTPS |

## One-time setup

### 1. Tools

- [Rust](https://rustup.rs/)
- [Android Studio](https://developer.android.com/studio) (SDK, NDK, platform tools)
- Java 17+
- Environment (Windows example):

  ```powershell
  $env:JAVA_HOME = "C:\Program Files\Android\Android Studio\jbr"
  $env:ANDROID_HOME = "$env:LOCALAPPDATA\Android\Sdk"
  $env:NDK_HOME = "$env:ANDROID_HOME\ndk\<version>"   # see SDK Manager
  $env:PATH += ";$env:ANDROID_HOME\platform-tools"
  ```

### 2. Install NDK (required)

In **Android Studio → SDK Manager → SDK Tools**, enable **NDK (Side by side)** and apply.

Then set env vars (PowerShell, from repo root):

```powershell
. .\scripts\setup-android-env.ps1
```

### 3. Initialize Android target (once per clone)

```bash
npm install
npm run tauri:android:init
```

This creates `src-tauri/gen/android/` (gitignored; regenerated per machine).

### 4. Deploy the API (required for real users)

Host `server/` with HTTPS and set:

- `JWT_SECRET` (long random string)
- `APP_URL`, `CORS_ORIGIN`
- `OPENAI_API_KEY` (Inkling + WordWeaver LLM)
- Email provider keys (password reset)

Persistent storage for `data/users/`.

## Build commands

| Command | Purpose |
|---------|---------|
| `npm run config:write` | Regenerate `inkling-config.js` from env |
| `npm run build:web` | `dist/` for Tauri + vendors Three.js offline |
| `npm run tauri:android:dev` | Run on device/emulator (debug) |
| `npm run tauri:android:build` | Release **AAB + APK** |

### Point the app at your API

```bash
# PowerShell
$env:INKLING_API_URL = "https://api.yourdomain.com"
$env:INKLING_PLATFORM = "android"
npm run tauri:android:build
```

```bash
# bash
INKLING_API_URL=https://api.yourdomain.com INKLING_PLATFORM=android npm run tauri:android:build
```

Config is baked into `inkling-config.js` at build time.

### Output paths

After a successful build, look under:

`src-tauri/gen/android/app/build/outputs/`

- `bundle/release/*.aab` — upload to Play Console
- `apk/release/*.apk` — local install / testers

## Google Play (when you are ready)

1. Create a Play Console app with package id `com.eugeousxr.notebookcalender` (matches `tauri.conf.json`).
2. Generate an **upload keystore** and configure signing in Android Studio / Gradle.
3. Upload the **AAB**, not the APK, for production.
4. Privacy policy, data safety form (accounts, optional LLM).
5. Test on real devices: login, sync, 3D, WordWeaver, offline skip-auth path.

## Desktop Tauri

```bash
npm run tauri:build
```

Same `INKLING_API_URL` rules apply for production desktop builds.

## Troubleshooting

### Windows: `Creation symbolic link is not allowed for this system`

Tauri links `libapp_lib.so` into `jniLibs` with a **symlink**. On Windows you must enable **Developer Mode**:

1. **Settings** → **System** → **For developers** (or search “Developer Mode”).
2. Turn **Developer Mode** **On**.
3. Close and reopen PowerShell (or restart Cursor).
4. Rerun:

   ```powershell
   . .\scripts\setup-android-env.ps1
   npm run tauri:android:dev
   ```

Rust does not need a full rebuild; the failed step is only the symlink at the end.

Alternative (advanced): grant your user **Create symbolic links** in Local Security Policy and run the terminal elevated — Developer Mode is simpler.

### Windows: Gradle `rustBuildX86_64Debug` fails / `Missing script: "tauri"`

Default Tauri `BuildTask.kt` runs `npm run -- tauri`, but this project had no `"tauri"` script in `package.json`. The patch uses **`npx tauri android android-studio-script`** via `cmd /c npx.cmd` instead.

Also add `"tauri": "tauri"` in `package.json` as a fallback.

### Windows: `A problem occurred starting process 'command npm.bat'`

Gradle cannot start `npm.bat` directly. This repo ships a patch:

```powershell
npm run android:patch
```

That copies `src-tauri/android-patch/BuildTask.kt` into `gen/android/buildSrc/...` (uses `cmd /c` + full path to npm).

Then rebuild:

```powershell
. .\scripts\setup-android-env.ps1
npm run build:web
npm run tauri:android:dev
```

`tauri:android:dev` now runs `build:web` + `android:patch` automatically.

### App opens then crashes / white screen on emulator

Log line `Unexpected token '<', "<!DOCTYPE "...` means the app called `/api/...` on `http://tauri.localhost` (not a real server). Rebuild after the `apiRuntime.js` fix:

```powershell
npm run build:web
npm run tauri:android:dev
```

Native Android runs **offline** without login when `INKLING_API_URL` is unset. Emulator OOM: use an **arm64** system image (not only x86), give the AVD **4GB+ RAM**, or test on a physical phone.

- **Blank 3D / module errors** — run `npm run build:web` so Three.js is vendored under `dist/vendor/three` (CDN is not used in release builds).
- **API 401 / network failed** — set `INKLING_API_URL`; Tauri WebView origin is not your Node server.
- **`tauri android init` fails** — install SDK 34+, NDK, accept licenses in Android Studio.
- **Skip auth on device** — use “Skip for now” on login; data stays local until you configure the API.

## Not production-ready yet

Treat this as a **scaffold**: harden JWT, hosting, LLM costs, CSP, signing, and QA before public release.
