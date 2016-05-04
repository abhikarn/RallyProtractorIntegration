using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace RallyProtractorIntegration
{
    public abstract class WorkItem
    {
        protected object ObjectData { get; set; }
        protected abstract Type DataType { get; }
        public abstract Task ExecuteWorkItem();
        public string Serialize()
        {
            var serializer = new XmlSerializer(DataType);
            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            serializer.Serialize(writer, ObjectData, xns);
            return sb.ToString();
        }
        public void DeSerialize(string serialzedJob)
        {
            serialzedJob = serialzedJob.Replace("&#x0;", "");
            var deserializer = new XmlSerializer(DataType);
            ObjectData = deserializer.Deserialize(new StringReader(serialzedJob));
        }
        public string RemoveXmlTag(string xml, string startXmlltag, string endXmlltag)
        {
            Regex start = new Regex(startXmlltag);
            Match startMatch = start.Match(xml);
            Regex end = new Regex(endXmlltag);
            Match endMatch = end.Match(xml, startMatch.Index);
            if (startMatch.Success)
            {
                endMatch = end.Match(xml, startMatch.Index);
                if (endMatch.Success)
                {
                    xml = xml.Remove(startMatch.Index, (endMatch.Index + endMatch.Length) - startMatch.Index);
                    return RemoveXmlTag(xml, startXmlltag, endXmlltag);
                }
            }

            return xml;
        }
        public string ReplaceXmlTag(string xml, string startXmlltag, string endXmlltag, string replaceWith)
        {
            Regex start = new Regex(startXmlltag);
            Match startMatch = start.Match(xml);
            Regex end = new Regex(endXmlltag);
            if (startMatch.Success)
            {
                var endMatch = end.Match(xml, startMatch.Index);
                if (endMatch.Success)
                {
                    string findxml = xml.Substring(startMatch.Index, (endMatch.Index + endMatch.Length) - startMatch.Index);
                    xml = xml.Replace(findxml, replaceWith);
                    return ReplaceXmlTag(xml, startXmlltag, endXmlltag, replaceWith);
                }
            }
            return xml;
        }
        public async Task WriteFile(string file, string content)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    await writer.WriteAsync(content.Trim());
                    await writer.FlushAsync();
                    writer.Close();
                }
            }
            catch { }
        }

        public async Task<string> ReadFile(string file)
        {
            try
            {
                string xml = "";
                using (StreamReader reader = new StreamReader(file))
                {
                    xml = await reader.ReadToEndAsync();
                    reader.Close();
                }
                return xml;
            }
            catch { }
            return null;
        }
    }

    public abstract class WorkItem<TData> : WorkItem
        where TData : class
    {
        public TData Data
        {
            get { return ObjectData as TData; }
            set { ObjectData = value; }
        }

        protected sealed override Type DataType
        {
            get { return typeof(TData); }
        }
    }
}
