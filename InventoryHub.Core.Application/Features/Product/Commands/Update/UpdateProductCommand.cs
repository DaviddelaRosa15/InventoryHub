using AutoMapper;
using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Features.Product.Command.Update
{
    public class UpdateProductCommand : IRequest<ProductDTO>
    {
        [SwaggerParameter(Description = "Identificador del producto")]
        [Required(ErrorMessage = "Debe ingresar el identificador")]
        public string Id { get; set; }

        [SwaggerParameter(Description = "Nombre del producto")]
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        public string Name { get; set; }

        [SwaggerParameter(Description = "Descripción")]
        public string? Description { get; set; }

        [SwaggerParameter(Description = "Precio de venta")]
        [Required(ErrorMessage = "Debe ingresar el precio de venta")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor que cero")]
        public double SalePrice { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ProductDTO response = new();

                var valueToUpdate = await _productRepository.GetByIdAsync(command.Id);

                if (valueToUpdate == null)
                    throw new Exception(ErrorMessages.NotFound);

                valueToUpdate.Name = command.Name;
                valueToUpdate.Description = command.Description;
                valueToUpdate.SalePrice = command.SalePrice;
                valueToUpdate.LastModifiedBy = "DefaultUser";
                valueToUpdate.LastModified = DateTime.UtcNow;

                await _productRepository.UpdateAsync(valueToUpdate);

                response = _mapper.Map<ProductDTO>(valueToUpdate);
                response.Status = "Exitoso";
                response.Details = [new ErrorDetailsDTO { Code = "000", Message = "Se modificó correctamente el producto" }];
                return response;
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
