--SP para registrar las entradas
CREATE PROCEDURE sp_register_entry (
    IN p_id STRING,
    IN p_product_id STRING,
    IN p_quantity INT,
    IN p_created_by STRING
)
LANGUAGE SQL
AS $$

    -- Insertar el movimiento de entrada
    INSERT INTO "InventoryMovements" (
        "Id", "ProductId", "MovementTypeId", "Quantity", "CreatedBy", "Created"
    )
    VALUES (
        p_id,
        p_product_id,
        (SELECT "Id" FROM "MovementTypes" WHERE "Name" = 'Entrada' LIMIT 1),
        p_quantity,
        p_created_by,
        NOW()
    );

    -- Actualizar el inventario si existe
    UPDATE "Inventories"
    SET "Stock" = "Stock" + p_quantity,
        "LastModifiedBy" = p_created_by,
        "LastModified" = NOW()
    WHERE "ProductId" = p_product_id;

    -- Insertar el inventario si no existe
    INSERT INTO "Inventories" ("Id", "ProductId", "Stock", "CreatedBy", "Created")
    SELECT p_id, p_product_id, p_quantity, p_created_by, NOW()
    WHERE NOT EXISTS (
        SELECT 1 FROM "Inventories" WHERE "ProductId" = p_product_id
    );

$$;