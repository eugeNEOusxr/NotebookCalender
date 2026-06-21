# Detect Android SDK / NDK and print env vars for the current PowerShell session.
# Usage: . .\scripts\setup-android-env.ps1

$sdk = $env:ANDROID_HOME
if (-not $sdk) {
  $candidates = @(
    "$env:LOCALAPPDATA\Android\Sdk",
    "$env:USERPROFILE\AppData\Local\Android\Sdk"
  )
  foreach ($c in $candidates) {
    if (Test-Path $c) { $sdk = $c; break }
  }
}

if (-not $sdk) {
  Write-Host "Android SDK not found. Install Android Studio and SDK Platform 34+." -ForegroundColor Red
  return
}

$ndkRoot = Join-Path $sdk "ndk"
$ndk = $null
if (Test-Path $ndkRoot) {
  $ndk = Get-ChildItem $ndkRoot -Directory | Sort-Object Name -Descending | Select-Object -First 1
}

$jbr = "C:\Program Files\Android\Android Studio\jbr"
if (-not (Test-Path $jbr)) {
  $jbr = $null
}

$env:ANDROID_HOME = $sdk
$env:PATH = "$sdk\platform-tools;$sdk\cmdline-tools\latest\bin;$env:PATH"
if ($jbr) { $env:JAVA_HOME = $jbr }

$nodeDir = "C:\Program Files\nodejs"
if (Test-Path "$nodeDir\npm.cmd") {
  $env:NPM_BIN = "$nodeDir\npm.cmd"
  $env:NPX_BIN = "$nodeDir\npx.cmd"
  $env:PATH = "$nodeDir;$env:PATH"
}
if ($ndk) {
  $env:NDK_HOME = $ndk.FullName
  Write-Host "NDK_HOME=$($env:NDK_HOME)" -ForegroundColor Green
} else {
  Write-Host "No NDK under $ndkRoot" -ForegroundColor Yellow
  Write-Host "Android Studio -> SDK Manager -> SDK Tools -> NDK (Side by side)" -ForegroundColor Yellow
}

Write-Host "ANDROID_HOME=$env:ANDROID_HOME"
if ($env:JAVA_HOME) { Write-Host "JAVA_HOME=$env:JAVA_HOME" }
Write-Host "Run: npm run tauri:android:init"
