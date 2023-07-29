using HotelListing.API.Data;
using HotelListing.API.Core.Models.Hotel;

namespace HotelListing.API.Core.Models.Country
{
    public class GetCountryDetailsDto : BaseCountryDto
    {
        public int Id { get; set; }
        public virtual IList<GetHotelDto> Hotels { get; set; }
    }
}
