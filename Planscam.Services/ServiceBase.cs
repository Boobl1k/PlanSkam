using Microsoft.AspNetCore.Identity;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Services;

public class ServiceBase
{
    protected readonly AppDbContext DataContext;
    protected readonly UserManager<User> UserManager;

    public ServiceBase(AppDbContext dataContext, UserManager<User> userManager)
    {
        DataContext = dataContext;
        UserManager = userManager;
    }
}
