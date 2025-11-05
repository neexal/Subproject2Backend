# Complete IMDB System Test Suite
# This script tests all functionality of the IMDB backend system

Write-Host "=== IMDB Complete System Test Suite ===" -ForegroundColor Green
Write-Host "Testing all functionality of the IMDB backend system" -ForegroundColor Cyan
Write-Host ""

# Configuration
$baseUrl = "http://localhost:5078"
$testResults = @{
    Total = 0
    Passed = 0
    Failed = 0
    Tests = @()
}

# Helper function to log test results
function Log-TestResult {
    param(
        [string]$TestName,
        [bool]$Passed,
        [string]$Message = "",
        [string]$Details = ""
    )
    
    $testResults.Total++
    if ($Passed) {
        $testResults.Passed++
        Write-Host "✅ $TestName" -ForegroundColor Green
    } else {
        $testResults.Failed++
        Write-Host "❌ $TestName" -ForegroundColor Red
        if ($Message) {
            Write-Host "   Error: $Message" -ForegroundColor Yellow
        }
    }
    
    if ($Details) {
        Write-Host "   Details: $Details" -ForegroundColor Cyan
    }
    
    $testResults.Tests += @{
        Name = $TestName
        Passed = $Passed
        Message = $Message
        Details = $Details
    }
}

# Test 1: Check if application is running
Write-Host "1. Checking Application Status..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=1" -Method GET -TimeoutSec 5
    Log-TestResult "Application Running" $true "Application is running and responding"
} catch {
    Log-TestResult "Application Running" $false "Application is not running or not responding" $_.Exception.Message
    Write-Host ""
    Write-Host "❌ APPLICATION NOT RUNNING!" -ForegroundColor Red
    Write-Host "Please start the application first:" -ForegroundColor Yellow
    Write-Host "1. Open terminal/command prompt" -ForegroundColor White
    Write-Host "2. Navigate to: cd IMDB\IMDB.WebServiceLayer" -ForegroundColor White
    Write-Host "3. Run: dotnet run" -ForegroundColor White
    Write-Host "4. Wait for 'Now listening on: http://localhost:5078'" -ForegroundColor White
    Write-Host "5. Run this test script again" -ForegroundColor White
    exit 1
}

# Test 2: Database Connection
Write-Host ""
Write-Host "2. Testing Database Connection..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=1" -Method GET
    if ($response.TotalCount -gt 0) {
        Log-TestResult "Database Connection" $true "Connected to database successfully" "Found $($response.TotalCount) movies"
    } else {
        Log-TestResult "Database Connection" $false "Database connected but no data found"
    }
} catch {
    Log-TestResult "Database Connection" $false "Database connection failed" $_.Exception.Message
}

# Test 3: User Registration
Write-Host ""
Write-Host "3. Testing User Registration..." -ForegroundColor Yellow
$registerData = @{
    username = "testuser_$(Get-Date -Format 'yyyyMMddHHmmss')"
    email = "test_$(Get-Date -Format 'yyyyMMddHHmmss')@example.com"
    password = "password123"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/register" -Method POST -Body $registerData -ContentType "application/json"
    Log-TestResult "User Registration" $true "User registered successfully" "User ID: $($registerResponse.UserId)"
    $registeredUserId = $registerResponse.UserId
} catch {
    Log-TestResult "User Registration" $false "User registration failed" $_.Exception.Message
    # Try to use existing user for login test
    $registeredUserId = 1
}

# Test 4: User Login
Write-Host ""
Write-Host "4. Testing User Login..." -ForegroundColor Yellow
$loginData = @{
    email = "test@example.com"
    password = "password123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    Log-TestResult "User Login" $true "Login successful" "Token generated: $($loginResponse.Token.Substring(0, 50))..."
    $authToken = $loginResponse.Token
    $userId = $loginResponse.UserId
} catch {
    Log-TestResult "User Login" $false "Login failed" $_.Exception.Message
    $authToken = $null
    $userId = 1
}

