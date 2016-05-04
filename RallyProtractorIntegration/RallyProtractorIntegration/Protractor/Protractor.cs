using System.Threading.Tasks;
using RallyProtractorIntegration.Config;
namespace RallyProtractorIntegration.Protractor
{
    public interface IProtractor
    {
        Task GetJuintResult();
    }
    public sealed class ProtractorProcessor : ProtractorWorkItem, IProtractor
    {
        private WorkItemElement _config;

        public ProtractorProcessor()
        {

        }
        public ProtractorProcessor(WorkItemElement config)
        {
            _config = config;
        }
        public async Task GetJuintResult()
        {
            string xml = await ReadFile(_config.ProtractorFilePath); 
            xml = ReplaceXmlTag(xml, @"<failure\s+", @"</failure>", "<fail message=\"true\"></fail>");
            DeSerialize(xml);
        }
        public async override Task ExecuteWorkItem()
        {
           await GetJuintResult();
        }
    }
    public abstract class ProtractorWorkItem : WorkItem<testsuites>
    {
        static ProtractorWorkItem()
        {

        }
        public abstract override Task ExecuteWorkItem();
    }
}
