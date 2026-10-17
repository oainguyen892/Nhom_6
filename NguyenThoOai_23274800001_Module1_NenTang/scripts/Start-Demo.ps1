param([string]$Instance = '(localdb)\MSSQLLocalDB', [string]$Database = 'QuanLyKhachSan_UNETI06_TI17A2ND')
$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
$taskProject = Join-Path $taskRoot 'QuanLyKhachSan_UNETI06_TI17A2ND'
$taskConnection = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()
$taskConnection['Data Source'] = $Instance
$taskConnection['Initial Catalog'] = $Database
$taskConnection['Integrated Security'] = $true
$taskConnection['TrustServerCertificate'] = $true
$env:ConnectionStrings__Hotel = $taskConnection.ConnectionString
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:Demo__Seed = 'true'
Push-Location $taskRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Tool restore thất bại.' }
    dotnet restore
    if ($LASTEXITCODE -ne 0) { throw 'Restore thất bại.' }
    dotnet ef database update --project $taskProject
    if ($LASTEXITCODE -ne 0) { throw 'Migration thất bại. Kiểm tra SQL Server và quyền tạo database.' }
    dotnet run --project $taskProject --no-launch-profile -- --urls http://localhost:5066
} finally { Pop-Location }
