# Data Fort - Personal Digital Vault

A full-stack secure personal digital vault built with ASP.NET Core Web API and Angular.

## Project Overview

Data Fort is an upgraded Personal Digital Vault application that provides authenticated users with a secure area for managing folders, documents, credentials, public file sharing, and account settings.

The current repository also includes:
- Email verification during registration
- Forgot/reset password flow
- JWT-based authentication and role-based authorization
- Administrator dashboard, user administration, and audit logs
- Document upload/download with encryption and SHA-256 integrity verification
- Encrypted credential password storage
- Public document sharing through revocable/expiring share links
- Search across vault content

## Technology Stack

### Backend
- ASP.NET Core Web API
- .NET 10.0
- Entity Framework Core 10.0.11
- SQL Server
- JWT Bearer Authentication
- Swagger / OpenAPI
- ASP.NET Core PasswordHasher
- AES-based encryption for protected vault data
- SHA-256 hashing for document integrity
- SMTP email delivery for verification and password reset

### Frontend
- Angular 21.2.x
- TypeScript 5.9.x
- RxJS 7.8.x
- Tailwind CSS 4.1.x
- Angular Router
- Angular Reactive Forms
- HTTP interceptors and route guards

## Repository Structure

```text
DATA FORT V 12.0/
|
+-- PersonalDigitalVault.API/
|   +-- Administration/
|   +-- Authentication/
|   +-- Data/
|   +-- Migrations/
|   +-- Models/
|   +-- PublicSharing/
|   +-- Repositories/
|   +-- SecureVault/
|   +-- Storage/
|   +-- Program.cs
|   +-- appsettings.json
|   +-- Properties/launchSettings.json
|
+-- PersonalDigitalVault.Frontend/
|   +-- src/app/
|   |   +-- core/
|   |   +-- features/
|   |   +-- layouts/
|   |   +-- pages/
|   |   +-- shared/
|   +-- package.json
|   +-- package-lock.json
|   +-- angular.json
|
+-- PersonalDigitalVault.sln
```

## Main Application Modules

### 1. Authentication
Routes and API operations cover:
- Register
- Login
- Verify email
- Forgot password
- Reset password
- Change password
- View profile
- Update profile

Email verification is required before login.

### 2. Secure Vault
Users can:
- Create, update, and delete folders
- Create nested folder structures
- Upload documents
- Download documents
- Update/delete document records
- Verify document integrity
- Store encrypted credentials
- Search vault content

### 3. Public Sharing
Authenticated users can:
- Create public share links for documents
- View their share links
- Update expiry
- Revoke share links
- Delete share links

Public recipients can access a valid share without authentication.

### 4. Administration
Users with the `Administrator` role can:
- View dashboard statistics
- View users
- Activate/deactivate users
- View audit logs

## Database

Database name used by the project:

```text
PersonalDigitalVaultDB
```

Configured connection string:

```text
Server=localhost;Database=PersonalDigitalVaultDB;Trusted_Connection=True;TrustServerCertificate=True;
```

### Tables in the current EF Core model

1. Roles
2. Users
3. PasswordResetTokens
4. EmailVerificationTokens
5. Folders
6. Documents
7. Credentials
8. ShareLinks
9. AuditLogs

The project contains migrations for:
- `InitialCreate`
- `AddEmailVerificationFields`
- `AddEmailVerificationTokenTable`

### Important relationships

- Role 1 -> many Users
- User 1 -> many PasswordResetTokens
- User 1 -> many EmailVerificationTokens
- User 1 -> many Folders
- Folder 1 -> many child Folders (self-reference)
- User 1 -> many Documents
- Folder 1 -> many Documents
- User 1 -> many Credentials
- Folder 1 -> many Credentials
- User 1 -> many ShareLinks
- Document 1 -> many ShareLinks
- User 1 -> many AuditLogs

## Prerequisites

Install before running on Windows:

