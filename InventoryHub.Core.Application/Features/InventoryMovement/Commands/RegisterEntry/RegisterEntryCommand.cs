using AutoMapper;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.InventoryMovement;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Features.InventoryMovement.Commands.RegisterEntry
{
    public class RegisterEntryCommand : IRequest<RegisterEntryDTO>
    {
        [SwaggerParameter(Description = "Id del producto")]
        [Required(ErrorMessage = "Debe ingresar el id del producto")]
        public string ProductId { get; set; }

        [SwaggerParameter(Description = "Cantidad a ingresar")]
        [Required(ErrorMessage = "Debe ingresar la cantidad a ingresar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        public int Quantity { get; set; }
    }

    public class RegisterEntryCommandHandler : IRequestHandler<RegisterEntryCommand, RegisterEntryDTO>
    {
        private readonly IInventoryMovementRepository _movementRepository;
        private readonly IMapper _mapper;

        public RegisterEntryCommandHandler(IInventoryMovementRepository movementRepository, IMapper mapper)
        {
            _movementRepository = movementRepository;
            _mapper = mapper;
        }

        public async Task<RegisterEntryDTO> Handle(RegisterEntryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                RegisterEntryDTO response = new();
                var valueToAdd = _mapper.Map<Domain.Entities.InventoryMovement>(command);
                valueToAdd.Created = DateTime.UtcNow;
                valueToAdd.CreatedBy = "DefaultUser";

                await _movementRepository.RegisterEntryAsync(valueToAdd);

                response = _mapper.Map<RegisterEntryDTO>(valueToAdd);
                response.EntryDate = valueToAdd.Created;
                response.Status = "Exitoso";
                response.Details = [new ErrorDetailsDTO { Code = "000", Message = "Se actualizó el inventario correctamente" }];
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error interno, valide con el administrador", ex);
            }
        }
    }
}