using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Common.Models;
using MyVet.Web.Data;
using MyVet.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AgendaController : Controller
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public AgendaController(
            DataContext dataContext,
            IConverterHelper converterHelper)
        {
            _context = dataContext;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        [Route("GetAgendaForOwner")]
        public async Task<IActionResult> GetAgendaForOwner(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agendas = await _context.Agendas
                .Include(a => a.Owner)
                .ThenInclude(o => o.User)
                .Include(a => a.Pet)
                .ThenInclude(p => p.PetType)
                .Where(a => a.Date >= DateTime.Today.ToUniversalTime())
                .OrderBy(a => a.Date)
                .ToListAsync();

            var response = new List<AgendaResponse>();
            foreach (var agenda in agendas)
            {
                var agendaRespose = new AgendaResponse
                {
                    Date = agenda.Date,
                    Id = agenda.Id,
                    IsAvailable = agenda.IsAvailable
                };

                if (agenda.Owner != null)
                {
                    if (agenda.Owner.User.Email.ToLower().Equals(emailRequest.Email.ToLower()))
                    {
                        agendaRespose.Owner = _converterHelper.ToOwnerResposne(agenda.Owner);
                        agendaRespose.Pet = _converterHelper.ToPetResponse(agenda.Pet);
                        agendaRespose.Remarks = agenda.Remarks;
                    }
                    else
                    {
                        agendaRespose.Owner = new OwnerResponse { FirstName = "Reserved" };
                    }
                }
                response.Add(agendaRespose);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("AssignAgenda")]
        public async Task<IActionResult> AssignAgenda(AssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agenda = await _context.Agendas.FindAsync(request.AgendaId);
            if (agenda == null)
            {
                return BadRequest("Cita no existe.");
            }

            if (!agenda.IsAvailable)
            {
                return BadRequest("Cita no disponible.");
            }

            var owner = await _context.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return BadRequest("Propietario no existe.");
            }

            var pet = await _context.Pets.FindAsync(request.PetId);
            if (pet == null)
            {
                return BadRequest("Mascota no existe.");
            }

            agenda.IsAvailable = false;
            agenda.Remarks = request.Remarks;
            agenda.Owner = owner;
            agenda.Pet = pet;

            _context.Agendas.Update(agenda);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        [Route("UnAssignAgenda")]
        public async Task<IActionResult> UnAssignAgenda(UnAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agenda = await _context.Agendas
                .Include(a => a.Owner)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(a => a.Id == request.AgendaId);
            if (agenda == null)
            {
                return BadRequest("Cita no existe.");
            }

            if (agenda.IsAvailable)
            {
                return BadRequest("Cita disponible.");
            }

            agenda.IsAvailable = true;
            agenda.Remarks = null;
            agenda.Owner = null;
            agenda.Pet = null;

            _context.Agendas.Update(agenda);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}