using System.Xml.Serialization;
namespace RallyProtractorIntegration
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public sealed class testsuites
    {
        [XmlElementAttribute("testsuite")]
        public testsuitesTestsuite[] testsuite { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class testsuitesTestsuite
    {

        [XmlElementAttribute("testcase")]
        public testsuitesTestsuiteTestcase[] testcase { get; set; }
        
        [XmlAttributeAttribute()]
        public string name { get; set; }
       
        [XmlAttributeAttribute()]
        public byte errors { get; set; }
       
        [XmlAttributeAttribute()]
        public byte tests { get; set; }
        
        [XmlAttributeAttribute()]
        public byte failures { get; set; }
        
        [XmlAttributeAttribute()]
        public decimal time { get; set; }
        
        [XmlAttributeAttribute()]
        public System.DateTime timestamp { get; set; }
       
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class testsuitesTestsuiteTestcase
    {

        public testsuitesTestsuiteTestcaseFailure failure { get; set; }
        
        [XmlAttributeAttribute()]
        public string classname { get; set; }
        
        [XmlAttributeAttribute()]
        public string name { get; set; }
       
        [XmlAttributeAttribute()]
        public decimal time { get; set; }
       
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public partial class testsuitesTestsuiteTestcaseFailure
    {
        [XmlAttributeAttribute()]
        public string type { get; set; }
       
        [XmlAttributeAttribute()]
        public string message { get; set; }
       
        [XmlTextAttribute()]
        public string Value { get; set; }
       
    }
}

