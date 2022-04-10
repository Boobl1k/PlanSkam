using Planscam.Entities;

namespace Planscam.Extensions;

public static class PicturesExtensions
{
    public static Picture ToPicture(this IFormFile formFile)
    {
        using var reader = new BinaryReader(formFile.OpenReadStream());
        return new Picture {Data = reader.ReadBytes((int) formFile.Length)};
    }
}
