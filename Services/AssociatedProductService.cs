using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Services;

/// <summary>
/// Default implementation of <see cref="IAssociatedProductService"/>
/// </summary>
public class AssociatedProductService : IAssociatedProductService
{
    #region Fields

    private readonly IRepository<AssociatedProductRecord> _associatedProductRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public AssociatedProductService(
        IRepository<AssociatedProductRecord> associatedProductRepository,
        IRepository<Product> productRepository,
        IStaticCacheManager staticCacheManager)
    {
        _associatedProductRepository = associatedProductRepository;
        _productRepository = productRepository;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public virtual async Task<IList<AssociatedProductRecord>> GetAssociatedProductsByProductIdAsync(
        int productId, bool showHidden = false)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            AssociatedProductsDefaults.AssociatedProductsCacheKey, productId, showHidden);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var query = from ap in _associatedProductRepository.Table
                        join p in _productRepository.Table on ap.AssociatedProductId equals p.Id
                        where ap.ProductId == productId &&
                              !p.Deleted &&
                              (showHidden || p.Published)
                        orderby ap.DisplayOrder, ap.Id
                        select ap;

            return await query.ToListAsync();
        });
    }

    /// <inheritdoc />
    public virtual async Task<AssociatedProductRecord> GetAssociatedProductByIdAsync(int id)
    {
        return await _associatedProductRepository.GetByIdAsync(id, cache => default);
    }

    /// <inheritdoc />
    public virtual async Task InsertAssociatedProductAsync(AssociatedProductRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        await _associatedProductRepository.InsertAsync(record);
    }

    /// <inheritdoc />
    public virtual async Task UpdateAssociatedProductAsync(AssociatedProductRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        await _associatedProductRepository.UpdateAsync(record);
    }

    /// <inheritdoc />
    public virtual async Task DeleteAssociatedProductAsync(AssociatedProductRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        await _associatedProductRepository.DeleteAsync(record);
    }

    /// <inheritdoc />
    public virtual AssociatedProductRecord FindAssociatedProduct(
        IList<AssociatedProductRecord> source, int productId, int associatedProductId)
    {
        return source.FirstOrDefault(ap =>
            ap.ProductId == productId && ap.AssociatedProductId == associatedProductId);
    }

    #endregion
}
