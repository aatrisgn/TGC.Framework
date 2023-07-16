# TGC.Framework

This is a personal project where I create NuGet packages for the software I use the most often. It is designed by the principles and methodologies I prefer, but I tried making it as generic as possible. I am always open for PR's if you find any bugs or things missing.

## Pipeline status
Build status:

[![Nightly build main](https://github.com/aatrisgn/TGC.Framework/actions/workflows/NightlyBuild.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/NightlyBuild.yml)
*Builds all projects and runs all tests nightly on main*

[![Build TGC.AzureTableStorage](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.AzureTableStorage.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.AzureTableStorage.yml)
[TGC.AzureTableStorage Documentation](https://github.com/aatrisgn/TGC.Framework/blob/main/src/AzureTableStorage/TGC.AzureTableStorage/README.md).

[![Build TGC.CodingStandards](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.CSharpCodingStandards.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.CSharpCodingStandards.yml)
[TGC.CodingStandards Documentation](https://github.com/aatrisgn/TGC.Framework/blob/main/src/CSharpCodingStandards/TGC.CSharpCodingStandards/README.md).

[![Build TGC.Configuration](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.Configuration.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.Configuration.yml)
[TGC.Configuration Documentation](https://github.com/aatrisgn/TGC.Framework/blob/main/src/Configuration/TGC.Configuration/README.md).

[![Build TGC.ConsoleBuilder](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.ConsoleBuilder.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.ConsoleBuilder.yml)
[TGC.Configuration Documentation](https://github.com/aatrisgn/TGC.Framework/blob/main/src/ConsoleBuilder/TGC.ConsoleBuilder/README.md).

[![Build TGC.EFCoreRepositories](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.EFCoreRepositories.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.EFCoreRepositories.yml)
[TGC.EFCoreRepositories Documentation](https://github.com/aatrisgn/TGC.Framework/blob/main/src/EFCoreRepositories/TGC.EFCoreRepositories/README.md).

[![Build TGC.HealthChecks](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.HealthChecks.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.HealthChecks.yml)

[![Build TGC.WebApiBuilder](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.WebApiBuilder.yml/badge.svg)](https://github.com/aatrisgn/TGC.Framework/actions/workflows/TGC.WebApiBuilder.yml)

## Roadmap

### Technical roadmap

### Abstracting away IoC specification
At the moment, most packages needs to be manually added to the service collection, which is fine. However, I plan on developing automatic injection of referenced packages if a specific WebAPI builder is used. This would provide the user with an even easier integration of different functionality which would be wired under the hood.

### Github action templates
I've recently started looking into GitHub actions, bicep and deployments hereof. I expect to try and make the steps used in my pipelines more generalizable and reusable.

This also involves making my current build pipelines more generic, since there is a lot of ctrl-c, ctrl-v.

Should also update pipelines to ignore certain file changes such as Readme, etc.

### Redis
I would like to make an easy integration for Redis (Even though Microsoft has already made it quite easy.). I've yet to look into it, but it is on the "to-do".

### SignalR
It's not something I've looked much into, but I would like to do so even further. Therefore, I would like to make an abstraction for it.

### Blob storage
Same as for some of the other services. At the moment I am only working on an Azure Table abstraction (which works), but I would like to make an easy integration for using Azure Blob Storage as an easy file service.

### Console builder
An easy abstraction for creating a console app with all the DI and such wired up and easy to use

### Authentication and Authorization - JWT, Azure AD and Azure B2C

### General roadmap

#### Allign changelog and better overview of packages and dependencies
At the moment there is no clear governance of packages, versioning and documentation. I plan on improving this on the go. However, I do not expect it to be perfect any time soon. If you stumble upon this and find it interesting, don't hesitate reaching out.