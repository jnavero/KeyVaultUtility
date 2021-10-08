using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultUtility
{
    public class Settings
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Tenant { get; set; }
        public string UniqueKeyVaultName { get; set; }

        public string ClientId { get; set; }
        public string SecretId { get; set; }

    }
}
