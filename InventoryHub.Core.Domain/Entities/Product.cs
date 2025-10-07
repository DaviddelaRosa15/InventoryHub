using InventoryHub.Core.Domain.Common;

namespace InventoryHub.Core.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double SalePrice { get; set; }

        // Navigation properties
        public Inventory Inventory { get; set; }
        public List<InventoryMovement> InventoryMovements { get; set; }
    }
}
