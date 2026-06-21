/**
 * Copy Windows-safe BuildTask.kt into the generated Android project.
 * Run after `tauri android init` (gen/android is gitignored).
 */
import fs from "node:fs/promises";
import path from "node:path";

const root = process.cwd();
const patchFile = path.join(root, "src-tauri", "android-patch", "BuildTask.kt");
const buildSrcJava = path.join(root, "src-tauri", "gen", "android", "buildSrc", "src", "main", "java");

async function walk(dir) {
  /** @type {string[]} */
  const hits = [];
  let entries = [];
  try {
    entries = await fs.readdir(dir, { withFileTypes: true });
  } catch {
    return hits;
  }
  for (const ent of entries) {
    const full = path.join(dir, ent.name);
    if (ent.isDirectory()) {
      hits.push(...(await walk(full)));
    } else if (ent.name === "BuildTask.kt") {
      hits.push(full);
    }
  }
  return hits;
}

const matches = await walk(buildSrcJava);
if (!matches.length) {
  console.error(
    "[patch-android] No BuildTask.kt under src-tauri/gen/android/buildSrc — run: npm run tauri:android:init"
  );
  process.exit(1);
}

const dest = matches[0];
await fs.copyFile(patchFile, dest);
console.log(`[patch-android] Patched ${dest}`);
