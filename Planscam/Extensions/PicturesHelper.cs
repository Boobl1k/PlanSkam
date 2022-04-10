using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Planscam.Entities;

namespace Planscam.Extensions;

public static class PicturesHelper
{
    public static IHtmlContent DrawUserAvatar(this IHtmlHelper helper, Picture? picture) =>
        helper.Raw(picture is null
            ? "<img style='width:80px; height:60px;' src=\"img/vopros.jpg\"/>"
            : $"<img style='width:80px; height:60px;' src=\"data:image/jpeg;base64,{Convert.ToBase64String(picture.Data)}\"/>");
}
