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
        private Dictionary<string, string> keyValueDictionary = new Dictionary<string, string>();

        public string ParseJSONString(string jsonString)
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

        public string FindValueFromKey(string JsonString, string jKey, string Delimiter = null, bool deserializeAndFlatten = false)
        {
            try
            {
                List<string> keyPath = new List<string>();
                if (!string.IsNullOrEmpty(Delimiter) || !string.IsNullOrWhiteSpace(Delimiter))
                {
                    char delimiter = Delimiter.ToCharArray()[0];
                    keyPath = jKey.Split(delimiter, (char)StringSplitOptions.RemoveEmptyEntries).ToList();

                    if ((keyPath.Count > 1) && (!string.IsNullOrEmpty(Delimiter) || !string.IsNullOrWhiteSpace(Delimiter)))
                    {

                        string jString = JsonString;
                        int keyCounter = 1;
                        for (int i = 0; i < keyPath.Count; i++)
                        {
                            ParseJSONString(jString);
                            jString = GetValue(keyPath[i]);
                            if (keyCounter == keyPath.Count)
                            {
                                return jString;
                            }
                            keyCounter++;
                        }
                    }

                }
                if (!deserializeAndFlatten)
                {

                    ParseJSONString(JsonString);
                    List<string> keyList = GetKeys().Split('~').ToList();
                
                    foreach(string key in keyList)
                    {
                        string arrayKey;
                        if (key.Equals(jKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ReturnStatusCode = 0;
                            return GetValue(jKey);
                        }
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key))
                        {
                            string valueHolder = GetValue(key).Replace("\r\n", string.Empty);
                            if (valueHolder.StartsWith("[") && valueHolder.EndsWith("]"))
                            {
                                int count = 0;
                                while (!string.IsNullOrEmpty(valueHolder) || !string.IsNullOrWhiteSpace(valueHolder))
                                {
                                    arrayKey = key + "[" + count + "]";
                                    valueHolder = GetValue(arrayKey)?.Replace("\r\n", string.Empty);

                                    if (valueHolder.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        keyStack.Push(arrayKey);
                                    }
                                    count++;
                                }
                            }
                            else
                            {
                                if (valueHolder.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    keyStack.Push(key);
                                }
                            }
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
                            return FindValueFromKey(jString, jKey);
                        }
                    
                    }
                    
                }
                else
                {
                    JSONProcessing jsonProcessing = new JSONProcessing();
                    char delimiter = Delimiter.ToCharArray()[0];
                    string[] jString = jsonProcessing.ResolveEntry(JsonString, jKey, Delimiter).Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                    //Dictionary<string, string> flatDic = jString.Select(item => item.Split('~')).ToDictionary(s => s[0], s => s[1]);
                    Dictionary<string, string> flatDic = jString.Aggregate(new Dictionary<string, string>(), (d, v) => { d[v.Split('~')[0]] = v.Split('~')[1]; return d; });

                    foreach(KeyValuePair<string, string> item in flatDic)
                    {
                        if (item.Key.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return item.Value;
                        }
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
