using AutoMapper;
using KoiCareSystem.Api.Controllers.BaseController;
using KoiCareSystem.Service.Helper;
using KoiCareSystem.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Common.DTOs.Response;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data.Models;

namespace KoiCareSystem.Api.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AuthenticateService _authenticateService;
        private readonly EmailService _emailService;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IMapper _mapper;
        public AuthController(UserService userService, RoleService roleService, AuthenticateService authenticateService, EmailService emailService, IUrlHelperService urlHelperService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _authenticateService = authenticateService;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
            _mapper = mapper;
        }
        //Login
        //[HttpPost("login")]
        //public async Task<ActionResult<ApiResponseDto<ResponseUserDto>>> Login([FromBody] RequestLoginDto request)
        //{
        //    if (!ModelState.IsValid) return BadRequest();

        //    var user = await _userService.GetByEmail(request.Email);
        //    if (user.Data == null)
        //    {
        //        return BadRequest();
        //    }

        //    var result = await _authenticateService.Login(request);
        //    if (result is null) return NotFound(new ApiResponseDto<string>
        //    {
        //        StatusCode = HttpStatusCode.NotFound,
        //        Message = "User not found"
        //    });
        //    var userExist = result.Data as User;
        //    if (!userExist.IsVerified)
        //    {
        //        userExist.EmailVerificationToken = Guid.NewGuid().ToString();
        //        await _userService.UpdateVerifyCode(userExist.Email, userExist.EmailVerificationToken);

        //        //Mail Service
        //        var verificationCode = userExist.EmailVerificationToken;
        //        var verificationLink = _urlHelperService.GenerateVerificationLink(HttpContext, verificationCode);

        //        // Tiếp tục logic gửi email
        //        await _emailService.SendVerificationEmailAsync(userExist.Email, verificationCode, verificationLink);

        //        var response = _mapper.Map<ResponseUserDto>(result);
        //        response.EmailVerificationToken = verificationCode;
        //        response.VerificationLink = verificationLink;

        //        return Ok(new ApiResponseDto<ResponseUserDto>
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = "Not already Success",
        //            Data = response
        //        });

        //    }
        //    else
        //    {
        //        var response = _mapper.Map<ResponseUserDto>(result);

        //        return Ok(new ApiResponseDto<ResponseUserDto>
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = "Login Success",
        //            Data = response
        //        });

        //    }

        //}
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto<ResponseUserDto>>> Login([FromBody] RequestLoginDto request)
        {
            Console.WriteLine("Login endpoint was hit.");
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userService.GetByEmail(request.Email);
            if (user.Data == null)
            {
                return BadRequest(new ApiResponseDto<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Email or password is incorrect"
                });
            }

            var result = await _authenticateService.Login(request);
            if (result is null) return NotFound(new ApiResponseDto<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "User not found"
            });

            var response = _mapper.Map<ResponseUserDto>(result);
            return Ok(new ApiResponseDto<ResponseUserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Login Success",
                Data = response
            });
        }
        //Register
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto<ResponseUserDto>>> Register([FromBody] RequestRegisterDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var userExist = _userService.UserEmailExists(request.Email);
            if (!userExist)
            {
                var result = await _userService.Save(request);
                if (result is null) return NotFound(new ApiResponseDto<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Register fail"
                });
                var userCreate = (User)result.Data;
                //Mail Service
                var verificationCode = userCreate.EmailVerificationToken;
                var verificationLink = "abc";/*_urlHelperService.GenerateVerificationLink(HttpContext, verificationCode);*/

                // Tiếp tục logic gửi email
                await _emailService.SendVerificationEmailAsync(userCreate.Email, verificationCode, verificationLink);

                var response = _mapper.Map<ResponseUserDto>(result);
                response.EmailVerificationToken = verificationCode;
                response.VerificationLink = verificationLink;

                return Ok(new ApiResponseDto<ResponseUserDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Register Success",
                    Data = response
                });
            }
            else
            {
                return BadRequest();
            }
        }
        //Verify
        [HttpGet("verify-email")]
        public async Task<ActionResult<ApiResponseDto<string>>> VerifyToken(string email,string token)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest(new ApiResponseDto<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Invalid verified email token!"
            });
            var userExist = await _userService.GetByEmail(email);
            if (userExist != null)
            {
                var requestVerify = new RequestVerifyEmailDto()
                {
                   Email = email,
                   Code = token
                };
                var result = _userService.VerifyCode(requestVerify);

                if (result is null) return NotFound(new ApiResponseDto<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Verify fail"
                });

                return Ok(new ApiResponseDto<ResponseUserDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Verify Success",
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
