using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using VillaAPI.DTOs.VillaNumber;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;

namespace VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IVillaNumberRepository dbVillaNumber;
        internal APIResponse res;
        public VillaNumberController(IMapper _mapper ,IVillaNumberRepository _dbVillaNumber )
        {
            mapper = _mapper;
            dbVillaNumber = _dbVillaNumber;
            res = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllVillaNumbers(int page = 1, int pageSize = 10) {
            var villaNumbers = await dbVillaNumber.GetAllAsync(page: page = 1, pageSize: pageSize = 10);

            var pagination = new Pagination()
            {
                Page = page,
                PageSize = pageSize,
            };

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(pagination));

            res.Result = mapper.Map<List<VillaNumberDTO>>(villaNumbers);
            res.StatusCode = villaNumbers.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            return Ok(res);

        }

        [HttpGet("{num:int}", Name = "GetVillaNumber")]
        public async Task<ActionResult<APIResponse>> GetOneVillaNumber(int num)
        {
            var villaNumberModel = await dbVillaNumber.GetOneAsync(v => v.NumberOfVilla == num , include:"Villa");

            if (villaNumberModel is null) throw new AppError("No Villa Found With This Id", HttpStatusCode.NotFound);

            var villaNumber = mapper.Map<VillaNumberDTO>(villaNumberModel);

            return Ok(villaNumber);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber(VillaNumberCreateDTO villaNumberCreate) {

            if ((await dbVillaNumber.GetOneAsync(v => v.NumberOfVilla == villaNumberCreate.NumberOfVilla)) != null) throw new AppError("VillaNumber is existed", HttpStatusCode.Conflict);

            var villaNumberModel = mapper.Map<VillaNumber>(villaNumberCreate);

            await dbVillaNumber.CreateOneAsync(villaNumberModel);

            res.Result = mapper.Map<VillaNumberDTO>(villaNumberModel);
            res.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVillaNumber", new { num = villaNumberModel.NumberOfVilla }, res);
        }

        [HttpDelete("{num:int}")]
        public async Task<ActionResult<APIResponse>> DeleteOneVilla(int num)
        {

            var villa = await dbVillaNumber.GetOneAsync(v => v.NumberOfVilla == num);

            if (villa == null) throw new AppError("Villa With this Id is not found", HttpStatusCode.NotFound);


            await dbVillaNumber.DeleteOneAsync(villa);

            res.StatusCode = HttpStatusCode.OK;
            
            return Ok(res);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdateNumberVilla(int id, VillaNumberUpdateDTO villaNumberUpdateDTO) {
            if (id <= 0 || id != villaNumberUpdateDTO.Id) throw new Exception("Id Is Not Equal Updated Villa Id");

            var villa = await dbVillaNumber.GetOneAsync(v => v.Id== id, tracked: false);

            if (villa is null) throw new AppError("VillaNumber not found" , HttpStatusCode.NotFound);

            var villaModel = mapper.Map<VillaNumber>(villaNumberUpdateDTO);

             await dbVillaNumber.UpdateOneAsync(villaModel);

            res.Result = mapper.Map<VillaNumberDTO>(villaModel);
            res.StatusCode = HttpStatusCode.Created;

            return Ok(res);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdateNumberPartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO>  updatePatchDTO) {
            if (id <= 0 || updatePatchDTO == null) throw new AppError("Id Is Not Equal Updated Villa Id" , HttpStatusCode.NotFound);

            var villaNumber = await dbVillaNumber.GetOneAsync(v => v.Id== id, tracked: false);

            if (villaNumber is null) throw new AppError("VillaNumber not found" , HttpStatusCode.NotFound);

            var villaNumberUpdate = mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            updatePatchDTO.ApplyTo(villaNumberUpdate, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var villaNumberModel = mapper.Map<VillaNumber>(villaNumberUpdate);

            await dbVillaNumber.UpdateOneAsync(villaNumberModel);


            res.Result = mapper.Map<VillaNumberDTO>(villaNumberModel);
            res.StatusCode = HttpStatusCode.Created;

            return Ok(res);
        }
    }
}
