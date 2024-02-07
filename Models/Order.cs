namespace ContosoPizza.Models;

public class Order{

    public int Id {get; set;}
    public decimal Price {get; set;}
    public string? orderDate {get; set;}
    public string? estimatedDelivery {get; set;}
    public List<Pizza> pizzas {get; set;}
}