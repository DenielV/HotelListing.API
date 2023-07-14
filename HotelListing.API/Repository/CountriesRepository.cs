using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _dbContext;
        public CountriesRepository(HotelListingDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> GetDetailsAsync(int id)
        {
            return await _dbContext.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
