﻿using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BlogsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            PagesVM pagesVM = new()
            {
                Blogs = await _context.Blogs.ToListAsync(),
                Courses = await _context.Courses.Include(c => c.CourseDetail).ToListAsync(),
                BlogCatagories = await _context.BlogCatagories.ToListAsync(),
            };
            if (pagesVM == null)
            {
                return NotFound();
            }
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int rescCout = pagesVM.Blogs.Count();
            var pager = new Pager(rescCout, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.pager = pager;

            pagesVM.blgoData = pagesVM.Blogs.Skip(recSkip).Take(pager.PageSize).ToList();
            return View(pagesVM);
        }
    }
}
