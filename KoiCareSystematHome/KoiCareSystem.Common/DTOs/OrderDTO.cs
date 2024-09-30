namespace KoiCareSystem.Common.DTOs
{
    public class OrderDTO
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
