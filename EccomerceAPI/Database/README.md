Scaffolding

dotnet ef dbcontext scaffold "Server=tcp:simpleapi.database.windows.net,1433;Database=simple_factura_ecommerce;User id=busti;password=O9imoyax!2;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force --context contextApp --table dbo.EMISOR --table dbo.PRODUCTO --table dbo.DTE_EMITIDO --table ecommerce.PLANTILLA --table ecommerce.TIENDA --table ecommerce.DOMINIO --table ecommerce.CATEGORIA --table ecommerce.PRODUCTO_CATEGORIA --table ecommerce.CLIENTE_TIENDA --table ecommerce.DIRECCION_CLIENTE --table ecommerce.ORDEN_ESTADO --table ecommerce.ORDEN --table ecommerce.ORDEN_ITEM
#PROD
