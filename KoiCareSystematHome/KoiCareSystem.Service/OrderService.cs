using AutoMapper;
using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace KoiCareSystem.Service
{
    public interface IOrderService
    {
        Task<ServiceResult> GetAllOrder();
        Task<ServiceResult> GetOrderByOrderId(long orderId);
        Task<ServiceResult> GetOrdersByUserId(long userId);
        Task<ServiceResult> Save(Order order);
        Task<ServiceResult> DeleteOrderByOrderId(long orderId);
    }
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        public OrderService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        //Get All
        public async Task<ServiceResult> GetAllOrder()
        {
            #region Business Rule

            #endregion Business Rule

            var Orders = await _unitOfWork.OrderRepository.GetAllAsync();
            if (Orders == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Orders);
            }
        }
        //Get By Id
        public async Task<ServiceResult> GetOrderByOrderId(long orderId)
        {
            #region Business Rule

            #endregion Business Rule

            var Order = await _unitOfWork.OrderRepository.GetByOrderIdAsync(orderId);
            if (Order == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                //var orderDTO = _mapper.Map<OrderDTO>(Order);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Order);
            }
        }
        public async Task<ServiceResult> GetOrdersByUserId(long userId)
        {
            #region Business Rule

            #endregion Business Rule

            var Orders = await _unitOfWork.OrderRepository.GetByUserIdAsync(userId);
            if (Orders == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                //var orderDTO = _mapper.Map<OrderDTO>(Orders);
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Orders);
            }
        }
        //Create/Update
        public async Task<ServiceResult> Save(Order order)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var item = await this.GetOrderByOrderId(order.OrderId);

                if (item.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.OrderRepository.UpdateAsync(order);
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
                    var newOrder = new Order
                    {
                        StatusId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        UserId = order.UserId,
                    };
                    result = await _unitOfWork.OrderRepository.CreateAsync(newOrder);
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
        public async Task<ServiceResult> DeleteOrderByOrderId(long id)
        {
            try
            {
                var result = false;

                var removeOrder = this.GetOrderByOrderId(id);

                if (removeOrder != null && removeOrder.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.OrderRepository.RemoveAsync((Order)removeOrder.Result.Data);

                    if (result)
                    {
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, removeOrder.Result.Data);
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

        //Helper
        public bool OrderExists(long id)
        {
            return _unitOfWork.OrderRepository.OrderExists(id);
        }
    }
}
