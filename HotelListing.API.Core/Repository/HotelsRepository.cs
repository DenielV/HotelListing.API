using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        HotelListingDbContext _dbContext;
        private readonly IMapper _mapper;

        public HotelsRepository(HotelListingDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetHotelDetailsDto> GetDetailsAsync(int id)
        {
            var record = await _dbContext.Hotels.Include(h => h.Country)
                .ProjectTo<GetHotelDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (record is null)
                throw new NotFoundException(nameof(Hotel), id);

            return record;
        }
    }
}
