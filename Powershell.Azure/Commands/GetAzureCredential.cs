﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClrPlus.Powershell.Azure.Commands
{
    using System.Management.Automation;
    using ClrPlus.Core.Extensions;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Provider;
    using Rest.Commands;

    [Cmdlet(VerbsCommon.Get, "AzureCredentials")]
    public class GetAzureCredentials: RestableCmdlet<GetAzureCredentials>
    {
        
        protected override void ProcessRecord() {
            if (Remote)
            {
                ProcessRecordViaRest();
                return;
            }

            //this actually connects to the Azure service
            CloudStorageAccount account = new CloudStorageAccount(new StorageCredentials(Credential.UserName, Credential.Password.ToUnsecureString()), true);
            var blobPolicy = new SharedAccessBlobPolicy {
                                                            Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List,
                                                           // SharedAccessStartTime = DateTimeOffset.Now,
                                                            SharedAccessExpiryTime = DateTimeOffset.Now.AddHours(1)
                                                            
                                                        };
            
            
            //we need to return
            //"username: account rootUri password:sas"

            var root = account.CreateCloudBlobClient().GetContainerReference("container");
            
            BlobContainerPermissions blobPermissions = new BlobContainerPermissions();
            blobPermissions.SharedAccessPolicies.Add("mypolicy", blobPolicy);
            blobPermissions.PublicAccess = BlobContainerPublicAccessType.Container;
            root.SetPermissions(blobPermissions);

            var sharedAccessSignature = root.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "mypolicy");

            var psCredential = new PSCredential("{0}{1}".format(AzureDriveInfo.SAS_GUID, account.Credentials.AccountName), sharedAccessSignature.ToSecureString());
           
            WriteObject(psCredential);
        }
    }
}
