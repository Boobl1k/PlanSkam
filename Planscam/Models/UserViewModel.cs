using Microsoft.AspNetCore.Mvc;
using Planscam.Entities;

namespace Planscam.Models;

public class UserViewModel
{
    [HiddenInput(DisplayValue = false)] public string Id { get; set; }
    public string Name { get; set; }
    [HiddenInput] public string Email { get; set; }
    public Picture? Picture { get; set; }

    public void Deconstruct(out string id, out string name, out string email, out Picture? picture)
    {
        id = this.Id;
        name = this.Name;
        email = this.Email;
        picture = this.Picture;
    }
}
