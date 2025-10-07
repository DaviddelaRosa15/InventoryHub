CREATE OR REPLACE PROCEDURE sp_product_update(
    p_id TEXT,
    p_name TEXT,
    p_description TEXT,
    p_saleprice NUMERIC,
    p_lastmodifiedby TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE "Products"
    SET
        "Name" = p_name,
        "Description" = p_description,
        "SalePrice" = p_saleprice,
        "LastModified" = NOW(),
        "LastModifiedBy" = p_lastmodifiedby
    WHERE "Id" = p_id;
END;
$$;