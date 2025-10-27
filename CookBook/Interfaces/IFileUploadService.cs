using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.DTOs.Files;

namespace CookBook.Interfaces
{
    public interface IFileUploadService
    {
        public Task<string> UploadProfilePictureAsync(ProfilePictureUploadDto dto, string userId);
    }
}