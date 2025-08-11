namespace GenericRepositoryWithUnitOfWork.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }

        // Foreign key to Product
        public int ProductId { get; set; }
        // Navigation property to Product
        public Product Product { get; set; }
    }
}
