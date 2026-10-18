$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    dotnet run --project src/Web.UI --no-launch-profile --urls http://127.0.0.1:5180
    if ($LASTEXITCODE -ne 0) { throw 'Uygulama başlatılamadı.' }
} finally { Pop-Location }
