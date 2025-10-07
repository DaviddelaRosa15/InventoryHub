using AutoMapper;
using InventoryHub.Core.Application.Features.Product.Queries.GetAll;
using InventoryHub.Core.Domain.Entities;

namespace InventoryHub.Core.Application.Mappings
{
    public class GeneralProfile : Profile
	{
		public GeneralProfile()
		{
            #region Product
            CreateMap<Product, GetAllProductQueryResponseChild>()
                .ReverseMap()
                .ForMember(x => x.Inventory, opt => opt.Ignore())
                .ForMember(x => x.InventoryMovements, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion
        }
    }
}
