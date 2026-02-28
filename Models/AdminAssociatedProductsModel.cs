using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Root model injected into the admin product-details card via the widget zone
/// </summary>
public record AdminAssociatedProductsModel : BaseNopModel
{
    public AdminAssociatedProductsModel()
    {
        AssociatedProductSearchModel = new AssociatedProductSearchModel();
    }

    /// <summary>
    /// Main product identifier (0 when product has not yet been saved)
    /// </summary>
    public int ProductId { get; set; }

    public AssociatedProductSearchModel AssociatedProductSearchModel { get; set; }
}
