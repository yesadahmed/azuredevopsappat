using Microsoft.Extensions.ObjectPool;
using azuredevopsappat.DevOpsBuilds.Manager.BuildManager;
using azuredevopsappat.Models;
using azuredevopsappat.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds
{

    public class BuildOperations : IBuildOperations
    {
        BuildManager buildManager = null;


        public BuildOperations(ObjectPool<DevOpsConnectionPool> builderPool)
        {
            buildManager = new BuildManager(builderPool);
        }

        /// <summary>
        /// Queue builds for solution
        /// </summary>
        /// <param name="ProjetcName">Devops project containg build</param>
        /// <param name="solname">always pass solution name property not display name.</param>
        /// <param name="pipelinevariablename">variable name to get organization url where we deploy solution</param>
        /// <param name="buildId">definitionId to run for solution deployment</param>
        /// <returns></returns>
        public async Task<int> CreateBuildForSolution(string ProjetcName, string solname, string pipelinevariablename, int definitionId)
        {
            int buldId =0;
            try
            {
                buldId = await buildManager.CreateBuildForSolution(ProjetcName, solname, pipelinevariablename, definitionId);
            }
            catch (Exception)
            {

            }
            return buldId;
        }

       /* public async Task<int> CreateCRMBuild(string solname)
        {
            int result = 0;
            try
            {
                result = await buildManager.CreateCRMTestBuild(solname);
            }
            catch (Exception)
            {

            }
            return result;
        }*/

        public async Task<List<ProjectModel>> GetAllProjetcs()
        {
            List<ProjectModel> lst = new List<ProjectModel>();


            try
            {
                lst = await buildManager.GetAllProjetcs();
            }
            catch (Exception)
            {

            }
            return lst;
        }

        public async Task<BuildHtml> GetBuildFinalReport(string projectName, int BuildId)
        {
            BuildHtml htmlcontent = new BuildHtml();
            try
            {
                htmlcontent = await buildManager.GetBuildFinalReport(projectName, BuildId);
            }
            catch (Exception)
            {
            }
            return htmlcontent;

        }

        public async Task<List<BuildLogDetail>> GetBuildLogs(string projectName, int BuildId)
        {
            List<BuildLogDetail> buildLogs = new List<BuildLogDetail>();
            try
            {
                buildLogs = await buildManager.GetBuildLogs(projectName, BuildId);
            }
            catch (Exception)
            {

            }
            return buildLogs;
        }

        public async Task<BuildStatusModel> GetBuildStatus(string projentname, int buildId)
        {
            BuildStatusModel buildStatusModel = new BuildStatusModel();
            try
            {
                buildStatusModel = await buildManager.GetBuildStatus(projentname, buildId);
            }
            catch (Exception)
            {


            }
            return buildStatusModel;
        }



        public async Task<BuildModel> GetProjectBuilds(string ProjetcName)
        {
            BuildModel buildModel = new BuildModel();
            try
            {
                buildModel = await buildManager.GetProjectBuilds(ProjetcName);


            }
            catch (Exception)
            {
            }
            return buildModel;
        }

        public async Task<List<TeamMemberModel>> GetProjectTeamMembers(string projectName)
        {
            List<TeamMemberModel> tempLst = new List<TeamMemberModel>();
            try
            {
                tempLst = await buildManager.GetProjectTeamMembers(projectName);
            }
            catch (Exception)
            {
            }
            return tempLst;
        }

        public async Task<List<TeamModel>> GetProjectTeams(string projectName)
        {
            List<TeamModel> lst = new List<TeamModel>();

            try
            {
                lst = await buildManager.GetProjectTeams(projectName);
            }
            catch (Exception)
            {

            }
            return lst;
        }

        public async Task<VariableModel> GetSpecificPipeLineVariable(string projectname, int definitionid, string variablekey)
        {
            VariableModel variableModel = new VariableModel();
            try
            {
                variableModel = await buildManager.GetSpecificPipeLineVariable(projectname, definitionid, variablekey);
            }
            catch (Exception)
            {

            }

            return variableModel;
        }
        public async Task<List<VariableModel>> GetPipeLineVariables(string projectname, int definitionid)
        {
            List<VariableModel> lst = new List<VariableModel>();
            try
            {
                lst = await buildManager.GetPipeLineVariables(projectname, definitionid);
            }
            catch (Exception)
            {

            }

            return lst;
        }

        public async Task<List<PipeLineModel>> GetProjectPipeLines(string projectname)
        {
            List<PipeLineModel> pipelines = new List<PipeLineModel>();
            try
            {
                pipelines = await buildManager.GetProjectPipeLines(projectname);
            }
            catch (Exception)
            {

            }
            return pipelines;
        }
    }
}
