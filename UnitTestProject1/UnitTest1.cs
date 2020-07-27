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
        public string thirdJson = "{\"web-app\":{\"servlet\":[{\"servlet-name\":\"cofaxCDS\",\"servlet-class\":\"org.cofax.cds.CDSServlet\",\"init-param\":{\"configGlossary:installationAt\":\"Philadelphia, PA\",\"configGlossary:adminEmail\":\"ksm @pobox.com\",\"configGlossary:poweredBy\":\"Cofax\",\"configGlossary:poweredByIcon\":\"/images/cofax.gif\",\"configGlossary:staticPath\":\"/content/static\",\"templateProcessorClass\":\"org.cofax.WysiwygTemplate\",\"templateLoaderClass\":\"org.cofax.FilesTemplateLoader\",\"templatePath\":\"templates\",\"templateOverridePath\":\"\",\"defaultListTemplate\":\"listTemplate.htm\",\"defaultFileTemplate\":\"articleTemplate.htm\",\"useJSP\":false,\"jspListTemplate\":\"listTemplate.jsp\",\"jspFileTemplate\":\"articleTemplate.jsp\",\"cachePackageTagsTrack\":200,\"cachePackageTagsStore\":200,\"cachePackageTagsRefresh\":60,\"cacheTemplatesTrack\":100,\"cacheTemplatesStore\":50,\"cacheTemplatesRefresh\":15,\"cachePagesTrack\":200,\"cachePagesStore\":100,\"cachePagesRefresh\":10,\"cachePagesDirtyRead\":10,\"searchEngineListTemplate\":\"forSearchEnginesList.htm\",\"searchEngineFileTemplate\":\"forSearchEngines.htm\",\"searchEngineRobotsDb\":\"WEB-INF/robots.db\",\"useDataStore\":true,\"dataStoreClass\":\"org.cofax.SqlDataStore\",\"redirectionClass\":\"org.cofax.SqlRedirection\",\"dataStoreName\":\"cofax\",\"dataStoreDriver\":\"com.microsoft.jdbc.sqlserver.SQLServerDriver\",\"dataStoreUrl\":\"jdbc:microsoft:sqlserver://LOCALHOST:1433;DatabaseName=goon\",\"dataStoreUser\":\"sa\",\"dataStorePassword\":\"dataStoreTestQuery\",\"dataStoreTestQuery\":\"SET NOCOUNT ON;select test='test';\",\"dataStoreLogFile\":\"/usr/local/tomcat/logs/datastore.log\",\"dataStoreInitConns\":10,\"dataStoreMaxConns\":100,\"dataStoreConnUsageLimit\":100,\"dataStoreLogLevel\":\"debug\",\"maxUrlLength\":500}},{\"servlet-name\":\"cofaxEmail\",\"servlet-class\":\"org.cofax.cds.EmailServlet\",\"init-param\":{\"mailHost\":\"mail1\",\"mailHostOverride\":\"mail2\"}},{\"servlet-name\":\"cofaxAdmin\",\"servlet-class\":\"org.cofax.cds.AdminServlet\"},{\"servlet-name\":\"fileServlet\",\"servlet-class\":\"org.cofax.cds.FileServlet\"},{\"servlet-name\":\"cofaxTools\",\"servlet-class\":\"org.cofax.cms.CofaxToolsServlet\",\"init-param\":{\"templatePath\":\"toolstemplates/\",\"log\":1,\"logLocation\":\"/usr/local/tomcat/logs/CofaxTools.log\",\"logMaxSize\":\"\",\"dataLog\":1,\"dataLogLocation\":\"/usr/local/tomcat/logs/dataLog.log\",\"dataLogMaxSize\":\"\",\"removePageCache\":\"/content/admin/remove?cache=pages&id=\",\"removeTemplateCache\":\"/content/admin/remove?cache=templates&id=\",\"fileTransferFolder\":\"/usr/local/tomcat/webapps/content/fileTransferFolder\",\"lookInContext\":1,\"adminGroupID\":4,\"betaServer\":true}}],\"servlet-mapping\":{\"cofaxCDS\":\"/\",\"cofaxEmail\":\"/cofaxutil/aemail/*\",\"cofaxAdmin\":\"/admin/*\",\"fileServlet\":\"/static/*\",\"cofaxTools\":\"/tools/*\"},\"taglib\":{\"taglib-uri\":\"cofax.tld\",\"taglib-location\":\"/WEB-INF/tlds/cofax.tld\"}}}";
        
        [TestMethod]
        public void TestMethod1()
        {
            //Console.WriteLine(jSONInspector.FindValueFromKey(initialJSONstring, "para"));
            jSONInspector.ParseJSONString(thirdJson);
            // Console.WriteLine(jSONInspector.GetKeys() + Environment.NewLine);
            //jSONInspector.GetKeys();
            //string key1 = jSONInspector.IsolateKey("web-app");
            string value1 = jSONInspector.GetValue("web-app");
            //Console.WriteLine(value2 + Environment.NewLine);



            jSONInspector.ParseJSONString(value1);
            //jSONInspector.GetKeys();
            //string key2 = jSONInspector.IsolateKey("servlet");
            string value2 = jSONInspector.GetValue("servlet[0]");
            //Console.WriteLine(value2);
            //Console.WriteLine(value2);


            jSONInspector.ParseJSONString(value2);
            //Console.WriteLine(jSONInspector.GetKeys());
            //string key3 = jSONInspector.IsolateKey("init-param");
            string value3 = jSONInspector.GetValue("init-param");
            //Console.WriteLine(value3);

            jSONInspector.ParseJSONString(value3);
            //jSONInspector.GetKeys();
            // string key4 = jSONInspector.IsolateKey("dataStoreUrl");
            string value4 = jSONInspector.GetValue("dataStoreUrl");
            Console.WriteLine(value4);


        }

        [TestMethod]
        public void TestMethod2()
        {
            Console.WriteLine(jSONProcessing.JsonToArray(thirdJson));
            Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            Console.WriteLine(jSONProcessing.ReturnStatusCode);

            //Console.WriteLine(jSONProcessing.FlattenJson(secondJsonString));
            //Console.WriteLine(jSONProcessing.ReturnStatusDescription);
            //Console.WriteLine(jSONProcessing.ReturnStatusCode);
        }

        [TestMethod]
        public void TestMethod3()
        {
            Console.WriteLine(jSONProcessing.ResolveEntry(thirdJson, "dataLog", "|"));
        }

        [TestMethod]
        public void GetValueFromKeyTest()
        {
            
            //Console.WriteLine(value3);
            //Console.WriteLine(jSONInspector.FindValueFromKey(thirdJson, "web-app|servlet[0]|init-param|dataStoreName", "|", true));
            Console.WriteLine(jSONInspector.FindValueFromKey(thirdJson, "servlet-name", "",false));
            //Console.WriteLine(jSONInspector.ReturnStatusCode);
            //Console.WriteLine(jSONInspector.ReturnStatusDescription);
        }
    }
}
