

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using azuredevopsappat.DevOpsBuilds;
using azuredevopsappat.Helper.HtmlText;


namespace azuredevopsappat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildHelperController : ControllerBase ///api/buildhelper
    {
        private readonly IBuildOperations _buildManager;

        #region  constructor
        public BuildHelperController(IBuildOperations buildManager)
        {
            _buildManager = buildManager;
            // Request a StringBuilder from the pool.
        }
        #endregion

        #region  Actions
        // GET: api/BuildHelper
        [HttpGet]
        [Route("createbuildforsolution/{projname}/{solname}/{pipelinevariablename}/{buildid}")]
        public async Task<IActionResult> CreateBuildForSolution(string projname, string solname, string pipelinevariablename, int buildid)
        {
            var result = await _buildManager.CreateBuildForSolution(projname, solname, pipelinevariablename, buildid);
            return Ok(result);
        }


        [HttpGet]
        [Route("getbuildstatus/{projentname}/{buildid}")]
        public async Task<IActionResult> Getbuildstatus(string projentname, int buildid)
        {
            var result = await _buildManager.GetBuildStatus(projentname, buildid);
            return Ok(result);
        }


        [HttpGet]
        [Route("getbuildlogs/{projectname}/{buildId}")]
        public async Task<IActionResult> GetBuildLogs(string projectname, int buildId)
        {
            var result = await _buildManager.GetBuildLogs(projectname, buildId);
            return Ok(result);
        }

        [HttpGet]
        [Route("gethtmlbuildreport/{projectname}/{buildId}")]
        public async Task<IActionResult> GetHtmlBuildReport(string projectname, int buildId)
        {
            var result = await _buildManager.GetBuildFinalReport(projectname, buildId);
            return Ok(result);
        }

        [HttpGet]
        [Route("getallprojects")]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _buildManager.GetAllProjetcs();
            return Ok(result);
        }

        [HttpGet]
        [Route("getprojectteams/{projectname}")]
        public async Task<IActionResult> GetProjectTeams(string projectname)
        {
            var result = await _buildManager.GetProjectTeams(projectname);
            return Ok(result);
        }

        [HttpGet]
        [Route("getprojectallteamsmembers/{projectname}")]
        public async Task<IActionResult> GetProjectallTeamsMembers(string projectname)
        {
            var result = await _buildManager.GetProjectTeamMembers(projectname);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProjectPipeLines/{projectname}")]
        public async Task<IActionResult> GetProjectPipeLines(string projectname)
        {
            var result = await _buildManager.GetProjectPipeLines(projectname);
            return Ok(result);
        }


        /* [HttpGet]
         [Route("formathtml/{html}")]
         public async Task<IActionResult> FormatHtml(string html)
         {
             var result = HelperUtility.HtmlToPlainText(html);
             return Ok(result);
         }*/

        [HttpGet]
        [Route("getpipelinevariables/{projectname}/{definitionid}")]
        public async Task<IActionResult> GetPipeLineVariables(string projectname, int definitionid)
        {
            var result = await _buildManager.GetPipeLineVariables(projectname, definitionid); ;
            return Ok(result);
        }

        [HttpGet]
        [Route("getpipelinevariable/{projectname}/{definitionid}/{variablekey}")]
        public async Task<IActionResult> GetSpecificPipeLineVariable(string projectname, int definitionid, string variablekey)
        {
            var result = await _buildManager.GetSpecificPipeLineVariable(projectname, definitionid, variablekey);
            return Ok(result);
        }


        #endregion

    }
}