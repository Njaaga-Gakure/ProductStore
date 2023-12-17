using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Model;
using ProductStore.Model.DTOs;
using ProductStore.Service.IService;
using ProductStore.Utils;
using System.Net;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productService;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _response;

        public ProductController(IProduct product, IMapper mapper, ResponseDTO response)
        {
            _mapper = mapper;
            _response = response;
            _productService = product;
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<ResponseDTO>> AddProduct(AddProductDTO newProduct)
        {
            try
            {

                var inputs = new List<string>()
                {
                    newProduct.Name,
                    newProduct.Description,
                };

                var isNullOrEmpty = ValidateInputs.Validate(inputs);
                if (isNullOrEmpty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Please fill in all the field :(";
                    return BadRequest(_response);
                }

                var product = _mapper.Map<Product>(newProduct);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = await _productService.CreateProduct(product);
                return Created($"Product/{product.Id}", _response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllProducts(int pageNumber = 1)
        {
            try
            {
                var products = await _productService.GetAllProducts(pageNumber);
                _response.Result = products;
                return Ok(_response);   
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, _response);
            }
        }
        [HttpGet("filter")]
        public async Task<ActionResult<ResponseDTO>> FilterProducts(string productName = "all", int? price = null, int pageNumber = 1)
        {
            try
            {
                var products = await _productService.GetAllProducts(pageNumber);
                if (productName != "all")
                {
                    products = products.FindAll(product => product.Name.ToLower() == productName.ToLower());
                }
                if (price.HasValue)
                {
                    products = products.FindAll(product => product.Price <= price);
                }
                _response.Result = products;
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
        public async Task<ActionResult<ResponseDTO>> GetSingleProducts(Guid id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                { 
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No product with id: {id}";
                    return NotFound(_response);
                }
                _response.Result = product;  
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
        public async Task<ActionResult<ResponseDTO>> UpdateProduct(Guid id, AddProductDTO product)
        {
            try
            {
                var inputs = new List<string>()
                {
                    product.Name,
                    product.Description,
                };

                var isNullOrEmpty = ValidateInputs.Validate(inputs);
                if (isNullOrEmpty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Please fill in all the field :(";
                    return BadRequest(_response);
                }

                var productExists = await _productService.UpdateProduct(id, product);
                if (!productExists) 
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No product with id: {id}";
                    return NotFound(_response);
                }

                _response.Message = "Product Updated Successfully :(";
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
        public async Task<ActionResult<ResponseDTO>> DeleteProduct(Guid id)
        {
            try
            {
                var productExists = await _productService.DeleteProduct(id);
                if (!productExists)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = $"No product with id: {id}";
                    return NotFound(_response);
                }

                _response.Message = "Product Deleted Successfully :)";
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
