using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using DatingApp.API.Models.Data;
using DatingApp.API.Repository;
using DatingApp.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _CloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo,
                 IMapper mapper, IOptions<CloudinarySettings> CloudinaryConfig)
        {
            _CloudinaryConfig = CloudinaryConfig;
            _repo = repo;
            _mapper = mapper;
            Account acc = new Account(
                        _CloudinaryConfig.Value.CloudName,
                        _CloudinaryConfig.Value.ApiKey,
                        _CloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var entity = await _repo.GetPhoto(id);

            var photoToReturn = _mapper.Map<PhotoToReturnViewModel>(entity);
            return Ok(photoToReturn);

        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreateViewModel model)
        {
            var user = await _repo.GetUser(userId);

            if (user == null)
            {
                return BadRequest("Could not find user");
            }
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (currentUser != user.Id)
            {
                return Unauthorized();
            }

            var file = model.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            model.Url = uploadResult.Uri.ToString();
            model.PublicId = uploadResult.PublicId;

            var photoEntity = _mapper.Map<Photo>(model);
            photoEntity.User = user;

            if (!user.Photos.Any(x => x.IsMain))
            {
                photoEntity.IsMain = true;
            }

            user.Photos.Add(photoEntity);
            if (await _repo.SaveAllAsync())
            {
                var photoToReturn = _mapper.Map<PhotoToReturnViewModel>(photoEntity);
                return CreatedAtRoute("GetPhoto", new { id = photoEntity.Id }, photoToReturn);
            }

            return BadRequest("Could Not Add the Photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int userId, int id)
        {
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(userId != currentUser)
            {
                return Unauthorized();
            }

            var entity = await _repo.GetPhoto(id);
            if(entity == null)
            {
                return NotFound();
            }

            if(entity.IsMain)
            {
                return BadRequest("This is already the main photo");
            }

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);

            if(currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }
            entity.IsMain = true;

            if(await _repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(userId != currentUser)
            {
                return Unauthorized();
            }

            var entity = await _repo.GetPhoto(id);
            if(entity == null)
            {
                return NotFound();
            }

            if(entity.IsMain)
            {
                return BadRequest("This is main photo. You can't delete it");
            }


            if(entity.PublicId == null)
            {
               _repo.Delete(entity);
                if(await _repo.SaveAllAsync())
                {
                    return Ok();
                } 
            }
            else
            {                
                var deleteParams = new DeletionParams(entity.PublicId);
                var result = _cloudinary.Destroy(deleteParams);
                if(result.Result == "ok")
                {
                    _repo.Delete(entity);

                    if(await _repo.SaveAllAsync())
                    {
                        return Ok();
                    }
                }
            }
            


            return BadRequest("Failed to delete the photo");

        }

    }
}