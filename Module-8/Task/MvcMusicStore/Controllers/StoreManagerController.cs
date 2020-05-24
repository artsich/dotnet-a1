using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using Serilog;

namespace MvcMusicStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        public async Task<ActionResult> Index()
        {
            return View(await _storeContext.Albums
                .Include(a => a.Genre)
                .Include(a => a.Artist)
                .OrderBy(a => a.Price).ToListAsync());
        }

        public async Task<ActionResult> Details(int id = 0)
        {
            var album = await GetAlbum(id);
            
            if (album == null)
            {
                return HttpNotFound();
            }

            return View(album);
        }

        public async Task<ActionResult> Create()
        {
            return await BuildView(null);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Albums.Add(album);
                
                await _storeContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return await BuildView(album);
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            var album = await GetAlbum(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return await BuildView(album);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                _storeContext.Entry(album).State = EntityState.Modified;

                await _storeContext.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            else
            {
                var errors = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)));
                Log.Logger.Error($"Invalid Album model state!. Errors: {errors}");
            }

            return await BuildView(album);
        }

        // GET: /StoreManager/Delete/5
        public async Task<ActionResult> Delete(int id = 0)
        {
            var album = await GetAlbum(id);
            if (album == null)
            {
                Log.Logger.Error($"The album with id: {id} can't be delted because not found.");
                return HttpNotFound();
            }

            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var album = await GetAlbum(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            _storeContext.Albums.Remove(album);

            await _storeContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<Album> GetAlbum(int id)
        {
            var album = await _storeContext.Albums.FindAsync(id);
            if (album == null)
            {
                Log.Logger.Error($"The album with id not found.");
                return null;
            }

            return album;
        }

        private async Task<ActionResult> BuildView(Album album)
        {
            ViewBag.GenreId = new SelectList(
                await _storeContext.Genres.ToListAsync(),
                "GenreId",
                "Name",
                album == null ? null : (object)album.GenreId);

            ViewBag.ArtistId = new SelectList(
                await _storeContext.Artists.ToListAsync(),
                "ArtistId",
                "Name",
                album == null ? null : (object)album.ArtistId);

            return View(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}