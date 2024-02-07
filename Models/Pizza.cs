namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Veggie { get; set; }
    public List<Ingredients> listIngr {get; set;}

    public Pizza(){}
}