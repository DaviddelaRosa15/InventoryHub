CREATE OR REPLACE PROCEDURE sp_product_delete(
    p_id TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM "Products"
    WHERE "Id" = p_id;
END;
$$;