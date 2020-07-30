using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomExtensions;
using Newtonsoft.Json.Linq;

namespace JSONParser
{
    public class JsonGetters : JsonParsing
    {
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
                        foreach (string t in keyPath)
                        {
                            ParseJsonString(jString);
                            jString = GetValue(t);
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

                    ParseJsonString(JsonString);
                    List<string> keyList = GetKeys().Split('~').ToList<string>();

                
                    foreach(string key in keyList)
                    {
                        string arrayKey;
                        if (key.Equals(jKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ReturnStatusCode = 0;
                            //returnValueBuilder += GetValue(jKey) + "~";
                            return GetValue(jKey);
                        }

                        if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key)) continue;
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
                
                    foreach (var jString in keyStack.Select(key => GetValue(key).Replace("\r\n", String.Empty)))
                    {
                        if(!jString.Contains(jKey, StringComparison.InvariantCultureIgnoreCase))
                        {
                            keyStack.Pop();
                        }
                        else
                        {
                            keyStack.Pop();
                            return FindValueFromKey(jString, jKey);
                            //FindValueFromKey(jString, jKey);
                        }
                    }
                    
                }
                else
                {
                    JSONProcessing jsonProcessing = new JSONProcessing();
                    if (Delimiter != null)
                    {
                        char delimiter = Delimiter.ToCharArray()[0];
                        string[] jString = jsonProcessing.ResolveEntry(JsonString, jKey, Delimiter).Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);

                        Dictionary<string, string> flatDic = jString.Aggregate(new Dictionary<string, string>(), (d, v) => { d[v.Split('~')[0]] = v.Split('~')[1]; return d; });

                        foreach (var item in flatDic.Where(item => item.Key.Contains(jKey, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            return item.Value;
                        }
                    }
                }


                throw new Exception(ErrorProperties.InvalidKeyErrorMessage);

            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorProperties.ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return null;
            }

            
        }

        public string GetSiblings(string jsonString, string key, string value)
        {
            string str;
            try
            {
                JObject jObject = JObject.Parse(jsonString);
                
                List<JProperty> properties = jObject.DescendantsAndSelf().OfType<JProperty>().Where(jProp => jProp.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase) && (bool)jProp.Value.ToString().Equals(value, StringComparison.InvariantCultureIgnoreCase)).Take(1).Select(jProp => jProp.Parent).SelectMany(jCont => jCont.Children<JProperty>()).ToList();
                
                string[] propArray = new string[properties.Count];
                int index = 0;
                foreach (JProperty prop in properties)
                {
                    //str += prop.Name + ":" + prop.Value;
                    propArray[index++] = prop.Name + ":" + prop.Value;
                }

                str = string.Join("~", propArray);
                
            }
            catch (Exception e)
            {
                str = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
            }
            return str;
        }
    }
}
