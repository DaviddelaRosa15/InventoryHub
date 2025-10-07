CREATE OR REPLACE PROCEDURE sp_product_create(
    p_id TEXT,
    p_name TEXT,
    p_description TEXT,
    p_saleprice NUMERIC,
    p_createdby TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO "Products" ("Id", "Name", "Description", "SalePrice", "Created", "CreatedBy")
    VALUES (
        p_id,
        p_name,
        p_description,
        p_saleprice,
        NOW(),
        p_createdby
    );
END;
$$;