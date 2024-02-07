namespace ContosoPizza.Models;

public class User{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Surname {get; set;}
    public string? Direction {get; set;}
    public Order order{get; set;}
}