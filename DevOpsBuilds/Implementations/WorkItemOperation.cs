using Microsoft.Extensions.ObjectPool;
using azuredevopsappat.DevOpsBuilds.Manager.BuildManager;
using azuredevopsappat.Models;
using azuredevopsappat.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds
{
    public class WorkItemOperation : IWorkItemOperation
    {
        WorkItemManager workItemManager = null;


        public WorkItemOperation(ObjectPool<DevOpsConnectionPool> builderPool)
        {
            workItemManager = new WorkItemManager(builderPool);
        }

        public async Task<WorkItemModel> CreateAttachmentToWorkitem(int workItemid, string solname, string projectid, 
                                                            string orgnizationurl, string builderroreport)
        {
            WorkItemModel workItemModel = new WorkItemModel();
            try
            {
                workItemModel = await workItemManager.CreateAttachmentToWorkitem(workItemid, solname, projectid,
                                                                               orgnizationurl, builderroreport);
            }
            catch (Exception)
            {
            }
            return workItemModel;
        }

        public async Task<WorkItemModel> CreateLinkedWorkitem(CreateWorkItem workItem)
        {
            WorkItemModel workItemModel = new WorkItemModel();
            try
            {
                workItemModel = await workItemManager.CreateLinkedWorkitem(workItem);
            }
            catch (Exception)
            {
            }
            return workItemModel;
        }

        public async Task<WorkItemModel> CreateWorkitem(CreateWorkItem workItem)
        {
            WorkItemModel workItemModel = new WorkItemModel();
            try
            {
                workItemModel = await workItemManager.CreateWorkitem(workItem);
            }
            catch (Exception)
            {
            }
            return workItemModel;
        }

      

        public async Task<List<WorkItemModel>> GetAllworkItems(string projectName, string searchText)
        {
            List<WorkItemModel> workItems = new List<WorkItemModel>();
            try
            {
                workItems = await workItemManager.GetAllworkItems(projectName, searchText);
            }
            catch (Exception)
            {
            }
            return workItems;
        }
    }
}
