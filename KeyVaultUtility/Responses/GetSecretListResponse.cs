using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultUtility.Responses
{
 
    public class GetSecretListResponse
    {
        public class RootElement
        {
            public Value[] value { get; set; }
            public object nextLink { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Attributes
        {
            public bool enabled { get; set; }
            public int exp { get; set; }
            public int created { get; set; }
            public int updated { get; set; }
            public string recoveryLevel { get; set; }
            public int recoverableDays { get; set; }
        }

    }



}
