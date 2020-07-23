using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSONParser
{
    public class JSONProcessing : JSONProperties
    {
        private static Dictionary<string, object> DeserializeAndFlatten(string json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            JToken token = JToken.Parse(json);
            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static Dictionary<string, object> DeserializeJson(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = serializer.Deserialize<Dictionary<string, object>>(json);
            return dictionary;
        }

        private void Resolve(Dictionary<string,object> dic, string SupKey, string Delimiter)
        {
            char delimiter = Delimiter.ToCharArray()[0];
            foreach (KeyValuePair<string, object> entry in dic)
            {
                if (entry.Value is Dictionary<string,object>)
                {
                    Resolve((Dictionary<string, object>)entry.Value, entry.Key, Delimiter);
                }
                else
                {
                    if (entry.Value is ICollection)
                    {
                        foreach (object item in (ICollection)entry.Value)
                        {
                            if (item is Dictionary<string,object>)
                            {
                                Resolve((Dictionary<string, object>)item, SupKey + " : " + entry.Key, Delimiter);
                            }
                            else
                            {
                                ResolvedEntryJson += item.ToString();
                            }
                        }
                    }
                    else
                    {
                        ResolvedEntryJson += SupKey + " : " + entry.Key.ToString() + "~" + entry.Value.ToString() + delimiter;
                    }
                }
            }
        }

        public string ResolveEntry(string JsonString, string key, string Delimiter)
        {
            Dictionary<string, object> dic = DeserializeJson(JsonString);
            Resolve(dic, key, Delimiter);
            return ResolvedEntryJson;
        }


        private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
        {
            JTokenType type = token.Type;
            int tokenType = (int)type;
            if (tokenType != 1)
            {
                if (tokenType == 2)
                {
                    int num = 0;
                    JEnumerable<JToken> jenumerable = token.Children();
                    using (IEnumerator<JToken> enumerator = jenumerable.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            JToken current = enumerator.Current;
                            FillDictionaryFromJToken(dict, current, Join(prefix, num.ToString()));
                            ++num;
                        }
                    }
                }
                else
                {
                    dict.Add(prefix, ((JValue)token).Value);
                }
            }
            else
            {
                JEnumerable<JProperty> jenumerable = token.Children<JProperty>();
                using (IEnumerator<JProperty> enumerator = jenumerable.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        JProperty current = enumerator.Current;
                        FillDictionaryFromJToken(dict, current.Value, Join(prefix, current.Name));
                    }
                }
            }
        }

        private static string Join(string prefix, string name)
        {
            return string.IsNullOrEmpty(prefix) ? name : prefix + "." + name;
        }


        public string[,] JsonToArray(string jsonString)
        {
            string empty = string.Empty;
            if (string.IsNullOrEmpty(jsonString.Trim()))
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = ErrorIntro + "Empty Json String Value, Enter a valid Json String";
                return null;
            }
            try
            {
                Dictionary<string, object> dictionary = DeserializeAndFlatten(jsonString);
                List<string> stringList = new List<string>();
                int num1 = 0;
                int num2 = 0;
                int num3 = 0;
                foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                {
                    int num4 = keyValuePair.Key.LastIndexOf(".");
                    string str1 = num4 > -1 ? keyValuePair.Key.Substring(num4 + 1) : keyValuePair.Key;
                    Match match = Regex.Match(keyValuePair.Key, "\\.([1-9]+)\\.");
                    if (match.Success)
                    {
                        string str2 = str1 + match.Groups[1].Value;
                        num1 = 1;
                    }
                    else
                        num2 = 1;
                    if (num1 == 1 && num2 == 1)
                        num3 = 1;
                }
                if (num3 == 1)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                    {
                        int num4 = keyValuePair.Key.LastIndexOf(".");
                        string str = num4 > -1 ? keyValuePair.Key.Substring(num4 + 1) : keyValuePair.Key;
                        if (!Regex.Match(keyValuePair.Key, "\\.([0-9]+)\\.").Success)
                            stringList.Add(keyValuePair.Key);
                    }
                    foreach (string key in stringList)
                    {
                        if (!string.IsNullOrEmpty(key))
                            dictionary.Remove(key);
                    }
                }
                DataTable dataTable = new DataTable();
                dataTable.Clear();
                DataRow row1 = (DataRow)null;
                int num5 = 0;
                string str3 = string.Empty;
                foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                {
                    int num4 = keyValuePair.Key.LastIndexOf(".");
                    if (num5 == 0)
                    {
                        str3 = keyValuePair.Key.Substring(num4 + 1);
                        Regex.Match(keyValuePair.Key, "\\.([0-9]+)\\.");
                        if (!dataTable.Columns.Contains(keyValuePair.Key.Substring(num4 + 1)))
                            dataTable.Columns.Add(keyValuePair.Key.Substring(num4 + 1));
                    }
                    else if (!str3.Equals(keyValuePair.Key.Substring(num4 + 1)))
                    {
                        Console.WriteLine(keyValuePair.Key.Substring(num4 + 1));
                        if (!dataTable.Columns.Contains(keyValuePair.Key.Substring(num4 + 1)))
                            dataTable.Columns.Add(keyValuePair.Key.Substring(num4 + 1));
                    }
                    else
                        break;
                    ++num5;
                }
                int num6 = 0;
                foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                {
                    int num4 = keyValuePair.Key.LastIndexOf(".");
                    string str1 = num4 > -1 ? keyValuePair.Key.Substring(num4 + 1) : keyValuePair.Key;
                    Match match = Regex.Match(keyValuePair.Key, "\\.([0-9]+)\\.");
                    if (match.Success)
                    {
                        string str2 = str1 + match.Groups[1].Value;
                    }
                    if (num6 == num5)
                    {
                        dataTable.Rows.Add(row1);
                        num6 = 0;
                    }
                    if (num6 == 0)
                        row1 = dataTable.NewRow();
                    row1[keyValuePair.Key.Substring(num4 + 1)] = keyValuePair.Value;
                    ++num6;
                }
                dataTable.Rows.Add(row1);
                int length = dataTable.Rows.Count + 1;
                int count = dataTable.Columns.Count;
                int index1 = 0;
                int index2 = 0;
                string[,] strArray = new string[length, count];
                foreach (DataColumn column in dataTable.Columns)
                {
                    strArray[index1, index2] = column.ColumnName;
                    ++index2;
                }
                int index3 = 0;
                int index4 = index1 + 1;
                foreach (DataRow row2 in (InternalDataCollectionBase)dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        ReturnStatusDescription = ReturnStatusDescription + row2[column]?.ToString() + "\t|";
                        strArray[index4, index3] = row2[column].ToString();
                        ++index3;
                    }
                    index3 = 0;
                    ++index4;
                    ReturnStatusDescription += "\n";
                }
                ReturnStatusCode = 0;
                return strArray;
            }
            catch (JsonReaderException e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return null;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return null;
            }
        }

        public int FlattenJson(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString.Trim()))
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = ErrorIntro + "Empty Json String Value, Enter a valid Json String";
                return ReturnStatusCode;
            }
            try
            {
                Dictionary<string, object> dictionary = DeserializeAndFlatten(jsonString);
                ReturnStatusDescription = string.Empty;
                foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                    ReturnStatusDescription = ReturnStatusDescription + keyValuePair.Key + ": " + keyValuePair.Value?.ToString() + "\n";
                return 0;
            }
            catch (JsonReaderException e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return ReturnStatusCode;
            }
            catch (Exception e)
            {
                ReturnStatusCode = -1;
                ReturnStatusDescription = $"{ErrorIntro}Message:  {e.Message}{Environment.NewLine}Source:  {e.Source}{Environment.NewLine}StackTrace:  {e.StackTrace}{Environment.NewLine}Inner Exception:  {e.InnerException}{Environment.NewLine}";
                return ReturnStatusCode;
            }
        }

    }
}
