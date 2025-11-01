using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookBook.Data;
using CookBook.DTOs;
using CookBook.DTOs.Files;
using CookBook.Helpers;
using CookBook.Interfaces;
using CookBook.Migrations;
using CookBook.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CookBook.Services
{
    public class FileUploads : IFileUploadService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioSettings _minioSettings;
        private readonly ApplicationDBContext _context;

        public FileUploads(IMinioClient minioClient, ApplicationDBContext context, IOptions<MinioSettings> options)
        {
            _minioClient = minioClient;
            _minioSettings = options.Value;
            _context = context;
        }

        public async Task<string?> UploadProfilePictureAsync(ProfilePictureUploadDto dto, string userId)
        {
            if (dto.File == null || dto.File.Length == 0)
                return null;

            var fileName = $"{userId}_profile_{Guid.NewGuid()}_{dto.File.FileName}";
            var bucketName = "profile-pictures";

            using var stream = dto.File.OpenReadStream();

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(dto.File.Length)
                .WithContentType(dto.File.ContentType));

            var fileUrl = $"http://{_minioSettings.Endpoint}/{bucketName}/{fileName}";

            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserId == userId);

            if (userSettings != null)
            {
                userSettings.ProfileImageUrl = fileUrl;
                await _context.SaveChangesAsync();
            }

            return fileUrl;
        }
        
        public async Task<bool> DeleteProfilePictureAsync(string userId)
        {
            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserId == userId);

            if (userSettings == null || string.IsNullOrEmpty(userSettings.ProfileImageUrl))
                return false;

            var fileUrl = userSettings.ProfileImageUrl;
            var bucketName = "profile-pictures";
            var fileName = fileUrl.Split('/').Last();

            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName));

            userSettings.ProfileImageUrl = "";
            await _context.SaveChangesAsync();

            return true;
        }   
    }
}