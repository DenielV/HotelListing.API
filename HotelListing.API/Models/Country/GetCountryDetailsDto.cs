using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country
{
    public class GetCountryDetailsDto : BaseCountryDto
    {
        public int Id { get; set; }
        public virtual IList<GetHotelDto> Hotels { get; set; }
    }
}
