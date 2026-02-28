using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Represents one row in the admin associated-products grid
/// </summary>
public record AssociatedProductModel : BaseNopEntityModel
{
    /// <summary>
    /// The linked product identifier
    /// </summary>
    public int AssociatedProductId { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.AssociatedProducts.Fields.Product")]
    public string AssociatedProductName { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.AssociatedProducts.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }
}
