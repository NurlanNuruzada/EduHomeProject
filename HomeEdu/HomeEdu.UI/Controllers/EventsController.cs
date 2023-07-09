using AutoMapper;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public EventsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            PagesVM pagesVM = new()
            {
                Events = await _context.Events.Include(e=>e.EventDetail).ToListAsync(),
                Blogs = await _context.Blogs.Include(b=>b.BlogCatagory).ToListAsync()
            };
            if (pagesVM == null)
            {
                return NotFound();
            }
            return View(pagesVM);
        }
    }
}
