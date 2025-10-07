using InventoryHub.Core.Domain.Common;

namespace InventoryHub.Core.Domain.Entities
{
    public class MovementType : AuditableBaseEntity
    {
        public string Name { get; set; }

        // Navigation property
        public List<InventoryMovement> InventoryMovements { get; set; }
    }
}
