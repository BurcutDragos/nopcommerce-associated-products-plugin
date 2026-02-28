using Nop.Data.Mapping;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Data.Mapping;

/// <summary>
/// Plugin table naming compatibility — maps domain type to its database table name
/// </summary>
public class AssociatedProductsNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {
        [typeof(AssociatedProductRecord)] = "AP_AssociatedProduct"
    };

    public Dictionary<(Type, string), string> ColumnName => [];
}
