using ContosoPizza.Models;
using ContosoPizza.Business;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService? orderService;
    public OrderController(IOrderService? _orderService)
    {
        orderService=_orderService;
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<Order>> GetAll() =>
        orderService.GetAll();
    // GET by Id action
    [HttpGet]
    [Route("{id}")]
    public ActionResult<Order> Get(int id)
    {
        var order = orderService.GetOrder(id);

        if(order == null)
            return NotFound();

        return order;
    }
    [HttpGet]
    [Route("{id}/pizza")]
    public ActionResult<List<Pizza>> GetPizzasInOrder(int id){
        var order = orderService.GetPizzasInOrder(id);
        if(order== null){
            return NotFound();
        }else{
            return order;
        }
    }
    // POST action
    [HttpPost]
    public IActionResult Create(Order order)
    {            
        orderService.AddOrder(order);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }
    // PUT action
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(int id, Order order)
    {
        if (id != order.Id)
            return BadRequest();
            
        var existingOrder = orderService.GetOrder(id);
        if(existingOrder is null)
            return NotFound();
    
        orderService.UpdateOrder(order);           
    
        return NoContent();
    }
    // DELETE action
   [HttpDelete]
   [Route("{id}")]
    public IActionResult Delete(int id)
    {
        var order = orderService.GetOrder(id);
    
        if (order is null)
            return NotFound();
        
        orderService.DeleteOrder(id);
    
        return NoContent();
    }
}