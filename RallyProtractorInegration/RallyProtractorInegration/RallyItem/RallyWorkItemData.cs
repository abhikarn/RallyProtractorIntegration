using System.Xml.Serialization;
namespace RallyProtractorIntegration.RallyItem
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public sealed class Rally
    {
        public RallyProjects Projects { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class RallyProjects
    {
        public RallyProjectsProject Project { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class RallyProjectsProject
    {
        [XmlElementAttribute("TestCase")]
        public RallyProjectsProjectTestCase[] TestCase { get; set; }

        [XmlAttributeAttribute()]
        public string name { get; set; }

        [XmlAttributeAttribute()]
        public string workspace { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class RallyProjectsProjectTestCase
    {
        [XmlAttributeAttribute()]
        public string id { get; set; }

        [XmlAttributeAttribute()]
        public string name { get; set; }

        [XmlAttributeAttribute()]
        public string reference { get; set; }
    }
}
