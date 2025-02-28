using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing sale operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesController> _logger;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            _logger.LogInformation($"CREATE SALE | Validating received data: {request}.");
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError($"CREATE SALE | Invalid received data.");
                return BadRequest(validationResult.Errors);
            }

            _logger.LogInformation($"CREATE SALE | Preparing data to be saved.");
            var command = _mapper.Map<CreateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation($"CREATE SALE | Sale created successfully.");
            return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale created successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateSale(int id, [FromBody] UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UPADTE SALE | Validating received id: {id} and data: {request}.");
            if (id != request.SaleId)
            {
                _logger.LogError($"UPADTE SALE | ID in request body ({id}) does not match ID in URL ({request.SaleId}).");
                return BadRequest("ID in request body does not match ID in URL");
            }

            _logger.LogInformation($"UPDATE SALE |  Preparing data to be updated.");
            var command = _mapper.Map<UpdateSaleCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation($"UPDATE SALE | Sale updated successfully.");
            return Ok(new ApiResponseWithData<CreateSaleResponse>
            {
                Success = true,
                Message = "Sale updated successfully",
                Data = _mapper.Map<CreateSaleResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves a sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();

            _logger.LogInformation($"GET SALE | Validating received id: {id}.");
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError($"GET SALE | Error while trying to get sale: {validationResult.Errors}.");
                return BadRequest(validationResult.Errors);
            }

            var command = new GetSaleCommand(request.Id);
            var response = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation($"GET SALE | Sale obtained successfully.");
            return Ok(new ApiResponseWithData<GetSaleResponse>
            {
                Success = true,
                Message = "Sale retrieved successfully",
                Data = _mapper.Map<GetSaleResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves all sales
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The list of sales</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<GetSalesResult>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSales(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GET SALES | Trying to get sales.");
            var command = new GetSalesCommand();
            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation($"GET SALES | Sales retrieved successfully.");
            return Ok(new ApiResponseWithData<List<GetSalesResult>>
            {
                Success = true,
                Message = "Sales retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Deletes a Sale by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the Sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the Sale was deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            var request = new DeleteSaleRequest { Id = id };
            var validator = new DeleteSaleRequestValidator();
            _logger.LogInformation($"DELETE SALE | Validating received id: {id}.");
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError($"DELETE SALE | Error while trying to get sale: {validationResult.Errors}.");
                return BadRequest(validationResult.Errors);
            }

            _logger.LogInformation($"DELETE SALE | Preparing data to be deleted.");
            var command = _mapper.Map<DeleteSaleCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation($"DELETE SALE | Sale from ID: {id} deleted successfully.");
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Sale deleted successfully"
            });
        }
    }
}
