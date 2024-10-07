using AutoMapper;
using Azure.Core;
using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.User;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Data.Repository;
using KoiCareSystematHome.Service.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Service
{
    public interface IUserService
    {
        Task<ServiceResult> GetAllUser();
        Task<ServiceResult> GetUserById(long id);
        Task<ServiceResult> GetUserByEmail(string email);
        Task<ServiceResult> Save(RegisterDto registerDto);
        Task<ServiceResult> SaveByAdmin(RegisterAdminDto registerAdminDto);
        Task<ServiceResult> DeleteUserByEmail(string email);
    }
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }
        //Get All
        public async Task<ServiceResult> GetAllUser()
        {
            #region Business Rule

            #endregion Business Rule

            var Users = await _unitOfWork.UserRepository.GetAllAsync();
            if (Users == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<User>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Users);
            }
        }
        //Get By Id
        public async Task<ServiceResult> GetUserById(long id)
        {
            #region Business Rule

            #endregion Business Rule

            var User = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (User == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, User);
            }
        }
        //Get By Email
        public async Task<ServiceResult> GetUserByEmail(string email)
        {
            #region Business Rule

            #endregion Business Rule

            var User = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            if (User == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, User);
            }
        }
        //Create/Update
        public async Task<ServiceResult> Save(RegisterDto registerDto)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var userExisted = this.GetUserByEmail(registerDto.Email);

                if (userExisted.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    var userToUpdate = _mapper.Map<User>(registerDto);
                    result = await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    var newUser = _mapper.Map<User>(registerDto);
                    newUser.PashwordHash = HashPassword(registerDto.Password);
                    newUser.RoleId = 1;
                    result = await _unitOfWork.UserRepository.CreateAsync(newUser);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        //Create/Update //ROLE: ADMIN
        public async Task<ServiceResult> SaveByAdmin(RegisterAdminDto registerAdminDto)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var userExisted = this.GetUserByEmail(registerAdminDto.Email);

                if (userExisted.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    var userToUpdate = _mapper.Map<User>(registerAdminDto);
                    result = await _unitOfWork.UserRepository.UpdateAsync(userToUpdate);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    var newUser = _mapper.Map<User>(registerAdminDto);
                    newUser.Id = 5;
                    newUser.PashwordHash = HashPassword(registerAdminDto.Password);
                    result = await _unitOfWork.UserRepository.CreateAsync(newUser);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        //Delete by Id
        public async Task<ServiceResult> DeleteUserByEmail(string email)
        {
            try
            {
                var result = false;

                var removeUser = this.GetUserByEmail(email);

                if (removeUser != null && removeUser.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.UserRepository.RemoveAsync((User)removeUser.Result.Data);

                    if (result)
                    {
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, removeUser.Result.Data);
                    }
                }
                else
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        //
        public bool UserExists(string email)
        {
            return _unitOfWork.UserRepository.UserExists(email);
        }
        //Helper
        private string HashPassword(string password)
        {
            // Thực hiện băm mật khẩu (sử dụng phương pháp băm phù hợp)
            return password; // Thay đổi thành phương pháp băm thực tế
        }
    }
}
