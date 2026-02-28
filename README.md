# Associated Products Plugin for nopCommerce

A nopCommerce widget plugin that displays a curated list of associated products on the storefront product page. Associations are configured per-product from the admin panel, with support for display order, store scope settings, and product visibility filters.

---

## 📱 Features

- **Admin card on product edit page** — manage associated products directly from the product editing page in the admin panel
- **Product search popup** — search and select products to associate using filters (category, manufacturer, store, vendor, product type)
- **Display order** — control the order in which associated products appear
- **Plugin configuration page** — set the maximum number of associated products to display, with per-store override support
- **Public storefront widget** — associated products are rendered at the bottom of the product detail page using the standard `_ProductBox` partial (image, name, price, add-to-cart)
- **Smart visibility filtering** — only shows products that are published, not deleted, visible individually, and accessible to the current customer (ACL + store mapping)
- **Caching** — results are cached and automatically invalidated on any create/update/delete operation

---

## ✅ Requirements

| Requirement | Version |
|---|---|
| nopCommerce | 5.x |
| .NET | 9.0 |
| Database | MySQL or SQL Server |

---

## 🚀 Installation

1. Clone or download this repository
2. Copy the plugin folder into `src/Plugins/` inside your nopCommerce source
3. Add the project to your `NopCommerce.sln` solution file
4. Build the solution
5. Start nopCommerce and go to **Admin → Configuration → Plugins**
6. Find **Widgets.AssociatedProducts** and click **Install**
7. Restart the application when prompted

---

## 🚀 Configuration

### Plugin settings
Go to **Admin → Configuration → Plugins → Widgets.AssociatedProducts → Configure**

| Setting | Description | Default |
|---|---|---|
| Number of products to display | Maximum number of associated products shown on the storefront | 4 |

This setting supports per-store overrides in multi-store installations.

### Managing associations
1. Go to **Admin → Catalog → Products** and open any product for editing
2. Scroll down to the **Associated Products** card
3. Click **Add new product** to open the product search popup
4. Select one or more products and click **Save**
5. Use the **Display order** column to control the sort order
6. Use the **Delete** button to remove an association

---

## 🚀 How It Works

```
Admin sets associations
        ↓
Saved to AP_AssociatedProduct table
        ↓
Customer visits a product page
        ↓
Plugin loads associations for that product
        ↓
Filters by: Published, Not deleted, Visible individually, ACL, Store mapping
        ↓
Renders up to N products using _ProductBox partial
        ↓
Widget appears at the bottom of the product page
```

---

## ✅ Project Structure

```
Nop.Plugin.Widgets.AssociatedProducts/
├── Components/
│   ├── AssociatedProductsAdminViewComponent.cs   # Admin card (product edit page)
│   └── AssociatedProductsViewComponent.cs        # Public storefront widget
├── Controllers/
│   └── AssociatedProductsController.cs           # Configure + CRUD + popup actions
├── Data/
│   ├── Mapping/
│   │   ├── Builders/
│   │   │   └── AssociatedProductRecordBuilder.cs # Table schema definition
│   │   └── AssociatedProductsNameCompatibility.cs# Custom table name mapping
│   └── Migrations/
│       └── SchemaMigration.cs                    # FluentMigrator migration
├── Domain/
│   └── AssociatedProductRecord.cs                # Entity (ProductId, AssociatedProductId, DisplayOrder)
├── Infrastructure/
│   ├── Cache/
│   │   └── AssociatedProductCacheEventConsumer.cs# Auto cache invalidation
│   ├── NopStartup.cs                             # DI service registration
│   └── RouteProvider.cs                          # Explicit route registration
├── Models/                                       # View models for admin and public UI
├── Services/
│   ├── IAssociatedProductService.cs              # Service interface
│   └── AssociatedProductService.cs               # Service implementation with caching
├── Views/
│   ├── Configure.cshtml                          # Plugin configuration page
│   ├── AssociatedProductAddPopup.cshtml          # Product selection popup
│   ├── Shared/Components/
│   │   ├── AssociatedProducts/Default.cshtml     # Public widget view
│   │   └── AssociatedProductsAdmin/Default.cshtml# Admin card view
│   └── _ViewImports.cshtml
├── AssociatedProductsDefaults.cs                 # Constants and cache keys
├── AssociatedProductsPlugin.cs                   # Main plugin class
├── AssociatedProductsSettings.cs                 # Plugin settings
├── Nop.Plugin.Widgets.AssociatedProducts.csproj
└── plugin.json
```

---

## 📱 Widget Zones

| Zone | Constant | Location |
|---|---|---|
| `admin_product_details_block` | `AdminWidgetZones.ProductDetailsBlock` | Admin product edit page |
| `productdetails_bottom` | `PublicWidgetZones.ProductDetailsBottom` | Public product detail page |

---

## 📱 Database

The plugin creates a single table on installation:

**`AP_AssociatedProduct`**

| Column | Type | Description |
|---|---|---|
| `Id` | int | Primary key |
| `ProductId` | int | The main product |
| `AssociatedProductId` | int | The associated product |
| `DisplayOrder` | int | Sort order |

The table is automatically removed when the plugin is uninstalled.

## 🤝 Contributing:
1. Fork the repository.
2. Create a new branch: `git checkout -b my-feature-branch`
3. Make your changes and commit them: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin my-feature-branch`
5. Submit a pull request.

## 🧑‍💻 Author(s):
Burcut Ioan Dragos.

## 💡 Acknowledgments:
Thanks to Anthropic (Claude AI) for providing assistance in the development of this project.

## 📄 License
This project is licensed under the terms found in the [LICENSE](LICENSE) file.
