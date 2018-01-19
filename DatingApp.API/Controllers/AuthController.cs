using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Helpers;
using DatingApp.API.Models.Data;
using DatingApp.API.Repository;
using DatingApp.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly HMACHelper _hmacHelper;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _config = config;
            _hmacHelper = new HMACHelper();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.UserName = model.UserName.ToLower();

            if (await _repo.UserExists(model.UserName))
            {
                ModelState.AddModelError("UserName", "User already exists");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<User>(model);
            var createduser = await _repo.Register(entity, model.Password);
            var userTOReturn = _mapper.Map<UserDetailViewModel>(createduser);

            return CreatedAtRoute("GetUser", new {controller = "Users", id = createduser.Id}, userTOReturn);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repo.Login(model.UserName.ToLower(), model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                };
            //generate token
            var token = _hmacHelper.GenerateToken(claims, _config.GetSection("AppSettings:SecurityToken").Value);

            var vm = _mapper.Map<UserListViewModel>(user);
             return Ok(new { token, user = vm });
        }
    }



}