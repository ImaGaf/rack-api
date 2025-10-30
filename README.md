# Rack Logic

Logistic & Warehouse Management System.

## Description:

Rack Logic is a project designed to ensure the safety of racks in warehouses. At regular intervals, safety inspections are carried out on the racks, and maintenance or replacement requests are created to comply with Australian workplace safety regulations.

## Features:

- User registration and payment are handled through in-app purchases.
- Users to create warehouse layout diagrams.
- Enables the creation of custom configurations for each type of storage rack.
- Create defect logs when racks require repairs or replacements.

## Architecture Overview:

Uses the Clean Architecture model by Ardalis.
For more details, refer to the following [link](https://github.com/ardalis/CleanArchitecture/tree/ardalis-patch-5).

## Tech Stack:

- .NET 8.0
- Entity Framework
- Identity Server
- Azure blob storage
- Fast Endpoints

## Requirements:

- .NET SDK 8.0
- MS SQL
- Visual Studio

## Run Project:

- Clone project.
- Environment Variable Configuration.
- Update the connection string and re-run the migrations if the previous environment is not available.
- Run project with **https**.

## Environment Variables:

### App Store config key:

- AUDIENCE
- BUNDLE_ID
- ISSUER_ID
- KEY_ID
- PRIVATE_KEY_APPSTORE

### GG Play config key:

- PLAY_KEY_PATH

### Azure blob storage key:

- AZ_CONNECTION_STRING
- AZ_Blob_Container_Name

### How to set environment variables on Windows:

Use the following command and run it via Command Prompt (CMD): **setx {ENV key} "{value}"**

### Deploying the app to the production environment:

- Request access permissions on Azure to be able to deploy the application.
- Follow the instructions in the article to deploy to Azure App Service. See the [link](https://learn.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vscode?view=aspnetcore-9.0) for details.

### Notes:

- ENV values are sensitive and should be kept confidential. When running the project, please request them from the project manager.
- **master** is the main branch of the project.

### How to add migration

- cd src\RackLogic.Web
- dotnet ef migrations add Initial -c AppDbContext -p ../RackLogic.Infrastructure/RackLogic.Infrastructure.csproj -s RackLogic.Web.csproj -o Data/Migrations
- dotnet ef database update -c AppDbContext -p ../RackLogic.Infrastructure/RackLogic.Infrastructure.csproj -s RackLogic.Web.csproj
