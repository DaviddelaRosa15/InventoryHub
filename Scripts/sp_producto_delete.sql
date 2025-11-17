CREATE OR REPLACE PROCEDURE sp_product_delete(
    p_id TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Marcar el producto como eliminado
    UPDATE "Products"
    SET "IsDeleted" = TRUE,
        "DeletedAt" = NOW(),
        "LastModified" = NOW()
    WHERE "Id" = p_id;

    -- Marcar los inventarios asociados como eliminados
    UPDATE "Inventories"
    SET "IsDeleted" = TRUE,
        "DeletedAt" = NOW(),
        "LastModified" = NOW()
    WHERE "ProductId" = p_id;
END;
$$;