using Microsoft.AspNetCore.Mvc;
using gaas.Data;
using gaas.Models;
namespace gaas.Controllers;

public class GaaSController : Controller
{
    // Database constructor
    private readonly AppDbContext _db;
    public GaaSController(AppDbContext db)
    {
        _db = db;
    }

    
    // Local db @ '/Users/mattrose570/Library/Application Support/gaas/gaas.db'
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetGuid()
    {
        var savedGuid = new SavedGuid
        {
            Value = Guid.NewGuid()
        };
        
        // Prepare new entry 
        _db.SavedGuids.Add(savedGuid);
        
        // Write to database
        await _db.SaveChangesAsync();
        
        // return guid to site
        return Json(savedGuid.Value.ToString());
    }

}