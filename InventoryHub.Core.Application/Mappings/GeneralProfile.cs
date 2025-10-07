using AutoMapper;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Mappings
{
    public class GeneralProfile : Profile
	{
		public GeneralProfile()
		{
            #region Product
            CreateMap<Product, ProductResponseDTO>()
                .ReverseMap()
                .ForMember(x => x.Inventory, opt => opt.Ignore())
                .ForMember(x => x.InventoryMovements, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion
        }
    }
}
