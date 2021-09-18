using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dyo.WebAPI.HelperDtos
{
    public class ProfilePhotoForCreationDto
    {
     
        public IFormFile File { get; set; }
        
    }
}
