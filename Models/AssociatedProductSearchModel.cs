using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Search/pagination parameters for the admin associated-products grid
/// </summary>
public record AssociatedProductSearchModel : BaseSearchModel
{
    /// <summary>
    /// Main product whose associations are being listed
    /// </summary>
    public int ProductId { get; set; }
}
