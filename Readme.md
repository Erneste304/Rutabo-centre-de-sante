# 🏥 Rutabo Centre de Santé — Hospital Management System

<p align="center">
  <img src="https://img.shields.io/badge/Platform-.NET%209%20%7C%20Blazor-512BD4?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Database-SQLite%20%7C%20EF%20Core-003B57?style=for-the-badge&logo=sqlite" />
  <img src="https://img.shields.io/badge/Status-In%20Development-orange?style=for-the-badge" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" />
</p>

> A comprehensive, modern Hospital Management System built with **Blazor** and **.NET 9**, designed to digitize and streamline all core operations of a health centre — from patient registration to ambulance dispatch.

---

## 👨‍💻 Developer

| Field | Details |
|---|---|
| **Name** | Erneste GISUBIZO |
| **GitHub** | [@Erneste304](https://github.com/Erneste304) |
| **Organisation / Brand** | **Erneste304Tech** |
| **Email** | [erneste304tech@gmail.com](mailto:erneste304tech@gmail.com) |
| **Role** | Web Developer |
| **University** | University of Rwanda |
| **School** | School of ICT |
| **Department** | Information Technology |

---

## 📌 Project Overview

**Rutabo Centre de Santé – Hospital Management System** is a full-stack web application that allows hospital administrators, doctors, nurses, receptionists, and patients to interact with a single integrated digital platform.

The system covers every aspect of a modern health centre:

- 🧑‍⚕️ **Patient & Doctor Management** — Registration, profiles, and records
- 📅 **Appointments** — Scheduling, confirmation, and tracking
- 🏥 **Room & Ward Management** — Rooms, room types, and assignments
- 💊 **Pharmacy & Medicine** — Medicine stock, pharmacy dispensing
- 🩺 **Medical Records** — Visit history, diagnoses, prescriptions, linked medicines
- 💳 **Billing & Payments** — Invoice generation, insurance, payment status
- 🚑 **Ambulance Service** — Fleet management and dispatch logs
- 🩸 **Blood Bank** — Blood type inventory tracking
- 🧹 **Cleaning Service** — Room cleaning schedules and logs
- 🔔 **Notifications & Audit Logs** — System-wide activity tracking

---

## 🗄️ Database Schema

The system uses a fully normalized relational database. Key tables include:

`Patients` · `Doctors` · `Appointments` · `Medical_Records` · `Billing` · `Pharmacy` · `Medicine` · `Rooms` · `Room_Types` · `Room_Assignments` · `Ambulance` · `Ambulance_Log` · `Blood_Bank` · `Cleaning_Service` · `Departments` · `Doctor_Department` · `Medical_Records_Medicine` · `Prescriptions`

---

## 🏗️ Project Structure

```
HospitalManagementSystem/
├── HospitalManagementSystem.Blazor/    # Frontend (Blazor Server)
│   ├── Pages/                          # Razor pages (Dashboard, Patients, Doctors...)
│   ├── Layout/                         # NavMenu, MainLayout
│   └── wwwroot/                        # Static assets (CSS, JS)
│
├── HospitalManagementSystem.API/       # REST API layer
│
├── HospitalManagementSystem.Core/      # DTOs / Models / Interfaces
│
├── HospitalManagementSystem.Data/      # EF Core Data Layer
│   ├── Entities/                       # All C# database entity classes
│   └── Data/                           # ApplicationDbContext + Migrations
│
└── HospitalManagementSystem.Tests/     # Unit & Integration Tests
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Any modern browser (Chrome, Edge, Firefox)

### Run Locally

```bash
# 1. Clone the repository
git clone https://github.com/Erneste304/Rutabo-centre-de-sante.git
cd Rutabo-centre-de-sante/HospitalManagementSystem

# 2. Run the Blazor app
dotnet run --project HospitalManagementSystem.Blazor

# 3. (Optional) Run the API
dotnet run --project HospitalManagementSystem.API/HospitalManagementSystem.API.csproj

# 4. (Optional) Run the Console
dotnet run --project HospitalManagementSystem.Console/HospitalManagementSystem.Console.csproj

# 5. Run tests
dotnet test HospitalManagementSystem.Tests/HospitalManagementSystem.Tests.csproj
```

Then open your browser at **https://localhost:5001**

### Default Login Credentials

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@123` |
| Doctor | `doctor` | `Admin@123` |
| Patient | `patient` | `Admin@123` |
| Receptionist | `reception` | `Admin@123` |

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (.NET 9) |
| Backend / API | ASP.NET Core Web API |
| ORM | Entity Framework Core 9 |
| Database | SQLite (development) |
| Authentication | ASP.NET Core Identity |
| UI Framework | Bootstrap 5 + Custom CSS |

---

## ✨ Features

- [x] Role-based access control (Admin, Doctor, Nurse, Patient, Receptionist)
- [x] Patient registration and profile management
- [x] Doctor and department management (many-to-many via Doctor_Department)
- [x] Appointment scheduling and status tracking
- [x] Medical records with linked medicines (Medical_Records_Medicine)
- [x] Pharmacy and medicine stock management
- [x] Room types, room assignments, and cleaning services
- [x] Billing, invoicing, and payment tracking
- [x] Ambulance fleet and dispatch log
- [x] Blood bank inventory per blood type
- [x] Notifications and audit logs

---

## 📄 License

This project is licensed under the **MIT License**.

---

<p align="center">
  Made with ❤️ by <strong>Erneste GISUBIZO</strong> — <a href="mailto:erneste304tech@gmail.com">erneste304tech@gmail.com</a><br/>
  <em>Erneste304Tech · University of Rwanda, School of ICT · Information Technology</em>
</p>