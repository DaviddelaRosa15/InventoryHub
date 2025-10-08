using AutoMapper;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Features.Product.Command.Add;
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

            CreateMap<Product, AddProductCommand>()
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Inventory, opt => opt.Ignore())
                .ForMember(x => x.InventoryMovements, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.Details, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Inventory, opt => opt.Ignore())
                .ForMember(x => x.InventoryMovements, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion
        }
    }
}
