using Microsoft.Extensions.ObjectPool;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using azuredevopsappat.Models;
using azuredevopsappat.ObjectPooling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds.Manager.BuildManager
{
    public class WorkItemManager
    {

        #region Data Memebers
        //const String c_collectionUri = "https://dev.azure.com/";
        //const String c_projectName = "S1";
        //const String c_repoName = "1";
        //const string personalaccesstoken = "zm2jtbjtxyyzhhf4ui4a";
        private readonly ObjectPool<DevOpsConnectionPool> _builderPool;

        public static string Task { get { return "Task"; } }
        public static string Bug { get { return "Bug"; } }
        public static string Feature { get { return "Feature"; } }
        public static string Issue { get { return "Issue"; } }

        #endregion


        public WorkItemManager(ObjectPool<DevOpsConnectionPool> builderPool)
        {
            _builderPool = builderPool;
        }



        #region Private Helper Functions
        static List<Params> GetTaskIssueFeatureFields(Fields fields)
        {
            List<Params> list = new List<Params>();
            list.Add(new Params() { Path = "/fields/System.Title", Value = fields.Title });
            list.Add(new Params() { Path = "/fields/Microsoft.VSTS.Common.Priority", Value = fields.Priority });
            list.Add(new Params() { Path = "/fields/System.Description", Value = fields.Description });
            list.Add(new Params() { Path = "/fields/Microsoft.VSTS.Common.Severity", Value = fields.Severity });
            if (!string.IsNullOrWhiteSpace(fields.AssignedTo))
                list.Add(new Params() { Path = "/fields/System.AssignedTo", Value = fields.AssignedTo });
            return list;
            //https://docs.microsoft.com/en-us/rest/api/azure/devops/wit/work%20item%20types%20field/list?view=azure-devops-rest-5.1
        }

        static List<Params> GetBugFields(Fields fields)
        {
            List<Params> list = new List<Params>();
            list.Add(new Params() { Path = "/fields/System.Title", Value = fields.Title });
            list.Add(new Params() { Path = "/fields/Microsoft.VSTS.Common.Priority", Value = fields.Priority });
            list.Add(new Params() { Path = "/fields/Microsoft.VSTS.TCM.ReproSteps", Value = fields.BugText });
            list.Add(new Params() { Path = "/fields/Microsoft.VSTS.Common.Severity", Value = fields.Severity });
            if (!string.IsNullOrWhiteSpace(fields.AssignedTo))
                list.Add(new Params() { Path = "/fields/System.AssignedTo", Value = fields.AssignedTo });

            return list;
        }
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        #endregion

        #region Public Functions
        public async Task<WorkItemModel> CreateWorkitem(CreateWorkItem workItem)
        {
            WorkItemModel workItemModel = new WorkItemModel();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                List<Params> ListParams = new List<Params>();
                //VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);
                //ProjectHttpClient projectClient = connection.GetClient<ProjectHttpClient>();      

                var workItemTrackingClient = poolObj.VssConnection.GetClient<WorkItemTrackingHttpClient>();

                JsonPatchDocument patchDocument = new JsonPatchDocument();
                Fields field = null;

                string title = $"error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}";
                string bugtest = $"error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}. \n see attachment file for full error log.";
                string description = $"error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}. \n see attachment file for full error log.";

                switch (workItem.type.ToLower())
                {

                    case "bug":
                        field = new Fields()
                        {
                            AssignedTo = workItem.assignedTo,
                            BugText = bugtest,//workItem.bugtest,
                            Priority = "2",
                            Title = title,//workItem.title,
                            Severity = "2 - High"
                        };
                        ListParams.AddRange(GetBugFields(field));
                        break;

                    default://Issue,Feature,Task
                        field = new Fields()
                        {
                            AssignedTo = workItem.assignedTo,
                            Description = description,//workItem.description,
                            Priority = "2",
                            Title = title,
                            Severity = "2 - High"
                        };
                        ListParams.AddRange(GetTaskIssueFeatureFields(field));
                        break;
                }

                foreach (var item in ListParams)
                {
                    patchDocument.Add(
                          new JsonPatchOperation()
                          {
                              Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                              Path = item.Path,
                              Value = item.Value,
                          }
                      );
                }

                var workitem = await workItemTrackingClient.CreateWorkItemAsync(patchDocument, workItem.projectid, workItem.type);
                if (workitem != null)
                {
                    workItemModel.Id = workitem.Id.Value;
                    workItemModel.url = workitem.Url;
                }


            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return workItemModel;
        }


        public async Task<WorkItemModel> CreateLinkedWorkitem(CreateWorkItem workItem)
        {
            WorkItemModel workItemModel = new WorkItemModel();
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                List<Params> ListParams = new List<Params>();
                //VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);
                //ProjectHttpClient projectClient = connection.GetClient<ProjectHttpClient>();      

                var workItemTrackingClient = poolObj.VssConnection.GetClient<WorkItemTrackingHttpClient>();

                JsonPatchDocument patchDocument = new JsonPatchDocument();
                Fields field = null;
                string title = $"Error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}.";
                string bugtest = $"Error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}. \n See attachment file for full error log.";
                string description = $"Error deploying solution {workItem.crmsolutioname} in {workItem.crmorgurl}. \n See attachment file for full error log.";

                switch (workItem.type.ToLower())
                {

                    case "bug":
                        field = new Fields()
                        {
                            AssignedTo = workItem.assignedTo,
                            BugText = bugtest,
                            Priority = "2",
                            Title = title,
                            Severity = "2 - High"
                        };
                        ListParams.AddRange(GetBugFields(field));
                        break;

                    default://Issue,Feature,Task
                        field = new Fields()
                        {
                            AssignedTo = workItem.assignedTo,
                            Description = description,
                            Priority = "2",
                            Title = title,
                            Severity = "2 - High"
                        };
                        ListParams.AddRange(GetTaskIssueFeatureFields(field));
                        break;
                }

                foreach (var item in ListParams)//there will be one type of witalways 
                {
                    patchDocument.Add(
                          new JsonPatchOperation()
                          {
                              Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                              Path = item.Path,
                              Value = item.Value,
                          }
                      );
                }

                if (workItem.parentWit > 0)
                {
                    /* patchDocument.Add(new Microsoft.VisualStudio.Services.WebApi.Patch.Json.JsonPatchOperation()
                     {
                         Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                         Path = "/fields/System.Title",
                         Value = "childWIT"
                     });*/
                    patchDocument.Add(new Microsoft.VisualStudio.Services.WebApi.Patch.Json.JsonPatchOperation()
                    {
                        Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                        Path = "/relations/-",
                        Value = new
                        {
                            rel = "System.LinkTypes.Hierarchy-Reverse",
                            url = poolObj.VssConnection.Uri.AbsoluteUri + "/" + workItem.projectname + "/_apis/wit/workItems/" + workItem.parentWit

                        }
                    });
                }

                var workitem = await workItemTrackingClient.CreateWorkItemAsync(patchDocument, workItem.projectid, workItem.type);
                if (workitem != null)
                {
                    workItemModel.Id = workitem.Id.Value;
                    workItemModel.url = workitem.Url;
                }


            }
            catch (Exception)
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return workItemModel;
        }

        public async Task<WorkItemModel> CreateAttachmentToWorkitem(int workItemid, string solname,
                           string projectid, string orgnizationurl, string builderroreport)
        {
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                string _filename = $"{solname}_error.html";
                //VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);

                var WitClient = poolObj.VssConnection.GetClient<WorkItemTrackingHttpClient>();

                JsonPatchDocument patchDocument = new JsonPatchDocument();

                Guid _projectId = new Guid(projectid);
                using (var stream = GenerateStreamFromString(builderroreport))
                {
                    var att = await WitClient.CreateAttachmentAsync(stream, _projectId, fileName: _filename); // upload the file
                    patchDocument.Add(new JsonPatchOperation()
                    {
                        Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                        Path = "/relations/-",
                        Value = new
                        {
                            rel = "AttachedFile",
                            url = att.Url,
                            attributes = new { comment = $"see attachment for full error details file: {_filename} " }
                        }
                    });
                }
                await WitClient.UpdateWorkItemAsync(patchDocument, workItemid);

            }
            catch (Exception )
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return null;
        }

        public async Task<List<WorkItemModel>> GetAllworkItems(string projectName, string searchText)
        {
            List<WorkItemModel> workItems = new List<WorkItemModel>();
            WorkItemModel workItemModel = null;
            DevOpsConnectionPool poolObj = _builderPool.Get();
            try
            {
                //VssCredentials creds = new VssBasicCredential(string.Empty, personalaccesstoken);
                //VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);
                var workItemTrackingClient = poolObj.VssConnection.GetClient<WorkItemTrackingHttpClient>();

                // create a wiql object and build our query
                var wiql = new Wiql()
                {
                    // NOTE: Even if other columns are specified, only the ID & URL will be available in the WorkItemReference
                    Query = "Select [Id] " +
                            "From WorkItems " +
                            "Where  " +
                            "[System.TeamProject] = '" + projectName + "' " +
                            "And [System.WorkItemType] <> '' AND [System.State] <> '' " +
                            "AND [System.Title] contains words '" + searchText + "' ORDER BY [Id]",
                };
                var httpClient = poolObj.VssConnection.GetClient<WorkItemTrackingHttpClient>();

                var result = await httpClient.QueryByWiqlAsync(wiql);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return workItems;
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.Title", "System.State" };

                // get work items for the ids found in query
                var witems = await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf);
                if (witems != null && witems.Count > 0)
                {
                    foreach (var witem in witems)
                    {
                        workItemModel = new WorkItemModel()
                        {
                            Id = witem.Id.HasValue ? witem.Id.Value : 01,
                            url = witem.Url
                        };
                        workItems.Add(workItemModel);
                    }
                }


            }
            catch (Exception )
            {
            }
            finally
            {
                _builderPool.Return(poolObj);
            }
            return workItems;
        }



        #endregion
    }

    #region Helper Code WorkItems
    public class Params
    {
        public string Path { get; set; }
        public object Value { get; set; }
    }
    //This class contain all the fields that are mandatory to create a bug
    public class Fields
    {
        public string Title { get; set; }
        public string Priority { get; set; }
        //public string ReproSteps { get; set; }
        public string Severity { get; set; }
        public string Issue { get; set; }
        public string BugText { get; set; }
        public string Description { get; internal set; }
        public string AssignedTo { get; set; }

        //public string AttachedFiles { get; internal set; } ///fields/System.AttachedFiles
        //public string Source { get; set; }
        //public string State { get; set; }
        //public string HowFoundCategory { get; set; }
        //public string Regression { get; set; }
        //public int AttachedFileCount { get; set; }

        //public string PathOfFile { get; internal set; }
    }

    #endregion

}
