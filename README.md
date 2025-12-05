# Klinik - Medical Practice Management System

A clinic scheduling and management application developed for COP4870 with Professor Chris Mills, Fall 2025.

## Overview

Klinik is a medical practice management system that allows healthcare facilities to manage patients, physicians, and appointments. The application demonstrates full-stack development skills using C# and .NET technologies.

## Features

### Patient Management
- Create, read, update, and delete patient records
- Track demographics (name, address, date of birth, race, gender)
- Maintain medical notes including diagnoses and prescriptions

### Physician Management
- Manage physician profiles with license numbers and graduation dates
- Track physician specializations
- Sort and search physician directory

### Appointment Scheduling
- Schedule appointments with validation
- Prevent physician double-booking
- Prevent room double-booking
- Enforce business hours (8 AM - 5 PM, Monday-Friday)
- Color-coded display for today's appointments
- Track appointment rooms

### RESTful Web API
- Patient CRUD operations via HTTP endpoints
- Search and filter functionality
- Data Transfer Objects (DTOs) for clean API design
- JSON serialization with proper naming conventions

## Technologies Used

- **.NET 9.0** - Core framework
- **C#** - Primary programming language
- **.NET MAUI** - Cross-platform UI framework
- **ASP.NET Core** - RESTful Web API
- **MVVM Pattern** - Model-View-ViewModel architecture
- **LINQ** - Data querying and manipulation
- **INotifyPropertyChanged** - Automatic UI updates

## Project Structure

- **CLI.Klinik** - Console application for basic functionality
- **Library.Klinik** - Core business logic and data models
- **Maui.Klinik** - Cross-platform GUI application
- **WebAPI.Klinik** - RESTful API server

## Course Requirements Met

**Assignment 1** - Console application with business logic  
**Assignment 2** - MAUI application with full CRUD and navigation  
**Assignment 3** - Advanced UI features (sorting, inline buttons, room management, color coding)  
**Assignment 4** - RESTful Web API with client integration  
**Honors Features** - All 4 features completed

## Running the Application

### MAUI Application
```bash
cd Maui.Klinik
dotnet build -f net9.0-windows10.0.19041.0
```

### Web API
```bash
cd WebAPI.Klinik
dotnet run
```
The API will be available at `http://localhost:5149`

### Console Application
```bash
cd CLI.Klinik
dotnet run
```

## Author

Ian Spence  
Florida State University
COP4870
Fall 2025
