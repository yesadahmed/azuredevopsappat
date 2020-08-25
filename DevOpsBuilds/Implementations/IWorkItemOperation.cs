using azuredevopsappat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds
{
    public interface IWorkItemOperation
    {
        public Task<List<WorkItemModel>> GetAllworkItems(string projectName, string searchText);

        public  Task<WorkItemModel> CreateWorkitem(CreateWorkItem workItem);

        public Task<WorkItemModel> CreateLinkedWorkitem(CreateWorkItem workItem);

        public  Task<WorkItemModel> CreateAttachmentToWorkitem(int workItemid, string solname,
                         string projectid, string orgnizationurl, string builderroreport);
    }
}
