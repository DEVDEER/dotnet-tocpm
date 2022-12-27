![CI](https://github.com/DEVDEER/dotnet-tocpm/actions/workflows/main.yml/badge.svg?branch=main)
![](https://img.shields.io/nuget/v/devdeer.tools.tocpm)

# dotnet-tocpm

Dotnet global tool to convert solutions to the [Nuget CPM](https://devblogs.microsoft.com/nuget/introducing-central-package-management/).

## Summary

Switching to Nuget CPM can be a pain when it comes to bigger solutions containing a number of projects. `dotnet-tocpm` was created to perform this operation in one simple step.

I made this video on my codingfreaks channel on yt where I explain the background and the usage of this tool:

<div align="left">
      <a href="https://www.youtube.com/watch?v=t0IOzOdRh2U">
         <img src="https://img.youtube.com/vi/t0IOzOdRh2U/0.jpg" style="width:50%;">
      </a>
</div>

## Requirements

The tool requires the .NET 7 runtime (or SDK) installed on the machine.

## Installation

Use the following command to install `dotnet-tocpm`:

```shell
dotnet tool install -g --prerelease devdeer.tools.tocpm
```

After this command ran you can check the success by running:

```shell
tocpm -v
```

This should result in an output like this:

```shell
tocpm -v
0.0.1-alpha
```

## Usage

If you run the command without any params it will display the help information:

```shell
tocpm
USAGE:
    tocpm [OPTIONS] <COMMAND>

EXAMPLES:
    tocpm simulate .
    tocpm run C:\temp\project
    tocpm run C:\temp\project -f

OPTIONS:
    -h, --help       Prints help information
    -v, --version    Prints version information

COMMANDS:
    simulate    Simulates the operation at the provided location and writes the results to the console
    execute     Executes a real run at the specified location

```

A simple and secure way to test the tool is to perform a dry run. Use the `simulate` command for this by specifying a folder. Lets assume that you've got a solution in the folder `C:\temp\test`. Your command then would be

```shell
tocpm simulate C:\temp\test
```

The output would look something like this:

```shell
Running...
Found 17 files.
Found 31 unique packages.
┌───────────────────────────────────────────────────────────────────┬─────────┐
│ Package ID                                                        │ Version │
├───────────────────────────────────────────────────────────────────┼─────────┤
│ AspNetCore.HealthChecks.AzureStorage                              │ 6.1.2   │
│ NUnit3TestAdapter                                                 │ 4.3.1   │
│ StackExchange.Redis                                               │ 2.6.86  │
│ Swashbuckle.AspNetCore                                            │ 6.4.0   │
│ Swashbuckle.AspNetCore.Annotations                                │ 6.4.0   │
└───────────────────────────────────────────────────────────────────┴─────────┘
The following content would be written to D:\repos\Heurich\Laker\Directory.Packages.props if executed with command
execute:
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="AspNetCore.HealthChecks.AzureStorage" Version="6.1.2" />
    <PackageVersion Include="NUnit3TestAdapter" Version="4.3.1" />
    <PackageVersion Include="StackExchange.Redis" Version="2.6.86" />
    <PackageVersion Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageVersion Include="Swashbuckle.AspNetCore.Annotations" Version="6.4.0" />
  </ItemGroup>
</Project>
```

To actually perform the operation you first replace `simulate` by `execute`. This also gives you 2 more options (run `tocpm execute -h`):

- `-f|--force` will prevent any confirmation from the user before executing the command.
- `-b|--backup` will create a copy of each project file in its folder with the file ending `.bak`.
