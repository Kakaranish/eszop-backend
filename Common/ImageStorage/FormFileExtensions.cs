using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Common.Utilities.ImageStorage
{
    public static class FormFileExtensions
    {
        public static bool HasValidSize(this IFormFile file, int maxSizeInKB)
        {
            return file.Length < maxSizeInKB * 1024;
        }

        // https://stackoverflow.com/questions/11063900/determine-if-uploaded-file-is-image-any-format-on-mvc
        public static bool IsImage(this IFormFile file)
        {
            var validContentTypes = new[] { "image/jpg", "image/jpeg", "image/pjpeg",
                "image/gif", "image/x-png", "image/png" };
            if (!validContentTypes.Contains(file.ContentType.ToLower())) return false;

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var validFileExtensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            if (!validFileExtensions.Contains(fileExtension)) return false;

            const int imageMinimumBytes = 512;
            try
            {
                if (!file.OpenReadStream().CanRead) return false;
                if (file.Length < imageMinimumBytes) return false;

                var buffer = new byte[imageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, imageMinimumBytes);
                var content = Encoding.UTF8.GetString(buffer);

                var regex = @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy";
                var regexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline;
                if (Regex.IsMatch(content, regex, regexOptions)) return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
