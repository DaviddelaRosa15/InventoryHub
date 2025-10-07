CREATE OR REPLACE FUNCTION fn_product_get_all()
RETURNS TABLE (
    "Id" INTEGER,
    "Name" TEXT,
    "Description" TEXT,
    "SalePrice" NUMERIC,
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
        "Created"::TIMESTAMP,          -- Cast explícito si la columna es TEXT
        "CreatedBy",
        "LastModified"::TIMESTAMP,     -- Cast explícito si la columna es TEXT
        "LastModifiedBy"
    FROM "Products";
END;
$$;