# makeITeasy.Jira2Azure

A very basic app that listen to JIRA webhook message and create a work item in a TFS Server. Process also create a new GIT branch and associate the branch with the newly created workitem.

Setup has to be done on JIRA to set the webhook to the following url : https://myserver/WebHookController/Jira
Fill the configuration with an azure valid URL, a project name and a personal  token with full code access.
