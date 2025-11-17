-- Products: agregar MinimumStock (tipo entero, NOT NULL, por defecto 0)
ALTER TABLE "Products"
ADD COLUMN "MinimumStock" integer NOT NULL DEFAULT 0;

--Insertado los datos para pruebas
INSERT INTO "MovementTypes" ("Id", "Name", "CreatedBy", "Created")
VALUES 
('movement-1', 'Entrada', 'admin', NOW());

INSERT INTO "MovementTypes" ("Id", "Name", "CreatedBy", "Created")
VALUES 
('movement-2', 'Salida', 'admin', NOW());

--Practica 6

-- Agregar IsDeleted y DeletedAt a las 4 tablas del diagrama
ALTER TABLE "InventoryMovements"
    ADD COLUMN "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN "DeletedAt" TIMESTAMP WITH TIME ZONE;

ALTER TABLE "MovementTypes"
    ADD COLUMN "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN "DeletedAt" TIMESTAMP WITH TIME ZONE;

ALTER TABLE "Inventories"
    ADD COLUMN "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN "DeletedAt" TIMESTAMP WITH TIME ZONE;

ALTER TABLE "Products"
    ADD COLUMN "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    ADD COLUMN "DeletedAt" TIMESTAMP WITH TIME ZONE;

-- Crear un índice para el campo IsDeleted en todas tus tablas principales

CREATE INDEX IDX_InventoryMovements_IsDeleted ON "InventoryMovements" ("IsDeleted");
CREATE INDEX IDX_MovementTypes_IsDeleted ON "MovementTypes" ("IsDeleted");
CREATE INDEX IDX_Inventories_IsDeleted ON "Inventories" ("IsDeleted");
CREATE INDEX IDX_Products_IsDeleted ON "Products" ("IsDeleted");

-- Eliminar la constraint anterior
ALTER TABLE "InventoryMovements" DROP CONSTRAINT "FK_InventoryMovements_MovementTypes_MovementTypeId";
ALTER TABLE "InventoryMovements" DROP CONSTRAINT "FK_InventoryMovements_Products_ProductId";

-- Crear la nueva constraint con ON DELETE NO ACTION
ALTER TABLE "InventoryMovements"
ADD CONSTRAINT "FK_InventoryMovements_MovementTypes_MovementTypeId"
    FOREIGN KEY ("MovementTypeId")
    REFERENCES "MovementTypes"("Id")
    ON DELETE NO ACTION;

ALTER TABLE "InventoryMovements"
ADD CONSTRAINT "FK_InventoryMovements_Products_ProductId"
    FOREIGN KEY ("ProductId")
    REFERENCES "Products"("Id")
    ON DELETE NO ACTION;