using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _dbContext;
        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetCountryDetailsDto> GetDetailsAsync(int id)
        {
            var record = await _dbContext.Countries.Include(c => c.Hotels)
                .ProjectTo<GetCountryDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(record is null) 
                throw new NotFoundException(typeof(Country).Name, id);

            return record; 
        }
    }
}
