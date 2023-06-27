using EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            List<Blog> Blogs = await _context.Blogs.Include(c => c.BlogCatagory).ToListAsync();
            return View(Blogs);
        }
      

        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostVM blogPostVM, int CatagoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(blogPostVM);
            }
            var catagory = _context.BlogCatagories.Find(CatagoryId);

            if (catagory is null)
            {
                return BadRequest();
            }
            Blog blog = new()
            {
                PostedBy = blogPostVM.PostedBy,
                PostTime = blogPostVM.PostTime,
                ImagePath = blogPostVM.ImagePath,
                CommentCount = blogPostVM.CommentCount,
                Comment = blogPostVM.Comment,
                BlogCatagoryId = CatagoryId
            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          
        }
        public async Task<IActionResult> Delete(int Id)
        {
            Blog? blogDb = await _context.Blogs.FindAsync(Id);
            if (blogDb is null)
            {
                return NotFound();
            }
            return View(blogDb);
        }
    }
}