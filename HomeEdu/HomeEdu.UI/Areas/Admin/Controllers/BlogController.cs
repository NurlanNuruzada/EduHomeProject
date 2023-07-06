using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using HomeEdu.UI.Services.Interfaces;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    //Blog blog = _mapper.Map<Blog>(BlogVM);
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlogService _blogService;
        public BlogController(AppDbContext context, IMapper mapper, IBlogService blogService)
        {
            _context = context;
            _mapper = mapper;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _blogService.GetAllBlogAsync();
            return View(blogs);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BlogVM BlogVM, int CatagoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(BlogVM);
            }
            var catagory = _context.BlogCatagories.Find(CatagoryId);

            if (catagory is null)
            {
                return NotFound();
            }
            DateTime time = DateTime.Now;
            Blog blog = new()
            {
                PostedBy = BlogVM.PostedBy,
                PostTime = time,
                ImagePath = BlogVM.ImagePath,
                CommentCount = BlogVM.CommentCount,
                Comment = BlogVM.Comment,
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
            BlogVM BlogVM =  _mapper.Map<BlogVM>(blog);
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View(BlogVM);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, BlogVM BlogVM,int blogCatagoryId)
        {
            if (BlogVM == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(BlogVM);
            }
            var catagory = _context.BlogCatagories.Find(blogCatagoryId);
            if(catagory== null)
            {
                return Json("catargory not found");
            }
            Blog? blogDb = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(s=>s.Id==Id);
            if (blogDb == null)
            {
                return NotFound();
            }
            blogDb.PostTime = BlogVM.PostTime;
            blogDb.ImagePath = BlogVM.ImagePath;
            blogDb.Comment = BlogVM.Comment;
            blogDb.PostedBy = BlogVM.PostedBy;
            blogDb.BlogCatagoryId = blogCatagoryId;
            blogDb.CommentCount = BlogVM.CommentCount;
            _context.Entry<Blog>(blogDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}