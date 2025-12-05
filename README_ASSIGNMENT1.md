# Medical Practice Charting System - Assignment 1

## Overview
This is a console-based medical practice charting system built in C# that allows management of patients, physicians, and appointments.

## Project Structure

### Library.Klinik (Class Library)
Contains the core business logic and data models:

#### Models
- **Patient.cs**: Represents a patient with demographics (name, address, birthdate, race, gender) and medical notes
- **Physician.cs**: Represents a physician with demographics (name, license number, graduation date) and specializations
- **Appointment.cs**: Represents an appointment between a patient and physician

#### Services
- **PatientService.cs**: Manages patient creation, retrieval, updates, deletions, and medical notes
- **PhysicianService.cs**: Manages physician creation, retrieval, updates, deletions, and specializations
- **AppointmentService.cs**: Manages appointment creation with validation for:
  - Double-booking prevention (no two appointments for same physician at same time)
  - Time constraints (only 8am-5pm Monday-Friday)

#### ChartingSystemManager.cs
Aggregates all services for easy access from the console application

### CLI.Klinik (Console Application)
Contains the main Program.cs with the user interface and menu system:
- Main menu for selecting between Patients, Physicians, or Appointments
- Patient management: CRUD operations, medical notes management
- Physician management: CRUD operations, specialization management
- Appointment management: CRUD operations, physician/patient appointment queries

## Features Implemented

### Patient Management
✅ Create patients with demographics (name, address, birthdate, race, gender)
✅ View all patients
✅ View individual patient details
✅ Update patient information
✅ Delete patients
✅ Add medical notes (diagnosis and prescription)
✅ View medical notes for a patient

### Physician Management
✅ Create physicians with demographics (name, license number, graduation date)
✅ View all physicians
✅ View individual physician details
✅ Update physician information
✅ Delete physicians
✅ Add specializations to physicians
✅ View specializations for a physician

### Appointment Management
✅ Create appointments with patient, physician, and reason
✅ View all appointments
✅ View individual appointment details
✅ Update appointments
✅ Delete appointments
✅ View appointments for a specific physician (prevents double-booking)
✅ View appointments for a specific patient
✅ Enforce time constraints: 8am-5pm Monday-Friday only
✅ Prevent physician double-booking (no overlapping appointments at same hour)

## Requirements Fulfillment

### Assignment 1 Requirements
- ✅ Application allows for creation of patients
- ✅ Application tracks demographics: name, address, birthdate, race, gender
- ✅ Application tracks medical notes including diagnoses and prescriptions
- ✅ Application allows for creation of physicians
- ✅ Application tracks demographics: name, license number, graduation date, specializations
- ✅ Application allows for creation of appointments
- ✅ Application does NOT allow a physician to be double-booked
- ✅ Appointments can only be made 8am-5pm Monday-Friday

### Course Requirements (C- Grade)
- ✅ Ability to create computational solutions to problems in C#
- ✅ Encapsulation used to structure solutions (Models and Services)
- ✅ Libraries used to modularize applications (separate Library.Klinik)

## Usage

### Running the Application
```bash
cd C:\Users\Ian\Desktop\COP4870
dotnet run --project CLI.Klinik
```

### Main Menu Navigation
1. **Manage Patients** - Access patient CRUD and medical notes
2. **Manage Physicians** - Access physician CRUD and specializations
3. **Manage Appointments** - Access appointment management
4. **Exit** - Close the application

## Technical Details

- **Framework**: .NET 9.0
- **Language**: C# 13
- **Architecture**: Service-based with model classes
- **Data Storage**: In-memory collections (survives for duration of application run)

## Design Patterns Used

- **Service Pattern**: Business logic separated from UI in service classes
- **Manager Pattern**: ChartingSystemManager aggregates services
- **Validation Pattern**: AppointmentService validates business rules

## Error Handling

- Invalid input handling with try-catch blocks
- Graceful error messages for validation failures
- Null checking for database lookups

## Notes

This implementation provides a solid foundation for medical practice management. Future enhancements could include:
- Data persistence (database or file storage)
- Additional validation rules
- Reporting and analytics features
- Multi-user support with authentication
