using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Services;

/// <summary>
/// Associated product service contract
/// </summary>
public interface IAssociatedProductService
{
    /// <summary>
    /// Get all association records for a main product
    /// </summary>
    Task<IList<AssociatedProductRecord>> GetAssociatedProductsByProductIdAsync(int productId, bool showHidden = false);

    /// <summary>
    /// Get a single record by its identifier
    /// </summary>
    Task<AssociatedProductRecord> GetAssociatedProductByIdAsync(int id);

    /// <summary>
    /// Insert a new association record
    /// </summary>
    Task InsertAssociatedProductAsync(AssociatedProductRecord record);

    /// <summary>
    /// Update an existing association record
    /// </summary>
    Task UpdateAssociatedProductAsync(AssociatedProductRecord record);

    /// <summary>
    /// Delete an association record
    /// </summary>
    Task DeleteAssociatedProductAsync(AssociatedProductRecord record);

    /// <summary>
    /// Check whether an association already exists in the given list
    /// </summary>
    AssociatedProductRecord FindAssociatedProduct(IList<AssociatedProductRecord> source, int productId, int associatedProductId);
}
