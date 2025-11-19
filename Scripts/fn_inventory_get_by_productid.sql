CREATE OR REPLACE FUNCTION fn_inventory_get_by_productid(
    p_productid TEXT
)
RETURNS TABLE (
    "Id" TEXT,
    "Stock" INT,
    "ProductId" TEXT,
    "CreatedBy" TEXT,
    "Created" TIMESTAMP,
    "LastModifiedBy" TEXT,
    "LastModified" TIMESTAMP,
    "IsDeleted" BOOLEAN,
    "DeletedAt" TIMESTAMP
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        "Id",
        "Stock",
        "ProductId",
        "CreatedBy",
        "Created"::TIMESTAMP,
        "LastModifiedBy",
        "LastModified"::TIMESTAMP,
        "IsDeleted",
        "DeletedAt"::TIMESTAMP
    FROM "Inventories"
    WHERE "ProductId" = p_productid AND "IsDeleted" = FALSE;
END;
$$;