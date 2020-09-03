[![MIT License][license-shield]][license-url]

<br />
<p align="center">
  <h3 align="center">makeITeasy.Jira2Azure</h3>
</p>

<!-- ABOUT THE PROJECT -->
## About The Project

Website app that listen to JIRA webhook messages and create a work item in TFS Server. 

app also creates a new GIT branch and associates the branch with the newly created workitem. Branch is created with ***NO_CI*** commit message to avoid unwanted build trigger.

### Built With

* aspnet core 3.1
* automapper / autofac
* Love

<!-- GETTING STARTED -->
## Getting Started


Setup has to be done on JIRA to set the webhook to the following url : https://myserver/WebHookController/Jira
Fill the configuration with an azure valid URL, a project name and a personal  token with full code access.

To get a local copy up and running follow these simple example steps.

### Installation

1. Setup webhook in JIRA with following url : https://myserver/WebHookController/Jira
2. Configure appsettings.json with your Azure Devops URL, your security toeken (full access to code)


<!-- LICENSE -->
## License

Distributed under the MIT License. 


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=flat-square
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
