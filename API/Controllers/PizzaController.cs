using ContosoPizza.Models;
using ContosoPizza.Business;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService? pizzaServ;
    public PizzaController(IPizzaService? _pizzaServ)
    {
        pizzaServ=_pizzaServ;
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => pizzaServ.GetAll();
    // GET by Id action
    [HttpGet]
    [Route("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = pizzaServ.GetPizza(id);

        if(pizza == null)
            return NotFound();

        return pizza;
    }
    // POST action
    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {            
        pizzaServ.AddPizza(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }
    // PUT action
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();
            
        var existingPizza = pizzaServ.GetPizza(id);
        if(existingPizza is null)
            return NotFound();
    
        pizzaServ.UpdatePizza(pizza);           
    
        return NoContent();
    }
    // DELETE action
   [HttpDelete]
   [Route("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = pizzaServ.GetPizza(id);
    
        if (pizza is null)
            return NotFound();
        
        pizzaServ.DeletePizza(id);
    
        return NoContent();
    }
}