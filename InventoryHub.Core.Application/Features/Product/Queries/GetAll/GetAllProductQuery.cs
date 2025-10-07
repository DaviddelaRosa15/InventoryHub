using AutoMapper;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;

namespace InventoryHub.Core.Application.Features.Product.Queries.GetAll
{
    public class GetAllProductQuery : IRequest<GetAllProductQueryResponse>
    {

    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, GetAllProductQueryResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQuery query, CancellationToken cancellationToken)
        {
            try
            {
                GetAllProductQueryResponse result = new();

                var getAlls = await _productRepository.GetAllAsync();
                var products = _mapper.Map<List<GetAllProductQueryResponseChild>>(getAlls.OrderByDescending(x => x.Created).ToList());

                result.Products = products;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
