using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Payload sent from the add-product popup to save selected products
/// </summary>
public record AddAssociatedProductModel : BaseNopModel
{
    public AddAssociatedProductModel()
    {
        SelectedProductIds = new List<int>();
    }

    /// <summary>
    /// The main product to which associations are being added
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Products selected in the popup grid
    /// </summary>
    public IList<int> SelectedProductIds { get; set; }
}
