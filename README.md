# Introduction 
This project consists of two Azure functions that allow you to interact with CampusNexus Student APIs for student account creation.

# Getting Started
1.  Clone the repository
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
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
