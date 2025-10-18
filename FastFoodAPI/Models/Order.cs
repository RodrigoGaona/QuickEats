namespace FastFoodAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string OrderId { get; set; } // El ID ingresado por el usuario
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}