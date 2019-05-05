using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productService.Get());
        }

        [HttpPost]
        public IActionResult Create([FromBody]Product product)
        {
           var result = _productService.Create(product);
            return Ok(result);
        }

        [Route("updateCount")]
        [HttpPost]
        public IActionResult UpdateCount([FromBody]UpdateCountRequestModel request)
        {
            var result = _productService.UpdateCount(request.Id, request.Quantity);
            if(result)
            {
                return Ok();
            }
            return StatusCode(400);
           
        }

    }
}