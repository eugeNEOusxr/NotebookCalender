/**
 * Reset a local Inkling account password (dev / recovery).
 * Usage: node scripts/reset-password.mjs you@example.com "NewPassword1!"
 */
import { readUser, writeUser } from "../server/lib/userStore.js";
import { hashPassword } from "../server/lib/cryptoAuth.js";
import { validatePassword } from "../server/lib/passwordPolicy.js";

const email = process.argv[2]?.trim().toLowerCase();
const password = process.argv[3] ?? "";

if (!email || !email.includes("@")) {
  console.error("Usage: node scripts/reset-password.mjs <email> <new-password>");
  process.exit(1);
}

if (!password) {
  console.error("Provide the new password as the second argument.");
  process.exit(1);
}

const pwCheck = validatePassword(password);
if (!pwCheck.ok) {
  console.error(pwCheck.error);
  console.error("New passwords need 12+ chars with upper, lower, number, and symbol.");
  process.exit(1);
}

const user = await readUser(email);
if (!user) {
  console.error(`No account found for ${email}`);
  console.error("Accounts on this machine are stored in data/users/ — use the exact email you registered.");
  process.exit(1);
}

user.passwordHash = await hashPassword(password);
await writeUser(user);
console.log(`Password updated for ${email}`);
if (user.username) console.log(`Sign in with username "${user.username}" or this email.`);
