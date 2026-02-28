using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Paged list of products shown inside the add-product popup
/// </summary>
public record AddAssociatedProductListModel : BasePagedListModel<Nop.Web.Areas.Admin.Models.Catalog.ProductModel>;
