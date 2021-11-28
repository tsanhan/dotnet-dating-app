using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        // Cloudinary will communicate with the api service 
        private readonly Cloudinary _cloudinary;

        //1. build a constructor
        public PhotoService(/*this is how we'll inject a congif file*/IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                // order is important
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            //we crete a Cloudinary onject by paaing the cofiguration in a dedicated object
            _cloudinary = new Cloudinary(acc);
        }

        //2. implement DeletePhotoAsync
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        //3. implement UploadPhotoAsync
        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream(); 

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),

                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams); 
                // the actual save of the file. this will return an object we want to inspect:
                // * the 'Error' property is null, that's good!
                // * in 'more' we see we have PublicId, SecureUrl, that's great!
                // now we can 'step out' ⬆ from the method

            }
            return uploadResult;
        }
        //4. add this service as a dependency in the ApplicationServiceExtensions.cs, go there. 
    }
}