# C# API Example for Aurora Licensing System

This repository contains a C# example implementation for interacting with the Aurora Licensing System API. The Aurora Licensing System is used for license management and software protection.

## Requirements

- .NET Framework 3.5 or later.
- Newtonsoft.Json

## Getting Started

1. Clone the repository to your local machine.

git clone https://github.com/aurora-licensing/csharp-console-example.git
cd aurora-csharp-example

Open the solution file AuroraExample.sln in your preferred C# development environment (e.g., Visual Studio, Visual Studio Code).

Edit the Program.cs file to add your Aurora API credentials:

```bash
Aurora aurora = new Aurora("app_name", "app_secret", "app_hash", "app_version", "https://aurora-licensing.pro/api/");
```

Replace "app_name", "app_secret", and "app_hash" with your actual Aurora API credentials. The version and url variables should also be set according to your API configuration.

Run the project. It will perform the following tasks:

    Initialize the API by sending an initialization request.
    Prompt the user to enter a license.
    Check the validity of the license.
    Check the expiry date of the license.
    Check the subscription level of the license.
    Get a variable message using a secret.
    Download a file using the DownloadFileAsync method.
    Send a webhook with provided information.

Usage

The example demonstrates how to use the Aurora class to interact with the Aurora Licensing System API. The class contains methods for various tasks such as initializing the API, checking licenses, getting variable messages, downloading files, and sending webhooks.

You can use this example as a starting point to integrate Aurora Licensing System functionality into your own C# projects.
API Documentation

For more details on the Aurora Licensing System API and its endpoints, refer to the official documentation.
License

This project is licensed under the MIT License. See the LICENSE file for details.
Contributions

Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or create a pull request.
