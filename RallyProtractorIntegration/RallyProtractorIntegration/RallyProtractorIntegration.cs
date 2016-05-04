using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using RallyProtractorIntegration.RallyItem;
using RallyProtractorIntegration.Protractor;
using RallyProtractorIntegration.Config;
namespace RallyProtractorIntegration
{
    public sealed class RallyProtractorIntegration
    {
        private WorkItemElement _config;
        public RallyProtractorIntegration()
        {
            var configSection = RallyConfig.WorkItems;
            if (configSection != null)
            {
                foreach (WorkItemElement workitem in configSection)
                {
                    var address = workitem.UserName;
                }
            }
        }

        public RallyProtractorIntegration(WorkItemElement config)
        {
            _config = config;
        }

        public async Task ExecuteWorkItem()
        {
            var rally = new RallyProcessor(_config);

            await rally.ExecuteWorkItem();

            var protractor = new ProtractorProcessor(_config);

            await protractor.ExecuteWorkItem();

            var protractorData = await ConvertProtractor(protractor.Data);
            var rallyData = await ConvertRally(rally.Data);
            ExportExcel(rallyData, protractorData);
        }

        private async Task<List<Result>> ConvertProtractor(testsuites ts)
        {
            var protractorData = new List<Result>();
            foreach (var suite in ts.testsuite)
            {
                foreach (var testcase in suite.testcase)
                {
                    string[] spitTestCases = testcase.name.Split('=');
                    if (spitTestCases.Length > 1)
                    {
                        string splitDelimeter = spitTestCases[0];
                        string testCaseDescription = spitTestCases[1];
                        if (splitDelimeter.Contains('|'))
                        {
                            string[] splitPipe = splitDelimeter.Split('|');
                            foreach (var item in splitPipe)
                            {
                                string testCaseId2 = item.Split('_')[0];
                                protractorData.Add(new Result() { TestCaseId = testCaseId2.TrimStart().TrimEnd(), Description = testCaseDescription, Failed = testcase.failure == null ? false : true });
                            }
                        }
                        else
                        {
                            string testCaseId1 = splitDelimeter.Split('_')[0];
                            protractorData.Add(new Result() { TestCaseId = testCaseId1.TrimStart().TrimEnd(), Description = testCaseDescription.Replace("&gt;", ""), Failed = testcase.failure == null ? false : true });
                        }
                    }
                }
            }

            return await Task.FromResult(protractorData);
        }

        private void ExportExcel(List<Result> rally, List<Result> protractor)
        {
            var report = rally.Where(x => protractor.All(b => x.TestCaseId != b.TestCaseId)).ToList();

            var fileName = _config.ReportPath + "_" + _config.ProjectName + ".xlsx";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            var file = new FileInfo(fileName);
            using (var package = new OfficeOpenXml.ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Attempts");
                var rallyWorksheet = package.Workbook.Worksheets.Add("Rally Test Cases");
                var protractorWorksheet = package.Workbook.Worksheets.Add("Protractor Report");
                var reportWorksheet = package.Workbook.Worksheets.Add("Report");
                reportWorksheet.Row(1).Height = 20;

                rallyWorksheet.TabColor = Color.Red;
                rallyWorksheet.DefaultRowHeight = 12;
                rallyWorksheet.Row(1).Height = 20;

                reportWorksheet.TabColor = Color.Blue;
                reportWorksheet.DefaultRowHeight = 12;
                reportWorksheet.Row(1).Height = 20;

                protractorWorksheet.TabColor = Color.Green;
                protractorWorksheet.DefaultRowHeight = 12;
                protractorWorksheet.Row(1).Height = 20;

                protractorWorksheet.Cells[1, 1].Value = "SlNo";
                protractorWorksheet.Cells[1, 2].Value = "TestCaseID";
                protractorWorksheet.Cells[1, 3].Value = "Description";


                rallyWorksheet.Cells[1, 1].Value = "SlNo";
                rallyWorksheet.Cells[1, 2].Value = "TestCaseID";
                rallyWorksheet.Cells[1, 3].Value = "Description";

                reportWorksheet.Cells[1, 1].Value = "SlNo";
                reportWorksheet.Cells[1, 2].Value = "TestCaseID";
                reportWorksheet.Cells[1, 3].Value = "Description";


                protractorWorksheet.Cells[1, 1].Value = "SlNo";
                protractorWorksheet.Cells[1, 2].Value = "TestCaseID";
                protractorWorksheet.Cells[1, 3].Value = "Description";
                protractorWorksheet.Cells[1, 4].Value = "IsFail";

                //var cells = rallyWorksheet.Cells["A1:J1"];
                var rowCounter = 2;
                foreach (var item in rally)
                {
                    rallyWorksheet.Cells[rowCounter, 1].Value = rowCounter - 1;
                    rallyWorksheet.Cells[rowCounter, 2].Value = item.TestCaseId;
                    rallyWorksheet.Cells[rowCounter, 3].Value = item.Description;
                    rowCounter++;
                }
                rowCounter = 2;
                foreach (var item in protractor)
                {
                    protractorWorksheet.Cells[rowCounter, 1].Value = rowCounter - 1;
                    protractorWorksheet.Cells[rowCounter, 2].Value = item.TestCaseId;
                    protractorWorksheet.Cells[rowCounter, 3].Value = item.Description;
                    protractorWorksheet.Cells[rowCounter, 4].Value = item.Failed;
                    rowCounter++;
                }
                rowCounter = 2;
                foreach (var item in report)
                {
                    reportWorksheet.Cells[rowCounter, 1].Value = rowCounter - 1;
                    reportWorksheet.Cells[rowCounter, 2].Value = item.TestCaseId;
                    reportWorksheet.Cells[rowCounter, 3].Value = item.Description;
                    rowCounter++;
                }

                reportWorksheet.Column(1).AutoFit();
                reportWorksheet.Column(2).AutoFit();

                rallyWorksheet.Column(1).AutoFit();
                rallyWorksheet.Column(2).AutoFit();

                protractorWorksheet.Column(1).AutoFit();
                protractorWorksheet.Column(2).AutoFit();

                package.Workbook.Properties.Title = "Rally Protractor Integartion Report";
                package.Save();
            }
        }

        private async Task<List<Result>> ConvertRally(RallyItem.Rally rally)
        {
            var protractorData = new List<Result>();
            foreach (var rallyitem in rally.Projects.Project.TestCase)
            {
                protractorData.Add(new Result() { TestCaseId = rallyitem.id, Description = rallyitem.name, Skipped = false, Failed = false });
            }

            return await Task.FromResult(protractorData);
        }

        public sealed class Result
        {
            public string TestCaseId { get; set; }
            public string Description { get; set; }
            public bool Skipped { get; set; }
            public bool Failed { get; set; }
        }
    }
}
