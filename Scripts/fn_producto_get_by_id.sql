CREATE OR REPLACE FUNCTION fn_product_get_by_id(
    p_id TEXT
)
RETURNS TABLE (
    "Id" TEXT,
    "Name" TEXT,
    "Description" TEXT,
    "SalePrice" NUMERIC,
    "MinimumStock" INT,
    "Created" TIMESTAMP,
    "CreatedBy" TEXT,
    "LastModified" TIMESTAMP,
    "LastModifiedBy" TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        "Id",
        "Name",
        "Description",
        "SalePrice",
        "MinimumStock" INT,
        "Created"::TIMESTAMP,          -- Cast explícito si la columna es TEXT
        "CreatedBy",
        "LastModified"::TIMESTAMP,     -- Cast explícito si la columna es TEXT
        "LastModifiedBy"
    FROM "Products"
    WHERE "Id" = p_id;
END;
$$;