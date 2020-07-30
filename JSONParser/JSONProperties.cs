namespace JSONParser
{
    public class JSONProperties : ErrorProperties
    {
        
        internal static string KeyDelimiter => keyDelimiter;
        
        public string ReturnStatusDescription { get; set; }

        public int ReturnStatusCode { get; set; }

        public string ResolvedEntryJson { get; set; }

        public string KeyList { get; set; }

        private const string keyDelimiter = "~";
    }
}
