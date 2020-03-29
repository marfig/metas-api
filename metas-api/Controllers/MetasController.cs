using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using metas_api.DTO;
using metas_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace metas_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetasController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MetasController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [Route("test")]
        [HttpGet]    
        public ActionResult test(){
            return Ok("Api ready to use");
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaDTO>>> GetAll(string api_key)
        {
            var app = await _dbContext.ExternalApplications.FindAsync(api_key);

            if (app == null) throw new UnauthorizedAccessException();

            var items = await _dbContext.Metas.Where(c => c.FechaDelete == null && c.ApplicationId == api_key)
                .Select(c => new MetaDTO()
                {
                    Id = c.Id,
                    Parentid = c.ParentId,
                    Titulo = c.Titulo,
                    Descripcion = c.Descripcion,
                    Notas = c.Notas,
                    Porcentaje = c.Porcentaje,
                    FechaAdd = c.FechaAdd
                })
                .ToListAsync();

            return items;
        }

        [Route("sincronize")]
        [HttpPost]
        public async Task<ActionResult> Sincronize([FromBody] IEnumerable<MetaDTO> model, string api_key)
        {
            var app = await _dbContext.ExternalApplications.FindAsync(api_key);

            if (app == null) throw new UnauthorizedAccessException();

            List<string> Ids = new List<string>();

            foreach(var m in model)
            {
                var item = await _dbContext.Metas.FindAsync(m.Id);

                if (item == null)
                {
                    Meta new_item = new Meta()
                    {
                        Id = m.Id,
                        ApplicationId = api_key,
                        Titulo = m.Titulo,
                        Descripcion = m.Descripcion,
                        Notas = m.Notas,
                        ParentId = m.Parentid,
                        Porcentaje = m.Porcentaje,
                        FechaAdd = DateTime.Now
                    };

                    if (m.Eliminado == 1) new_item.FechaDelete = DateTime.Now;

                    _dbContext.Metas.Add(new_item);

                    Ids.Add(new_item.Id);
                }
                else
                {
                    item.Titulo = m.Titulo;
                    item.Descripcion = m.Descripcion;
                    item.Notas = m.Notas;
                    item.Porcentaje = m.Porcentaje;

                    if (m.Eliminado == 1) item.FechaDelete = DateTime.Now;

                    Ids.Add(item.Id);
                }
            }
            
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Ids = string.Join(',', Ids)
            });
        }

    }
}
