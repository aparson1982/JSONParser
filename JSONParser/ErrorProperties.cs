using System;

namespace JSONParser
{
    public class ErrorProperties
    {
        internal static string ErrorIntro { get; set; } = "**Error**" + Environment.NewLine;

        public static string JsonObjectNotSetErrorMessage => jsonObjectNotSetErrorMessage;
        public static string InvalidKeyErrorMessage => invalidKeyErrorMessage;
        public static string InvalidJsonStringErrorMessage => invalidJsonStringErrorMessage;

        private const string jsonObjectNotSetErrorMessage = "Call ParseJSONString function with valid Json String as an argument before using this function.";
        private const string invalidKeyErrorMessage = "The JSON does not contain the provided key.";
        private const string invalidJsonStringErrorMessage = "The given JSON String is invalid.";
        

    }
}
