using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SliderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {

            List<Slider> Slider = await _context.Sliders.ToListAsync();
            return View(Slider);
        }
    }
}
