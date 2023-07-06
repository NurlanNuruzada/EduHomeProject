using AutoMapper;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Controllers
{
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
