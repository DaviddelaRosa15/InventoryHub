--SP para obtener la información de inventario de un producto
CREATE OR REPLACE FUNCTION fn_get_inventory_info(p_product_id STRING)
RETURNS TABLE(MinimumStock INT, CurrentStock INT)
LANGUAGE SQL
AS $$
    SELECT
        COALESCE(p."MinimumStock", 0) AS MinimumStock,
        COALESCE(i."Stock", 0) AS CurrentStock
    FROM "Products" p
    LEFT JOIN "Inventories" i ON i."ProductId" = p."Id"
    WHERE p."Id" = p_product_id;
$$;
