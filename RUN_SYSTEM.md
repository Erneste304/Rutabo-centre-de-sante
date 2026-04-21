# 🚀 Hospital Management System - Complete Startup Guide

## ✅ System Status
- ✅ Backend API: Ready
- ✅ Blazor UI: Ready (13 pages + 29 total pages)
- ✅ Database: SQLite configured
- ✅ Services: 20+ services available
- ✅ Documentation: Complete

---

## 🎯 Quick Start Commands

### Option 1: Run Everything (Recommended)

#### Terminal 1 - Start the API Server
```bash
cd HospitalManagementSystem/HospitalManagementSystem.API
dotnet run
```
**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5051
```

#### Terminal 2 - Start the Blazor UI
```bash
cd HospitalManagementSystem/HospitalManagementSystem.Blazor
dotnet run
```
**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

#### Terminal 3 - (Optional) Run Tests
```bash
cd HospitalManagementSystem/HospitalManagementSystem.Tests
dotnet test
```

---

## 📍 Access Points

Once everything is running, open these URLs in your browser:

| Service | URL | Purpose |
|---------|-----|---------|
| **UI** | http://localhost:5000/ | Main application |
| **API** | http://localhost:5051/ | REST API |
| **Swagger** | http://localhost:5051/swagger | API documentation |

---

## 🔐 Default Login Credentials

Use these credentials to log in:

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@123` |
| Doctor | `doctor` | `Admin@123` |
| Patient | `patient` | `Admin@123` |
| Receptionist | `reception` | `Admin@123` |

---

## 📊 Available Pages

### Dashboard & Overview
- 📊 Dashboard - Main overview with statistics

### Patient Management
- 👥 Patients - Patient management system
- 📋 Medical Records - Patient history

### Clinical Operations
- 📅 Appointments - Appointment scheduling
- 💊 Pharmacy - Prescription management
- 🩸 Blood Bank - Blood inventory

### Administrative
- 💰 Billing - Invoice management
- 📦 Inventory - Supply tracking
- 👨‍⚕️ Staff - Personnel management
- 🏥 Rooms - Room & bed management

### Emergency & Services
- 🚨 Emergency Alerts - Alert system
- 🚑 Ambulance - Fleet management

### Analytics
- 📈 Reports - Business intelligence

---

## 🛠️ Advanced Commands

### Build the Solution
```bash
cd HospitalManagementSystem
dotnet build
```

### Clean Build
```bash
cd HospitalManagementSystem
dotnet clean
dotnet build
```

### Run Specific Project
```bash
# API only
dotnet run --project HospitalManagementSystem.API/HospitalManagementSystem.API.csproj

# Blazor UI only
dotnet run --project HospitalManagementSystem.Blazor/HospitalManagementSystem.Blazor.csproj

# Console app
dotnet run --project HospitalManagementSystem.Console/HospitalManagementSystem.Console.csproj

# Tests
dotnet test HospitalManagementSystem.Tests/HospitalManagementSystem.Tests.csproj
```

### Run in Release Mode
```bash
# API
dotnet run --project HospitalManagementSystem.API/HospitalManagementSystem.API.csproj --configuration Release

# UI
dotnet run --project HospitalManagementSystem.Blazor/HospitalManagementSystem.Blazor.csproj --configuration Release
```

---

## 🐳 Docker Commands (Optional)

### Build Docker Image
```bash
docker build -t hospital-management-system .
```

### Run Docker Container
```bash
docker run -p 5000:5000 -p 5051:5051 hospital-management-system
```

---

## 📋 Step-by-Step Setup

### Step 1: Navigate to Project
```bash
cd HospitalManagementSystem
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build Solution
```bash
dotnet build
```

### Step 4: Run API (Terminal 1)
```bash
cd HospitalManagementSystem.API
dotnet run
```

### Step 5: Run UI (Terminal 2)
```bash
cd HospitalManagementSystem.Blazor
dotnet run
```

### Step 6: Open Browser
- Navigate to: **http://localhost:5000/**
- Login with credentials above

---

## 🔍 Troubleshooting

### Port Already in Use
If port 5000 or 5051 is already in use:

```bash
# Find process using port 5000
lsof -i :5000

