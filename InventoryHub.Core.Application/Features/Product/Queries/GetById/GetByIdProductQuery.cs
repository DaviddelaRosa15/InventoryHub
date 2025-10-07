using AutoMapper;
using InventoryHub.Core.Application.Constants;
using InventoryHub.Core.Application.Dtos.Product;
using InventoryHub.Core.Application.Interfaces.Repositories;
using MediatR;

namespace InventoryHub.Core.Application.Features.Product.Queries.GetById
{
    public class GetByIdProductQuery : IRequest<GetByIdProductQueryResponse>
    {
        public string Id { get; set; }
    }

    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, GetByIdProductQueryResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetByIdProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQuery query, CancellationToken cancellationToken)
        {
            try
            {
                GetByIdProductQueryResponse result = new();

                var getById = await _productRepository.GetByIdAsync(query.Id);

                if (getById == null)
                {
                    throw new Exception(ErrorMessages.NotFound);
                }

                var product = _mapper.Map<ProductResponseDTO>(getById);

                result.Product = product;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
