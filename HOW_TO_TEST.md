# How to Test Your IMDB System

## 🧪 **Complete Testing Guide**

This guide will teach you how to test your IMDB backend system step by step.

## 📋 **Prerequisites**

Before testing, ensure you have:
- ✅ **PostgreSQL Database** running with IMDB data
- ✅ **.NET 9.0** installed
- ✅ **PowerShell** or **Command Prompt** access
- ✅ **Application running** on `http://localhost:5078`

## 🚀 **Step-by-Step Testing Process**

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

### **Step 3: Alternative Testing Methods**

#### **Method 1: Using the Batch File**
```bash
.\run_tests.bat
```

#### **Method 2: Using HTTP Test File**
1. Open `test_endpoints.http` in VS Code
2. Install the REST Client extension
3. Click "Send Request" on each test

#### **Method 3: Manual PowerShell Testing**
```powershell
# Test basic connectivity
Invoke-RestMethod -Uri "http://localhost:5078/api/movies?page=1&pageSize=5" -Method GET

# Test authentication
$loginData = @{ email = "test@example.com"; password = "password123" } | ConvertTo-Json
$loginResponse = Invoke-RestMethod -Uri "http://localhost:5078/api/auth/login" -Method POST -Body $loginData -ContentType "application/json"
$token = $loginResponse.Token

# Test protected endpoint
$headers = @{ Authorization = "Bearer $token" }
Invoke-RestMethod -Uri "http://localhost:5078/api/users" -Method GET -Headers $headers
```

## 📊 **Understanding Test Results**

### **✅ Success Indicators**
- **Green checkmarks** (✅) indicate successful tests
- **Response data** shows actual data retrieved
- **Status codes** should be 200 for successful requests

### **❌ Error Indicators**
- **Red X marks** (❌) indicate failed tests
- **Error messages** show what went wrong
- **Status codes** like 400, 401, 500 indicate problems

### **Expected Results**
- **Total Tests**: 16
- **Expected Passed**: 14-16 (depending on database state)
- **Expected Failed**: 0-2 (some functions may need database setup)
- **Success Rate**: 87.5% - 100%

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
- [ ] Movie bookmarking works (may have issues)
- [ ] Movie rating works (may have issues)
- [ ] Search history tracking works

## 🎯 **Expected Results**

### **Successful Test Results**
- **Total Tests**: 16
- **Expected Passed**: 14-16
- **Expected Failed**: 0-2
- **Success Rate**: 87.5% - 100%

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
