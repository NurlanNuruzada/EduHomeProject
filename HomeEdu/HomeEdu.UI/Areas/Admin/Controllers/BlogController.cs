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
using Microsoft.AspNetCore.Authorization;
using HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel;
using HomeEdu.UI.Helpers.Extentions;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;
using HomeEdu.DataAccess.Migrations;
using System.Reflection.Metadata;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    //Blog blog = _mapper.Map<Blog>(BlogVM);
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IMapper mapper, IBlogService blogService, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _blogService = blogService;
            _env = env;
        }
        public async Task<IActionResult> Index(int pg=1)
        {
            List<Blog> blogs = await _blogService.GetAllBlogAsync();
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int rescCout = blogs.Count();
            var pager = new Pager(rescCout,pg,pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = blogs.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.pager = pager;
            return View(data);
        }

        //blog category start
        public async Task<IActionResult> GetBlogCaragory()
        {
            List<BlogCatagory> blogCatagories = await _context.BlogCatagories.ToListAsync();
            return View(blogCatagories);
        }
        public IActionResult CreateBlogCaragory()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateBlogCaragory(BlogCatagory blogCatagory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Couldn't add Categry!");
                return View();
            }
            if (blogCatagory is null)
            {
                return View();
            }
            _context.BlogCatagories.AddAsync(blogCatagory);
            _context.SaveChangesAsync();
            return RedirectToAction("GetBlogCaragory", "Blog");
        }
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteBlogCategory(int Id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            BlogCatagory FingCatagory = await _context.BlogCatagories.FindAsync(Id);
            if (FingCatagory is null)
            {
                return NotFound();
            }
            _context.Entry<BlogCatagory>(FingCatagory).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("GetBlogCaragory", "Blog");
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
            if (BlogVM.Image is null)
            {
                ModelState.AddModelError("Image", "Image Can't be null!");
                return View(BlogVM);
            }
            var catagory = _context.BlogCatagories.Find(CatagoryId);
            if (catagory is null)
            {
                return NotFound("Catagory not Found");
            }
            BlogVM.blogCatagory = catagory;
            if (!ModelState.IsValid)
            {
                return View(BlogVM);
            }
            if (!BlogVM.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(BlogVM);
            }
            if (!BlogVM.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(BlogVM);
            }
            string filePath = await BlogVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "blog");
            Blog Blog = _mapper.Map<Blog>(BlogVM);
            Blog.ImagePath = filePath;
            DateTime time = DateTime.Now;
            Blog blog = new()
            {
                PostedBy = BlogVM.PostedBy,
                PostTime = time,
                ImagePath = filePath,
                CommentCount = 0,
                Comment = BlogVM.Comment,
                BlogCatagoryId = CatagoryId,
                Title = BlogVM.Title 
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
            Blog? blog = await _context.Blogs
                 .Where(e => e.Id == id)
                 .Include(e => e.BlogCatagory)
                 .FirstOrDefaultAsync();
            if (blog == null)
            {
                return NotFound();
            }
            if (blog.BlogCatagory == null)
            {
                ModelState.AddModelError("", "BlogCatagoryNotFound");
                return View();
            }
            var BlogVM = _mapper.Map<BlogVM>(blog);
            BlogVM.Comment = blog.Comment;
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View(BlogVM);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, BlogVM BlogVM, int CatagoryId)
        {
            Blog? blogDb = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Id);
            BlogVM.BlogCatagoryId = CatagoryId;
            if (BlogVM == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(BlogVM);
            }
            var catagory = await _context.BlogCatagories.AsNoTracking().FirstOrDefaultAsync(s => s.Id == CatagoryId); ;
            if (BlogVM.Image is not null)
            {
                if (catagory == null)
                {
                    return Json("catargory not found");
                }
                if (blogDb == null)
                {
                    return NotFound();
                }
                if (!BlogVM.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Sellect Correct Format!");
                    return View(blogDb);
                }
                if (!BlogVM.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                    return View(blogDb);
                }
                string filePath = await BlogVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "blog");
                blogDb.ImagePath = filePath;
            }
            blogDb.Comment = BlogVM.Comment;
            blogDb.PostedBy = BlogVM.PostedBy;
            blogDb.BlogCatagoryId = CatagoryId;
            blogDb.CommentCount = BlogVM.CommentCount;
            blogDb.Title = BlogVM.Title;
            var BlogDb = _mapper.Map<Blog>(BlogVM);
            _context.Entry<Blog>(blogDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Details(int Id)
        {
            Blog? Blog = await _context.Blogs
                .Where(e => e.Id == Id)
                .Include(e => e.BlogCatagory)
                .FirstOrDefaultAsync();

            return View(Blog);
        }
    }
}