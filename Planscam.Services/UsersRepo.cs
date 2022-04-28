using Planscam.Entities;

namespace Planscam.Services;

public class UsersRepo
{
    public User CreateNewUser(string name, string email) =>
        new()
        {
            UserName = name,
            Email = email,
            FavouriteTracks = new FavouriteTracks($"{name}'s favorite tracks"),
            OwnedPlaylists = new OwnedPlaylists()
        };
}
