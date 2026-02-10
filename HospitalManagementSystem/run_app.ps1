Write-Host "Starting Hospital Management System..." -ForegroundColor Green

# Define paths
$apiProject = "HospitalManagementSystem.API"

# Check if dotnet is installed
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet SDK is not installed or not in PATH."
    exit 1
}

# Restore and Run
Write-Host "Restoring and Running API project..." -ForegroundColor Cyan
dotnet run --project $apiProject --urls "http://localhost:5051"

Write-Host "Application stopped." -ForegroundColor Yellow
