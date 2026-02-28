using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.AssociatedProducts;

/// <summary>
/// Persisted settings for the Associated Products widget plugin
/// </summary>
public class AssociatedProductsSettings : ISettings
{
    /// <summary>
    /// Maximum number of associated products to display in the public widget
    /// </summary>
    public int NumberOfProductsToDisplay { get; set; } = 4;
}
