$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    dotnet build AutomotiveServiceManagement.sln
    if ($LASTEXITCODE -ne 0) { throw 'Derleme başarısız.' }
    dotnet run --project tests/IntegrationTests --no-build
    if ($LASTEXITCODE -ne 0) { throw 'Entegrasyon testi başarısız.' }
} finally { Pop-Location }
