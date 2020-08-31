# makeITeasy.Jira2Azure

A very basic website that listen to JIRA webhook messages and create a work item in TFS Server. Process also create a new GIT branch and associate the branch with the newly created workitem. Branch is created with ***NO_CI*** commit message to avoid unwanted build trigger.

Setup has to be done on JIRA to set the webhook to the following url : https://myserver/WebHookController/Jira
Fill the configuration with an azure valid URL, a project name and a personal  token with full code access.
