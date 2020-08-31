using System;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.Threading.Tasks;
using System.Linq;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsSourceControlRepository : ISourceControlRepository
    {
        private const string ciFilePath = "/_no_ci_file";
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;

        public AzureDevopsSourceControlRepository(AzureDevopsConfiguration azureDevopsConfiguration)
        {
            _azureDevopsConfiguration = azureDevopsConfiguration;
        }

        public async Task<OperationResult<GitCommitInfo>> CreateNewBranch(string name)
        {
            OperationResult<GitCommitInfo> result = new OperationResult<GitCommitInfo>();
            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var client = connection.GetClient<GitHttpClient>())
            {
                var repositories = await client.GetRepositoriesAsync(_azureDevopsConfiguration.ProjectName);

                var repository = repositories?.First(r => string.Compare(r.Name, _azureDevopsConfiguration.GITSourceControl.RepositoryName, true) == 0);

                if (repository == null)
                {
                    throw 
                        new Exception($"Unable to find repository {_azureDevopsConfiguration.GITSourceControl.RepositoryName} in the project {_azureDevopsConfiguration.ProjectName}. List of repositories : {String.Join(",", repositories.Select(r => r.Name).ToArray())})");
                }

                var masterBranch = await client.GetBranchAsync(repository.Id, _azureDevopsConfiguration.GITSourceControl.MasterBranch);

                if (masterBranch == null)
                {
                    throw new Exception($"Unable to find masterBranch {_azureDevopsConfiguration.GITSourceControl.MasterBranch}");
                }

                string branchCommitID = await InnerCreateBranch(name, client, repository.Id, masterBranch.Commit.CommitId);

                GitCommitInfo gci = new GitCommitInfo();
                gci.ProjectID = repository.ProjectReference.Id.ToString();
                gci.RepositoryID = repository.Id.ToString();
                gci.CommitID = branchCommitID;
                gci.BranchName = name;

                result.Item = gci;
                result.HasSucceed = !string.IsNullOrWhiteSpace(branchCommitID);
            }

            return result;
        }

        private async Task<string> InnerCreateBranch(string name, GitHttpClient client, Guid repositoryID, string newObjectCommitID)
        {
            bool isCIFileExist = true;

            try
            {
                _ = await client.GetItemAsync(repositoryID, ciFilePath);
            }
            catch (Exception)
            {
                isCIFileExist = false;
            }

            GitRefUpdate newBranch = new GitRefUpdate()
            {
                Name = $"{_azureDevopsConfiguration.GITSourceControl.NewBranchPath}/{name}",
                OldObjectId = newObjectCommitID
            };

            GitCommitRef newCommit = new GitCommitRef()
            {
                Comment = "***No_CI****",
                Changes = new GitChange[]
                {
                        new GitChange()
                        {
                            ChangeType = isCIFileExist ? VersionControlChangeType.Edit : VersionControlChangeType.Add,
                            Item = new GitItem() { Path = $"/_no_ci_file" },
                            NewContent = new ItemContent()
                            {
                                Content = "",
                                ContentType = ItemContentType.RawText,
                            },
                        }
                 }
            };

            GitPush push = await client.CreatePushAsync(new GitPush()
            {
                RefUpdates = new GitRefUpdate[] { newBranch },
                Commits = new GitCommitRef[] { newCommit },
            }, repositoryID);

            return push.Commits.First(x => !String.IsNullOrEmpty(x.CommitId))?.CommitId;
        }
    }
}