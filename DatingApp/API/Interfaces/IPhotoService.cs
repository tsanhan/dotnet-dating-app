using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        // 1. remember we have this information when we created the Photo entity (Photo.cs)?
        // * no need for a new migration 😉

        //2. now that we created the interface, we need to implement it in the PhotoService.cs
            // create and go to services/PhotoService.cs
    }
}