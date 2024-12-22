using PizzaOrder.Data.Enums;

namespace PizzaOrder.Data.Entities;

public class PizzaDetails : BaseEntity
{ 
    #region Properties
    public string? Name { get; set; }
    public Toppings? Toppings { get; set; }
    public decimal? Price { get; set; }
    public int? Size { get; set; }
    public Guid? OrderDetailsId { get; set; }
    #endregion

    #region Ctor
    public PizzaDetails()
    {
        
    }

    public PizzaDetails(string? Name, Toppings? Toppings, decimal? Price, int? Size, Guid? OrderDetailsId)
    {
        this.Name = Name;
        this.Toppings = Toppings;
        this.Price = Price;
        this.Size = Size;
        this.OrderDetailsId = OrderDetailsId;
    }
    #endregion
}
