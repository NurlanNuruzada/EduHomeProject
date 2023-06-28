using AutoMapper;
using EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    //Blog blog = _mapper.Map<Blog>(blogPostVM);
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        [AutoValidateAntiforgeryToken]
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
            DateTime time = DateTime.Now;
            Blog blog = new()
            {
                PostedBy = blogPostVM.PostedBy,
                PostTime = time,
                ImagePath = blogPostVM.ImagePath,
                CommentCount = blogPostVM.CommentCount,
                Comment = blogPostVM.Comment,
                BlogCatagoryId = CatagoryId
            };
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }
        [HttpPost]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View(blog);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, Blog blog)
        {
            if (Id != blog.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(blog);
            }
            Blog? blogDb = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(s=>s.Id==Id);
            if (blogDb == null)
            {
                return NotFound();
            }
            _context.Entry<Blog>(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}