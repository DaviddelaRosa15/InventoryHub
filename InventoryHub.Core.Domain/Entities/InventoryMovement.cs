using InventoryHub.Core.Domain.Common;

namespace InventoryHub.Core.Domain.Entities
{
    public class InventoryMovement : AuditableBaseEntity
    {
        public int Quantity { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public string ProductId { get; set; }
        public MovementType MovementType { get; set; }
        public string MovementTypeId { get; set; }
    }
}
