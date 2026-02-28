using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using Nop.Plugin.Widgets.AssociatedProducts.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Components;

/// <summary>
/// View component rendered in <c>AdminWidgetZones.ProductDetailsBlock</c>.
/// Injects the Associated Products management card into the product edit page.
/// </summary>
[ViewComponent(Name = AssociatedProductsDefaults.AdminViewComponentName)]
public class AssociatedProductsAdminViewComponent : NopViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        // additionalData is the ProductModel passed by the admin product edit page
        if (additionalData is not ProductModel productModel)
            return Task.FromResult<IViewComponentResult>(Content(string.Empty));

        var model = new AdminAssociatedProductsModel
        {
            ProductId = productModel.Id
        };

        model.AssociatedProductSearchModel.ProductId = productModel.Id;
        model.AssociatedProductSearchModel.SetGridPageSize();

        return Task.FromResult<IViewComponentResult>(View(
            "~/Plugins/Widgets.AssociatedProducts/Views/Shared/Components/AssociatedProductsAdmin/Default.cshtml",
            model));
    }
}
