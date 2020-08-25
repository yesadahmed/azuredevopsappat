using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using azuredevopsappat.DevOpsBuilds;
using azuredevopsappat.ObjectPooling;
using Microsoft.Extensions.ObjectPool;
using azuredevopsappat.DevOpsBuilds.Manager.BuildManager;
using azuredevopsappat.Models;



namespace azuredevopsappat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        #region  class memebers
        private readonly IWorkItemOperation _workItemManager;
        private readonly ObjectPool<DevOpsConnectionPool> _builderPool;

        #endregion


        #region  constructor
        public WorkItemController(IWorkItemOperation workItemManager,
                        ObjectPool<DevOpsConnectionPool> builderPool)
        {
            this._workItemManager = workItemManager;
            _builderPool = builderPool;
        }

        #endregion


        #region  actions

        [HttpPost]
        [Route("createworkitem")]
        public async Task<IActionResult> CreateWorkItem(CreateWorkItem workItem)
        {

            var result = await _workItemManager.CreateWorkitem(workItem);
            return Ok(result);
        }

        [HttpPost]
        [Route("createlinkedworkitem")]
        public async Task<IActionResult> CreateLinkedWorkitem(CreateWorkItem workItem)
        {
            var result = await _workItemManager.CreateLinkedWorkitem(workItem);
            return Ok(result);
        }


        [HttpGet]
        [Route("getallworkitems/{projectname}/{searchtext}")]
        public async Task<IActionResult> GetAllWorkItems(string projectname, string searchtext)
        {
            var result = await _workItemManager.GetAllworkItems(projectname, searchtext);
            return Ok(result);
        }

        [HttpPost]
        [Route("createattachmenttoworkitem")]
        public async Task<IActionResult> CreateAttachmentToWorkitem(CreateWorkItem wit)
        {
            BuildManager buildManager = new BuildManager(_builderPool);
            var buildReport = await buildManager.GetBuildFinalReport(wit.projectname, wit.buildId);
            var result = await _workItemManager.CreateAttachmentToWorkitem(wit.WitId, wit.crmsolutioname,
               wit.projectid, wit.crmorgurl, buildReport.Html);
            return Ok(result);
        }


        #endregion

    }
}