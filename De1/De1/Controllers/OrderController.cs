using De1.Models;
using De1.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace De1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderController()
        {
        }

        [HttpGet("GetAllOrder")]
        public List<OrderResponse> Get()
        {
            var _context = new PRN_Sum22_B1Context();

            var data = _context.Orders
                        .Include(x=> x.Customer)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.Department).ToList();

            var response = new List<OrderResponse>();

            foreach (var item in data)
            {
                var responseItem = new OrderResponse();
                responseItem.OrderId = item.OrderId;
                responseItem.CustomerId = item.CustomerId;
                responseItem.CustomerName = item?.Customer?.ContactName;
                responseItem.EmployeeId = item?.EmployeeId;
                responseItem.EmployeeName = item?.Employee?.FirstName + item?.Employee?.LastName;
                responseItem.EmployeeDepartmentId = item?.Employee?.DepartmentId;
                responseItem.EmployeeDepartmentNmae = item?.Employee?.Department?.DepartmentName;
                responseItem.OrderDate = item?.OrderDate;
                responseItem.RequiredDate = item?.RequiredDate;
                responseItem.ShippedDate = item?.ShippedDate;
                responseItem.Freight = item?.Freight;
                responseItem.ShipName = item?.ShipName;
                responseItem.ShipAddress = item?.ShipAddress;
                responseItem.ShipCity = item?.ShipCity;
                responseItem.ShipRegion = item?.ShipRegion;
                responseItem.ShipPostalCode = item?.ShipPostalCode;
                responseItem.ShipCountry = item?.ShipCountry;
                response.Add(responseItem);
            }
            return response;
        }

        [HttpGet("GetOrderByDate/{From}/{To}")]

        public List<OrderResponse> GetOrderByDate(DateTime From, DateTime To)
        {
            var _context = new PRN_Sum22_B1Context();

            var data = _context.Orders
                        .Include(x => x.Customer)
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.Department)
                        .Where(x=> x.OrderDate >= From && x.OrderDate <= To)
                        .ToList();

            var response = new List<OrderResponse>();
            foreach (var item in data)
            {
                var responseItem = new OrderResponse();
                responseItem.OrderId = item.OrderId;
                responseItem.CustomerId = item.CustomerId;
                responseItem.CustomerName = item?.Customer?.ContactName;
                responseItem.EmployeeId = item?.EmployeeId;
                responseItem.EmployeeName = item?.Employee?.FirstName + item?.Employee?.LastName;
                responseItem.EmployeeDepartmentId = item?.Employee?.DepartmentId;
                responseItem.EmployeeDepartmentNmae = item?.Employee?.Department?.DepartmentName;
                responseItem.OrderDate = item?.OrderDate;
                responseItem.RequiredDate = item?.RequiredDate;
                responseItem.ShippedDate = item?.ShippedDate;
                responseItem.Freight = item?.Freight;
                responseItem.ShipName = item?.ShipName;
                responseItem.ShipAddress = item?.ShipAddress;
                responseItem.ShipCity = item?.ShipCity;
                responseItem.ShipRegion = item?.ShipRegion;
                responseItem.ShipPostalCode = item?.ShipPostalCode;
                responseItem.ShipCountry = item?.ShipCountry;
                response.Add(responseItem);
            }
            return response;
        }

        [HttpPost("Delete/{CustomerId}")]
        public IActionResult Delete(string CustomerId)
        {
            try
            {
                using (var _context = new PRN_Sum22_B1Context())
                {
                    var customer = _context.Customers
                        .Include(c => c.Orders)
                        .ThenInclude(o => o.OrderDetails)
                        .FirstOrDefault(x => x.CustomerId.Equals(CustomerId));

                    if (customer == null)
                    {
                        return NotFound();
                    }
                    var response = new OrderDeleteResponse();
                    response.CustomerDeletedCount = 1;
                    response.OrderDeletedCount = customer.Orders.Count();
                    var orderDetails = 0;
                    foreach (var item in customer.Orders)
                    {
                        orderDetails += item.OrderDetails.Count();
                        _context.OrderDetails.RemoveRange(item.OrderDetails);
                    }
                    response.OrderDetailDeletedCount = orderDetails;

                    _context.Orders.RemoveRange(customer.Orders);

                    _context.Customers.Remove(customer);

                    _context.SaveChanges();

                    

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer: {ex.Message}");

                return StatusCode(StatusCodes.Status409Conflict, new { message = "There was an unknown error when performing data deletion." });
            }
        }
    }
}
