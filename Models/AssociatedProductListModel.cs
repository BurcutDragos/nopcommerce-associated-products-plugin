using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.AssociatedProducts.Models;

/// <summary>
/// Paged list model returned to the admin DataTables grid
/// </summary>
public record AssociatedProductListModel : BasePagedListModel<AssociatedProductModel>;
