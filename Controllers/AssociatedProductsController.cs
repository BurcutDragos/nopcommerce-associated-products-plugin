using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;
using Nop.Plugin.Widgets.AssociatedProducts.Models;
using Nop.Plugin.Widgets.AssociatedProducts.Services;
using AdminCatalog = Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Widgets.AssociatedProducts.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class AssociatedProductsController : BasePluginController
{
    #region Fields

    private readonly IAssociatedProductService _associatedProductService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IPermissionService _permissionService;
    private readonly IProductService _productService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;

    #endregion

    #region Ctor

    public AssociatedProductsController(
        IAssociatedProductService associatedProductService,
        IBaseAdminModelFactory baseAdminModelFactory,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        IProductService productService,
        ISettingService settingService,
        IStoreContext storeContext,
        IWorkContext workContext,
        IUrlRecordService urlRecordService)
    {
        _associatedProductService = associatedProductService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _productService = productService;
        _settingService = settingService;
        _storeContext = storeContext;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
    }

    #endregion

    #region Configuration

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<AssociatedProductsSettings>(storeScope);

        var model = new ConfigurationModel
        {
            NumberOfProductsToDisplay = settings.NumberOfProductsToDisplay,
            ActiveStoreScopeConfiguration = storeScope
        };

        if (storeScope > 0)
        {
            model.NumberOfProductsToDisplay_OverrideForStore =
                await _settingService.SettingExistsAsync(settings, x => x.NumberOfProductsToDisplay, storeScope);
        }

        return View("~/Plugins/Widgets.AssociatedProducts/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<AssociatedProductsSettings>(storeScope);

        settings.NumberOfProductsToDisplay = model.NumberOfProductsToDisplay;

        await _settingService.SaveSettingOverridablePerStoreAsync(
            settings, x => x.NumberOfProductsToDisplay,
            model.NumberOfProductsToDisplay_OverrideForStore, storeScope, false);

        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion

    #region Associated product CRUD (called from the admin DataTables grid)

    [HttpPost]
    [CheckPermission(StandardPermission.Catalog.PRODUCTS_VIEW)]
    public async Task<IActionResult> AssociatedProductList(AssociatedProductSearchModel searchModel)
    {
        var product = await _productService.GetProductByIdAsync(searchModel.ProductId)
            ?? throw new ArgumentException("No product found with the specified id");

        var currentVendor = await _workContext.GetCurrentVendorAsync();
        if (currentVendor != null && product.VendorId != currentVendor.Id)
            return Content("This is not your product");

        var records = (await _associatedProductService.GetAssociatedProductsByProductIdAsync(
            product.Id, showHidden: true)).ToPagedList(searchModel);

        var model = await new AssociatedProductListModel().PrepareToGridAsync(searchModel, records, () =>
        {
            return records.SelectAwait(async record =>
            {
                var rowModel = new AssociatedProductModel
                {
                    Id = record.Id,
                    AssociatedProductId = record.AssociatedProductId,
                    DisplayOrder = record.DisplayOrder,
                    AssociatedProductName =
                        (await _productService.GetProductByIdAsync(record.AssociatedProductId))?.Name ?? string.Empty
                };
                return rowModel;
            });
        });

        return Json(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> AssociatedProductUpdate(AssociatedProductModel model)
    {
        var record = await _associatedProductService.GetAssociatedProductByIdAsync(model.Id)
            ?? throw new ArgumentException("No associated product record found with the specified id");

        var currentVendor = await _workContext.GetCurrentVendorAsync();
        if (currentVendor != null)
        {
            var product = await _productService.GetProductByIdAsync(record.ProductId);
            if (product != null && product.VendorId != currentVendor.Id)
                return Content("This is not your product");
        }

        record.DisplayOrder = model.DisplayOrder;
        await _associatedProductService.UpdateAssociatedProductAsync(record);

        return new NullJsonResult();
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> AssociatedProductDelete(int id)
    {
        var record = await _associatedProductService.GetAssociatedProductByIdAsync(id)
            ?? throw new ArgumentException("No associated product record found with the specified id");

        var currentVendor = await _workContext.GetCurrentVendorAsync();
        if (currentVendor != null)
        {
            var product = await _productService.GetProductByIdAsync(record.ProductId);
            if (product != null && product.VendorId != currentVendor.Id)
                return Content("This is not your product");
        }

        await _associatedProductService.DeleteAssociatedProductAsync(record);

        return new NullJsonResult();
    }

    #endregion

    #region Add-product popup

    [CheckPermission(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> AssociatedProductAddPopup(int productId)
    {
        var searchModel = new AddAssociatedProductSearchModel();
        searchModel.IsLoggedInAsVendor = await _workContext.GetCurrentVendorAsync() != null;

        await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);
        await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);
        await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);
        await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);
        await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);
        searchModel.SetPopupGridPageSize();

        return View("~/Plugins/Widgets.AssociatedProducts/Views/AssociatedProductAddPopup.cshtml", searchModel);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> AssociatedProductAddPopupList(AddAssociatedProductSearchModel searchModel)
    {
        var currentVendor = await _workContext.GetCurrentVendorAsync();
        if (currentVendor != null)
            searchModel.SearchVendorId = currentVendor.Id;

        var products = await _productService.SearchProductsAsync(
            showHidden: true,
            categoryIds: new List<int> { searchModel.SearchCategoryId },
            manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
            storeId: searchModel.SearchStoreId,
            vendorId: searchModel.SearchVendorId,
            productType: searchModel.SearchProductTypeId > 0
                ? (ProductType?)searchModel.SearchProductTypeId
                : null,
            keywords: searchModel.SearchProductName,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new AddAssociatedProductListModel().PrepareToGridAsync(searchModel, products, () =>
        {
            return products.SelectAwait(async product =>
            {
                var productModel = new AdminCatalog.ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Published = product.Published
                };
                productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);
                return productModel;
            });
        });

        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    [CheckPermission(StandardPermission.Catalog.PRODUCTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> AssociatedProductAddPopup(AddAssociatedProductModel model)
    {
        var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());

        if (selectedProducts.Any())
        {
            var existing = await _associatedProductService.GetAssociatedProductsByProductIdAsync(
                model.ProductId, showHidden: true);

            var currentVendor = await _workContext.GetCurrentVendorAsync();

            foreach (var product in selectedProducts)
            {
                if (currentVendor != null && product.VendorId != currentVendor.Id)
                    continue;

                if (_associatedProductService.FindAssociatedProduct(existing, model.ProductId, product.Id) != null)
                    continue;

                await _associatedProductService.InsertAssociatedProductAsync(new AssociatedProductRecord
                {
                    ProductId = model.ProductId,
                    AssociatedProductId = product.Id,
                    DisplayOrder = 1
                });
            }
        }

        ViewBag.RefreshPage = true;

        var emptySearch = new AddAssociatedProductSearchModel();
        emptySearch.SetPopupGridPageSize();
        return View("~/Plugins/Widgets.AssociatedProducts/Views/AssociatedProductAddPopup.cshtml", emptySearch);
    }

    #endregion
}
