using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

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
        public async Task<IActionResult> Index(string search, int pg = 1)
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
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int rescCout = pagesVM.Events.Count();
            var pager = new Pager(rescCout, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.pager = pager;

            pagesVM.EventsData = pagesVM.Events.Skip(recSkip).Take(pager.PageSize).ToList();
            if (string.IsNullOrEmpty(search))
            {
                TempData["InfoMessage"] = "Please Provide Search Value";
                pagesVM.EventsData = pagesVM.EventsData.Skip(recSkip).Take(pager.PageSize).ToList();
                return View(pagesVM);
            }
            else
            {
                var searchByTitle = pagesVM.EventsData.Where(c => c.Title.ToLower().Contains(search.ToLower())).ToList();
                int searchResultCount = searchByTitle.Count;
                pager = new Pager(searchResultCount, pg, pageSize);
                recSkip = (pg - 1) * pageSize;
                pagesVM.EventsData = searchByTitle.Skip(recSkip).Take(pager.PageSize).ToList();
                ViewBag.pager = pager;
                return View(pagesVM);
            }
        }
    }
}
