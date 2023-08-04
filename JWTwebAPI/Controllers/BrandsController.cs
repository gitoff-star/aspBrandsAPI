using JWTwebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace JWTwebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        DBContextcs _db;
        public BrandsController(DBContextcs db)
        {
            _db = db;
        }

        
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Brands>>> getBrands()
        {
            if (_db.Brands == null)
            {
                return NotFound();
            }


            return await _db.Brands.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Brands>> getBrands(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _db.Brands.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpPost]
        public async Task<ActionResult<Brands>> insertBrands(Brands brands)
        {
            if (brands == null)
            {
                return BadRequest();
            }
            _db.Brands.Add(brands);
            await _db.SaveChangesAsync();



            return CreatedAtAction(nameof(getBrands), new { id = brands.Id }, brands);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Brands>> updateBrands(Brands brands, int id)
        {
            if (id != brands.Id)
            {
                return BadRequest();
            }
           
            _db.Entry(brands).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {

                return NotFound();
            }

            return Ok();
        }
        [HttpDelete("{id}")]

            public async Task<ActionResult> deleteBrand(int id){

            if (id == 0)
            {
                return NotFound();
            }

           var data= await _db.Brands.FindAsync(id);
            if(data == null)
            {
                return NotFound() ;
            }

            _db.Brands.Remove(data);

            try
            {
                await _db.SaveChangesAsync();

            }
            catch (Exception)
            {

                return NotFound();
            }


            return Ok();
            }

    }
}
