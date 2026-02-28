using Nop.Core;

namespace Nop.Plugin.Widgets.AssociatedProducts.Domain;

/// <summary>
/// Represents an associated product mapping record.
/// ProductId is the main product; AssociatedProductId is the linked product.
/// </summary>
public class AssociatedProductRecord : BaseEntity
{
    /// <summary>
    /// Gets or sets the main product identifier
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the associated (linked) product identifier
    /// </summary>
    public int AssociatedProductId { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}
