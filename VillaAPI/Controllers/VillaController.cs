using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using VillaAPI.DTOs.Villa;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;

namespace VillaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaController : ControllerBase
    {
        private readonly IVillaRepository dbVilla;
        private readonly IMapper mapper;
        internal APIResponse res;

        public VillaController(IMapper _mapper, IVillaRepository _dbVilla)
        {
            mapper = _mapper;
            dbVilla = _dbVilla;
            res = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllVillas(int page = 1, int pageSize = 10) {

            var villas = await dbVilla.GetAllAsync(page: page, pageSize: pageSize);

            var pagination = new Pagination()
            {
                Page = page,
                PageSize = pageSize,
            };

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(pagination));

            res.Result = mapper.Map<List<VillaDTO>>(villas);
            res.StatusCode = villas.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent;
            return Ok(res);
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        public async Task<ActionResult<APIResponse>> GetOneVilla(int id)
        {
            var villaModel = await dbVilla.GetOneAsync(v => v.Id == id ,include:"VillaNumber");

            if (villaModel is null) throw new AppError("No Villa Found With This Id", HttpStatusCode.NotFound);

            var villa = mapper.Map<VillaDTO>(villaModel);

            return Ok(villa);
        }

        [HttpPost]
        [ResponseCache(Duration =20) , Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> CreateVilla(VillaCreateDTO villaCreateDTO) {

            if ((await dbVilla.GetOneAsync(v => v.Name == villaCreateDTO.Name)) != null) throw new AppError("Villa Name is existed", HttpStatusCode.Conflict);

            var villaModel = mapper.Map<Villa>(villaCreateDTO);

            await dbVilla.CreateOneAsync(villaModel);

            // Upload Image
            if (villaCreateDTO.Image != null)
            {
                string fileName = "villa" + villaModel.Id + Path.GetExtension(villaCreateDTO.Image.FileName);
                string filePath = @"wwwroot\VillaImages\" + fileName;

                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                FileInfo file = new FileInfo(directoryLocation);

                if (file.Exists) file.Delete();

                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    villaCreateDTO.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                villaModel.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                villaModel.ImageLocalPath = filePath;

            }
            else
            {
                villaModel.ImageUrl = "https://placehold.co/600x400";

            }

            await dbVilla.UpdateOneAsync(villaModel);

            res.Result = mapper.Map<VillaDTO>(villaModel);
            res.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villaModel.Id }, res);

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<APIResponse>> DeleteOneVilla(int id) {

            var villa = await dbVilla.GetOneAsync(v=>v.Id == id);
            
            if (villa == null) throw new AppError("Villa With this Id is not found" , HttpStatusCode.NotFound);


            await dbVilla.DeleteOneAsync(villa);
            
            if (!string.IsNullOrEmpty(villa.ImageLocalPath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), villa.ImageLocalPath);

                FileInfo file = new FileInfo(oldFilePath);

                if (file.Exists) file.Delete();
            }

            res.StatusCode = HttpStatusCode.OK;

            return Ok(res);
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateVilla( VillaUpdateDTO villaUpdateDTO) {
            var villa = await dbVilla.GetOneAsync(v => v.Id == villaUpdateDTO.Id, tracked: false);

            if (villa == null) throw new AppError("Villa not found", HttpStatusCode.NotFound);

            var villaModel = mapper.Map<Villa>(villaUpdateDTO);


            if (villaUpdateDTO.Image != null)
            {
                if (!string.IsNullOrEmpty(villaModel.ImageLocalPath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), villaModel.ImageLocalPath);

                    FileInfo file = new FileInfo(oldFilePath);

                    if (file.Exists) file.Delete();
                }

                string fileName = "villa" + villaModel.Id + Path.GetExtension(villaUpdateDTO.Image.FileName);
                string filePath = @"wwwroot\VillaImages\" + fileName;

                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    villaUpdateDTO.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                villaModel.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                villaModel.ImageLocalPath = filePath;
            }

            await dbVilla.UpdateOneAsync(villaModel);

            res.StatusCode = HttpStatusCode.Created;
            res.Result =mapper.Map<VillaDTO>(villaModel);

            return CreatedAtRoute("GetVilla", new { id = villaModel.Id }, res);

        }
    }
}
