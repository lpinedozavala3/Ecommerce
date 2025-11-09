using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Models
{
    public partial class contextApp : DbContext
    {
        public contextApp()
        {
        }

        public contextApp(DbContextOptions<contextApp> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorium> Categoria { get; set; } = null!;
        public virtual DbSet<ClienteTiendum> ClienteTienda { get; set; } = null!;
        public virtual DbSet<DireccionCliente> DireccionClientes { get; set; } = null!;
        public virtual DbSet<Dominio> Dominios { get; set; } = null!;
        public virtual DbSet<DteEmitido> DteEmitidos { get; set; } = null!;
        public virtual DbSet<Emisor> Emisors { get; set; } = null!;
        public virtual DbSet<Orden> Ordens { get; set; } = null!;
        public virtual DbSet<OrdenEstado> OrdenEstados { get; set; } = null!;
        public virtual DbSet<OrdenItem> OrdenItems { get; set; } = null!;
        public virtual DbSet<Plantilla> Plantillas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Tiendum> Tienda { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:simpleapi.database.windows.net,1433;Database=simple_factura_ecommerce;User id=busti;password=O9imoyax!2;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__CATEGORI__4BD51FA511B785EE");

                entity.ToTable("CATEGORIA", "ecommerce");

                entity.HasIndex(e => new { e.IdEmisor, e.SlugCategoria }, "UX_CATEGORIA_EMISOR_SLUG")
                    .IsUnique()
                    .HasFilter("([SLUG_CATEGORIA] IS NOT NULL)");

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("ID_CATEGORIA")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.IdCategoriaPadre).HasColumnName("ID_CATEGORIA_PADRE");

                entity.Property(e => e.IdEmisor).HasColumnName("ID_EMISOR");

                entity.Property(e => e.NombreCategoria)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_CATEGORIA");

                entity.Property(e => e.SlugCategoria)
                    .HasMaxLength(140)
                    .IsUnicode(false)
                    .HasColumnName("SLUG_CATEGORIA");

                entity.HasOne(d => d.IdCategoriaPadreNavigation)
                    .WithMany(p => p.InverseIdCategoriaPadreNavigation)
                    .HasForeignKey(d => d.IdCategoriaPadre)
                    .HasConstraintName("FK_CATEGORIA_PADRE");

                entity.HasOne(d => d.IdEmisorNavigation)
                    .WithMany(p => p.Categoria)
                    .HasForeignKey(d => d.IdEmisor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORIA_EMISOR");
            });

            modelBuilder.Entity<ClienteTiendum>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PK__CLIENTE___23A34130316EC17B");

                entity.ToTable("CLIENTE_TIENDA", "ecommerce");

                entity.HasIndex(e => new { e.IdTienda, e.Email }, "UX_CLIENTE_TIENDA_EMAIL")
                    .IsUnique();

                entity.Property(e => e.IdCliente)
                    .HasColumnName("ID_CLIENTE")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("CREADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.HashPassword)
                    .HasMaxLength(256)
                    .HasColumnName("HASH_PASSWORD");

                entity.Property(e => e.IdTienda).HasColumnName("ID_TIENDA");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO");

                entity.Property(e => e.UltimoLoginEn).HasColumnName("ULTIMO_LOGIN_EN");

                entity.HasOne(d => d.IdTiendaNavigation)
                    .WithMany(p => p.ClienteTienda)
                    .HasForeignKey(d => d.IdTienda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENTE_TIENDA");
            });

            modelBuilder.Entity<DireccionCliente>(entity =>
            {
                entity.HasKey(e => e.IdDireccion)
                    .HasName("PK__DIRECCIO__FC7E9E8E10C06FA2");

                entity.ToTable("DIRECCION_CLIENTE", "ecommerce");

                entity.HasIndex(e => e.IdCliente, "UX_DIRECCION_PRINCIPAL")
                    .IsUnique()
                    .HasFilter("([ES_PRINCIPAL]=(1))");

                entity.Property(e => e.IdDireccion)
                    .HasColumnName("ID_DIRECCION")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActualizadoEn).HasColumnName("ACTUALIZADO_EN");

                entity.Property(e => e.Calle)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CALLE");

                entity.Property(e => e.Ciudad)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("CIUDAD");

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_POSTAL");

                entity.Property(e => e.Comuna)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("COMUNA");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("CREADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Depto)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEPTO");

                entity.Property(e => e.EsPrincipal).HasColumnName("ES_PRINCIPAL");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.Numero)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NUMERO");

                entity.Property(e => e.Pais)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("PAIS")
                    .HasDefaultValueSql("('Chile')");

                entity.Property(e => e.Referencias)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("REFERENCIAS");

                entity.Property(e => e.Region)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("REGION");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithOne(p => p.DireccionCliente)
                    .HasForeignKey<DireccionCliente>(d => d.IdCliente)
                    .HasConstraintName("FK_DIRECCION_CLIENTE");
            });

            modelBuilder.Entity<Dominio>(entity =>
            {
                entity.HasKey(e => e.IdDominio)
                    .HasName("PK__DOMINIO__75383B33FFF8DC3E");

                entity.ToTable("DOMINIO", "ecommerce");

                entity.HasIndex(e => e.ValorDominio, "UX_DOMINIO_VALOR")
                    .IsUnique();

                entity.Property(e => e.IdDominio)
                    .HasColumnName("ID_DOMINIO")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActualizadoEn)
                    .HasColumnName("ACTUALIZADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("CREADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.EstadoVerificacion)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_VERIFICACION")
                    .HasDefaultValueSql("('PENDIENTE')");

                entity.Property(e => e.IdTienda).HasColumnName("ID_TIENDA");

                entity.Property(e => e.TipoDominio)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DOMINIO");

                entity.Property(e => e.ValorDominio)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_DOMINIO");

                entity.HasOne(d => d.IdTiendaNavigation)
                    .WithMany(p => p.Dominios)
                    .HasForeignKey(d => d.IdTienda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOMINIO_TIENDA");
            });

            modelBuilder.Entity<DteEmitido>(entity =>
            {
                entity.HasKey(e => e.DteId)
                    .HasName("PK_DTE");

                entity.ToTable("DTE_EMITIDO");

                entity.HasIndex(e => e.Fecha, "FECHA_INDEX");

                entity.HasIndex(e => new { e.AmbienteId, e.EmisorId, e.SucursalId, e.FolioReutilizado }, "IDX_DTE_EMITIDO_COMBINADO");

                entity.HasIndex(e => new { e.EmisorId, e.CodigoTipoDte, e.Folio, e.FolioReutilizado }, "IDX_DTE_EMITIDO_QUERY");

                entity.HasIndex(e => new { e.EstadoSii, e.CodigoTipoDte, e.FechaProcesamiento, e.Importado, e.AmbienteId }, "IX_DTE_EMITIDO_FILTER");

                entity.HasIndex(e => new { e.FechaCreacion, e.EstadoSii }, "IX_DTE_ObtenerDtes_FCreacion");

                entity.HasIndex(e => new { e.EmisorId, e.CodigoTipoDte }, "nci_msft_1_DTE_EMITIDO_1DBB40C2A124E281819E03D52D82E443");

                entity.HasIndex(e => new { e.EmisorId, e.AmbienteId, e.FolioReutilizado, e.Importado, e.Fecha }, "nci_msft_1_DTE_EMITIDO_269F3C918540CBDAC31791C84B6F2318");

                entity.HasIndex(e => new { e.Trackid, e.Folio }, "nci_msft_1_DTE_EMITIDO_6A3A6F53254E10947756720151E227E3");

                entity.HasIndex(e => new { e.AmbienteId, e.CodigoTipoDte, e.Folio, e.RutEmisor }, "nci_msft_1_DTE_EMITIDO_DE21968CF655D933245EF6DD9E61BF23");

                entity.Property(e => e.DteId)
                    .ValueGeneratedNever()
                    .HasColumnName("DTE_ID");

                entity.Property(e => e.AmbienteId).HasColumnName("AMBIENTE_ID");

                entity.Property(e => e.Anulado).HasColumnName("ANULADO");

                entity.Property(e => e.ArchivoExcelId).HasColumnName("ARCHIVO_EXCEL_ID");

                entity.Property(e => e.Cajero)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CAJERO");

                entity.Property(e => e.CedidoCompleto).HasColumnName("CEDIDO_COMPLETO");

                entity.Property(e => e.CodigoTipoDte).HasColumnName("CODIGO_TIPO_DTE");

                entity.Property(e => e.CodigoTipoDteReferencia)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_TIPO_DTE_REFERENCIA");

                entity.Property(e => e.DtePagado).HasColumnName("DTE_PAGADO");

                entity.Property(e => e.EmisorId).HasColumnName("EMISOR_ID");

                entity.Property(e => e.EstadoAcuse)
                    .HasColumnName("ESTADO_ACUSE")
                    .HasComment("[Description(\"Pendiente de Acuse\")]\r\n        Pendiente = 5,\r\n        [Description(\"No Reclamado en Plazo\")]\r\n        A = 1,\r\n        [Description(\"Forma de Pago Contado\")]\r\n        P = 2, \r\n        [Description(\"Rechazado por el Receptor\")]\r\n        R = 3,\r\n        [Description(\"Recibo Otorgado por el Receptor\")]\r\n        C = 4");

                entity.Property(e => e.EstadoSii).HasColumnName("ESTADO_SII");

                entity.Property(e => e.Exento).HasColumnName("EXENTO");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CREACION");

                entity.Property(e => e.FechaProcesamiento)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PROCESAMIENTO");

                entity.Property(e => e.FechaReferencia)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REFERENCIA");

                entity.Property(e => e.FechaUltimaSincronizacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_ULTIMA_SINCRONIZACION");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_VENCIMIENTO");

                entity.Property(e => e.Folio).HasColumnName("FOLIO");

                entity.Property(e => e.FolioReferencia)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("FOLIO_REFERENCIA");

                entity.Property(e => e.FolioReutilizado).HasColumnName("FOLIO_REUTILIZADO");

                entity.Property(e => e.FormaPago)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FORMA_PAGO");

                entity.Property(e => e.IdTipoRechazo).HasColumnName("ID_TIPO_RECHAZO");

                entity.Property(e => e.ImportacionMasivaId).HasColumnName("IMPORTACION_MASIVA_ID");

                entity.Property(e => e.Importado).HasColumnName("IMPORTADO");

                entity.Property(e => e.Impuestos).HasColumnName("IMPUESTOS");

                entity.Property(e => e.Informado).HasColumnName("INFORMADO");

                entity.Property(e => e.IsMarked).HasColumnName("IS_MARKED");

                entity.Property(e => e.Iva).HasColumnName("IVA");

                entity.Property(e => e.IvaIncluido).HasColumnName("IVA_INCLUIDO");

                entity.Property(e => e.IvaPropio).HasColumnName("IVA_PROPIO");

                entity.Property(e => e.IvaTerceros).HasColumnName("IVA_TERCEROS");

                entity.Property(e => e.Mediopago)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MEDIOPAGO");

                entity.Property(e => e.Neto).HasColumnName("NETO");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("OBSERVACIONES");

                entity.Property(e => e.OrigenId).HasColumnName("ORIGEN_ID");

                entity.Property(e => e.Propina).HasColumnName("PROPINA");

                entity.Property(e => e.ProveedorId).HasColumnName("PROVEEDOR_ID");

                entity.Property(e => e.RazonReferencia)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("RAZON_REFERENCIA");

                entity.Property(e => e.RazonSocialReceptor)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("RAZON_SOCIAL_RECEPTOR");

                entity.Property(e => e.ReceptorId).HasColumnName("RECEPTOR_ID");

                entity.Property(e => e.ReferenciaGlobal).HasColumnName("REFERENCIA_GLOBAL");

                entity.Property(e => e.RutEmisor).HasColumnName("RUT_EMISOR");

                entity.Property(e => e.RutReceptor).HasColumnName("RUT_RECEPTOR");

                entity.Property(e => e.Sincronizado).HasColumnName("SINCRONIZADO");

                entity.Property(e => e.SucursalId).HasColumnName("SUCURSAL_ID");

                entity.Property(e => e.TipoDteId).HasColumnName("TIPO_DTE_ID");

                entity.Property(e => e.Tipopago)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPOPAGO");

                entity.Property(e => e.Total).HasColumnName("TOTAL");

                entity.Property(e => e.TotalImpuestosAdicionales).HasColumnName("TOTAL_IMPUESTOS_ADICIONALES");

                entity.Property(e => e.Trackid).HasColumnName("TRACKID");

                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");

                entity.HasOne(d => d.Emisor)
                    .WithMany(p => p.DteEmitidos)
                    .HasForeignKey(d => d.EmisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DTE_EMISOR_DT_EMISOR");
            });

            modelBuilder.Entity<Emisor>(entity =>
            {
                entity.ToTable("EMISOR");

                entity.HasIndex(e => e.ResellerId, "UQ_RESELLERID");

                entity.HasIndex(e => e.Rut, "UQ__EMISOR__CAF332580C10B17C")
                    .IsUnique();

                entity.Property(e => e.EmisorId)
                    .ValueGeneratedNever()
                    .HasColumnName("EMISOR_ID");

                entity.Property(e => e.Activo).HasColumnName("ACTIVO");

                entity.Property(e => e.Ambiente).HasColumnName("AMBIENTE");

                entity.Property(e => e.Ciudad)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CIUDAD");

                entity.Property(e => e.Comuna)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("COMUNA");

                entity.Property(e => e.CorreoFact)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CORREO_FACT");

                entity.Property(e => e.CorreoPar)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CORREO_PAR");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.DetallesIvaIncluido).HasColumnName("DETALLES_IVA_INCLUIDO");

                entity.Property(e => e.DirFact)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("DIR_FACT");

                entity.Property(e => e.DirPart)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("DIR_PART");

                entity.Property(e => e.Dv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DV")
                    .IsFixedLength();

                entity.Property(e => e.EnvioAutomaticoCorreos)
                    .IsRequired()
                    .HasColumnName("ENVIO_AUTOMATICO_CORREOS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EsAgenteRetenedor).HasColumnName("ES_AGENTE_RETENEDOR");

                entity.Property(e => e.FechaResol)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_RESOL");

                entity.Property(e => e.Giro)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("GIRO");

                entity.Property(e => e.NroResol).HasColumnName("NRO_RESOL");

                entity.Property(e => e.PasswordSii)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD_SII");

                entity.Property(e => e.PasswordSiiVerificada).HasColumnName("PASSWORD_SII_VERIFICADA");

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("RAZON_SOCIAL");

                entity.Property(e => e.RepresentanteLegalDv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTANTE_LEGAL_DV")
                    .IsFixedLength();

                entity.Property(e => e.RepresentanteLegalRut)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("REPRESENTANTE_LEGAL_RUT");

                entity.Property(e => e.ResellerId).HasColumnName("RESELLER_ID");

                entity.Property(e => e.Rut)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("RUT");

                entity.Property(e => e.Telefono)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TELEFONO");

                entity.Property(e => e.UnidadSii)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_SII");
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.HasKey(e => e.IdOrden)
                    .HasName("PK__ORDEN__D23A85693FCE43F5");

                entity.ToTable("ORDEN", "ecommerce");

                entity.HasIndex(e => new { e.IdTienda, e.IdEstado }, "IX_ORDEN_TIENDA_ESTADO");

                entity.Property(e => e.IdOrden)
                    .HasColumnName("ID_ORDEN")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActualizadoEn)
                    .HasColumnName("ACTUALIZADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("CREADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdDte).HasColumnName("ID_DTE");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.IdTienda).HasColumnName("ID_TIENDA");

                entity.Property(e => e.TotalBruto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TOTAL_BRUTO");

                entity.Property(e => e.TotalIva)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TOTAL_IVA");

                entity.Property(e => e.TotalNeto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("TOTAL_NETO");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDEN_CLIENTE");

                entity.HasOne(d => d.IdDteNavigation)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.IdDte)
                    .HasConstraintName("FK_ORDEN_DTE");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDEN_ESTADO");

                entity.HasOne(d => d.IdTiendaNavigation)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.IdTienda)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDEN_TIENDA");
            });

            modelBuilder.Entity<OrdenEstado>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK__ORDEN_ES__241E2E0141281B99");

                entity.ToTable("ORDEN_ESTADO", "ecommerce");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ESTADO");
            });

            modelBuilder.Entity<OrdenItem>(entity =>
            {
                entity.HasKey(e => e.IdOrdenItem)
                    .HasName("PK__ORDEN_IT__75E98FFAEA9DFB53");

                entity.ToTable("ORDEN_ITEM", "ecommerce");

                entity.Property(e => e.IdOrdenItem)
                    .HasColumnName("ID_ORDEN_ITEM")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Cantidad)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("CANTIDAD");

                entity.Property(e => e.EsExento).HasColumnName("ES_EXENTO");

                entity.Property(e => e.IdOrden).HasColumnName("ID_ORDEN");

                entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");

                entity.Property(e => e.Iva)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("IVA");

                entity.Property(e => e.NombreItem)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ITEM");

                entity.Property(e => e.PrecioNeto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("PRECIO_NETO");

                entity.HasOne(d => d.IdOrdenNavigation)
                    .WithMany(p => p.OrdenItems)
                    .HasForeignKey(d => d.IdOrden)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDENITEM_ORDEN");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.OrdenItems)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDENITEM_PRODUCTO");
            });

            modelBuilder.Entity<Plantilla>(entity =>
            {
                entity.HasKey(e => e.IdPlantilla)
                    .HasName("PK__PLANTILL__9C81DDB27D038A52");

                entity.ToTable("PLANTILLA", "ecommerce");

                entity.Property(e => e.IdPlantilla)
                    .HasColumnName("ID_PLANTILLA")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.JsonConfigDefecto).HasColumnName("JSON_CONFIG_DEFECTO");

                entity.Property(e => e.NombrePlantilla)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PLANTILLA");

                entity.Property(e => e.TipoEstructura)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ESTRUCTURA");

                entity.Property(e => e.UrlBundle)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL_BUNDLE");

                entity.Property(e => e.UrlRepositorio)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL_REPOSITORIO");

                entity.Property(e => e.Version)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VERSION");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTO");

                entity.HasIndex(e => e.EmisorId, "IX_PRODUCTO_EMISOR_VISIBLE")
                    .HasFilter("([VISIBLE_EN_TIENDA]=(1))");

                entity.HasIndex(e => new { e.EmisorId, e.Slug }, "UX_PRODUCTO_EMISOR_SLUG")
                    .IsUnique()
                    .HasFilter("([SLUG] IS NOT NULL)");

                entity.Property(e => e.ProductoId)
                    .ValueGeneratedNever()
                    .HasColumnName("PRODUCTO_ID");

                entity.Property(e => e.Activo).HasColumnName("ACTIVO");

                entity.Property(e => e.CodigoBarra)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("CODIGO_BARRA")
                    .IsFixedLength();

                entity.Property(e => e.DescripcionCorta)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_CORTA");

                entity.Property(e => e.DescripcionLarga)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_LARGA");

                entity.Property(e => e.EmisorId).HasColumnName("EMISOR_ID");

                entity.Property(e => e.Exento).HasColumnName("EXENTO");

                entity.Property(e => e.ImagenBase64)
                    .HasColumnName("IMAGEN_BASE64")
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.NombrePublico)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_PUBLICO");

                entity.Property(e => e.Novedad).HasColumnName("NOVEDAD");

                entity.Property(e => e.Precio).HasColumnName("PRECIO");

                entity.Property(e => e.Slug)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SLUG");

                entity.Property(e => e.Stock).HasColumnName("STOCK");

                entity.Property(e => e.SucursalId).HasColumnName("SUCURSAL_ID");

                entity.Property(e => e.UnidadMedida)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");

                entity.Property(e => e.VisibleEnTienda).HasColumnName("VISIBLE_EN_TIENDA");

                entity.HasOne(d => d.Emisor)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.EmisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRODUCTO_EMISOR");

                entity.HasMany(d => d.IdCategoria)
                    .WithMany(p => p.IdProductos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductoCategorium",
                        l => l.HasOne<Categorium>().WithMany().HasForeignKey("IdCategoria").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_CATEGORIA"),
                        r => r.HasOne<Producto>().WithMany().HasForeignKey("IdProducto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PC_PRODUCTO"),
                        j =>
                        {
                            j.HasKey("IdProducto", "IdCategoria");

                            j.ToTable("PRODUCTO_CATEGORIA", "ecommerce");

                            j.IndexerProperty<Guid>("IdProducto").HasColumnName("ID_PRODUCTO");

                            j.IndexerProperty<Guid>("IdCategoria").HasColumnName("ID_CATEGORIA");
                        });
            });

            modelBuilder.Entity<Tiendum>(entity =>
            {
                entity.HasKey(e => e.IdTienda)
                    .HasName("PK__TIENDA__E47F76035668A465");

                entity.ToTable("TIENDA", "ecommerce");

                entity.HasIndex(e => e.IdEmisor, "UX_TIENDA_EMISOR")
                    .IsUnique();

                entity.Property(e => e.IdTienda)
                    .HasColumnName("ID_TIENDA")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActualizadoEn)
                    .HasColumnName("ACTUALIZADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.CreadoEn)
                    .HasColumnName("CREADO_EN")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.EstadoTienda)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ESTADO_TIENDA")
                    .HasDefaultValueSql("('ACTIVA')");

                entity.Property(e => e.IdEmisor).HasColumnName("ID_EMISOR");

                entity.Property(e => e.IdPlantilla).HasColumnName("ID_PLANTILLA");

                entity.Property(e => e.JsonBranding).HasColumnName("JSON_BRANDING");

                entity.Property(e => e.NombreComercial)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_COMERCIAL");

                entity.Property(e => e.NombreFantasia)
                    .HasMaxLength(255)
                    .HasColumnName("NOMBRE_FANTASIA");

                entity.HasOne(d => d.IdEmisorNavigation)
                    .WithOne(p => p.Tiendum)
                    .HasForeignKey<Tiendum>(d => d.IdEmisor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TIENDA_EMISOR");

                entity.HasOne(d => d.IdPlantillaNavigation)
                    .WithMany(p => p.Tienda)
                    .HasForeignKey(d => d.IdPlantilla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TIENDA_PLANTILLA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
