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
END;
$$;