using AutoMapper;
using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;
using Org.BouncyCastle.Crypto.Fpe;
using System.Text;

namespace KoiCareSystem.Service
{
    public interface IAuthenticateService
    {
        Task<ServiceResult> Login(RequestLoginDto requestLoginDto);

        //Task<User?> GetByVerificationToken(string token);


    }
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthenticateService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
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

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, userExist);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
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
