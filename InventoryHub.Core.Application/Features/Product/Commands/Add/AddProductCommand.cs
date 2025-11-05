using AutoMapper;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Features.Product.Command.Add
{
    public class AddProductCommand : IRequest<ProductDTO>
    {
        [SwaggerParameter(Description = "Nombre del producto")]
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        public string Name { get; set; }

        [SwaggerParameter(Description = "Descripción")]
        public string? Description { get; set; }

        [SwaggerParameter(Description = "Precio de venta")]
        [Required(ErrorMessage = "Debe ingresar el precio de venta")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor que cero")]
        public double SalePrice { get; set; }

        [SwaggerParameter(Description = "Minimo de existencia")]
        [Required(ErrorMessage = "Debe ingresar el minimo de existencia")]
        [Range(1, int.MaxValue, ErrorMessage = "El mínimo debe ser mayor que cero")]
        public int MinimumStock { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDTO>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ProductDTO response = new();
                var valueToAdd = _mapper.Map<Domain.Entities.Product>(command);
                valueToAdd.Created = DateTime.UtcNow;
                valueToAdd.CreatedBy = "DefaultUser";

                await _productRepository.CreateAsync(valueToAdd);

                response = _mapper.Map<ProductDTO>(valueToAdd);
                response.Status = "Exitoso";
                response.Details = [new ErrorDetailsDTO { Code = "000", Message = "Se insertó correctamente el producto" }];
                return response;
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
