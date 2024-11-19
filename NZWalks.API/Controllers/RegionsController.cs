using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:7066/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET https://localhost:7066/api/regions
        // Get all regions
        [HttpGet]
        public async Task<IActionResult> GetRegions()
        {
            // Get Data from Database - Domain models
            var regionsDomainModel = await regionRepository.GetRegionsAsync();            

            //Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomainModel);

            //Return DTOs
            return Ok(regionsDto);
        }

        // GET https://localhost:7066/api/regions/{id}
        // Get a region by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get Data from Database - Domain models
            var regionDomainModel =  await regionRepository.GetRegionByIdAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Models to DTOs
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //Return DTO
            return Ok(regionDto);
        }

        // POST https://localhost:7066/api/regions
        // Create a new region
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);            

            //Add to Database
            regionDomainModel = await regionRepository.CreateRegionAsync(regionDomainModel);

            //Map Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //Return 201 Created
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        // PUT https://localhost:7066/api/regions/{id}
        // Update a region by id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Get Data from Database - Domain models
            await regionRepository.UpdateRegionAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }            

            //Map Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //Return DTO
            return Ok(regionDto);
        }

        // DELETE https://localhost:7066/api/regions/{id}
        // Delete a region by id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            //Get Data from Database - Domain models
            var regionDomainModel = await regionRepository.DeleteRegionAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }            

            //Map Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //Return deleted Region back
            return Ok(regionDto);
        }
    }
}
