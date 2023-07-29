using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        HotelListingDbContext _dbContext;
        public HotelsRepository(HotelListingDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
        }

        public async Task<Hotel> GetDetailsAsync(int id)
        {
            return await _dbContext.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
