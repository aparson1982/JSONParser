using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    public class JSONProperties : ErrorProperties
    {
        
        internal static string KeyDelimiter => keyDelimiter;
        
        public string ReturnStatusDescription { get => returnStatusDescription; set => returnStatusDescription = value; }
        public int ReturnStatusCode { get => returnStatusCode; set => returnStatusCode = value; }
        public string ResolvedEntryJson { get => resolvedEntryJson; set => resolvedEntryJson = value; }
        public string KeyList { get => keyList; set => keyList = value; }

        private const string keyDelimiter = "~";
        private string returnStatusDescription;
        private int returnStatusCode;
        private string resolvedEntryJson;
        private string keyList;
        
    }
}
