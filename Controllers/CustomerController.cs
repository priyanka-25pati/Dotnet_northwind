using API3.Models;
using API3.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
         readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }



        // Create - POST: api/customers
        [HttpPost]
        public IActionResult Create([FromBody] Customer customer)
        {
            var createdCustomer = _customerService.CreateCustomer(customer);
            return CreatedAtAction(nameof(Get), new { id = createdCustomer.CustomerId }, createdCustomer);
        }




        // Read all - GET: api/customers
        [HttpGet]
        public IActionResult Get()
        {
           
            return Ok();
        }

        // Read by ID - GET: api/customers/{id}
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var customer = _customerService.GetCustomerById(id);
            return Ok(customer);
        }




        // Update - PUT: api/customers/{id}
        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Customer customer, string id)
        {
            var updatedCustomer = _customerService.UpdateCustomer(customer,id);
            return Ok(updatedCustomer);
        }



        // Delete - DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _customerService.DeleteCustomer(id);
            return NoContent();
        }
    }
}
