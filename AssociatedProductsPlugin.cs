using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.AssociatedProducts.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.AssociatedProducts;

/// <summary>
/// Main plugin class — registers widget zones and handles install / uninstall
/// </summary>
public class AssociatedProductsPlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly ISettingService _settingService;
    private readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public AssociatedProductsPlugin(
        ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        ISettingService settingService,
        WidgetSettings widgetSettings)
    {
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _settingService = settingService;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region IWidgetPlugin

    /// <summary>
    /// Widget zones this plugin participates in:
    /// - admin_product_details_block : injects the management card into the product edit page
    /// - productdetails_bottom       : renders the public product carousel
    /// </summary>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            AdminWidgetZones.ProductDetailsBlock,
            PublicWidgetZones.ProductDetailsBottom
        });
    }

    /// <inheritdoc />
    public Type GetWidgetViewComponent(string widgetZone)
    {
        return widgetZone == AdminWidgetZones.ProductDetailsBlock
            ? typeof(AssociatedProductsAdminViewComponent)
            : typeof(AssociatedProductsViewComponent);
    }

    /// <inheritdoc />
    public bool HideInWidgetList => false;

    #endregion

    #region BasePlugin

    public override string GetConfigurationPageUrl()
    {
        return _nopUrlHelper.RouteUrl(AssociatedProductsDefaults.ConfigurationRouteName);
    }

    public override async Task InstallAsync()
    {
        // Save default settings
        await _settingService.SaveSettingAsync(new AssociatedProductsSettings
        {
            NumberOfProductsToDisplay = 4
        });

        // Mark the plugin as active in the widget system
        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(PluginDescriptor.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        // Locale resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            // Configuration page
            ["Plugins.Widgets.AssociatedProducts.NumberOfProductsToDisplay"] =
                "Number of products to display",
            ["Plugins.Widgets.AssociatedProducts.NumberOfProductsToDisplay.Hint"] =
                "Maximum number of associated products shown in the public storefront widget.",

            // Admin grid fields
            ["Plugins.Widgets.AssociatedProducts.Fields.Product"] = "Product",
            ["Plugins.Widgets.AssociatedProducts.Fields.DisplayOrder"] = "Display order",

            // Admin card / tab
            ["Plugins.Widgets.AssociatedProducts.AdminCard.Title"] = "Associated Products",
            ["Plugins.Widgets.AssociatedProducts.AdminCard.Hint"] =
                "Add products that will appear in the 'Associated Products' widget on the product page.",
            ["Plugins.Widgets.AssociatedProducts.AdminCard.AddNew"] = "Add new product",
            ["Plugins.Widgets.AssociatedProducts.AdminCard.SaveBeforeEdit"] =
                "You need to save the product before you can add associated products.",

            // Public widget
            ["Plugins.Widgets.AssociatedProducts.Public.Title"] = "Associated Products",
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        // Remove settings
        await _settingService.DeleteSettingAsync<AssociatedProductsSettings>();

        // Remove from active widgets
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(PluginDescriptor.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        // Remove locale resources
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.AssociatedProducts");

        await base.UninstallAsync();
    }

    #endregion
}
