using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultUtility.Responses
{
    public class PutSecretResponse
    {


        public class Rootobject
        {
            public string value { get; set; }
            public string id { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Attributes
        {
            public bool enabled { get; set; }
            public int created { get; set; }
            public int updated { get; set; }
            public string recoveryLevel { get; set; }
        }


    }
}
