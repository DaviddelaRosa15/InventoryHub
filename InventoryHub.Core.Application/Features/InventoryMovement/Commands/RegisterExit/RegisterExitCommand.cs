using AutoMapper;
using InventoryHub.Core.Application.Dtos.Common;
using InventoryHub.Core.Application.Dtos.InventoryMovement;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace InventoryHub.Core.Application.Features.InventoryMovement.Commands.RegisterExit
{
    public class RegisterExitCommand : MovementRequestDTO, IRequest<RegisterMovementDTO>
    {
    }

    public class RegisterExitCommandHandler : IRequestHandler<RegisterExitCommand, RegisterMovementDTO>
    {
        private readonly IInventoryMovementRepository _movementRepository;
        private readonly IMapper _mapper;

        public RegisterExitCommandHandler(IInventoryMovementRepository movementRepository, IMapper mapper)
        {
            _movementRepository = movementRepository;
            _mapper = mapper;
        }

        public async Task<RegisterMovementDTO> Handle(RegisterExitCommand command, CancellationToken cancellationToken)
        {
            try
            {
                RegisterMovementDTO response = new();
                var valueToAdd = _mapper.Map<Domain.Entities.InventoryMovement>(command);
                valueToAdd.Created = DateTime.UtcNow;
                valueToAdd.CreatedBy = "DefaultUser";

                var inventoryInfo = await _movementRepository.GetInventoryInfoAsync(command.ProductId);

                if (inventoryInfo == null)
                {
                    response.Status = "Fallido";
                    response.Details = [new ErrorDetailsDTO { Code = "001", Message = "El producto no existe en el inventario." }];
                    return response;
                }else if (inventoryInfo.CurrentStock < command.Quantity)
                {
                    response.Status = "Fallido";
                    response.Details = [new ErrorDetailsDTO { Code = "002", Message = "No hay suficiente inventario para completar la salida." }];
                    return response;
                }

                await _movementRepository.RegisterExitAsync(valueToAdd);

                response = _mapper.Map<RegisterMovementDTO>(valueToAdd);
                response.MovementDate = valueToAdd.Created;
                response.Status = "Exitoso";

                if (inventoryInfo.CurrentStock - command.Quantity <= inventoryInfo.MinimumStock)
                {
                    response.Details = [new ErrorDetailsDTO { Code = "003", Message = "El stock del producto es inferior al nivel mínimo recomendado después de esta salida." }];
                }
                else
                {
                    response.Details = [new ErrorDetailsDTO { Code = "000", Message = "Se actualizó el inventario correctamente" }];
                }

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error interno, valide con el administrador", ex);
            }
        }
    }
}