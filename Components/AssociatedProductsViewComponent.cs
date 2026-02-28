using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using Nop.Plugin.Widgets.AssociatedProducts.Services;

namespace Nop.Plugin.Widgets.AssociatedProducts.Components;

/// <summary>
/// Public view component rendered in <c>PublicWidgetZones.ProductDetailsBottom</c>.
/// Displays the associated products grid on the storefront product page.
/// </summary>
[ViewComponent(Name = AssociatedProductsDefaults.PublicViewComponentName)]
public class AssociatedProductsViewComponent : NopViewComponent
{
    #region Fields

    private readonly IAclService _aclService;
    private readonly IAssociatedProductService _associatedProductService;
    private readonly IProductModelFactory _productModelFactory;
    private readonly IProductService _productService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public AssociatedProductsViewComponent(
        IAclService aclService,
        IAssociatedProductService associatedProductService,
        IProductModelFactory productModelFactory,
        IProductService productService,
        ISettingService settingService,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService)
    {
        _aclService = aclService;
        _associatedProductService = associatedProductService;
        _productModelFactory = productModelFactory;
        _productService = productService;
        _settingService = settingService;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Methods

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        // additionalData on ProductDetailsBottom is the public ProductDetailsModel
        if (additionalData is not ProductDetailsModel detailsModel)
            return Content(string.Empty);

        // Load settings respecting the current store scope
        var storeId = (await _storeContext.GetCurrentStoreAsync()).Id;
        var settings = await _settingService.LoadSettingAsync<AssociatedProductsSettings>(storeId);

        var productId = detailsModel.Id;

        // Load association records ordered by DisplayOrder
        var records = await _associatedProductService.GetAssociatedProductsByProductIdAsync(productId);

        if (!records.Any())
            return Content(string.Empty);

        // Resolve full Product entities, apply visibility filters
        var associatedIds = records.Select(r => r.AssociatedProductId).ToArray();
        var allProducts = await _productService.GetProductsByIdsAsync(associatedIds);

        var filteredProducts = new List<Nop.Core.Domain.Catalog.Product>();
        foreach (var p in allProducts)
        {
            if (p.Deleted || !p.Published || !p.VisibleIndividually)
                continue;

            if (await _aclService.AuthorizeAsync(p) &&
                await _storeMappingService.AuthorizeAsync(p) &&
                _productService.ProductIsAvailable(p))
            {
                filteredProducts.Add(p);
            }
        }

        if (!filteredProducts.Any())
            return Content(string.Empty);

        // Respect the configured limit (default 4)
        var limit = settings.NumberOfProductsToDisplay > 0 ? settings.NumberOfProductsToDisplay : 4;
        var displayed = filteredProducts.Take(limit).ToList();

        var model = (await _productModelFactory.PrepareProductOverviewModelsAsync(displayed, true, true)).ToList();

        return View(
            "~/Plugins/Widgets.AssociatedProducts/Views/Shared/Components/AssociatedProducts/Default.cshtml",
            model);
    }

    #endregion
}
