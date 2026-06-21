/**
 * Production Android build: static web (offline three) + Tauri AAB/APK.
 *
 * Set INKLING_API_URL to your deployed Node server before building, e.g.:
 *   INKLING_API_URL=https://api.example.com npm run tauri:android:build
 */
import { spawn } from "node:child_process";

const env = {
  ...process.env,
  INKLING_PLATFORM: process.env.INKLING_PLATFORM || "android",
  INKLING_BUILD_TAG: process.env.INKLING_BUILD_TAG || "android-release"
};

function run(cmd, args) {
  return new Promise((resolve, reject) => {
    const child = spawn(cmd, args, { stdio: "inherit", env, shell: true });
    child.on("exit", (code) => (code === 0 ? resolve() : reject(new Error(`${cmd} exited ${code}`))));
  });
}

if (!env.INKLING_API_URL) {
  console.warn(
    "[android] INKLING_API_URL is not set — the app will only talk to same-origin (fine for dev APK tests)."
  );
  console.warn("[android] For Play Store, set INKLING_API_URL=https://your-api.example.com");
}

await run("npm", ["run", "build:web"]);
await run("npx", ["tauri", "android", "build", "--aab", "--apk"]);

console.log("\nAndroid artifacts are under src-tauri/gen/android/app/build/outputs/");
