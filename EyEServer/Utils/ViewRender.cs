using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using System.Threading.Tasks;
namespace EyEServer.Utils;

//todo is this needed?
//https://dotnetstories.com/blog/Generate-a-HTML-string-from-cshtml-razor-view-using-ASPNET-Core-that-can-be-used-in-the-c-controlle-7173969632 слишком
//https://stackoverflow.com/questions/483091/how-to-render-an-asp-net-mvc-view-as-a-string ответ от Marcin(ответил 28 сентября 19 в 8:19)
public class ViewRenderer(IRazorViewEngine _viewEngine)
{
    public async Task<string> RenderViewToStringAsync<TModel>(Controller controller, string path, TModel model)
    {
        var viewEngineResult = _viewEngine.GetView(path, path, false);

        if (!viewEngineResult.Success)
        {
            return default;
            throw new InvalidOperationException(string.Format("Could not find view: {0}", path));
        }

        var view = viewEngineResult.View;
        controller.ViewData.Model = model;

        await using var writer = new StringWriter();
        var viewContext = new ViewContext(
           controller.ControllerContext,
           view,
           controller.ViewData,
           controller.TempData,
           writer,
           new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return writer.ToString();
    }
}
