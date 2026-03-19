# This script runs all tests and generates a coverage report

# Paths
$testResultsDir = ".\tests\TestResults"
$coverageDir = ".\tests\coverage"

# Clean old results
Write-Host "Cleaning old test results..." -ForegroundColor Cyan
Remove-Item -Recurse -Force $testResultsDir -ErrorAction Ignore

Write-Host "Cleaning old coverage..." -ForegroundColor Cyan
Remove-Item -Recurse -Force $coverageDir -ErrorAction Ignore

# Run tests WITH coverage and force output location
Write-Host "Running tests with coverage..." -ForegroundColor Cyan
dotnet test .\PortfolioWebApp.slnx `
  --collect:"XPlat Code Coverage" `
  --results-directory $testResultsDir `
  --logger "console;verbosity=normal"

# Generate HTML report
Write-Host "Generating coverage report..." -ForegroundColor Cyan
reportgenerator `
  -reports:"$testResultsDir\**\coverage.cobertura.xml" `
  -targetdir:$coverageDir `
  -reporttypes:Html `
  -filefilters:"-**/obj/**;-**/Migrations/**"

# Open report
Write-Host "Done..." -ForegroundColor Cyan
Start-Process "$coverageDir\index.html"