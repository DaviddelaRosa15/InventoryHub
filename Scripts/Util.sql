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