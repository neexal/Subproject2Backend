# IMDB System Testing Guide

## 🧪 **Complete Testing Guide for IMDB Backend System**

This guide will teach you how to test your IMDB backend system comprehensively.

## 📋 **Prerequisites**

Before testing, ensure you have:
- ✅ **PostgreSQL Database** running with IMDB data
- ✅ **.NET 9.0** installed
- ✅ **PowerShell** or **Command Prompt** access
- ✅ **Application running** on `http://localhost:5078`

## 🚀 **Quick Start Testing**

### **Step 1: Start the Application**

1. **Open Terminal/Command Prompt**
2. **Navigate to the project directory**:
   ```bash
   cd IMDB\IMDB.WebServiceLayer
   ```
3. **Start the application**:
   ```bash
   dotnet run
   ```
4. **Wait for the message**: `Now listening on: http://localhost:5078`

### **Step 2: Run the Complete Test Suite**

1. **Open a new terminal window**
2. **Navigate to the project root**:
   ```bash
   cd C:\Users\LENOVO\Desktop\RUC\Subproject2Backend
   ```
3. **Run the test script**:
   ```powershell
   .\complete_test_suite.ps1
   ```

## 📊 **What the Test Suite Tests**

The complete test suite performs **16 comprehensive tests**:

### **1. Application Status Check**
- ✅ Verifies the application is running
- ✅ Checks if endpoints are responding

### **2. Database Connection Test**
- ✅ Tests database connectivity
- ✅ Verifies data is accessible
- ✅ Checks movie and person counts

### **3. Authentication Tests**
- ✅ User registration
- ✅ User login with JWT tokens
- ✅ Token verification
- ✅ Protected endpoint access

### **4. Core Functionality Tests**
- ✅ Movies endpoint with pagination
- ✅ Persons endpoint with pagination
- ✅ Movie search functionality
- ✅ Person search functionality

### **5. Framework Functionality Tests**
- ✅ Framework string search
- ✅ Framework name search
- ✅ Movie bookmarking
- ✅ Movie rating
- ✅ Protected endpoint access

### **6. Advanced Feature Tests**
- ✅ Detailed movie information
- ✅ Detailed person information
- ✅ Search history tracking
- ✅ User-specific data access

## 🔧 **Manual Testing Methods**

### **Method 1: PowerShell Testing**

#### **Test Basic Connectivity**
```powershell
# Test if application is running
Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=1" -Method GET

# Test database connection
$response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=1" -Method GET
Write-Host "Total movies: $($response.TotalCount)"
```

#### **Test Authentication**
```powershell
# Login
$loginData = @{
    email = "test@example.com"
    password = "password123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "http://localhost:5078/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
$token = $loginResponse.Token

# Test protected endpoint
$headers = @{ Authorization = "Bearer $token" }
$usersResponse = Invoke-RestMethod -Uri "http://localhost:5078/api/users" -Method GET -Headers $headers
```

#### **Test Framework Functions**
```powershell
# Test string search
$searchData = @{
    userId = 1
    searchString = "test"
} | ConvertTo-Json

$headers = @{ Authorization = "Bearer $token" }
$searchResponse = Invoke-RestMethod -Uri "http://localhost:5078/api/framework/search/string" -Method POST -Body $searchData -ContentType "application/json" -Headers $headers
```

### **Method 2: HTTP File Testing**

Create a file called `test_endpoints.http`:

```http
### Test Basic Connectivity
GET http://localhost:5078/api/movies?page=1&pageSize=5

### Test Authentication
POST http://localhost:5078/api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "password123"
}

### Test Protected Endpoint (replace YOUR_TOKEN_HERE with actual token)
GET http://localhost:5078/api/users
Authorization: Bearer YOUR_TOKEN_HERE

### Test Framework Search (replace YOUR_TOKEN_HERE with actual token)
POST http://localhost:5078/api/framework/search/string
Content-Type: application/json
Authorization: Bearer YOUR_TOKEN_HERE

{
  "userId": 1,
  "searchString": "test"
}
```

### **Method 3: Browser Testing**

For GET endpoints, you can test directly in the browser:
- `http://localhost:5078/api/movies?page=1&pageSize=5`
- `http://localhost:5078/api/persons?page=1&pageSize=5`

## 📈 **Understanding Test Results**

### **✅ Success Indicators**
- **Green checkmarks** (✅) indicate successful tests
- **Response data** shows actual data retrieved
- **Status codes** should be 200 for successful requests

### **❌ Error Indicators**
- **Red X marks** (❌) indicate failed tests
- **Error messages** show what went wrong
- **Status codes** like 400, 401, 500 indicate problems

### **Common Error Codes**
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required
- **404 Not Found**: Endpoint doesn't exist
- **500 Internal Server Error**: Server-side error

## 🛠️ **Troubleshooting Common Issues**

### **Issue 1: Application Not Running**
**Symptoms**: "Unable to connect to remote server"
**Solution**:
1. Check if application is running: `netstat -an | findstr :5078`
2. Start the application: `dotnet run` in the WebServiceLayer directory
3. Wait for "Now listening on: http://localhost:5078"

### **Issue 2: Database Connection Failed**
**Symptoms**: 500 Internal Server Error on data endpoints
**Solution**:
1. Check PostgreSQL is running: `netstat -an | findstr :5432`
2. Verify database credentials in connection string
3. Check if IMDB database exists and has data

