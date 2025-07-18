using System;
using System.Text.Json;
using System.Xml.Linq;

/*
Adapter Pattern

Summary:
The Adapter Pattern allows incompatible interfaces to work together by wrapping an existing class with a new interface. It acts as a bridge between two incompatible interfaces, enabling classes to collaborate that couldn't otherwise because of incompatible interfaces.

Problem to Solve:
In enterprise applications, you often need to integrate with legacy XML-based systems while your modern application produces JSON data. The challenge is to make these incompatible data formats work together without modifying the existing legacy system or the modern JSON producer, requiring a translation layer between them.
*/

namespace DotNetDesignPatterns.Patterns.Structural
{
    /// <summary>
    /// Data formatter interface that provides a unified contract for processing data regardless of input format
    /// </summary>
    public interface IDataFormatter
    {
        string ProcessData(string inputData);
        string FormatterType { get; }
        bool IsConverting { get; }
    }

    internal class LegacyXmlProcessor
    {
        public string ProcessXml(string xmlData)
        {
            return $"Processed: {xmlData}";
        }
    }

    internal class JsonDataSource
    {
        public string GetJsonData()
        {
            return "{\"name\":\"John\",\"age\":30,\"city\":\"New York\"}";
        }
    }

    internal class JsonToXmlAdapter : IDataFormatter
    {
        private readonly JsonDataSource _jsonDataSource;
        private readonly LegacyXmlProcessor _xmlProcessor;
        public string FormatterType => "JSON-to-XML Adapter";
        public bool IsConverting { get; private set; }

        public JsonToXmlAdapter(JsonDataSource jsonDataSource, LegacyXmlProcessor xmlProcessor)
        {
            _jsonDataSource = jsonDataSource;
            _xmlProcessor = xmlProcessor;
        }

        public string ProcessData(string inputData)
        {
            if (IsValidXml(inputData))
            {
                IsConverting = false;
                return _xmlProcessor.ProcessXml(inputData);
            }
            else
            {
                IsConverting = true;
                var convertedXml = ConvertJsonToXml(_jsonDataSource.GetJsonData());
                return _xmlProcessor.ProcessXml(convertedXml);
            }
        }

        private bool IsValidXml(string data)
        {
            if (string.IsNullOrEmpty(data))
                return false;

            try
            {
                XDocument.Parse(data);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private string ConvertJsonToXml(string jsonData)
        {
            var jsonDocument = JsonDocument.Parse(jsonData);
            var xmlElement = new XElement("person");

            foreach (var property in jsonDocument.RootElement.EnumerateObject())
            {
                xmlElement.Add(new XElement(property.Name, property.Value.ToString()));
            }

            return xmlElement.ToString();
        }
    }


    /// <summary>
    /// Creates a JSON-to-XML adapter for legacy system integration
    /// </summary>
    public static class AdapterPattern
    {
        public static IDataFormatter CreateJsonToXmlAdapter()
        {

            var jsonDataSource = new JsonDataSource();
            var xmlProcessor = new LegacyXmlProcessor();

            return new JsonToXmlAdapter(jsonDataSource, xmlProcessor);

        }
    }

    /*
    Requirements:

    To pass the tests, implement the following:

    1. JsonDataSource class (Adaptee):
       - GetJsonData() method returns: {"name":"John","age":30,"city":"New York"}
       - Represents the modern system that produces JSON data
       - Should be independent of the XML processing system

    2. LegacyXmlProcessor class (Target):
       - ProcessXml(string xmlData) method returns: "Processed: " + xmlData
       - Represents the legacy system that expects XML format
       - Cannot be modified to accept JSON directly

    3. JsonToXmlAdapter class implementing IDataFormatter (Adapter):
       - FormatterType property returns "JSON-to-XML Adapter"
       - Constructor takes JsonDataSource and LegacyXmlProcessor
       - ProcessData() converts JSON to XML and delegates to XML processor
       - IsConverting property returns true when converting JSON to XML

    4. Conversion logic:
       - Convert {"name":"John","age":30,"city":"New York"} to XML format
       - Target XML: <person><name>John</name><age>30</age><city>New York</city></person>
       - Handle both JSON input (convert then process) and XML input (process directly)

    The tests will verify that:
    - Adapter successfully converts JSON to XML format
    - Legacy XML processor receives properly formatted XML data
    - Adapter pattern enables incompatible interfaces to work together
    - IsConverting flag correctly indicates when conversion is happening
    */
}
