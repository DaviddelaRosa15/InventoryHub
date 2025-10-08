using AutoMapper;
using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Features.Product.Command.Delete
{
    public class DeleteProductCommand : IRequest<ProductDTO>
    {
        [SwaggerParameter(Description = "Identificador del producto")]
        [Required(ErrorMessage = "Debe de ingresar el identificador")]
        public string Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ProductDTO response = new();
                var valueToDelete = await _productRepository.GetByIdAsync(command.Id);

                if (valueToDelete == null)
                    throw new Exception(ErrorMessages.NotFound);

                await _productRepository.DeleteAsync(valueToDelete.Id);

                response = _mapper.Map<ProductDTO>(valueToDelete);
                response.Status = "Exitoso";
                response.Details = [new ErrorDetailsDTO { Code = "000", Message = "Se eliminó correctamente el producto" }];
                return response;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
