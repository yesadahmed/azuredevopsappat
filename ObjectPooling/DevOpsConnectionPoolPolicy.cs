using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azuredevopsappat.ObjectPooling
{
    public class DevOpsConnectionPoolPolicy : IPooledObjectPolicy<DevOpsConnectionPool>
    {
        string _personalaccesstoken = string.Empty;
        string _collectionUri = string.Empty;        
        public static DevOpsConnectionPool devOpsConnectionPool;

        public DevOpsConnectionPoolPolicy(string personalaccesstoken, string collectionUri)
        {
            _personalaccesstoken = personalaccesstoken;
            _collectionUri = collectionUri;           
        }

        public DevOpsConnectionPool Create()
        {
            if (devOpsConnectionPool == null)
            {
                devOpsConnectionPool = new DevOpsConnectionPool();
                devOpsConnectionPool.VssCredentials = new VssBasicCredential(string.Empty, _personalaccesstoken);
                devOpsConnectionPool.VssConnection = new VssConnection(new Uri(_collectionUri), devOpsConnectionPool.VssCredentials);
                devOpsConnectionPool.CollUrl = _collectionUri;
            }

            return devOpsConnectionPool;
        }


        public bool Return(DevOpsConnectionPool obj)
        {
            return true;
        }
    }
}
