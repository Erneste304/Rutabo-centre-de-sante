$apiProject = "HospitalManagementSystem.API"
$rootProject = "."

Write-Host "Starting Hospital Management System..." -ForegroundColor Green

# Start API in a new window
Write-Host "Launching API Backend (http://localhost:5051)..." -ForegroundColor Cyan
Start-Process dotnet -ArgumentList "run --project $apiProject --urls http://localhost:5051" -NoNewWindow
# Simple wait to give API time to start
Start-Sleep -Seconds 3

# Start Hosted Blazor (Root Project)
Write-Host "Launching Hosted Blazor Frontend (http://localhost:5000)..." -ForegroundColor Cyan
dotnet watch run --project $rootProject --urls http://localhost:5000
