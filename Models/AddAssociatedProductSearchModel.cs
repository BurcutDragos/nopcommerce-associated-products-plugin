using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Search parameters used in the add-product popup grid
/// </summary>
public record AddAssociatedProductSearchModel : BaseSearchModel
{
    public AddAssociatedProductSearchModel()
    {
        AvailableCategories = new List<SelectListItem>();
        AvailableManufacturers = new List<SelectListItem>();
        AvailableStores = new List<SelectListItem>();
        AvailableVendors = new List<SelectListItem>();
        AvailableProductTypes = new List<SelectListItem>();
    }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
    public string SearchProductName { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
    public int SearchCategoryId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
    public int SearchManufacturerId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
    public int SearchStoreId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
    public int SearchVendorId { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
    public int SearchProductTypeId { get; set; }

    public IList<SelectListItem> AvailableCategories { get; set; }
    public IList<SelectListItem> AvailableManufacturers { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }
    public IList<SelectListItem> AvailableVendors { get; set; }
    public IList<SelectListItem> AvailableProductTypes { get; set; }

    public bool IsLoggedInAsVendor { get; set; }
}
