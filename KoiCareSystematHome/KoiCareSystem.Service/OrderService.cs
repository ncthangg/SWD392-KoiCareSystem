using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;
namespace KoiCareSystem.Service
{
    public interface IOrderService
    {
        Task<ServiceResult> GetAllOrder();
        Task<ServiceResult> GetByOrderId(int orderId);
        Task<ServiceResult> GetByUserId(int userId);
        Task<ServiceResult> Save(Order order);
        Task<ServiceResult> DeleteByOrderId(int orderId);
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
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Order>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Orders);
            }
        }
        //Get By Id
        public async Task<ServiceResult> GetByOrderId(int orderId)
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
        public async Task<ServiceResult> GetByUserId(int userId)
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
                var item = await this.GetByOrderId(order.OrderId);

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
                    if (await HasPendingOrders(order.UserId))
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, "Không thể tạo đơn hàng mới. Người dùng đã có đơn hàng đang chờ xử lý.");
                    }

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
        public async Task<ServiceResult> DeleteByOrderId(int id)
        {
            try
            {
                var result = false;

                var removeOrder = this.GetByOrderId(id);

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
        public async Task<bool> UpdateOrderStatusAsync(int orderId, int statusId)
        {
            var orderExist = await this.GetByOrderId(orderId);
            if (orderExist != null && (orderExist.Data as Order).Quantity != null)
            {
                var order = (Order)orderExist.Data;
                order.OrderDate = DateTime.Now;
                order.StatusId = statusId; // Cập nhật trạng thái
                order.Status = _unitOfWork.OrderStatusRepository.GetById(statusId);
                await Save(order);
                return true;
            }
            return false;
        }
        //Helper
        public bool OrderExists(int id)
        {
            return _unitOfWork.OrderRepository.OrderExists(id);
        }
        public async Task<bool> HasPendingOrders(int userId)
        {
            var ordersResult = await GetByUserId(userId);

            if (ordersResult.Status>0 && ordersResult.Data != null)
            {
                var orderList = ordersResult.Data as List<Order>;
                var pendingOrders = orderList.Where(o => o.StatusId == 1).ToList();

                return pendingOrders.Any();
            }

            return false;
        }
    }
}
