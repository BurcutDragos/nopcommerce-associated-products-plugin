using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.AssociatedProducts.Domain;

namespace Nop.Plugin.Widgets.AssociatedProducts.Data.Migrations;

[NopMigration("2025-01-01 00:00:00", "Widgets.AssociatedProducts schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    /// <summary>
    /// Collect the UP migration expressions — creates the AP_AssociatedProduct table
    /// </summary>
    public override void Up()
    {
        this.CreateTableIfNotExists<AssociatedProductRecord>();
    }
}
