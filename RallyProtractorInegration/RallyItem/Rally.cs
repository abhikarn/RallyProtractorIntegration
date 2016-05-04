using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rally.RestApi.Response;
using Rally.RestApi;
using RallyProtractorIntegration.Config;
namespace RallyProtractorIntegration.RallyItem
{
    public interface IRally
    {
        Task<List<string>> GetWorkSpace();
        Task<List<string>> GetProjects();
        void GetTestCases(string projectCode, string workSpace);
    }

    public sealed class RallyProcessor : RalyWorkItem, IRally
    {
        private WorkItemElement _config;
        private RallyRestApi restApi = new RallyRestApi();

        public RallyProcessor ()
        {

        }
        public RallyProcessor(WorkItemElement config)
        {
            _config = config;
            if(config.IsRallyRefresh)
            restApi.Authenticate(config.UserName, config.Password, config.Server, proxy: null, allowSSO: false);
        }
        public async Task<List<string>> GetWorkSpace()
        {
            var workspace = new List<string>() { "" };
            return await Task.FromResult(workspace);
        }
        public async Task<List<string>> GetProjects()
        {
            var projects = new List<string>();
            Request request = new Request("Project");
            request.Query = new Query("");
            request.Limit = 1000;
            QueryResult queryResult = restApi.Query(request);
            foreach (var result in queryResult.Results)
            {
                try
                {
                    string parent = result.Parent._ref;
                    projects.Add(parent.Split('/')[7]);
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            return await Task.FromResult(projects.Distinct().ToList());
        }
        public void GetTestCases(string projectCode, string workSpace)
        {
            //TODO:Fecthing Projects and Workspace from Rally
            String projectRef = "/project/" + projectCode; 
            String workspaceRef = "/workspace/" + workSpace; 
            Request request = new Request("TestCase");
            request.Project = projectRef;
            request.Workspace = workspaceRef;
            request.Fetch = new List<string>() { "Name", "FormattedID", "TestCases" };
            request.Query = new Query("FormattedID", Query.Operator.DoesNotEqual, "TC1"); //TODO: Make it configurable
            request.Limit = _config.RallyPageSize;  
            QueryResult queryResult = restApi.Query(request);
            //TODO:Add Support for Pagination
            var TestCases = new List<RallyProjectsProjectTestCase>();

            foreach (var result in queryResult.Results)
            {
                TestCases.Add(new RallyProjectsProjectTestCase() { id = result.FormattedID, name = result._refObjectName, reference = result._ref });
            }
            Data = new Rally() { Projects = new RallyProjects() { Project = new RallyProjectsProject() { name = projectCode, workspace = workSpace, TestCase = TestCases.ToArray() } } };
        }

        public async override Task ExecuteWorkItem()
        {
            if (_config.IsRallyRefresh)
            {
                GetTestCases(_config.RallyProjectCode, _config.RallyProjectWorkspace);
                string ser = Serialize();
                await WriteFile(_config.RallyFilePath, ser);
            }
            else
            {
                string xml = await ReadFile(_config.RallyFilePath);
                DeSerialize(xml);
            }
        }
    }

    public abstract class RalyWorkItem : WorkItem<Rally>
    {
        public RalyWorkItem()
        {

        }
        public abstract override Task ExecuteWorkItem();
    }
}