# Find process using port 5051
lsof -i :5051

# Kill process (replace PID with actual process ID)
kill -9 PID
```

### Database Issues
```bash
# Reset database
cd HospitalManagementSystem.API
dotnet ef database drop
dotnet ef database update
```

### Dependencies Not Found
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

---

## 📊 Project Structure

```
HospitalManagementSystem/
├── HospitalManagementSystem.API/          # REST API (Port 5051)
├── HospitalManagementSystem.Blazor/       # UI (Port 5000)
├── HospitalManagementSystem.Core/         # Models & Interfaces
├── HospitalManagementSystem.Data/         # Database Layer
├── HospitalManagementSystem.Console/      # Console App
└── HospitalManagementSystem.Tests/        # Unit Tests
```

---

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Project
```bash
dotnet test HospitalManagementSystem.Tests/HospitalManagementSystem.Tests.csproj
```

### Run Tests with Verbose Output
```bash
dotnet test --verbosity detailed
```

---

## 📈 Performance Tips

1. **Use Release Mode for Production**
   ```bash
   dotnet run --configuration Release
   ```

2. **Enable Caching**
   - Already configured in API

3. **Database Optimization**
   - SQLite is optimized for development
   - Use SQL Server for production

4. **Browser Optimization**
   - Clear cache if UI doesn't update
   - Use Chrome DevTools for debugging

---

## 🔐 Security Notes

- Change default credentials in production
- Use HTTPS in production
- Implement proper authentication
- Validate all user inputs
- Use environment variables for secrets

---

## 📞 Common Issues & Solutions

### Issue: "Connection refused" on API
**Solution:** Ensure API is running on port 5051
```bash
cd HospitalManagementSystem.API
dotnet run
```

### Issue: UI shows blank page
**Solution:** 
1. Clear browser cache
2. Check browser console for errors
3. Verify API is running

### Issue: Login fails
**Solution:**
1. Check default credentials
2. Verify database is seeded
3. Check API logs

### Issue: Slow performance
**Solution:**
1. Run in Release mode
2. Check database queries
3. Clear browser cache

---

## ✅ Verification Checklist

Before considering the system ready:

- [ ] API running on http://localhost:5051/
- [ ] UI running on http://localhost:5000/
- [ ] Can access Swagger at http://localhost:5051/swagger
- [ ] Can login with default credentials
- [ ] Dashboard loads with data
- [ ] All 13 pages are accessible
- [ ] No console errors
- [ ] Database is seeded

---

## 🎯 Next Steps

1. **Customize Configuration**
   - Edit `appsettings.json` files
   - Update connection strings if needed

2. **Add Real Data**
   - Use API endpoints to add data
   - Or modify database seeder

3. **Implement Features**
   - Connect services to UI
   - Add form validation
   - Implement error handling

4. **Deploy**
   - Build for production
   - Deploy to server
   - Configure SSL/TLS

---

## 📚 Documentation

- **Main README**: See `Readme.md` for complete project documentation
- **API Docs**: Available at http://localhost:5051/swagger
- **Code Comments**: Check source files for implementation details

---

## 🚀 Quick Reference

| Task | Command |
|------|---------|
| Start API | `cd HospitalManagementSystem.API && dotnet run` |
| Start UI | `cd HospitalManagementSystem.Blazor && dotnet run` |
| Run Tests | `dotnet test` |
| Build Solution | `dotnet build` |
| Clean Build | `dotnet clean && dotnet build` |
| View Swagger | http://localhost:5051/swagger |
| Access UI | http://localhost:5000/ |

---

## 💡 Tips

1. **Use Multiple Terminals**
   - Terminal 1: API
   - Terminal 2: UI
   - Terminal 3: Tests/Monitoring

2. **Monitor Logs**
   - Check console output for errors
   - Use browser DevTools for UI debugging

3. **Hot Reload**
   - Changes to code automatically reload
   - No need to restart (usually)

4. **Database**
   - SQLite file: `hospital.db`
   - Located in API project root

---

**Status**: ✅ System Ready to Run
**Last Updated**: April 21, 2026
