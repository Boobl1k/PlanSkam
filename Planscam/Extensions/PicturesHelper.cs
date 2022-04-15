using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Planscam.Entities;

namespace Planscam.Extensions;

public static class PicturesHelper
{
    public static IHtmlContent DrawUserAvatar(this IHtmlHelper helper, Picture? picture) =>
        helper.DrawPic(picture, 80, 60);

    public static IHtmlContent DrawSmallTrackPic(this IHtmlHelper helper, Picture? picture) =>
        helper.DrawPic(picture, 40);

    public static IHtmlContent DrawHugeTrackPic(this IHtmlHelper helper, Picture? picture) =>
        helper.DrawPic(picture, 500);
    
    public static IHtmlContent DrawSmallPlaylistPic(this IHtmlHelper helper, Picture? picture) =>
        helper.DrawPic(picture, 300);
    
    public static IHtmlContent DrawHugePlaylistPic(this IHtmlHelper helper, Picture? picture) =>
        helper.DrawPic(picture, 700);

    private static IHtmlContent DrawPic(this IHtmlHelper helper, Picture? picture, int x, int? y = default) =>
        helper.Raw($"<img style='width:{x}px; height:{y ?? x}px;' src=\"{CreateSrc(picture)}\"/>");

    private static string CreateSrc(Picture? picture) =>
        picture?.Data.ToPictureString() ?? "/img/vopros.jpg";

    private static string ToPictureString(this byte[] arr) =>
        $"data:image/jpeg;base64,{Convert.ToBase64String(arr)}";
}