- Visual Studio 2022/2026 with ASP.NET and web development workload
- .NET 10 SDK
- SQL Server / SQL Server Express / LocalDB
- SQL Server Management Studio (recommended)
- Node.js with npm
- A browser such as Chrome or Edge

The frontend package metadata in this repository uses Angular CLI 21.2.23 and npm 11.19.0.

## Backend Configuration

`appsettings.json` contains the non-secret settings. The source code also requires these secret configuration values:

```text
Jwt:Key
DocumentEncryption:Key
DocumentEncryption:KeyId
Smtp:Host
Smtp:Port
Smtp:User
Smtp:Password
```

Do NOT commit real secret values to GitHub.

For local development, user-secrets are recommended.

Example commands from the backend project folder:

```bash
dotnet user-secrets init

dotnet user-secrets set "Jwt:Key" "REPLACE_WITH_A_LONG_RANDOM_SECRET"
dotnet user-secrets set "DocumentEncryption:Key" "REPLACE_WITH_BASE64_AES_KEY"
dotnet user-secrets set "DocumentEncryption:KeyId" "REPLACE_WITH_A_GUID"

dotnet user-secrets set "Smtp:Host" "smtp.example.com"
dotnet user-secrets set "Smtp:Port" "587"
dotnet user-secrets set "Smtp:User" "your-email@example.com"
dotnet user-secrets set "Smtp:Password" "your-app-password"
```

For `DocumentEncryption:Key`, provide a valid Base64-encoded AES key. A 32-byte key (AES-256) is suitable.

## Database Setup

The project already contains EF Core migrations.

From the API project directory:

```bash
dotnet ef database update
```

If the EF command is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

The application initializer creates the `User` and `Administrator` roles when the API starts.

Important: the ZIP does NOT contain a `.bak`, `.mdf`, `.ldf`, or SQL seed database. `DbInitializer` creates roles only; it does not seed the supplied administrator and user accounts.

## Running the Backend

Open a terminal in:

```text
PersonalDigitalVault.API
```

Then:

```bash
dotnet restore
dotnet build
dotnet run --launch-profile https
```

The configured launch profile exposes:

```text
HTTPS: https://localhost:7166
HTTP : http://localhost:5223
```

Swagger is available in Development at:

```text
https://localhost:7166/swagger
```

Because the application calls `UseHttpsRedirection()`, using the HTTPS profile is the recommended local setup.

### Development certificate

If the HTTPS certificate is not trusted:

```bash
dotnet dev-certs https --trust
```

## Running the Angular Frontend

Open a second terminal in:

```text
PersonalDigitalVault.Frontend
```

Install packages:

```bash
npm ci
```

Run:

```bash
npm start
```

The Angular application should open at:

```text
http://localhost:4200
```

The current `src/environments/environment.ts` points to:

```text
https://localhost:7166/api
```

That matches the backend HTTPS launch profile.

## Test Accounts

The requested test credentials are:

### Administrator
Email:
```text
Piriyaram2004@gmail.com
```

Password:
```text
Piriyaram2004@
```

Required role:
```text
Administrator
```

### Normal User
Email:
```text
Piriyaram2025@gmail.com
```

Password:
```text
Piriyaram2025@
```

Required role:
```text
User
```

### Important account note

The repository does not seed these accounts automatically.

For these credentials to work on a new database, the corresponding user records must exist, have the correct role, be active, and have `IsEmailVerified = true`.

## API Endpoint Summary

### Authentication

```text
POST /api/auth/register
POST /api/auth/login
POST /api/auth/forgot-password
POST /api/auth/reset-password
POST /api/auth/change-password
GET  /api/auth/profile
PUT  /api/auth/profile
GET  /api/auth/verify-email
```

### Secure Vault

