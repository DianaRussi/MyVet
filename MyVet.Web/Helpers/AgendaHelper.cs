using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public class AgendaHelper : IAgendaHelper
    {
        private readonly DataContext _context;

        public AgendaHelper(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task AddDaysAsync(int days)
        {
            DateTime initialDate;

            if (!_context.Agendas.Any())
            {
                initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            }
            else
            {
                var agenda = _context.Agendas.LastOrDefault();
                initialDate = new DateTime(agenda.Date.Year, agenda.Date.Month, agenda.Date.AddDays(1).Day, 8, 0, 0);
            }

            var finalDate = initialDate.AddDays(days);
            while (initialDate < finalDate)
            {
                if (initialDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    var finalDate2 = initialDate.AddHours(10);
                    while (initialDate < finalDate2)
                    {
                        _context.Agendas.Add(new Agenda
                        {
                            Date = initialDate.ToUniversalTime(),
                            IsAvailable = true
                        });

                        initialDate = initialDate.AddMinutes(30);
                    }

                    initialDate = initialDate.AddHours(14);
                }
                else
                {
                    initialDate = initialDate.AddDays(1);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
