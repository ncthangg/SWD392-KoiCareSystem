using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Data
{
    public class UnitOfWork
    {
        private ApplicationDbContext _dbContext;

        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;

        private OrderRepository _orderRepository;
        private OrderStatusRepository _orderStatusRepository;
        private OrderItemRepository _orderItemRepository;
        private PaymentRepository _paymentRepository;

        private KoiFishRepository _koiFishRepository;
        private PondRepository _pondRepository;
        private WaterParameterRepository _waterParameterRepository;
        private WaterStatusRepository _waterStatusRepository;

        private UserRepository _userRepository;
        private RoleRepository _roleRepository;


        public UnitOfWork()
        {
            _dbContext ??= new ApplicationDbContext(); // nếu null thì mới new 
        }
        //USER
        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new Repository.UserRepository(_dbContext);
            }
        }
        public RoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new Repository.RoleRepository(_dbContext);
            }
        }
        //PRODUCT
        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new Repository.ProductRepository(_dbContext);
            }
        }
        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new Repository.CategoryRepository(_dbContext);
            }
        }
        //ORDER
        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new Repository.OrderRepository(_dbContext);
            }
        }
        public OrderStatusRepository OrderStatusRepository
        {
            get
            {
                return _orderStatusRepository ??= new Repository.OrderStatusRepository(_dbContext);
            }
        }
        public OrderItemRepository OrderItemRepository
        {
            get
            {
                return _orderItemRepository ??= new Repository.OrderItemRepository(_dbContext);
            }
        }
        public PaymentRepository PaymentRepository
        {
            get
            {
                return _paymentRepository ??= new Repository.PaymentRepository(_dbContext);
            }
        }
        //KoiFish
        public KoiFishRepository KoiFishRepository
        {
            get
            {
                return _koiFishRepository ??= new Repository.KoiFishRepository(_dbContext);
            }
        }
        public PondRepository PondRepository
        {
            get
            {
                return _pondRepository ??= new PondRepository(_dbContext);
            }
        }
        public WaterParameterRepository WaterParameterRepository
        {
            get
            {
                return _waterParameterRepository ??= new WaterParameterRepository(_dbContext);
            }
        }
        public WaterStatusRepository WaterStatusRepository
        {
            get
            {
                return _waterStatusRepository ??= new WaterStatusRepository(_dbContext);
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