```text
GET    /api/folders
POST   /api/folders
PUT    /api/folders/{id}
DELETE /api/folders/{id}

GET    /api/documents
POST   /api/documents
PUT    /api/documents/{id}
DELETE /api/documents/{id}
POST   /api/documents/upload
GET    /api/documents/{id}/download
POST   /api/documents/{id}/verify-integrity

GET    /api/credentials
POST   /api/credentials
PUT    /api/credentials/{id}
DELETE /api/credentials/{id}

GET    /api/search?searchTerm=...
```

### Public Sharing

```text
POST   /api/share-links
GET    /api/share-links
POST   /api/share-links/{id}/revoke
PUT    /api/share-links/{id}
DELETE /api/share-links/{id}

GET    /api/public/share/{token}
GET    /api/public/share/{token}/download
```

### Administration

```text
GET /api/admin/dashboard
GET /api/admin/users
PUT /api/admin/users/{id}/status
GET /api/admin/audit-logs
```

## Authentication / Security Summary

- JWT access tokens are generated after successful login.
- JWT contains user identity, email, username, and role claims.
- Role-based authorization protects administrator APIs.
- Passwords are stored as hashes using ASP.NET Core's `PasswordHasher<User>`.
- Credential passwords are encrypted before storage.
- Uploaded document data is protected by the document encryption service.
- SHA-256 is used to calculate/verify document integrity.
- Password reset and email verification tokens are stored as hashes.
- Share tokens are unique and can be revoked or expired.

## CORS

The backend currently allows the Angular development origin:

```text
http://localhost:4200
```

If the frontend is served from another origin, update the CORS policy in `Program.cs`.

## Common Problems

### JWT configuration error

Message:

```text
JWT key is not configured.
```

Fix by setting `Jwt:Key` through user-secrets or another configuration provider.

### Document encryption configuration error

Messages can include:

```text
Document encryption key is not configured.
Document encryption key ID is not configured.
```

Set both:
- `DocumentEncryption:Key`
- `DocumentEncryption:KeyId`

### SMTP configuration missing

Message:

```text
SMTP configuration is missing.
```

Configure:
- `Smtp:Host`
- `Smtp:Port`
- `Smtp:User`
- `Smtp:Password`

This is required for registration email verification and password reset email delivery.

### Angular points to the wrong API

Check:

```text
PersonalDigitalVault.Frontend/src/environments/environment.ts
```

The current value should match:

```text
https://localhost:7166/api
```

### HTTPS certificate warning

Run:

```bash
dotnet dev-certs https --trust
```

Then restart the API.

### Fresh clone and node_modules

Do not rely on a copied `node_modules` folder. Run:

```bash
npm ci
```

after extracting/cloning the project.

## Validation Checklist

After setup, verify in this order:

1. SQL Server is running.
2. `PersonalDigitalVaultDB` exists after migration.
3. `Roles` contains `User` and `Administrator`.
4. JWT and document-encryption secrets are configured.
5. API starts on `https://localhost:7166`.
6. Swagger opens.
7. Angular starts on `http://localhost:4200`.
8. The frontend can reach the API without CORS errors.
9. A verified user can log in.
10. An administrator can open `/admin`.
11. A user can create a folder and upload/download a document.
12. Document integrity verification returns a valid result.
13. A user can create and revoke a public share link.

## Verification Performed While Preparing This README

Repository inspection confirmed:
- Backend target framework: `net10.0`
- EF Core / SQL Server packages: `10.0.11`
- Angular CLI: `21.2.23`
- Angular packages: `21.2.x`
- Tailwind CSS: `4.1.12`
- Database model: 9 tables
- EF Core migrations: 3
- Backend launch ports: HTTPS 7166 and HTTP 5223
- Frontend API URL: `https://localhost:7166/api`

A frontend build was attempted in the inspection environment, but the ZIP includes Windows-native `node_modules` binaries and was being built on Linux. Re-running `npm ci` on the actual Windows development machine is the correct fix before `npm start` / `npm run build`.
