using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    public class EventDetailController : Controller
    {
        private readonly AppDbContext _context;

        public EventDetailController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int Id)
        {
            var Blogs = await _context.Blogs.ToListAsync();
            var @event = await _context.Events.FindAsync(Id);
            var eventDetails = await _context.EventDetails
                .Where(e => e.EventId == Id)
                .ToListAsync();
            var speakers = await _context.Speakers
       .Include(s => s.EventDetail)
       .Where(s => s.EventDetail.EventId == Id)
       .ToListAsync();

            var eventDetailVM = new EventDetailViewModel
            {
                Event = @event,
                eventDetails = eventDetails,
                 Speakers = speakers,
                 Blog= Blogs
            };

            return View(eventDetailVM);
        }
    }
}