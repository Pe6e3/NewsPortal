using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;

namespace News.Controllers
{
    public class PostsController : Controller
    {
        private readonly NewsContext _db;

        public PostsController(NewsContext context)
        {
            _db = context;
        }


        public async Task<IActionResult> MainPage()
        {
            ViewBag.Categories = _db.Category;
            var postList = _db.Post
                .Include(c => c.Category);

            return View(await postList.ToListAsync());
        }


        // GET: Posts

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var newsContext = _db.Post.Include(p => p.Category);
            return View(await newsContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Post == null)
            {
                return NotFound();
            }

            var post = await _db.Post
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewBag.Category = _db.Category;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,PostTitle,PostBody,CategoryId,PostDate")] Post post)
        {
            if (ModelState.IsValid)
            {
                _db.Add(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_db.Category, "CatehoryId", "CatehoryId", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Post == null)
            {
                return NotFound();
            }

            var post = await _db.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_db.Category, "CatehoryId", "CatehoryId", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,PostTitle,PostBody,CategoryId,PostDate")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(post);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_db.Category, "CatehoryId", "CatehoryId", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Post == null)
            {
                return NotFound();
            }

            var post = await _db.Post
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Post == null)
            {
                return Problem("Entity set 'NewsContext.Post'  is null.");
            }
            var post = await _db.Post.FindAsync(id);
            if (post != null)
            {
                _db.Post.Remove(post);
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_db.Post?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
