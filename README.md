# Introduction 
This project consists of two Azure functions that allow you to interact with CampusNexus Student APIs for student account creation.

# Getting Started
1.  Clone the repository
```
git clone https://github.com/ewellnitz/student-account-creation.git
```
2.  Open **Student.AccountCreation.sln** in Visual Studio
3.  Duplicate the contents of **sample.local.settings.json** into a file named **local.settings.json** and set the following settings:
- **CampusNexusUrl** - The host url for your CampusNexus Student instance
- **User** - The username associated with your service account
- **Password** - The password associated with your service account
```
{
  "IsEncrypted": false,
  "CampusNexusUrl": "https://hostName.campusnexus.cloud/",
  "User": "user@hostName.campusnexus.cloud",
  "Password": "***",
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

# Build and Test
1.  Run the project, you should see in the console window the URLs associated with the function endpoints:
```
GetStudents: [GET] http://localhost:7071/api/GetStudents
UpdateEmail: [POST] http://localhost:7071/api/UpdateEmail
{
	"id": 0,
	"email": "string"
}
```
# Deploy to Azure
1.  Right-click on the **Student.AccountCreation** project
2.  Select **Publish...**
3.  Choose **Azure Consumption Plan** and either select an existing plan or create a new one.
4.  Choose the settings for the **App Service** and select **Create** and wait for the solution to deploy.
5.  Click the **Edit Azure App Service settings** and add settings/values for the following:
- **CampusNexusUrl** - The host url for your CampusNexus Student instance
- **User** - The username associated with your service account
- **Password** - The password associated with your service account
