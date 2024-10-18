using AutoMapper;
using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;

namespace KoiCareSystem.Service
{
    public interface IRoleService
    {
        Task<ServiceResult> GetAllRole();
        Task<ServiceResult> GetRoleById(int id);
        Task<ServiceResult> GetRoleByName(string name);
        Task<ServiceResult> Save(Role Role);
        Task<ServiceResult> DeleteRoleById(int id);
    }
    public class RoleService : IRoleService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        //Get All
        public async Task<ServiceResult> GetAllRole()
        {
            #region Business Rule

            #endregion Business Rule

            var Roles = await _unitOfWork.RoleRepository.GetAllAsync();
            if (Roles == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Role>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Roles);
            }
        }

        //Get By Id
        public async Task<ServiceResult> GetRoleById(int id)
        {
            #region Business Rule

            #endregion Business Rule

            var Role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (Role == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Role);
            }
        }

        //Get By Name
        public async Task<ServiceResult> GetRoleByName(string name)
        {
            #region Business Rule

            #endregion Business Rule

            var Role = await _unitOfWork.RoleRepository.GetByNameAsync(name);
            if (Role == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Role);
            }
        }

        //Create/Update
        public async Task<ServiceResult> Save(Role Role)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var item = this.GetRoleByName(Role.Name);

                if (item.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.RoleRepository.UpdateAsync(Role);
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
                    result = await _unitOfWork.RoleRepository.CreateAsync(Role);
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
        public async Task<ServiceResult> DeleteRoleById(int id)
        {
            try
            {
                var result = false;

                var removeRole = this.GetRoleById(id);

                if (removeRole != null && removeRole.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.RoleRepository.RemoveAsync((Role)removeRole.Result.Data);

                    if (result)
                    {
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, removeRole.Result.Data);
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
        public bool RoleExists(int id)
        {
            return _unitOfWork.RoleRepository.RoleExists(id);
        }


    }
}
