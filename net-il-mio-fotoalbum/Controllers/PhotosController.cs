using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "PHOTOGRAPHER")]
    public class PhotosController : Controller
    {
        private readonly PhotoContext _context;

        public PhotosController(PhotoContext context)
        {
            _context = context;
        }

        // GET: Photos
        public async Task<IActionResult> Index()
        {
            ClaimsIdentity? user = (ClaimsIdentity?)User.Identity;
            Claim? id = user.FindFirst(ClaimTypes.NameIdentifier);

            return _context.Photos != null ?
                        View(await _context.Photos.Where(photo => photo.UserId == id.Value).Include(photo => photo.Categories).ToListAsync()) :
                        Problem("Entity set 'PhotoContext.Photos'  is null.");
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {

            List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
            List<Category> databaseAllCategory = _context.Categories.ToList();

            foreach (Category category in databaseAllCategory)
            {
                allCategoriesSelectList.Add(
                    new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
            }

            PhotoFormModel model = new PhotoFormModel
            {
                Photo = new Photo(),
                Categories = allCategoriesSelectList
            };

            return View("Create", model);
        }

        // POST: Photos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhotoFormModel data)
        {
            ClaimsIdentity? user = (ClaimsIdentity?)User.Identity;
            Claim? id = user.FindFirst(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategory = _context.Categories.ToList();

                foreach (Category category in databaseAllCategory)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Name,
                            Value = category.Id.ToString()
                        });
                }

                data.Categories = allCategoriesSelectList;

                return View("Create", data);
            }

            data.Photo.Categories = new List<Category>();

            if (data.SelectedCategoriesId != null)
            {
                foreach (string categorySelectedId in data.SelectedCategoriesId)
                {
                    int intTagSelectedId = int.Parse(categorySelectedId);

                    Category? categoryInDb = _context.Categories.Where(category => category.Id == intTagSelectedId).FirstOrDefault();

                    if (categoryInDb != null)
                    {
                        data.Photo.Categories.Add(categoryInDb);
                    }
                }
            }

            MemoryStream stream = new MemoryStream();
            data.ImageFormFile.CopyTo(stream);
            data.Photo.Image = stream.ToArray(); ;

            data.Photo.UserId = id.Value;
            _context.Add(data.Photo);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Photos/Edit/5
        public IActionResult Edit(int id)
        {

            Photo? photoToEdit = _context.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound();
            }
            else
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategory = _context.Categories.ToList();

                foreach (Category category in databaseAllCategory)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Name,
                            Value = category.Id.ToString(),
                            Selected = photoToEdit.Categories.Any(categoryId => categoryId.Id == category.Id)
                        });
                }

                PhotoFormModel model = new PhotoFormModel
                {
                    Photo = photoToEdit,
                    Categories = allCategoriesSelectList
                };

                return View("Edit", model);
            }
        }

        // POST: Photos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PhotoFormModel data)
        {

            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategory = _context.Categories.ToList();

                foreach (Category category in databaseAllCategory)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Name,
                            Value = category.Id.ToString()
                        });
                }

                PhotoFormModel model = new PhotoFormModel
                {
                    Photo = new Photo(),
                    Categories = allCategoriesSelectList
                };

                data.Categories = allCategoriesSelectList;

                return View("Edit", data);
            }

            Photo? photoToEdit = _context.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToEdit != null)
            {
                photoToEdit.Categories.Clear();

                photoToEdit.Title = data.Photo.Title;
                photoToEdit.Description = data.Photo.Description;
                photoToEdit.IsVisible = data.Photo.IsVisible;

                MemoryStream stream = new MemoryStream();
                data.ImageFormFile.CopyTo(stream);
                photoToEdit.Image = stream.ToArray();

                if (data.SelectedCategoriesId != null)
                {
                    foreach (string selectedCategorytId in data.SelectedCategoriesId)
                    {
                        int intSelectedCategoryId = int.Parse(selectedCategorytId);

                        Category? categoryInDb = _context.Categories.Where(category => category.Id == intSelectedCategoryId).FirstOrDefault();

                        if (categoryInDb != null)
                        {
                            photoToEdit.Categories.Add(categoryInDb);
                        }
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("Details", "Photos", new { id = photoToEdit.Id });
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Photos == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Photos == null)
            {
                return Problem("Entity set 'PhotoContext.Photos'  is null.");
            }
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return (_context.Photos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
