using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Core.Contracts;
using AutoMapper;
using HotelListing.API.Core.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;
        public HotelsController(IHotelsRepository hotelsRepository)
        {
            _hotelsRepository = hotelsRepository;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
        {
            var hotels = await _hotelsRepository.GetAllAsync<GetHotelDto>();
            return Ok(hotels);
        }

        [HttpGet("pagedHotels")]
        public async Task<ActionResult<PagedResult<GetHotelDto>>> GetPagedHotels(QueryParameters queryParameters)
        {
            var pagedHotelsResul = await _hotelsRepository.GetAllAsync<GetHotelDto>(queryParameters);
            return Ok(pagedHotelsResul);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDetailsDto>> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetDetailsAsync(id);
            return Ok(hotel);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updateHotelDto)
        {
            if (id != updateHotelDto.Id)
            {
                throw new BadRequestException("Invalid record ID");
            }

            await _hotelsRepository.UpdateAsync<UpdateHotelDto>(id, updateHotelDto);
            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto createHotelDto)
        {           
            var hotelDto = await _hotelsRepository.AddAsync<CreateHotelDto, GetHotelDto>(createHotelDto);
            return CreatedAtAction("GetHotel", new { id = hotelDto.Id }, hotelDto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {   
            await _hotelsRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
