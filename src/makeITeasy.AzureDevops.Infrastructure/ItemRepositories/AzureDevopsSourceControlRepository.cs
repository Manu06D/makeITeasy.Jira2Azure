using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
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
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;
        private readonly IMapper _mapper;

        public AzureDevopsSourceControlRepository(AzureDevopsConfiguration azureDevopsConfiguration, IMapper mapper)
        {
            _azureDevopsConfiguration = azureDevopsConfiguration;
            _mapper = mapper;
        }

        public async Task<OperationResult<GitCommitInfo>> CreateNewBranch(string name)
        {
            OperationResult<GitCommitInfo> result = new OperationResult<GitCommitInfo>();

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var client = connection.GetClient<GitHttpClient>())
            {
                var repositories = await client.GetRepositoriesAsync(_azureDevopsConfiguration.ProjectName);

                var repository = repositories.Where(r => string.Compare(r.Name, _azureDevopsConfiguration.GITSourceControl.RepositoryName, true) == 0).First();

                if(repository == null)
                {
                    throw new Exception($"Unable to find repository {_azureDevopsConfiguration.GITSourceControl.RepositoryName} in the project {_azureDevopsConfiguration.ProjectName}. List of repositories : {String.Join(",", repositories.Select(r => r.Name).ToArray())})");
                }

                var masterBranch = await client.GetBranchAsync(repository.Id, _azureDevopsConfiguration.GITSourceControl.MasterBranch);

                if (repository == null)
                {
                    throw new Exception($"Unable to find masterBranch {_azureDevopsConfiguration.GITSourceControl.MasterBranch}");
                }

                //TODO associate with workitem
                var branchResult = await client.UpdateRefsAsync(
                    new GitRefUpdate[] {
                        new GitRefUpdate
                        {
                            Name = $"{_azureDevopsConfiguration.GITSourceControl.NewBranchPath}/{name}",
                            NewObjectId = masterBranch.Commit.CommitId,
                            OldObjectId = new string('0', 40)} 
                    }, 
                    repositoryId: repositories.First().Id);

                String GitURL = $"{repository.ProjectReference.Id}%2F{branchResult.FirstOrDefault()?.NewObjectId}%2FGBfeatures%2F{name}";

                GitCommitInfo gci = new GitCommitInfo();
                gci.ProjectID = repository.ProjectReference.Id.ToString();
                gci.RepositoryID = repository.Id.ToString();
                gci.CommitID = branchResult.FirstOrDefault()?.NewObjectId;
                gci.BranchName = name;

                result.Item = gci;
                result.HasSucceed = branchResult.All(x => x.Success);

                return result;
            }
        }
    }
}
