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

    /// <summary>
    /// Creates a JSON-to-XML adapter for legacy system integration
    /// </summary>
    public static class AdapterPattern
    {
        public static IDataFormatter CreateJsonToXmlAdapter()
        {
            throw new NotImplementedException("Implement Adapter pattern components");
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
