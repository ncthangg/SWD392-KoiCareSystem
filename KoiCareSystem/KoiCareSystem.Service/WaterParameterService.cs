using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Service
{
    public interface IWaterParameterService
    {
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> GetByPondId(int pondId);
        Task<ServiceResult> GetLastestByPondId(int pondId);
        Task<ServiceResult> Create(WaterParameter param);
        Task<ServiceResult> Update(WaterParameter param);
        Task<ServiceResult> Status(WaterParameter param);
    }
    public class WaterParameterService : IWaterParameterService
    {
        private readonly UnitOfWork _unitOfWork;

        public WaterParameterService() => _unitOfWork ??= new UnitOfWork();

        public async Task<ServiceResult> GetById(int id)
        {
            try
            {
                var param = await _unitOfWork.WaterParameterRepository.GetByIdAsync(id);
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> GetByPondId(int pondId)
        {
            try
            {
                var param = await _unitOfWork.WaterParameterRepository.GetByPondIdAsync(pondId);
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<WaterParameter>());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> GetLastestByPondId(int pondId)
        {
            try
            {
                var param = await _unitOfWork.WaterParameterRepository.GetLatestByPondIdAsync(pondId);
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new WaterParameter());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> Create(WaterParameter param)
        {
            try
            {
                int result = -1;
                var paramExist = await this.GetById(param.ParameterId);

                // Update existing
                if (paramExist.Status != Const.SUCCESS_READ_CODE)
                {
                    var status = (int)(await this.Status(param)).Data;
                    param.CreatedAt = DateTime.UtcNow;
                    param.UpdatedAt = DateTime.UtcNow;
                    param.StatusId = status;
                    result = await _unitOfWork.WaterParameterRepository.CreateAsync(param);

                    if(result == Const.SUCCESS_READ_CODE)
                   
                    return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, param);

                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }

        }
        public async Task<ServiceResult> Update(WaterParameter param)
        {
            try
            {
                int result = -1;
                var paramExist = await this.GetById(param.ParameterId);

                // Update existing
                if (paramExist.Status == Const.SUCCESS_READ_CODE)
                {
                    var waterParamToUpdate = (WaterParameter)paramExist.Data;
                    if(waterParamToUpdate == null)
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                    param.CreatedAt = waterParamToUpdate.CreatedAt;
                    param.UpdatedAt = DateTime.UtcNow;

                    if (waterParamToUpdate != null)
                    {
                        _unitOfWork.WaterParameterRepository.UpdateEntity(waterParamToUpdate, param);

                        result = await _unitOfWork.SaveChangesAsync();

                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, param);
                    }
                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public async Task<ServiceResult> Status(WaterParameter param)
        {
            try
            {
                var paramLimits = await _unitOfWork.WaterParameterLimitRepository.GetAllAsync();
                int outsideGoodCount = 0;
                int outsideAcceptCount = 0;

                var parameterEvaluations = new Dictionary<string, string>();

                foreach (var limit in paramLimits)
                {
                    decimal currentValue = 0;

                    // Get the current value based on the parameter name
                    switch (limit.ParameterName.ToLower())
                    {
                        case "temperature":
                            currentValue = (decimal)param.Temperature;
                            break;
                        case "salinity":
                            currentValue = (decimal)param.Salinity;
                            break;
                        case "ph":
                            currentValue = (decimal)param.Ph;
                            break;
                        case "o2":
                            currentValue = (decimal)param.O2;
                            break;
                        case "no2":
                            currentValue = (decimal)param.No2;
                            break;
                        case "no3":
                            currentValue = (decimal)param.No3;
                            break;
                        case "po4":
                            currentValue = (decimal)param.Po4;
                            break;
                        default:
                            continue; 
                    }
                    if (currentValue < limit.MinGoodValue || currentValue > limit.MaxGoodValue)
                    {
                        outsideGoodCount++;
                    }
                    else if (currentValue < limit.MinAcceptValue || currentValue > limit.MaxAcceptValue)
                    {
                        outsideAcceptCount++;
                    }

                }

                int statusId;
                if (outsideGoodCount == 0 && outsideAcceptCount == 0)
                {
                    statusId = 1; // All parameters within good range
                }
                else if ((outsideGoodCount >= 1 && outsideGoodCount <= 3) && outsideAcceptCount == 0)
                {
                    statusId = 2; // More than 2 parameters outside good range
                }
                else
                {
                    statusId = 3; // At least one parameter outside acceptable range
                }

                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, statusId);

            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<Dictionary<string, string>> Evaluate(WaterParameter param)
        {
            try
            {
                var paramLimits = await _unitOfWork.WaterParameterLimitRepository.GetAllAsync();
                var parameterEvaluations = new Dictionary<string, string>();


                foreach (var limit in paramLimits)
                {
                    decimal currentValue = 0;

                    // Get the current value based on the parameter name
                    switch (limit.ParameterName.ToLower())
                    {
                        case "temperature":
                            currentValue = (decimal)param.Temperature;
                            break;
                        case "salinity":
                            currentValue = (decimal)param.Salinity;
                            break;
                        case "ph":
                            currentValue = (decimal)param.Ph;
                            break;
                        case "o2":
                            currentValue = (decimal)param.O2;
                            break;
                        case "no2":
                            currentValue = (decimal)param.No2;
                            break;
                        case "no3":
                            currentValue = (decimal)param.No3;
                            break;
                        case "po4":
                            currentValue = (decimal)param.Po4;
                            break;
                        default:
                            continue;
                    }
                    if (currentValue >= limit.MinGoodValue && currentValue <= limit.MaxGoodValue)
                    {
                        parameterEvaluations[limit.ParameterName] = $"Good";
                    }
                    else if (currentValue < limit.MinAcceptValue)
                    {
                        var distance = limit.MinAcceptValue - currentValue;
                        parameterEvaluations[limit.ParameterName] = $" ⭣ {distance:F2}";
                    }
                    else if (currentValue > limit.MaxAcceptValue)
                    {
                        var distance = currentValue - limit.MaxAcceptValue;
                        parameterEvaluations[limit.ParameterName] = $" ⭡ {distance:F2}";
                    }
                    else
                    {
                        parameterEvaluations[limit.ParameterName] = "Within acceptable range";
                    }
                }

                //LogParameterEvaluations(parameterEvaluations);

                return parameterEvaluations;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Example logging method
        private void LogParameterEvaluations(Dictionary<string, string> evaluations)
        {
            foreach (var evaluation in evaluations)
            {
                // Here you could log to a file, database, or console
                Console.WriteLine($"{evaluation.Key}: {evaluation.Value}");
            }
        }
    }
}
