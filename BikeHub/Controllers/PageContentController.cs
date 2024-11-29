using BikeHub.Data;
using BikeHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeHub.Controllers
{
    public class PageContentController : Controller
    {
        private readonly BikeHubDBContext dbContext;

        public PageContentController(BikeHubDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: PageContent
        public  IActionResult Index()
        {
            
            return View();
        }


        [HttpGet]
        public IActionResult ManageContent(int? id)
        {
            var model = new PageContentViewModel();

            // If editing an existing entry, fetch it from the database
            if (id.HasValue)
            {
                var pageContent = dbContext.PageContent.Find(id.Value);
                if (pageContent != null)
                {
                    model.Id = pageContent.Id;
                    model.SelectedPageName = pageContent.PageName;
                    model.SelectedSectionName = pageContent.SectionName;
                    model.Content = pageContent.Content;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageContent(PageContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                PageContent pageContent;

                if (model.Id == 0)
                {
                    // Create new entry
                    pageContent = new PageContent
                    {
                        PageName = model.SelectedPageName,
                        SectionName = model.SelectedSectionName,
                        Content = model.Content
                    };
                    dbContext.PageContent.Add(pageContent);
                }
                else
                {
                    // Update existing content
                    pageContent = dbContext.PageContent.Find(model.Id);
                    if (pageContent != null)
                    {
                        pageContent.PageName = model.SelectedPageName;
                        pageContent.SectionName = model.SelectedSectionName;
                        pageContent.Content = model.Content;
                        pageContent.LastUpdated = DateTime.Now;
                    }
                }

                await dbContext.SaveChangesAsync();
                return RedirectToAction("ContentList");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ContentList()
        {
            var contentList = await  dbContext.PageContent.ToListAsync();
            return View("ContentList", contentList);
        }
        // GET: Delete confirmation view
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var content = await dbContext.PageContent.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Delete confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await dbContext.PageContent.FindAsync(id);
            if (content != null)
            {
                dbContext.PageContent.Remove(content);
                await dbContext.SaveChangesAsync();
            }
            return View("Index"); // Redirect to the main content list page
        }
        public IActionResult AboutUs()
        {
            var pageContents = dbContext.PageContent
                                        .Where(pc => pc.PageName == "AboutUs")
                                        .ToList();
            return View(pageContents);
        }
        [HttpGet]
        public IActionResult UpdateHomePage()
        {
            var content = dbContext.PageContent
                .FirstOrDefault(p => p.PageName == "Home");

            ViewBag.Content = content?.Content ?? string.Empty;

            return View();
        }

        [HttpPost]
        public  IActionResult UpdateHomePage(string content)
        {
            var pageContent = dbContext.PageContent
                .FirstOrDefault(p => p.PageName == "Home");

            if (pageContent == null)
            {
                return NotFound("Home page content not found.");
            }

            pageContent.Content = content;
            pageContent.LastUpdated = DateTime.Now;

            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Home page updated successfully!";
            return RedirectToAction("ContentList");
        }

        [HttpGet]
        public IActionResult UpdateAboutUsPage()
        {
            var content = dbContext.PageContent
                .FirstOrDefault(p => p.PageName == "AboutUs");

            ViewBag.Content = content?.Content ?? string.Empty;

            return View();
        }

        [HttpPost]
        public IActionResult UpdateAboutUsPage(string content, IFormFile imageFile)
        {
            var pageContent = dbContext.PageContent
                .FirstOrDefault(p => p.PageName == "AboutUs");

            if (pageContent == null)
            {
                return NotFound("About Us page content not found.");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Append the image to the content
                content += $"<img src='/images/{fileName}' alt='About Us Image' />";
            }

            pageContent.Content = content;
            pageContent.LastUpdated = DateTime.Now;

            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "About Us page updated successfully!";
            return RedirectToAction("UpdateContent");
        }
    }


}
