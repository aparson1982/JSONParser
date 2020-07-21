using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    public class JSONProperties
    {
        internal static string ErrorIntro { get; set; } = "**Error**" + Environment.NewLine;

        internal static string KeyDelimiter => keyDelimiter;

        public static string JsonObjectNotSetErrorMessage => jsonObjectNotSetErrorMessage;

        public static string InvalidKeyErrorMessage => invalidKeyErrorMessage;

        public static string InvalidJsonStringErrorMessage => invalidJsonStringErrorMessage;

        public string ReturnStatusDescription { get => returnStatusDescription; set => returnStatusDescription = value; }
        public int ReturnStatusCode { get => returnStatusCode; set => returnStatusCode = value; }

        private const string invalidJsonStringErrorMessage = "The given JSON String is invalid.";
        private const string invalidKeyErrorMessage = "The JSON does not contain the provided key.";
        private const string jsonObjectNotSetErrorMessage = "Call ParseJSONString function with valid Json String as an argument before using this function.";
        private const string keyDelimiter = "@AA@";
        private string returnStatusDescription;
        private int returnStatusCode;
    }
}
