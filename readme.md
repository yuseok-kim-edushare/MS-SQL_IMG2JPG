# MS-SQL Image column data to jpg Converter .NET Library For PowerBuilder

This is a .NET library that converts any MS-SQL Image type Column's Data into jpg file.
The library is specifically designed to be used with PowerBuilder applications.
but any COM or .NET(C#) method calling can use this project artifact

## Features

- Convert Image byte stream to JPG byte stream

## Requirements

- Windows 10 20H2 or Later (server 2022 or later)
  - .NET Framework 4.8.1
- Not Ensured, but it should work with .NET 4.8 and windows 7 or later
  - MS introduce no comapatibility change from .NET 4.8 to .NET 4.8.1
    - then, it should work with .NET 4.8 and windows 7 or later
- (For Development) Visual Studio 2022+
  - or .NET 8+ SDK for using dotnet build command

## Building the Library

1. Open the solution in Visual Studio
2. Build the solution in Release mode
3. if you want to build with dotnet cli(cause of not having visual studio)
   ```powershell
   dotnet build ms-sql-image2jpg.csproj --configuration Release
   ```

# Following docs are under construction
## Usage in PowerBuilder
1. Use PowerBuilder's ".NET DLL Importer" tool to import the assembly:
   - Open your PowerBuilder project
   - Select Tools → ".NET DLL Importer"
   - Browse to and select dll file
   - Generate the proxy object

2. Create an instance of the converter:


