using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;
using HomeEdu.UI.Areas.Admin.ViewModels.NoticeBoardViewModels;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NoticeBoardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public NoticeBoardController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<NoticeBoard> noticeBoards = await _context.noticeBoards.ToListAsync();
            return View(noticeBoards);
        }
        public IActionResult Create()
        {
            NoticeBoard noticeBoard = new NoticeBoard();

            return View(noticeBoard);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(NoticeBoard noticeBoard)
        {
            if (!ModelState.IsValid)
            {
                return View(noticeBoard);
            }

            if (noticeBoard.Time <= DateTime.Now.Date)
            {
                return BadRequest();
            }

            await _context.noticeBoards.AddAsync(noticeBoard);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var noticeBoard = await _context.noticeBoards.FindAsync(id);
            if (noticeBoard == null)
            {
                return NotFound();
            }
            return View(noticeBoard);
        }
        [HttpPost]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var noticeBoard = await _context.noticeBoards.FindAsync(id);
            if (noticeBoard == null)
            {
                return NotFound();
            }
            _context.noticeBoards.Remove(noticeBoard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ActionName("Update")]
        public async Task<IActionResult> Update(int id)
        {
            var noticeBoard = await _context.noticeBoards.FindAsync(id);
            if (noticeBoard == null)
            {
                return NotFound();
            }
            var noticeBoardDb = new UpdateNoticeBoardViewModel
            {
                Time = noticeBoard.Time,
                Detail = noticeBoard.Detail,
            };
            return View(noticeBoardDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateNoticeBoardViewModel noticeBoard)
        {
            if (id != noticeBoard.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(noticeBoard);
            }
            if (noticeBoard.Time <= DateTime.Now.Date)
            {
                return BadRequest();
            }
            NoticeBoard? noticeBoard1 = await _context.noticeBoards.FindAsync(id);
            if (noticeBoard1 == null)
            {
                return BadRequest();
            }
            if(noticeBoard.Time != null)
            {
            noticeBoard1.Time = noticeBoard.Time;
            }
            noticeBoard1.Detail = noticeBoard.Detail;
            _context.Entry(noticeBoard1).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
