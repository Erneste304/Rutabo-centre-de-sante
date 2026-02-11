$apiProject = "HospitalManagementSystem.API"
$blazorProject = "HospitalManagementSystem.Blazor"

Write-Host "Starting Hospital Management System..." -ForegroundColor Green

# Start API in a new window
Write-Host "Launching API Backend..." -ForegroundColor Cyan
Start-Process dotnet -ArgumentList "run --project $apiProject --urls http://localhost:5051" -NoNewWindow
# Simple wait to give API time to start
Start-Sleep -Seconds 5

# Start Blazor Frontend
Write-Host "Launching Blazor Frontend..." -ForegroundColor Cyan
Start-Process dotnet -ArgumentList "run --project $blazorProject --urls http://localhost:5167" -NoNewWindow

Write-Host "Services started!" -ForegroundColor Green
Write-Host "API: http://localhost:5051"
Write-Host "Frontend: http://localhost:5167"
