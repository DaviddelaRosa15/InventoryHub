using InventoryHub.Core.Domain.Common;

namespace InventoryHub.Core.Domain.Entities
{
    public class Inventory : AuditableBaseEntity
    {
        public int Stock { get; set; }

        // Navigation property
        public Product Product { get; set; }
        public string ProductId { get; set; }
    }
}
