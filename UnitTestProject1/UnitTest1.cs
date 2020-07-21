using System;
using JSONParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        JSONInspector jSONInspector = new JSONInspector();
        JSONProcessing jSONProcessing = new JSONProcessing();
        public string initialJSONstring = "{\"glossary\": {\"title\": \"example glossary\",\"GlossDiv\": {\"title\": \"S\",\"GlossList\": {\"GlossEntry\": {\"ID\": \"SGML\",\"SortAs\": \"SGML\",\"GlossTerm\": \"Standard Generalized Markup Language\",\"Acronym\": \"SGML\",\"Abbrev\": \"ISO 8879:1986\",\"GlossDef\": {\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\",\"GlossSeeAlso\": [\"GML\", \"XML\"]},\"GlossSee\": \"markup\"}}}}}";
        public string substringOfInitialString = "{  \"title\": \"example glossary\",  \"GlossDiv\": {    \"title\": \"S\",    \"GlossList\": {      \"GlossEntry\": {        \"ID\": \"SGML\",        \"SortAs\": \"SGML\",        \"GlossTerm\": \"Standard Generalized Markup Language\",        \"Acronym\": \"SGML\",        \"Abbrev\": \"ISO 8879:1986\",        \"GlossDef\": {          \"para\": \"A meta-markup language, used to create markup languages such as DocBook.\",          \"GlossSeeAlso\": [            \"GML\",            \"XML\"          ]    },        \"GlossSee\": \"markup\"      }    }  }}";
        [TestMethod]
        public void TestMethod1()
        {

            Console.WriteLine(jSONInspector.IsValidJson(substringOfInitialString));

            Console.WriteLine("Initial JSON Parsed:  " + jSONInspector.ParseJSONString(substringOfInitialString));

            string key1 = jSONInspector.GetKeys();
            Console.WriteLine("Key1:  " + key1 + Environment.NewLine);

            string value1 = jSONInspector.GetValue(key1);
            Console.WriteLine("Value1:  " + value1 + Environment.NewLine);

            Console.WriteLine(jSONInspector.IsValidJson(substringOfInitialString));
            
            jSONInspector.ParseJSONString(substringOfInitialString);
            string key2 = jSONInspector.GetKeys();
            string value2 = jSONInspector.GetValue(key2);
            Console.WriteLine(value2);
           

        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine(jSONProcessing.JsonToArray(initialJSONstring));
            Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            Console.WriteLine(jSONProcessing.ReturnStatusCode);

            Console.WriteLine(jSONProcessing.FlattenJson(initialJSONstring));
            Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            Console.WriteLine(jSONProcessing.ReturnStatusCode);
        }
    }
}
