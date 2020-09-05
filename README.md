[![MIT License][license-shield]][license-url]

# makeITeasy.Jira2Azure

<!-- ABOUT THE PROJECT -->
## About The Project

This webApp listens to JIRA webhook messages and creates work items in Azure Devops Server (cloud or on-premise). 

The webApp can also create a new GIT branch and associate the branch with the newly created workitem. Branch is created with ***NO_CI*** commit message to avoid unwanted build triggers.

### Built With

* aspnet core 3.1
* automapper / autofac
* Love

<!-- GETTING STARTED -->
## Getting Started

Once configuration is properly done, just take a seat a watch your azure devops backlog filled with your jira created/updated tickets

When you set a label named "git" in your jira ticket, git branch will be automatically created and the branch name pattern will be features/[JIRA-ID] (configurable)

### Installation

1. Configure appsettings.json  
```
"ItemRepositories": {
    "AzureDevops": {
      "Uri": "",
      "Token": "",
      "GITSourceControl": {
        "RepositoryName": "MakeITEasy.VacationRequest",
        "MasterBranch": "master",
        "NewBranchPath": "refs/heads/features"
      }
    }
```
AzureDevops/Uri is your top level azure devops site url

AzureDevops/Token  is your security token generated on azure devops server with full access to code

AzureDevops/GITSourceControl/RepositoryName : Name of your project as defined in Azure Devops

AzureDevops/GITSourceControl/MasterBranch : Name of your base branch

AzureDevops/GITSourceControl/NewBranchPath : Git virtual path to your new branch

2. Deploy the aspnet core web app
3. Setup webhook in JIRA with following url : https://myserver/WebHookController/Jira

<!-- LICENSE -->
## License

Distributed under the MIT License. 

Don't hesitate to contact me for any inquieries


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=flat-square
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