### **Issue 3: Authentication Failed**
**Symptoms**: 401 Unauthorized errors
**Solution**:
1. Check if user exists in database
2. Verify password is correct
3. Check JWT token expiration

### **Issue 4: Framework Functions Not Working**
**Symptoms**: 400 Bad Request on framework endpoints
**Solution**:
1. Check if PostgreSQL functions are deployed
2. Verify function parameters are correct
3. Check database function results

## 📊 **Performance Testing**

### **Load Testing with PowerShell**
```powershell
# Test multiple concurrent requests
$jobs = @()
for ($i = 1; $i -le 10; $i++) {
    $jobs += Start-Job -ScriptBlock {
        Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=10" -Method GET
    }
}

# Wait for all jobs to complete
$jobs | Wait-Job | Receive-Job
$jobs | Remove-Job
```

### **Response Time Testing**
```powershell
# Measure response time
$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=100" -Method GET
$stopwatch.Stop()
Write-Host "Response time: $($stopwatch.ElapsedMilliseconds) ms"
```

## 🔍 **Advanced Testing Scenarios**

### **Test 1: Large Dataset Handling**
```powershell
# Test with large page sizes
$response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=1000" -Method GET
Write-Host "Retrieved $($response.Data.Count) movies in $($response.TotalCount) total"
```

### **Test 2: Search Performance**
```powershell
# Test search with different terms
$searchTerms = @("action", "drama", "comedy", "horror")
foreach ($term in $searchTerms) {
    $searchData = @{ searchTerm = $term; page = 1; pageSize = 10 } | ConvertTo-Json
    $response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies/search" -Method POST -Body $searchData -ContentType "application/json"
    Write-Host "Search '$term': $($response.Data.Count) results"
}
```

### **Test 3: Framework Function Stress Test**
```powershell
# Test framework functions with various parameters
$testCases = @(
    @{ userId = 1; searchString = "test" },
    @{ userId = 1; searchString = "movie" },
    @{ userId = 1; searchString = "action" }
)

foreach ($testCase in $testCases) {
    $searchData = $testCase | ConvertTo-Json
    $headers = @{ Authorization = "Bearer $token" }
    $response = Invoke-RestMethod -Uri "http://localhost:5078/api/framework/search/string" -Method POST -Body $searchData -ContentType "application/json" -Headers $headers
    Write-Host "Search '$($testCase.searchString)': $($response.Count) results"
}
```

## 📋 **Testing Checklist**

### **Pre-Testing Checklist**
- [ ] Application is running on port 5078
- [ ] PostgreSQL database is running
- [ ] IMDB database has data (movies and persons)
- [ ] All PostgreSQL functions are deployed
- [ ] Test user exists in database

### **Core Functionality Checklist**
- [ ] Movies endpoint returns data
- [ ] Persons endpoint returns data
- [ ] Pagination works correctly
- [ ] Search functionality works
- [ ] Detailed information endpoints work

### **Authentication Checklist**
- [ ] User registration works
- [ ] User login works
- [ ] JWT tokens are generated
- [ ] Token verification works
- [ ] Protected endpoints require authentication

### **Framework Functionality Checklist**
- [ ] String search works
- [ ] Name search works
- [ ] Movie bookmarking works
- [ ] Movie rating works
- [ ] Search history tracking works

## 🎯 **Expected Results**

### **Successful Test Results**
- **Total Tests**: 16
- **Expected Passed**: 16
- **Expected Failed**: 0
- **Success Rate**: 100%

### **Data Expectations**
- **Movies**: Should have 158,999+ movies
- **Persons**: Should have 513,987+ persons
- **Search Results**: Should return relevant results
- **Framework Functions**: Should execute successfully

## 🚨 **Emergency Testing**

If the main test suite fails, run these basic tests:

```powershell
# Test 1: Basic connectivity
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=1" -Method GET
    Write-Host "✅ Basic connectivity: OK"
} catch {
    Write-Host "❌ Basic connectivity: FAILED"
}

# Test 2: Database connection
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=1" -Method GET
    if ($response.TotalCount -gt 0) {
        Write-Host "✅ Database connection: OK ($($response.TotalCount) movies)"
    } else {
        Write-Host "❌ Database connection: NO DATA"
    }
} catch {
    Write-Host "❌ Database connection: FAILED"
}

# Test 3: Authentication
try {
    $loginData = @{ email = "test@example.com"; password = "password123" } | ConvertTo-Json
    $response = Invoke-RestMethod -Uri "http://localhost:5078/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
    Write-Host "✅ Authentication: OK"
} catch {
    Write-Host "❌ Authentication: FAILED"
}
```

## 📚 **Additional Resources**

### **Useful Commands**
```bash
# Check if application is running
netstat -an | findstr :5078

# Check if PostgreSQL is running
netstat -an | findstr :5432

# Check application logs
dotnet run --verbosity detailed

# Test database connection
psql -h localhost -U postgres -d imdb -c "SELECT COUNT(*) FROM movie;"
```

### **Debugging Tips**
1. **Check application logs** for detailed error messages
2. **Verify database connection** with direct SQL queries
3. **Test endpoints individually** to isolate issues
4. **Check network connectivity** and firewall settings
5. **Verify all dependencies** are installed correctly

## 🎉 **Success!**

When all tests pass, you'll see:
```
🎉 ALL TESTS PASSED! System is fully operational!
```

This means your IMDB backend system is working perfectly and ready for production use!

---

**Happy Testing! 🧪✨**
