@echo off
echo ========================================
echo IMDB System Test Runner
echo ========================================
echo.

echo Checking if application is running...
powershell -Command "try { Invoke-RestMethod -Uri 'http://localhost:5078/api/movies?page=1&pageSize=1' -Method GET -TimeoutSec 5 | Out-Null; Write-Host 'Application is running!' -ForegroundColor Green } catch { Write-Host 'Application is not running!' -ForegroundColor Red; Write-Host 'Please start the application first:' -ForegroundColor Yellow; Write-Host '1. Open terminal' -ForegroundColor White; Write-Host '2. Navigate to: cd IMDB\IMDB.WebServiceLayer' -ForegroundColor White; Write-Host '3. Run: dotnet run' -ForegroundColor White; Write-Host '4. Wait for application to start' -ForegroundColor White; Write-Host '5. Run this test script again' -ForegroundColor White; exit 1 }"

if %errorlevel% neq 0 (
    echo.
    echo Press any key to exit...
    pause >nul
    exit /b 1
)

echo.
echo Running complete test suite...
echo.

powershell -ExecutionPolicy Bypass -File "complete_test_suite.ps1"

echo.
echo Test completed!
echo Press any key to exit...
pause >nul
