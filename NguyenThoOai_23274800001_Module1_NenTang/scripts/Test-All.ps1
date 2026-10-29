$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
Push-Location $taskRoot
try {
    dotnet --info
    dotnet restore
    if ($LASTEXITCODE -ne 0) { throw 'Restore thất bại.' }
    dotnet build -c Release --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'Build thất bại.' }
    dotnet test -c Release --no-build --logger 'trx' --results-directory TestResults
    if ($LASTEXITCODE -ne 0) { throw 'Test thất bại. Xem TestResults và hướng dẫn cấu hình SQL Server test.' }
} finally { Pop-Location }
