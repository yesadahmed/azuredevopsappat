using Microsoft.Extensions.ObjectPool;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using azuredevopsappat.Helper;
using azuredevopsappat.Helper.HtmlText;
using azuredevopsappat.Models;
using azuredevopsappat.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds.Manager.BuildManager
{
    public class BuildManager
    {
        #region Memebers
        //const String c_collectionUri = "https://dev.azure.com/";
        //const String c_projectName = "1";
        //const String c_repoName = "S1";
        //const string personalaccesstoken = "2m2jtbjtxyyzhhf4ui4a";
        private readonly ObjectPool<DevOpsConnectionPool> _builderPool;

        #endregion



        public BuildManager(ObjectPool<DevOpsConnectionPool> builderPool)
        {
            _builderPool = builderPool;

        }

        /// <summary>


        /// <summary>
        /// there should be parameter in pipeline named SolutionName
        /// </summary>
        /// <param name="ProjetcName"></param>
        /// <param name="solname"></param>
        /// <param name="buildId"></param>
        /// <returns></returns>
        public async Task<int> CreateBuildForSolution(string ProjetcName, string solname,
                                                         string pipelinevariablename, int definitionId)
        {

            int BuildID = 0;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);
                var definitions = await buildclient.GetDefinitionsAsync(ProjetcName);

                // Get the specified id of build definition.
                var target = definitions.First(d => d.Id == definitionId);
                var res = await buildclient.QueueBuildAsync(new Build
                {
                    Definition = new DefinitionReference
                    {
                        Id = target.Id
                    },
                    Project = target.Project,
                    Parameters = "{\"" + pipelinevariablename + "\":\"" + solname + "\",\"system.debug\":\"false\"}"
                    //k   Parameters = "{\"SolutionName\":\"" + solname + "\",\"system.debug\":\"false\"}"
                });
                BuildID = res.Id;

                //var status = await buildclient.GetBuildAsync(ProjetcName, res.Id);//call after some time
                //BuildID = status.Id;

            }
            catch (Exception )
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return BuildID;
        }
        public async Task<BuildStatusModel> GetBuildStatus(string projectname, int buildId)
        {
            BuildStatusModel statusModel = new BuildStatusModel();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {

                // Create instance of VssConnection using Personal Access Token
                //var buildclient = new BuildHttpClient(new Uri(c_collectionUri), new VssBasicCredential(string.Empty, personalaccesstoken));
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);
                var status = await buildclient.GetBuildAsync(projectname, buildId);//call after some time

                if (status.Result.HasValue) //result of build
                {
                    switch ((int)status.Result.Value)
                    {
                        case 0://none
                            statusModel.BuildResult = "None";
                            break;

                        case 2://none
                            statusModel.BuildResult = "Succeeded";
                            break;
                        case 4://none
                            statusModel.BuildResult = "PartiallySucceeded";
                            break;
                        case 8://none
                            statusModel.BuildResult = "Failed";
                            break;
                        case 32://none
                            statusModel.BuildResult = "Canceled";
                            break;
                    }
                }
                else
                {
                    statusModel.BuildResult = "";
                }

                if (status.Status.HasValue)//status of buid
                {
                    switch ((int)status.Status.Value)
                    {
                        case 0://none
                            statusModel.status = "None";
                            break;

                        case 1://none
                            statusModel.status = "InProgress";
                            break;
                        case 2://none
                            statusModel.status = "Completed";
                            break;
                        case 4://none
                            statusModel.status = "Cancelling";
                            break;
                        case 8://none
                            statusModel.status = "Postponed";
                            break;
                        case 32://none
                            statusModel.status = "NotStarted";
                            break;
                        case 47://none
                            statusModel.status = "All";
                            break;
                    }

                    if (status.StartTime.HasValue)
                    {
                        statusModel.StartTime = status.StartTime.Value.ToUniversalTime(); // if build started
                    }

                    if (status.FinishTime.HasValue)
                    {
                        statusModel.FinishTime = status.FinishTime.Value.ToUniversalTime();
                    }

                }
                else
                {
                    statusModel.status = "";
                }


            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return statusModel;
        }


        public async Task<BuildModel> GetProjectBuilds(string ProjetcName)
        {
            BuildModel buildModel = new BuildModel();
            BuildModelValue buildModelvalue = null;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                buildModel.Builds = new List<BuildModelValue>(); //UI aspect
                // Create instance of VssConnection using Personal Access Token
                //var buildclient = new BuildHttpClient(new Uri(c_collectionUri), new VssBasicCredential(string.Empty, personalaccesstoken));
                using (var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials))
                {
                    var definitions = await buildclient.GetDefinitionsAsync(ProjetcName);

                    if (definitions.Any())
                    {

                        foreach (var def in definitions)
                        {
                            buildModelvalue = new BuildModelValue()
                            {
                                BuildID = def.Id,
                                BuildName = def.Name,
                                BuildUrl = def.Url,
                                DateCreated = def.CreatedDate,
                                BuildPath = def.Path,
                                ProjectId = def.Project.Id,
                                ProjectName = def.Project.Name,
                                ProjectUrl = def.Project.Url
                            };
                            buildModel.Builds.Add(buildModelvalue);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return buildModel;
        }

        public async Task<List<BuildLogDetail>> GetBuildLogs(string projectName, int BuildId)
        {
            List<BuildLogDetail> buildLogs = new List<BuildLogDetail>();
            BuildLogDetail buildLogDetail = new BuildLogDetail();
            ResultModel resultModel = new ResultModel();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {


                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);

                var result = await buildclient.GetBuildLogsAsync(projectName, BuildId);
                if (result != null && result.Count > 0)
                {


                    foreach (var log in result)
                    {
                        buildLogDetail = new BuildLogDetail();
                        buildLogDetail.Id = log.Id;
                        buildLogDetail.Url = log.Url;
                        buildLogDetail.CreatedOn = log.CreatedOn;
                        resultModel = await GetLogContents(projectName, BuildId, log.Id, buildclient);
                        if (resultModel != null)
                        {
                            buildLogDetail.IsError = resultModel.IsError;
                            buildLogDetail.HtmlContent = resultModel.HtmContent;
                            buildLogDetail.TaskName = resultModel.TaskName;
                            buildLogDetail.TaskColor = resultModel.TaskColor;
                        }
                        buildLogs.Add(buildLogDetail);
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return buildLogs;

        }

        public async Task<List<ProjectModel>> GetAllProjetcs()
        {
            List<ProjectModel> lst = new List<ProjectModel>();
            ProjectModel projectModel = null;
           
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {


                ProjectHttpClient projectClient = poolObj.VssConnection.GetClient<ProjectHttpClient>();

                var projetcs = await projectClient.GetProjects(ProjectState.All);
                if (projetcs != null && projetcs.Count > 0)
                {
                    foreach (var project in projetcs)
                    {
                        var idd = await projectClient.GetProject(project.Id.ToString());
                        projectModel = new ProjectModel()
                        {
                            Id = project.Id.ToString(),
                            Name = project.Name,
                            Url = project.Url
                        };
                        lst.Add(projectModel);
                    }
                    lst = lst.OrderBy(n => n.Name).ToList();
                }

            }
            catch (Exception )
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return lst;
        }


        public async Task<List<TeamModel>> GetProjectTeams(string projectName)
        {
            List<TeamModel> lst = new List<TeamModel>();
            TeamModel teammodel = null;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                //    VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //    // Connect to Azure DevOps Services
                //    VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);
                TeamHttpClient teamClient = poolObj.VssConnection.GetClient<TeamHttpClient>();
                List<WebApiTeam> teams = await teamClient.GetTeamsAsync(projectName);
                if (teams != null && teams.Count > 0)
                {
                    foreach (var team in teams)
                    {
                        teammodel = new TeamModel();
                        teammodel.TeamId = team.Id;
                        teammodel.TeamName = team.Name;
                        teammodel.TeamUrl = team.Url;
                        var teamsMembers = await GetTeamMembers(team.ProjectId.ToString(), team.Id.ToString(), teamClient);
                        if (teamsMembers != null && teamsMembers.Count > 0)
                        {
                            teammodel.TeamMembers = new List<TeamMemberModel>();
                            teammodel.TeamMembers.AddRange(teamsMembers);
                        }
                        lst.Add(teammodel);
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return lst;
        }

        public async Task<List<TeamMemberModel>> GetProjectTeamMembers(string projectName)
        {
            List<TeamMemberModel> tempLst = new List<TeamMemberModel>();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                //VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);
                TeamHttpClient teamClient = poolObj.VssConnection.GetClient<TeamHttpClient>();
                List<WebApiTeam> teams = await teamClient.GetTeamsAsync(projectName);
                if (teams != null && teams.Count > 0)
                {
                    foreach (var team in teams)
                    {
                        var teamsMembers = await GetTeamMembers(team.ProjectId.ToString(), team.Id.ToString(), teamClient);
                        if (teamsMembers != null && teamsMembers.Count > 0)
                            tempLst.AddRange(teamsMembers);

                    }
                    tempLst = tempLst.GroupBy(x => x.Email).Select(x => x.First()).ToList();//remove dupicate
                }
                else
                    tempLst.Add(new TeamMemberModel() { Email = string.Empty });

            }
            catch (Exception)
            {
                tempLst.Add(new TeamMemberModel() { Email = string.Empty });
            }
            finally
            {
                _builderPool.Return(poolObj);
            }

            return tempLst;
        }


        /// <summary>
        /// HTML REport
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="BuildId"></param>
        /// <returns></returns>
        public async Task<BuildHtml> GetBuildFinalReport(string projectName, int BuildId)
        {
            BuildHtml buildHtml = new BuildHtml();
            //string htmlcontent = string.Empty;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                buildHtml.BuildId = BuildId;
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);

                var metadata = await buildclient.GetBuildReportAsync(projectName, BuildId);
                if (metadata != null && (!string.IsNullOrWhiteSpace(metadata.Content)))
                    buildHtml.Html = metadata.Content;


            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return buildHtml;
        }

        public async Task<List<VariableModel>> GetPipeLineVariables(string projectname, int definitionid)
        {
            List<VariableModel> lst = new List<VariableModel>();
            VariableModel variableModel = null;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);

                var defnition = await buildclient.GetDefinitionAsync(projectname, definitionid);
                if (defnition != null && defnition.Variables != null)
                {
                    foreach (var kv in defnition.Variables)
                    {
                        variableModel = new VariableModel();
                        variableModel.Key = kv.Key;
                        if (kv.Value != null)
                            variableModel.Value = kv.Value.Value;
                        lst.Add(variableModel);
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return lst;
        }

        public async Task<VariableModel> GetSpecificPipeLineVariable(string projectname, int definitionid,
                                                                            string variablekey)
        {
            VariableModel variableModel = new VariableModel();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);

                var defnition = await buildclient.GetDefinitionAsync(projectname, definitionid);
                if (defnition != null && defnition.Variables != null)
                {
                    foreach (var kv in defnition.Variables)
                    {
                        if (kv.Key.ToLower().Equals(variablekey.ToLower()))
                        {
                            variableModel = new VariableModel();
                            variableModel.Key = kv.Key;
                            if (kv.Value != null)
                                variableModel.Value = kv.Value.Value;
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return variableModel;
        }

        //        

        public async Task<List<PipeLineModel>> GetProjectPipeLines(string projectname)
        {
            List<PipeLineModel> pipelines = new List<PipeLineModel>();
            PipeLineModel pipeLine = null;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                var buildclient = new BuildHttpClient(new Uri(poolObj.CollUrl), poolObj.VssCredentials);
                var result = await buildclient.GetDefinitionsAsync(projectname);//call after some time

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        pipeLine = new PipeLineModel();
                        pipeLine.Id = item.Id;
                        pipeLine.Url = item.Url;
                        pipeLine.Name = item.Name;
                        pipeLine.Path = item.Path;
                        pipelines.Add(pipeLine);
                    }
                }
                else
                {
                    pipelines.Add(new PipeLineModel()
                    {
                        Id = 0,
                        Name = string.Empty,
                        Path = string.Empty,
                        Url = string.Empty
                    });//just one dummy
                }
            }
            catch (Exception 
            )
            {
                pipelines.Add(new PipeLineModel()
                {
                    Id = 0,
                    Name = string.Empty,
                    Path = string.Empty,
                    Url = string.Empty
                });//just one dummy
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return pipelines;
        }

        #region Helper Funcitons

        async Task<ResultModel> GetLogContents(string projectName, int buildId,
                                            int logid, BuildHttpClient client)
        {
            ResultModel resultModel = new ResultModel();

            List<string> logContents = new List<string>();
            try
            {
                logContents = await client.GetBuildLogLinesAsync(projectName, buildId, logid);
                if (logContents.Count > 0)
                    resultModel = GetformatedHtmlContents(logContents);
            }
            catch (Exception)
            {
            }

            return resultModel;
        }

        ResultModel GetformatedHtmlContents(List<string> logContents)
        {
            ResultModel resultModel = new ResultModel();
            StringBuilder contents = new StringBuilder();
            contents.AppendLine(buildLogTextHelper.InsertHtmlStartform());
            int counter = 0; int idx = 0;
            try
            {

                bool isError = IsErrorInLog(logContents);

                if (isError)
                {
                    contents.Append(buildLogTextHelper.FailImage);
                    contents.Append(buildLogTextHelper.Extraspace);
                    resultModel.IsError = isError;
                    resultModel.TaskColor = "Red";
                }
                else
                {
                    contents.Append(buildLogTextHelper.SuccessImage);
                    contents.Append(buildLogTextHelper.Extraspace);
                    resultModel.IsError = isError;
                    resultModel.TaskColor = "Green";
                }

                counter = 0;//double check
                foreach (var str in logContents)
                {
                    if (counter.Equals(0))
                    {
                        if (str.Contains("Starting: "))
                        {
                            idx = str.IndexOf("Starting: ");
                            if (idx > 0)
                            {
                                resultModel.TaskName = str.Substring(idx, str.Length - idx);
                                if (isError)
                                    contents.Append(buildLogTextHelper.RedSpanStart);
                                else
                                    contents.Append(buildLogTextHelper.GreenSpanStart);
                                contents.Append($"{resultModel.TaskName}");
                                contents.Append(buildLogTextHelper.spanEnd);
                                contents.AppendLine(buildLogTextHelper.HtmlLineBreak);
                                contents.AppendLine(buildLogTextHelper.HtmlLineBreak);
                                counter++;
                                continue;

                            }
                        }
                    }

                    contents.AppendLine(str.ToString());
                    contents.AppendLine(buildLogTextHelper.HtmlLineBreak);
                    counter++;
                }
                contents.AppendLine(buildLogTextHelper.HtmlLineBreak);

                contents.AppendLine(buildLogTextHelper.InsertHtmlEndForm());
                resultModel.HtmContent = contents.ToString();
            }
            catch (Exception)
            {
            }

            return resultModel;
        }


        bool IsErrorInLog(List<string> logContents)
        {
            bool isError = false;
            try
            {
                foreach (var str in logContents)
                {
                    {
                        if (str.Contains("##[error]"))
                        {
                            isError = true;
                            break;
                        }
                    }
                }

            }
            catch (Exception)
            {
            }

            return isError;
        }




        async Task<List<TeamMemberModel>> GetTeamMembers(string projectId, string teamId, TeamHttpClient teamClient)
        {
            List<TeamMemberModel> lst = null;
            TeamMemberModel teamMemberModel = null;
            try
            {
                var teamMembers = await teamClient.GetTeamMembersWithExtendedPropertiesAsync(projectId, teamId);

                if (teamMembers != null && teamMembers.Count > 0)
                {
                    lst = new List<TeamMemberModel>();
                    foreach (var member in teamMembers)
                    {
                        if (member.Identity != null)
                        {
                            teamMemberModel = new TeamMemberModel()
                            {
                                Email = member.Identity.UniqueName,
                                MemberId = member.Identity.Id,
                                MemberName = member.Identity.DisplayName,
                                MemberUrl = member.Identity.Url
                            };
                        }
                        lst.Add(teamMemberModel);
                    }
                }
            }
            catch (Exception)
            {

            }

            return lst;
        }

        #endregion


    }
}
