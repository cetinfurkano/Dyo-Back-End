using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dyo.Core.Utilities.Cloud.CloudinaryCloud
{
    public class CloudinaryImageUpload:ICloudRepo
    {
       
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageUpload(IConfiguration configuration)
        {
            _cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            
            Account account = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<UploadResult> UploadAsync(string fileName, Stream stream)
        {
            var uploadParams = new ImageUploadParams {
                File = new FileDescription(fileName, stream)
            };

            var imageUploadResult = await _cloudinary.UploadAsync(uploadParams);
                       
            return new UploadResult
            {
                PublicId = imageUploadResult.PublicId,
                Url = imageUploadResult.Url.ToString()
            };
        }
    }
}
