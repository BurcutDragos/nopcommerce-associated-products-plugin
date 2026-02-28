using Nop.Services.Caching;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Infrastructure.Cache;

/// <summary>
/// Clears the associated-products cache whenever an <see cref="AssociatedProductRecord"/>
/// is inserted, updated, or deleted.
/// </summary>
public class AssociatedProductCacheEventConsumer : CacheEventConsumer<AssociatedProductRecord>
{
    protected override async Task ClearCacheAsync(AssociatedProductRecord entity)
    {
        await RemoveByPrefixAsync(AssociatedProductsDefaults.AssociatedProductsPrefix, entity.ProductId);
    }
}
