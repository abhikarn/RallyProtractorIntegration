using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace RallyProtractorIntegration
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public sealed class email
    {
        [XmlArrayItemAttribute("to", IsNullable = false)]
        public emailTO[] to { get; set; }
       
        [XmlArrayItemAttribute("cc", IsNullable = false)]
        public emailCC[] cc { get; set; }
       
        public emailBody body { get; set; }
       
        [XmlAttributeAttribute("from-address")]
        public string fromaddress { get; set; }

        [XmlAttributeAttribute("from-name")]
        public string fromname { get; set; }
       
        [XmlAttributeAttribute()]
        public string subject { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class emailTO
    {
        [XmlAttributeAttribute()]
        public string address { get; set; }
        
        [XmlAttributeAttribute()]
        public string name { get; set; }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class emailCC
    {

        [XmlAttributeAttribute()]
        public string address { get; set; }
        
        [XmlAttributeAttribute()]
        public string name { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public sealed class emailBody
    {
        [XmlAttributeAttribute("is-html")]
        public bool ishtml { get; set; }
        
        [XmlTextAttribute()]
        public string Value { get; set; }
    }
}
