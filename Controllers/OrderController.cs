using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Model;
using ProductStore.Model.DTOs;
using ProductStore.Service.IService;
using System.Net;
using System.Security.Claims;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderService;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _response;

        public OrderController(IOrder orderService, IMapper mapper, ResponseDTO response)
        {
            _mapper = mapper;
            _response = response;
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> AddOrder(AddNewOrderDTO order)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    _response.StatusCode = HttpStatusCode.Forbidden;
                    _response.Message = "You are not authorized";
                    return StatusCode(403, _response);
                }

                var newOrder = _mapper.Map<Order>(order);
                newOrder.UserId = new Guid(userId);
                _response.Result = await _orderService.CreateOrder(newOrder);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetAllOrders()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    _response.StatusCode = HttpStatusCode.Forbidden;
                    _response.Message = "You are not authorized";
                    return StatusCode(403, _response);
                }

                var role = User.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
                var orders = await _orderService.GetAllOrders();
                var orderDTO = _mapper.Map<List<OrderDTO>>(orders);
                if (role == "Admin")
                {
                    _response.Result = orderDTO;
                    return Ok(_response);
                }
                orderDTO = orderDTO.FindAll(order => order.UserId == new Guid(userId));
                _response.Result = orderDTO;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDTO>> GetSingleOrder(Guid id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    _response.StatusCode = HttpStatusCode.Forbidden;
                    _response.Message = "You are not authorized";
                    return StatusCode(403, _response);
                }

                var role = User.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No order with id: `{id}`";
                    return NotFound(_response);
                }

                if (order.UserId != new Guid(userId) && role != "Admin")
                {
                    _response.StatusCode = HttpStatusCode.Forbidden;
                    _response.Message = "You are not authorized";
                    return StatusCode(403, _response);
                }
                var orderDTO = _mapper.Map<OrderDTO>(order);    
                _response.Result = orderDTO;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<ResponseDTO>> UpdateOrder(Guid id, UpdateOrderDTO order)
        {
            try
            {
                var orderExists = await _orderService.UpdateOrder(id, order);
                if (!orderExists)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No order with id: {id}";
                    return NotFound(_response);
                }

                _response.Result = "Order Updated Successfully :)";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }

        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<ResponseDTO>> DeleteOrder(Guid id)
        {
            try
            {
                var orderExists = await _orderService.DeleteOrder(id);
                if (!orderExists)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No order with id: {id}";
                    return NotFound(_response);
                }

                _response.Result = "Order Deleted Successfully :)";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }
        }
    }
}
