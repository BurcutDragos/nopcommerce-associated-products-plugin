using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.AssociatedProducts;

/// <summary>
/// Plugin-wide constants and cache keys
/// </summary>
public static class AssociatedProductsDefaults
{
    /// <summary>
    /// Plugin system name
    /// </summary>
    public const string SystemName = "Widgets.AssociatedProducts";

    /// <summary>
    /// Cache key for associated products by main product id
    /// {0} : main product id
    /// {1} : show hidden records?
    /// </summary>
    public static CacheKey AssociatedProductsCacheKey =>
        new("Nop.plugin.associatedproducts.byproduct.{0}-{1}");

    /// <summary>
    /// Cache key prefix — used to invalidate entries for a specific product
    /// {0} : main product id
    /// </summary>
    public const string AssociatedProductsPrefix = "Nop.plugin.associatedproducts.byproduct.{0}";

    /// <summary>
    /// Admin widget zone component name
    /// </summary>
    public const string AdminViewComponentName = "AssociatedProductsAdmin";

    /// <summary>
    /// Public widget zone component name
    /// </summary>
    public const string PublicViewComponentName = "AssociatedProducts";

    /// <summary>
    /// Route name for the configuration page
    /// </summary>
    public const string ConfigurationRouteName = "Plugin.Widgets.AssociatedProducts.Configure";
}
