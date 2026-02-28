using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Data.Mapping.Builders;

/// <summary>
/// Represents the associated product record entity builder (schema definition)
/// </summary>
public class AssociatedProductRecordBuilder : NopEntityBuilder<AssociatedProductRecord>
{
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AssociatedProductRecord.ProductId)).AsInt32().NotNullable()
            .WithColumn(nameof(AssociatedProductRecord.AssociatedProductId)).AsInt32().NotNullable()
            .WithColumn(nameof(AssociatedProductRecord.DisplayOrder)).AsInt32().NotNullable();
    }
}
