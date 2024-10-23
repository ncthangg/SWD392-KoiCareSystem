using AutoMapper;
using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Service.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystem.Service
{
    public interface IAuthenticateService
    {
        Task<ServiceResult> Login(RequestLoginDto requestLoginDto);
        ServiceResult Logout();
    }
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthenticateService(IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult> Login(RequestLoginDto requestLoginDto)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule
                if (string.IsNullOrEmpty(requestLoginDto.Email) || string.IsNullOrEmpty(requestLoginDto.Password))
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                var userExist = await _unitOfWork.UserRepository.GetByEmailAsync(requestLoginDto.Email);

                if (userExist == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                // So sánh mật khẩu sau khi băm
                if (!VerifyPasswordHash(requestLoginDto.Password, userExist.PasswordHash))
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                // Kiểm tra trạng thái xác minh email
                if (!userExist.IsVerified)
                {
                    // Chuyển hướng người dùng tới trang VerifyEmail
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, userExist);
                }

                var session = _httpContextAccessor.HttpContext.Session;
                session.SetString("UserEmail", userExist.Email); // Lưu UserName vào session
                session.SetInt32("UserId", userExist.Id); // Lưu UserId vào session
                session.SetString("UserRole", userExist.Role.Name);

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, userExist);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public ServiceResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear(); // Xóa toàn bộ session

            // Trả về một thông báo khi đăng xuất thành công
            return new ServiceResult(Const.SUCCESS_READ_CODE, "You have logged out successfully.");
        }

        #region Helper Methods

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            //// Sử dụng SHA256 để băm mật khẩu
            //using (var sha256 = SHA256.Create())
            //{
            //    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    var hashString = Convert.ToBase64String(hashBytes);

            //    return hashString == storedHash;
            //}
            if (password != passwordHash)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
