using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CustomExtensions;

namespace JSONParser
{
    public class JSONInspector : JSONProperties
    {
        private JObject jObject;
        private string keyBase;
        private Stack<string> keyStack = new Stack<string>();
        private Stack<string> valueStack = new Stack<string>();

        public string ParseJSONString(string jsonString)
        {
            string str = string.Empty;
            try
            {
                if (IsValidJson(jsonString))
                {
                    if (jsonString.StartsWith("[") && jsonString.EndsWith("]"))
                    {
                        string jStr = "{'data' : " + jsonString + "}";
                        keyBase = "data";
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
                JToken jtoken = string.IsNullOrEmpty(this.keyBase) ? jObject.SelectToken(keyName, false) : (!string.IsNullOrEmpty(keyName) ? jObject.SelectToken(this.keyBase + "." + keyName, false) : jObject.SelectToken(this.keyBase, false));
                if (jtoken != null)
                {
                    ReturnStatusCode = 0;
                    return jtoken.ToString();
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
                    stringBuilder.Append("data");
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
                        if (key.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
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

        public string FindValueFromKey(string JsonString, string jKey, string KeyPath = null, string Delimiter = null)
        {
            
            try
            {
                ParseJSONString(JsonString);

                List<string> keyList = GetKeys().Split('~').ToList();
                List<string> valueList = new List<string>();
                foreach(string key in keyList)
                {
                    if (key.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ReturnStatusCode = 0;
                        return GetValue(jKey);
                    }
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key))
                    {
                        keyStack.Push(key);
                    }
                    
                }

                foreach (string key in keyStack)
                {
                    string jString = GetValue(key).Replace("\r\n", string.Empty);
                    if(!jString.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        keyStack.Pop();
                    }
                    else
                    {
                        keyStack.Pop();
                        FindValueFromKey(jString, jKey);
                        
                    }
                    
                }

                throw new Exception(InvalidKeyErrorMessage);

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
