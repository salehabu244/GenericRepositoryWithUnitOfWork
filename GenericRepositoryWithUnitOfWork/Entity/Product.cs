namespace GenericRepositoryWithUnitOfWork.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }

        // Navigation property to Order
        public List<Order> Orders { get; set; }
    }
}
