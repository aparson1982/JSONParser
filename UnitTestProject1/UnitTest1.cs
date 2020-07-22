using System;
using System.Collections.Generic;
using System.Linq;
using JSONParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        JSONInspector jSONInspector = new JSONInspector();
        JSONProcessing jSONProcessing = new JSONProcessing();
        JSONProperties jsonProp = new JSONProperties();
        public string initialJSONstring = "{\"glossary\": {\"title\": \"example glossary\",\"GlossDiv\": {\"title\": \"S\",\"GlossList\": {\"GlossEntry\": {\"ID\": \"SGML\",\"SortAs\": \"SGML\",\"GlossTerm\": \"Standard Generalized Markup Language\",\"Acronym\": \"SGML\",\"Abbrev\": \"ISO 8879:1986\",\"GlossDef\": {\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\",\"GlossSeeAlso\": [\"GML\", \"XML\"]},\"GlossSee\": \"markup\"}}}}}";
        public string substringOfInitialString = "{  \"title\": \"example glossary\",  \"GlossDiv\": {    \"title\": \"S\",    \"GlossList\": {      \"GlossEntry\": {        \"ID\": \"SGML\",        \"SortAs\": \"SGML\",        \"GlossTerm\": \"Standard Generalized Markup Language\",        \"Acronym\": \"SGML\",        \"Abbrev\": \"ISO 8879:1986\",        \"GlossDef\": {          \"para\": \"A meta-markup language, used to create markup languages such as DocBook.\",          \"GlossSeeAlso\": [            \"GML\",            \"XML\"          ]    },        \"GlossSee\": \"markup\"      }    }  }}";
        public string secondJsonString = "{\"menu\": {  \"id\": \"file\",  \"value\": \"File\",  \"popup\": {    \"menuitem\": [      {\"value\": \"New\", \"onclick\": \"CreateNewDoc()\"},      {\"value\": \"Open\", \"onclick\": \"OpenDoc()\"},      {\"value\": \"Close\", \"onclick\": \"CloseDoc()\"}    ]  }}}";
        
        [TestMethod]
        public void TestMethod1()
        {

            Console.WriteLine("Valid JSON?:  " + jSONInspector.IsValidJson(initialJSONstring));
            jSONInspector.ParseJSONString(initialJSONstring);
            string key1 = jSONInspector.GetKeys();
            Console.WriteLine("Key1:  " + key1 + Environment.NewLine);
            key1 = jSONInspector.IsolateKey("glossary");
            string value1 = jSONInspector.GetValue(key1);
            Console.WriteLine("Value1:  " + value1 + Environment.NewLine);
            
            Console.WriteLine(jSONInspector.IsValidJson(value1));
            jSONInspector.ParseJSONString(value1);
            string key2 = jSONInspector.GetKeys();
            key2 = jSONInspector.IsolateKey("GlossDiv");
            string value2 = jSONInspector.GetValue(key2);
            Console.WriteLine("Value2:  " + value2);

            
           

        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine(jSONProcessing.JsonToArray(secondJsonString));
            Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            Console.WriteLine(jSONProcessing.ReturnStatusCode);

            Console.WriteLine(jSONProcessing.FlattenJson(secondJsonString));
            Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            Console.WriteLine(jSONProcessing.ReturnStatusCode);
        }

        [TestMethod]
        public void TestMethod3()
        {
            Console.WriteLine(jSONProcessing.ResolveEntry(initialJSONstring, "GlossList"));
        }
    }
}
