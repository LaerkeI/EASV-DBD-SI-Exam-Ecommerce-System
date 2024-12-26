using Microsoft.AspNetCore.Mvc;
using OrderManagementService.Application.DTOs;
using OrderManagementService.Application.Interfaces;

namespace OrderManagementService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/{orderId}
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderByOrderId(int orderId)
        {
            var order = await _orderService.GetOrderByOrderIdAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdOrder = await _orderService.CreateOrderAsync(orderDto);

            // Return CreatedAt route for RESTful conventions
            return CreatedAtAction(nameof(GetOrderByOrderId), new { orderId = createdOrder.OrderId }, createdOrder);
        }

        // PUT: api/Order/{orderId}
        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (orderId != orderDto.OrderId)
                return BadRequest("Order OrderID mismatch.");

            await _orderService.UpdateOrderAsync(orderDto);
            return NoContent();
        }

        // DELETE: api/Order/{orderId}
        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);
            return NoContent();
        }
    }
}
