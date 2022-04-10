using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;

namespace Planscam.Controllers;

public class PlaylistsController : Controller
{
    private readonly AppDbContext _dataContext;

    public PlaylistsController(AppDbContext dataContext) => 
        _dataContext = dataContext;

    [HttpGet, Route("favoriteTracks")]
    public IActionResult FavoriteTracks()
    {
        return default;
    }
}
