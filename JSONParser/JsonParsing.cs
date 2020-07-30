using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSONParser
{
    public class JsonParsing : JSONProperties
    {
        internal JObject jObject;
        internal string keyBase;
        
        internal Stack<string> keyStack = new Stack<string>();
        internal Dictionary<string, string> keyValueDictionary = new Dictionary<string, string>();

        public string jArrayParser(string jsonString)
        {
            string str = string.Empty;
            JArray a = JArray.Parse(jsonString);

            
            foreach (JObject o in a.Children<JObject>())
            {
                string[] jArray = new string[o.Count];
                int i = 0;
                foreach (JProperty p in o.Properties())
                {
                    string name = p.Name;
                    string value = (string)p.Value;
                    jArray[i++] = name + ":" + value;
                    
                }

                str += string.Join("~", jArray);
            }
            return str;
        }

        public string ParseJsonString(string jsonString)
        {
            string str = string.Empty;
            try
            {
                if (IsValidJson(jsonString))
                {
                    if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
                    {
                        string jStr = "{'array' : " + jsonString + "}";
                        keyBase = "array";
                        jObject = JObject.Parse(jStr);
                    }
                    else
                    {
                        keyBase = null;
                        jObject = JObject.Parse(jsonString);
                    }
                }
                else
                {
                    throw new Exception(InvalidJsonStringErrorMessage);
                }
                
            }
            catch (JsonReaderException e)
            {
                str = $"{ErrorIntro}{InvalidJsonStringErrorMessage}{Environment.NewLine}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}Parameters:  jsonString = {jsonString}{Environment.NewLine}";

            }
            catch (Exception e)
            {
                str = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}Parameters:  jsonString = {jsonString}{Environment.NewLine}";
            }
            return str;
        }


        public string GetValue(string keyName)
        {
            if (this.jObject == null)
                throw new Exception(JsonObjectNotSetErrorMessage);
            try
            {
                JToken jToken = string.IsNullOrEmpty(this.keyBase) ? jObject.SelectToken(keyName, false) : (!string.IsNullOrEmpty(keyName) ? jObject.SelectToken(this.keyBase + "." + keyName, false) : jObject.SelectToken(this.keyBase, false));
                if (jToken != null)
                {
                    ReturnStatusCode = 0;
                    return jToken.ToString();
                }
   
                throw new Exception(InvalidKeyErrorMessage);
            }
            catch (Exception e)
            {
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}Parameters:  keyName = {keyName}{Environment.NewLine}";
                return null;
            }
        }

        public string GetKeys()
        {
            if (this.jObject == null)
                throw new Exception(JsonObjectNotSetErrorMessage);
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(this.keyBase))
                {
                    stringBuilder.Append("array");
                }
                else
                {
                    using (IEnumerator<KeyValuePair<string, JToken>> enumerator = this.jObject.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<string, JToken> current = enumerator.Current;
                            stringBuilder.Append(current.Key + KeyDelimiter);
                        }
                    }
                    //stringBuilder.Length -= 4;
                }
                ReturnStatusCode = 0;
                KeyList = stringBuilder.ToString();
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return null;
            }
            
        }

        public string IsolateKey(string jKey)
        {
            try
            {
                List<string> keyList = KeyList.Split('~').ToList();
                if (keyList.Count >= 1)
                {
                    foreach (var key in keyList)
                    {
                        if (key.Equals(jKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            jKey = key;
                        }
                    }
                }
                else
                {
                    jKey = keyList[0];
                }
                ReturnStatusCode = 0;
                return jKey.Trim();
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return null;
            }
        }


        public bool IsValidJson(string jsonString)
        {
            jsonString = jsonString.Trim();
            if ((!jsonString.StartsWith("{") || !jsonString.EndsWith("}")) && (!jsonString.StartsWith("[") || !jsonString.EndsWith("]")))
                return false;
            try
            {
                JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
