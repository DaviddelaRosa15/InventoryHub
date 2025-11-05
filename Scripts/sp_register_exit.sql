--SP para registrar salidas
CREATE OR REPLACE PROCEDURE sp_register_exit (
    IN p_id STRING,
    IN p_product_id STRING,
    IN p_quantity INT,
    IN p_created_by STRING
)
LANGUAGE SQL
AS $$
    -- Registrar movimiento de salida
    INSERT INTO "InventoryMovements" (
        "Id", "ProductId", "MovementTypeId", "Quantity", "CreatedBy", "Created"
    )
    SELECT
        p_id,
        p_product_id,
        (SELECT "Id" FROM "MovementTypes" WHERE "Name" = 'Salida' LIMIT 1),
        p_quantity,
        p_created_by,
        NOW();

    -- Descontar el inventario
    UPDATE "Inventories"
    SET "Stock" = "Stock" - p_quantity,
        "LastModifiedBy" = p_created_by,
        "LastModified" = NOW()
    WHERE "ProductId" = p_product_id;
$$;
