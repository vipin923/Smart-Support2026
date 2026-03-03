using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Smart_Support2026.DataAccessLayer;
using Smart_Support2026.Models;
using System.Data;
using System.Data.Common;

namespace Smart_Support2026.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string MenuCacheKey = "AppNavigationMenu";
        public MenuViewComponent(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;


        }
        public async Task<IViewComponentResult> InvokeAsync(string role)
        {
            try
            {
                string cacheKey = $"Menu_{role}";
                if (!_cache.TryGetValue(cacheKey, out List<Menu.Links> menu))
                {
                    using (DbConnection connection = _context.Database.GetDbConnection())
                    {
                        var menuDictionary = new Dictionary<int, Menu.Links>();
                        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

                        await connection.QueryAsync<Menu.Links, Menu.SubLinks, Menu.Links>(
            "sp_ssgetlink",
            (main, sub) =>
            {
                if (!menuDictionary.TryGetValue(main.Id, out var currentMain))
                {
                    currentMain = main;
                    menuDictionary.Add(currentMain.Id, currentMain);
                }
                if (sub != null)
                {
                    currentMain.SubLinks.Add(sub);
                }
                return currentMain;
            },
            new { role = role },
            splitOn: "SubLinkId", // Tells Dapper where the Sublink columns start
            commandType: CommandType.StoredProcedure
        );
                        menu = menuDictionary.Values.ToList();
                        // Logic to nest sublinks
                        _cache.Set(cacheKey, menu, TimeSpan.FromMinutes(60));



                    }
                }
                return View("Menu", menu);
            }
            catch (Exception ex)
            {

                throw new Exception($"SQL Error: {ex.Message}");
                return View("Error");
            }
        }
    }
}
