using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        Task<GetHotelDetailsDto> GetDetailsAsync(int id);
    }
}
