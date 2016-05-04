using System.Configuration;
namespace RallyProtractorIntegration.Config
{
    public static class RallyConfig
    {
        private static WorkItemCollection _config;
        static RallyConfig()
        {
            var configSection = ConfigurationManager.GetSection(RallyProtractorConfigSection.sectionName) as RallyProtractorConfigSection;
            _config = configSection.workItems;
        }
        public static WorkItemCollection WorkItems { get { return _config; } }
    }
    public sealed class RallyProtractorConfigSection : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string sectionName = "rally-protractor-service";

        private const string workItemCollectionName = "workitems";

        [ConfigurationProperty(workItemCollectionName)]
        [ConfigurationCollection(typeof(WorkItemCollection), AddItemName = "workitem")]
        public WorkItemCollection workItems { get { return (WorkItemCollection)base[workItemCollectionName]; } }
    }

    public sealed class WorkItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WorkItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkItemElement)element).UserName;
        }
    }

    public sealed class WorkItemElement : ConfigurationElement
    {
        [ConfigurationProperty("user-name", IsRequired = true)]
        public string UserName
        {
            get { return (string)this["user-name"]; }
            set { this["user-name"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("server", IsRequired = false, DefaultValue = "https://rally1.rallydev.com")]
        public string Server
        {
            get { return (string)this["server"]; }
            set { this["server"] = value; }
        }

        [ConfigurationProperty("proxy", IsRequired = false)]
        public string Proxy
        {
            get { return (string)this["proxy"]; }
            set { this["proxy"] = value; }
        }

        [ConfigurationProperty("allowSSO", IsRequired = false)]
        public string AllowSSO
        {
            get { return (string)this["allowSSO"]; }
            set { this["allowSSO"] = value; }
        }

        [ConfigurationProperty("project-name", IsRequired = true)]
        public string ProjectName
        {
            get { return (string)this["project-name"]; }
            set { this["project-name"] = value; }
        }

        [ConfigurationProperty("rally-project-code", IsRequired = true)]
        public string RallyProjectCode
        {
            get { return (string)this["rally-project-code"]; }
            set { this["rally-project-code"] = value; }
        }

        [ConfigurationProperty("rally-project-workspace", IsRequired = true)]
        public string RallyProjectWorkspace
        {
            get { return (string)this["rally-project-workspace"]; }
            set { this["rally-project-workspace"] = value; }
        }

        [ConfigurationProperty("rally-file-path", IsRequired = true)]
        public string RallyFilePath
        {
            get { return (string)this["rally-file-path"]; }
            set { this["rally-file-path"] = value; }
        }

        [ConfigurationProperty("rally-filter-condition", IsRequired = false)]
        public string RallyFilterCondition
        {
            get { return (string)this["rally-filter-condition"]; }
            set { this["rally-filter-condition"] = value; }
        }

        [ConfigurationProperty("protractor-file-path", IsRequired = true)]
        public string ProtractorFilePath
        {
            get { return (string)this["protractor-file-path"]; }
            set { this["protractor-file-path"] = value; }
        }

        [ConfigurationProperty("is-rally-refresh", IsRequired = false, DefaultValue = true)]
        public bool IsRallyRefresh
        {
            get { return (bool)this["is-rally-refresh"]; }
            set { this["is-rally-refresh"] = value; }
        }

        [ConfigurationProperty("is-protractor-refresh", IsRequired = false, DefaultValue = true)]
        public bool IsProtractorRefresh
        {
            get { return (bool)this["is-protractor-refresh"]; }
            set { this["is-protractor-refresh"] = value; }
        }

        [ConfigurationProperty("rally-page-size", IsRequired = true, DefaultValue = 5000)]
        public int RallyPageSize
        {
            get { return (int)this["rally-page-size"]; }
            set { this["rally-page-size"] = value; }
        }

        [ConfigurationProperty("reportPath", IsRequired = true)]
        public string ReportPath
        {
            get { return (string)this["reportPath"]; }
            set { this["reportPath"] = value; }
        }
    }
}
