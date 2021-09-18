using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Core.Utilities.Cloud
{
    public interface ICloudRepo
    {
        Task<UploadResult> UploadAsync(string fileName, Stream stream);
        
    }
}
