import { spawn } from "node:child_process";

function run(cmd, args) {
  return new Promise((resolve, reject) => {
    const child = spawn(cmd, args, { stdio: "inherit", shell: true });
    child.on("exit", (code) => (code === 0 ? resolve() : reject(new Error(`${cmd} exited ${code}`))));
  });
}

process.env.INKLING_PLATFORM = process.env.INKLING_PLATFORM || "android";
await run("npm", ["run", "build:web"]);
await run("node", ["scripts/patch-android-buildtask.mjs"]);
await run("npx", ["tauri", "android", "dev"]);
