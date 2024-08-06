# Patient Administration System

## Overview

The Patient Administration System is a web application designed to manage patient information efficiently. It consists of a .NET backend and a React frontend.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Setup and Installation](#setup-and-installation)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Contributing](#contributing)

## Technologies Used

### Backend
- .NET Core
- Entity Framework Core
- AutoMapper
- Swagger

### Frontend
- React 18
- TypeScript
- Vite

## Project Structure

### Backend

```
PatientAdministrationSystem/
├── PatientAdministrationSystem.Application/
│   ├── Dependencies/
│   ├── DTOs/
│   │   ├── HospitalDto.cs
│   │   ├── PatientDto.cs
│   │   ├── PatientVisitDto.cs
│   │   └── VisitDto.cs
│   ├── Entities/
│   │   ├── Entity.cs
│   │   ├── HospitalEntity.cs
│   │   ├── PatientEntity.cs
│   │   ├── PatientHospitalRelation.cs
│   │   └── VisitEntity.cs
│   ├── Mapping/
│   │   └── MappingProfile.cs
│   ├── Repositories/
│   │   └── Interfaces/
│   │       └── IPatientsRepository.cs
│   ├── Services/
│   │   └── Interfaces/
│   │       ├── IPatientsService.cs
│   │       └── PatientsService.cs
│   └── Utilities/
│       └── DataSeed.cs
│
├── PatientAdministrationSystem.Infra/
│   ├── Dependencies/
│   ├── Repositories/
│   │   ├── PatientsRepository.cs
│   │   └── Interfaces/
│   │       └── IHciDataContext.cs
│   └── HciDataContext.cs
│
├── PatientAdministrationSystem.API/
│   ├── Connected Services/
│   ├── Dependencies/
│   ├── Properties/
│   ├── Controllers/
│   │   └── PatientsController.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── sample-data.json
│
├── PatientAdministrationSystem.Tests/
│   └── PatientsControllerTests.cs
│
└── PatientAdministrationSystem.sln
```

### Frontend

```
patient-administration-system-app/
├── public/
│   └── vite.svg
│
├── src/
│   ├── api/
│   │   ├── api-client.ts
│   │   └── api-client.test.ts
│   ├── components/
│   │   ├── ExportDataForm.tsx
│   │   ├── Footer.tsx
│   │   ├── Header.tsx
│   │   ├── PatientDetailsTable.tsx
│   │   └── VisitsModal.tsx
│   ├── services/
│   │   ├── patient-interface.ts
│   │   └── patient-service.ts
│   ├── App.tsx
│   ├── App.css
│   ├── index.tsx
│   ├── index.css
│   └── main.tsx
│
├── .eslintrc.cjs
├── .gitignore
├── jest.config.mjs
├── package-lock.json
├── package.json
├── README.md
├── tsconfig.app.json
├── tsconfig.json
├── tsconfig.node.json
└── vite.config.ts
```

## Setup and Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)
- [Visual Studio or Visual Studio Code](https://visualstudio.microsoft.com/)

### Backend Setup

1. Clone the repository:
    ```sh
    https://github.com/ashiqali/hci-take-home-interview-v3.git
    ```

2. Navigate to the backend project directory:
    ```sh
    cd PatientAdministrationSystem.Api
    ```

3. Restore the .NET dependencies:
    ```sh
    dotnet restore
    ```

### Frontend Setup

1. Navigate to the frontend project directory:
    ```sh
    cd PatientAdministrationSystem.App
    ```

2. Install the npm dependencies:
    ```sh
    npm install
    ```

## Running the Application

### Backend

Start the .NET backend:
```sh
cd PatientAdministrationSystem.Api
dotnet run
```

### Frontend

Start the React frontend:
```sh
npm run dev
```

The backend will be running at `https://localhost:7260` and the frontend at `http://localhost:5173`.

## Testing

### Backend

To run the tests for the backend:
```sh
cd PatientAdministrationSystem.Tests
dotnet test
```

### Frontend

To run the tests for the frontend:
```sh
npm test
```

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

1. Fork the repository.
2. Create a new branch:
    ```sh
    git checkout -b feature-branch
    ```
3. Commit your changes:
    ```sh
    git commit -m 'Add some feature'
    ```
4. Push to the branch:
    ```sh
    git push origin feature-branch
    ```
5. Open a pull request.
