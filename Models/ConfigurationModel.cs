using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Model for the plugin global configuration page
/// </summary>
public record ConfigurationModel : BaseNopModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.AssociatedProducts.NumberOfProductsToDisplay")]
    [Range(1, 100)]
    public int NumberOfProductsToDisplay { get; set; }

    public bool NumberOfProductsToDisplay_OverrideForStore { get; set; }
}
