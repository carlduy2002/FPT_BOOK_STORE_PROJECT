using FPT_Book_Store.Constants;
using FPT_Book_Store.Data;
using FPT_Book_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FPT_Book_Store.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("Owner/[controller]/[action]")]
    [Authorize(Roles = "Owner")]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> ds = _db.Books.Where(b => b.Book_Deleted == Status.Existing.ToString()).ToList();
            ViewData["Publisher"] = _db.Publishers.ToList();
            ViewData["Category"] = _db.Categories.ToList();

            return View(ds);
        }

        public async Task<ActionResult> Create()
        {
            ViewData["Publisher"] = _db.Publishers.Where(p => p.Publisher_Deleted == Status.Existing.ToString()).ToList();
            ViewData["Category"] = _db.Categories.Where(c => c.Category_TypeConfirm != null && c.Category_Deleted == Status.Existing.ToString()).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile Book_Image, Book books)
        {
            if (ModelState.IsValid)
            {
                var filePaths = new List<string>();
                string file = Path.GetExtension(Book_Image.FileName).ToLower().Trim();
                if (Book_Image.Length > 0)
                {
                    if (file == ".jpg" || file == ".png")
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads//Item_Image", Book_Image.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Book_Image.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        TempData["message"] = "File Type invalid. Only accept the file .jpg and .png!";
                        return RedirectToAction("Create");
                    }

                }

                books.Book_Date = DateTime.Now;
                books.Book_Image = Book_Image.FileName;
                _db.Books.Add(books);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(books);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Book obj = _db.Books.Find(id);
            // ViewData["Publisher_ID"] = new SelectList(_db.Publishers, "Publisher_ID", "Publisher_Name");
            ViewData["Publisher"] = _db.Publishers.Where(p => p.Publisher_Deleted == Status.Existing.ToString()).ToList();
            ViewData["Category"] = _db.Categories.Where(c => c.Category_TypeConfirm != null && c.Category_Deleted == Status.Existing.ToString()).ToList();
            if (obj == null)
            {

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book obj, IFormFile? Book_Images)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filePaths = new List<string>();
                    string file = Path.GetExtension(Book_Images.FileName).ToLower().Trim();
                    if (Book_Images != null)
                    {
                        // Check 
                        if (file == ".jpg" || file == ".png")
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads//Item_Image", Book_Images.FileName);

                            using (var stream = new FileStream(filePath, FileMode.Append))
                            {
                                await Book_Images.CopyToAsync(stream);
                            }
                            obj.Book_Image = Book_Images.FileName;
                        }
                        else
                        {
                            TempData["message"] = "File Type invalid. Only accept the file .jpg and .png!";
                            return RedirectToAction("Edit", new { id = obj.Book_ID });
                        }
                    }

                    obj.Book_Date = DateTime.Now;
                    _db.Books.Update(obj);
                    _db.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                return RedirectToAction("Index");
            }

            return View("Book/Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Book obj = _db.Books.Find(id);
            if (obj != null)
            {
                obj.Book_Deleted = Status.Deleted.ToString();
                _db.Books.Update(obj);
                _db.SaveChanges();

                IEnumerable<Cart> ds = _db.Carts.Where(c => c.Book_ID == obj.Book_ID).ToList();
                foreach (var i in ds)
                {
                    _db.Carts.Remove(i);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

    }
}