# Test 5: Token Verification
Write-Host ""
Write-Host "5. Testing Token Verification..." -ForegroundColor Yellow
if ($authToken) {
    try {
        $headers = @{ Authorization = "Bearer $authToken" }
        $verifyResponse = Invoke-RestMethod -Uri "$baseUrl/api/auth/verify-token" -Method GET -Headers $headers
        Log-TestResult "Token Verification" $true "Token is valid" "User: $($verifyResponse.Username)"
    } catch {
        Log-TestResult "Token Verification" $false "Token verification failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Token Verification" $false "No token available for testing"
}

# Test 6: Movies Endpoint
Write-Host ""
Write-Host "6. Testing Movies Endpoint..." -ForegroundColor Yellow
try {
    $moviesResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=5" -Method GET
    Log-TestResult "Movies Endpoint" $true "Movies endpoint working" "Found $($moviesResponse.Data.Count) movies, Total: $($moviesResponse.TotalCount)"
} catch {
    Log-TestResult "Movies Endpoint" $false "Movies endpoint failed" $_.Exception.Message
}

# Test 7: Persons Endpoint
Write-Host ""
Write-Host "7. Testing Persons Endpoint..." -ForegroundColor Yellow
try {
    $personsResponse = Invoke-RestMethod -Uri "$baseUrl/api/persons?page=1&pageSize=5" -Method GET
    Log-TestResult "Persons Endpoint" $true "Persons endpoint working" "Found $($personsResponse.Data.Count) persons, Total: $($personsResponse.TotalCount)"
} catch {
    Log-TestResult "Persons Endpoint" $false "Persons endpoint failed" $_.Exception.Message
}

# Test 8: Movie Search
Write-Host ""
Write-Host "8. Testing Movie Search..." -ForegroundColor Yellow
try {
    $searchData = @{
        searchTerm = "test"
        page = 1
        pageSize = 5
    } | ConvertTo-Json
    
    $searchResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies/search" -Method POST -Body $searchData -ContentType "application/json"
    Log-TestResult "Movie Search" $true "Movie search working" "Found $($searchResponse.Data.Count) results"
} catch {
    Log-TestResult "Movie Search" $false "Movie search failed" $_.Exception.Message
}

# Test 9: Person Search
Write-Host ""
Write-Host "9. Testing Person Search..." -ForegroundColor Yellow
try {
    $searchData = @{
        searchTerm = "test"
        page = 1
        pageSize = 5
    } | ConvertTo-Json
    
    $searchResponse = Invoke-RestMethod -Uri "$baseUrl/api/persons/search" -Method POST -Body $searchData -ContentType "application/json"
    Log-TestResult "Person Search" $true "Person search working" "Found $($searchResponse.Data.Count) results"
} catch {
    Log-TestResult "Person Search" $false "Person search failed" $_.Exception.Message
}

# Test 10: Framework String Search (if authenticated)
Write-Host ""
Write-Host "10. Testing Framework String Search..." -ForegroundColor Yellow
if ($authToken) {
    try {
        $searchData = @{
            userId = $userId
            searchString = "test"
        } | ConvertTo-Json
        
        $headers = @{ Authorization = "Bearer $authToken" }
        $searchResponse = Invoke-RestMethod -Uri "$baseUrl/api/framework/search/string" -Method POST -Body $searchData -ContentType "application/json" -Headers $headers
        Log-TestResult "Framework String Search" $true "Framework search working" "Found $($searchResponse.Count) results"
    } catch {
        Log-TestResult "Framework String Search" $false "Framework search failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Framework String Search" $false "No authentication token available"
}

# Test 11: Framework Name Search (if authenticated)
Write-Host ""
Write-Host "11. Testing Framework Name Search..." -ForegroundColor Yellow
if ($authToken) {
    try {
        $searchData = @{
            userId = $userId
            searchString = "test"
        } | ConvertTo-Json
        
        $headers = @{ Authorization = "Bearer $authToken" }
        $searchResponse = Invoke-RestMethod -Uri "$baseUrl/api/framework/search/names" -Method POST -Body $searchData -ContentType "application/json" -Headers $headers
        Log-TestResult "Framework Name Search" $true "Framework name search working" "Found $($searchResponse.Count) results"
    } catch {
        Log-TestResult "Framework Name Search" $false "Framework name search failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Framework Name Search" $false "No authentication token available"
}

# Test 12: Movie Bookmark (if authenticated)
Write-Host ""
Write-Host "12. Testing Movie Bookmark..." -ForegroundColor Yellow
if ($authToken) {
    try {
        # First get a movie ID
        $moviesResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=1" -Method GET
        if ($moviesResponse.Data.Count -gt 0) {
            $movieId = $moviesResponse.Data[0].MovieId
            
            $bookmarkData = @{
                userId = $userId
                movieId = $movieId
            } | ConvertTo-Json
            
            $headers = @{ Authorization = "Bearer $authToken" }
            $bookmarkResponse = Invoke-RestMethod -Uri "$baseUrl/api/framework/bookmarks/movies/toggle" -Method POST -Body $bookmarkData -ContentType "application/json" -Headers $headers
            Log-TestResult "Movie Bookmark" $true "Movie bookmark working" $bookmarkResponse.Message
        } else {
            Log-TestResult "Movie Bookmark" $false "No movies available for bookmarking"
        }
    } catch {
        Log-TestResult "Movie Bookmark" $false "Movie bookmark failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Movie Bookmark" $false "No authentication token available"
}

# Test 13: Movie Rating (if authenticated)
Write-Host ""
Write-Host "13. Testing Movie Rating..." -ForegroundColor Yellow
if ($authToken) {
    try {
        # First get a movie ID
        $moviesResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=1" -Method GET
        if ($moviesResponse.Data.Count -gt 0) {
            $movieId = $moviesResponse.Data[0].MovieId
            
            $ratingData = @{
                userId = $userId
                movieId = $movieId
                rating = 8
            } | ConvertTo-Json
            
            $headers = @{ Authorization = "Bearer $authToken" }
            $ratingResponse = Invoke-RestMethod -Uri "$baseUrl/api/framework/rate" -Method POST -Body $ratingData -ContentType "application/json" -Headers $headers
            Log-TestResult "Movie Rating" $true "Movie rating working" $ratingResponse.Message
        } else {
            Log-TestResult "Movie Rating" $false "No movies available for rating"
        }
    } catch {
        Log-TestResult "Movie Rating" $false "Movie rating failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Movie Rating" $false "No authentication token available"
}

# Test 14: Protected Endpoints (if authenticated)
Write-Host ""
Write-Host "14. Testing Protected Endpoints..." -ForegroundColor Yellow
if ($authToken) {
    try {
        $headers = @{ Authorization = "Bearer $authToken" }
        $usersResponse = Invoke-RestMethod -Uri "$baseUrl/api/users" -Method GET -Headers $headers
        Log-TestResult "Protected Endpoints" $true "Protected endpoints working" "Found $($usersResponse.Count) users"
    } catch {
        Log-TestResult "Protected Endpoints" $false "Protected endpoints failed" $_.Exception.Message
    }
} else {
    Log-TestResult "Protected Endpoints" $false "No authentication token available"
}

# Test 15: Detailed Movie Information
Write-Host ""
Write-Host "15. Testing Detailed Movie Information..." -ForegroundColor Yellow
try {
    $moviesResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies?page=1&pageSize=1" -Method GET
    if ($moviesResponse.Data.Count -gt 0) {
        $movieId = $moviesResponse.Data[0].MovieId
        
        try {
            $detailsResponse = Invoke-RestMethod -Uri "$baseUrl/api/movies/$movieId/details" -Method GET
            Log-TestResult "Detailed Movie Information" $true "Detailed movie info working" "Retrieved details for movie ID: $movieId"
        } catch {
            Log-TestResult "Detailed Movie Information" $false "Detailed movie info failed" $_.Exception.Message
        }
    } else {
        Log-TestResult "Detailed Movie Information" $false "No movies available for detailed info"
    }
} catch {
    Log-TestResult "Detailed Movie Information" $false "Could not retrieve movie list" $_.Exception.Message
}

# Test 16: Detailed Person Information
Write-Host ""
Write-Host "16. Testing Detailed Person Information..." -ForegroundColor Yellow
try {
    $personsResponse = Invoke-RestMethod -Uri "$baseUrl/api/persons?page=1&pageSize=1" -Method GET
    if ($personsResponse.Data.Count -gt 0) {
        $personId = $personsResponse.Data[0].PersonId
        
        try {
            $detailsResponse = Invoke-RestMethod -Uri "$baseUrl/api/persons/$personId/details" -Method GET
            Log-TestResult "Detailed Person Information" $true "Detailed person info working" "Retrieved details for person ID: $personId"
        } catch {
            Log-TestResult "Detailed Person Information" $false "Detailed person info failed" $_.Exception.Message
        }
    } else {
        Log-TestResult "Detailed Person Information" $false "No persons available for detailed info"
    }
} catch {
    Log-TestResult "Detailed Person Information" $false "Could not retrieve person list" $_.Exception.Message
}

# Summary
Write-Host ""
Write-Host "=== TEST SUMMARY ===" -ForegroundColor Green
Write-Host "Total Tests: $($testResults.Total)" -ForegroundColor White
Write-Host "Passed: $($testResults.Passed)" -ForegroundColor Green
Write-Host "Failed: $($testResults.Failed)" -ForegroundColor Red
Write-Host "Success Rate: $([math]::Round(($testResults.Passed / $testResults.Total) * 100, 2))%" -ForegroundColor Cyan

Write-Host ""
Write-Host "=== DETAILED RESULTS ===" -ForegroundColor Green
foreach ($test in $testResults.Tests) {
    $status = if ($test.Passed) { "✅" } else { "❌" }
    Write-Host "$status $($test.Name)" -ForegroundColor $(if ($test.Passed) { "Green" } else { "Red" })
    if ($test.Details) {
        Write-Host "   $($test.Details)" -ForegroundColor Cyan
    }
    if ($test.Message -and -not $test.Passed) {
        Write-Host "   Error: $($test.Message)" -ForegroundColor Yellow
    }
}

Write-Host ""
if ($testResults.Failed -eq 0) {
    Write-Host "🎉 ALL TESTS PASSED! System is fully operational!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Some tests failed. Please check the errors above." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== TEST COMPLETE ===" -ForegroundColor Green
