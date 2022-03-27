using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Planscam.Models;

public class LoginViewModel
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
    
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
         
    [HiddenInput(DisplayValue = false)]
    public string? ReturnUrl { get; set; }
}
