using azuredevopsappat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.DevOpsBuilds
{
    public interface IBuildOperations
    {
       // public Task<int> CreateCRMBuild(string solname);
        /// <summary>
        /// Queue builds for solution
        /// </summary>
        /// <param name="ProjetcName">Devops project containg build</param>
        /// <param name="solname">always pass solution name property not display name.</param>
        /// <param name="pipelinevariablename">variable name to get organization url where we deploy solution</param>
        /// <param name="buildId">definitionId to run for solution deployment</param>
        /// <returns></returns>
        public Task<int> CreateBuildForSolution(string ProjetcName, string solname, string pipelinevariablename,int definitionId);

        public Task<BuildStatusModel> GetBuildStatus(string projentname, int buildId);

        public Task<BuildModel> GetProjectBuilds(string ProjetcName);

        public Task<List<BuildLogDetail>> GetBuildLogs(string projectName, int BuildId);

        public Task<BuildHtml> GetBuildFinalReport(string projectName, int BuildId);

        public Task<List<ProjectModel>> GetAllProjetcs();

        public Task<List<TeamModel>> GetProjectTeams(string projectName);
        public Task<List<TeamMemberModel>> GetProjectTeamMembers(string projectName);

        public Task<VariableModel> GetSpecificPipeLineVariable(string projectname, int definitionid, string variablekey);

        public Task<List<VariableModel>> GetPipeLineVariables(string projectname, int definitionid);

        public Task<List<PipeLineModel>> GetProjectPipeLines(string projectname);
    }
}
