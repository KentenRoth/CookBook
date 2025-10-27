using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Files
{
    public class ProfilePictureUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }
}