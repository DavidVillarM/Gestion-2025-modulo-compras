using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AjustesStock> AjustesStocks { get; set; }

    public virtual DbSet<AuditLogEntry> AuditLogEntries { get; set; }

    public virtual DbSet<BajasProducto> BajasProductos { get; set; }

    public virtual DbSet<Bucket> Buckets { get; set; }

    public virtual DbSet<CategoriaProveedor> CategoriaProveedors { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<DetalleAjusteStock> DetalleAjusteStocks { get; set; }

    public virtual DbSet<Embedding> Embeddings { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FacturaDetalle> FacturaDetalles { get; set; }

    public virtual DbSet<FlowState> FlowStates { get; set; }

    public virtual DbSet<Identity> Identities { get; set; }

    public virtual DbSet<Instance> Instances { get; set; }

    public virtual DbSet<MfaAmrClaim> MfaAmrClaims { get; set; }

    public virtual DbSet<MfaChallenge> MfaChallenges { get; set; }

    public virtual DbSet<MfaFactor> MfaFactors { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<Migration1> Migrations1 { get; set; }

    public virtual DbSet<NotaCreditoDetalle> NotaCreditoDetalles { get; set; }

    public virtual DbSet<NotasCredito> NotasCreditos { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<OneTimeToken> OneTimeTokens { get; set; }

    public virtual DbSet<OrdenDetalle> OrdenDetalles { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Precio> Precios { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<PresupuestoDetalle> PresupuestoDetalles { get; set; }

    public virtual DbSet<PresupuestosProveedore> PresupuestosProveedores { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoProveedor> ProductoProveedors { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<S3MultipartUpload> S3MultipartUploads { get; set; }

    public virtual DbSet<S3MultipartUploadsPart> S3MultipartUploadsParts { get; set; }

    public virtual DbSet<SamlProvider> SamlProviders { get; set; }

    public virtual DbSet<SamlRelayState> SamlRelayStates { get; set; }

    public virtual DbSet<SchemaMigration> SchemaMigrations { get; set; }

    public virtual DbSet<SchemaMigration1> SchemaMigrations1 { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServiciosProductosUtilizado> ServiciosProductosUtilizados { get; set; }

    public virtual DbSet<ServiciosRealizado> ServiciosRealizados { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SsoDomain> SsoDomains { get; set; }

    public virtual DbSet<SsoProvider> SsoProviders { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User Id=postgres.unnbfpjxfdyrkuddnqjr;Password=grutus24;Server=aws-0-us-east-2.pooler.supabase.com;Port=5432;Database=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vector")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<AjustesStock>(entity =>
        {
            entity.HasKey(e => e.IdAjuste).HasName("ajustes_stock_pkey");

            entity.ToTable("ajustes_stock");

            entity.Property(e => e.IdAjuste)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_ajuste");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPersonal).HasColumnName("id_personal");
            entity.Property(e => e.Motivo).HasColumnName("motivo");
            entity.Property(e => e.TipoAjuste).HasColumnName("tipo_ajuste");

            entity.HasOne(d => d.IdPersonalNavigation).WithMany(p => p.AjustesStocks)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("ajustes_stock_id_personal_fkey");
        });

        modelBuilder.Entity<AuditLogEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_log_entries_pkey");

            entity.ToTable("audit_log_entries", "auth", tb => tb.HasComment("Auth: Audit trail for user actions."));

            entity.HasIndex(e => e.InstanceId, "audit_logs_instance_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.InstanceId).HasColumnName("instance_id");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(64)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("ip_address");
            entity.Property(e => e.Payload)
                .HasColumnType("json")
                .HasColumnName("payload");
        });

        modelBuilder.Entity<BajasProducto>(entity =>
        {
            entity.HasKey(e => e.IdBajaProducto).HasName("bajas_producto_pkey");

            entity.ToTable("bajas_producto");

            entity.Property(e => e.IdBajaProducto)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_baja_producto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.BajasProductos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("bajas_producto_id_producto_fkey");
        });

        modelBuilder.Entity<Bucket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buckets_pkey");

            entity.ToTable("buckets", "storage");

            entity.HasIndex(e => e.Name, "bname").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllowedMimeTypes).HasColumnName("allowed_mime_types");
            entity.Property(e => e.AvifAutodetection)
                .HasDefaultValue(false)
                .HasColumnName("avif_autodetection");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FileSizeLimit).HasColumnName("file_size_limit");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Owner)
                .HasComment("Field is deprecated, use owner_id instead")
                .HasColumnName("owner");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Public)
                .HasDefaultValue(false)
                .HasColumnName("public");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<CategoriaProveedor>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProveedor).HasName("categoria_proveedor_pkey");

            entity.ToTable("categoria_proveedor");

            entity.Property(e => e.IdCategoriaProveedor)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_categoria_proveedor");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.CategoriaProveedors)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("categoria_proveedor_id_categoria_fkey");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.CategoriaProveedors)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("categoria_proveedor_id_proveedor_fkey");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("categoria_pkey");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<DetalleAjusteStock>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("detalle_ajuste_stock_pkey");

            entity.ToTable("detalle_ajuste_stock");

            entity.Property(e => e.IdDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdAjuste).HasColumnName("id_ajuste");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");

            entity.HasOne(d => d.IdAjusteNavigation).WithMany(p => p.DetalleAjusteStocks)
                .HasForeignKey(d => d.IdAjuste)
                .HasConstraintName("detalle_ajuste_stock_id_ajuste_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleAjusteStocks)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("detalle_ajuste_stock_id_producto_fkey");
        });

        modelBuilder.Entity<Embedding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("embeddings_pkey");

            entity.ToTable("embeddings", "meta");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("facturas_pkey");

            entity.ToTable("facturas");

            entity.Property(e => e.IdFactura)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_factura");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Iva10)
                .HasPrecision(10, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(10, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(10, 2)
                .HasColumnName("monto_total");
            entity.Property(e => e.NombreProveedor).HasColumnName("nombre_proveedor");
            entity.Property(e => e.Ruc).HasColumnName("ruc");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Timbrado).HasColumnName("timbrado");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("facturas_id_proveedor_fkey");
        });

        modelBuilder.Entity<FacturaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdFacturaDetalle).HasName("factura_detalle_pkey");

            entity.ToTable("factura_detalle");

            entity.Property(e => e.IdFacturaDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_factura_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Iva10)
                .HasPrecision(10, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(10, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.FacturaDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("factura_detalle_id_producto_fkey");
        });

        modelBuilder.Entity<FlowState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("flow_state_pkey");

            entity.ToTable("flow_state", "auth", tb => tb.HasComment("stores metadata for pkce logins"));

            entity.HasIndex(e => e.CreatedAt, "flow_state_created_at_idx").IsDescending();

            entity.HasIndex(e => e.AuthCode, "idx_auth_code");

            entity.HasIndex(e => new { e.UserId, e.AuthenticationMethod }, "idx_user_id_auth_method");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthCode).HasColumnName("auth_code");
            entity.Property(e => e.AuthCodeIssuedAt).HasColumnName("auth_code_issued_at");
            entity.Property(e => e.AuthenticationMethod).HasColumnName("authentication_method");
            entity.Property(e => e.CodeChallenge).HasColumnName("code_challenge");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ProviderAccessToken).HasColumnName("provider_access_token");
            entity.Property(e => e.ProviderRefreshToken).HasColumnName("provider_refresh_token");
            entity.Property(e => e.ProviderType).HasColumnName("provider_type");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Identity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("identities_pkey");

            entity.ToTable("identities", "auth", tb => tb.HasComment("Auth: Stores identities associated to a user."));

            entity.HasIndex(e => e.Email, "identities_email_idx").HasOperators(new[] { "text_pattern_ops" });

            entity.HasIndex(e => new { e.ProviderId, e.Provider }, "identities_provider_id_provider_unique").IsUnique();

            entity.HasIndex(e => e.UserId, "identities_user_id_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasComputedColumnSql("lower((identity_data ->> 'email'::text))", true)
                .HasComment("Auth: Email is a generated column that references the optional email property in the identity_data")
                .HasColumnName("email");
            entity.Property(e => e.IdentityData)
                .HasColumnType("jsonb")
                .HasColumnName("identity_data");
            entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");
            entity.Property(e => e.Provider).HasColumnName("provider");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Identities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("identities_user_id_fkey");
        });

        modelBuilder.Entity<Instance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("instances_pkey");

            entity.ToTable("instances", "auth", tb => tb.HasComment("Auth: Manages users across multiple sites."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.RawBaseConfig).HasColumnName("raw_base_config");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        modelBuilder.Entity<MfaAmrClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("amr_id_pk");

            entity.ToTable("mfa_amr_claims", "auth", tb => tb.HasComment("auth: stores authenticator method reference claims for multi factor authentication"));

            entity.HasIndex(e => new { e.SessionId, e.AuthenticationMethod }, "mfa_amr_claims_session_id_authentication_method_pkey").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AuthenticationMethod).HasColumnName("authentication_method");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Session).WithMany(p => p.MfaAmrClaims)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("mfa_amr_claims_session_id_fkey");
        });

        modelBuilder.Entity<MfaChallenge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mfa_challenges_pkey");

            entity.ToTable("mfa_challenges", "auth", tb => tb.HasComment("auth: stores metadata about challenge requests made"));

            entity.HasIndex(e => e.CreatedAt, "mfa_challenge_created_at_idx").IsDescending();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FactorId).HasColumnName("factor_id");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            entity.Property(e => e.OtpCode).HasColumnName("otp_code");
            entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");
            entity.Property(e => e.WebAuthnSessionData)
                .HasColumnType("jsonb")
                .HasColumnName("web_authn_session_data");

            entity.HasOne(d => d.Factor).WithMany(p => p.MfaChallenges)
                .HasForeignKey(d => d.FactorId)
                .HasConstraintName("mfa_challenges_auth_factor_id_fkey");
        });

        modelBuilder.Entity<MfaFactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mfa_factors_pkey");

            entity.ToTable("mfa_factors", "auth", tb => tb.HasComment("auth: stores metadata about factors"));

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "factor_id_created_at_idx");

            entity.HasIndex(e => e.LastChallengedAt, "mfa_factors_last_challenged_at_key").IsUnique();

            entity.HasIndex(e => new { e.FriendlyName, e.UserId }, "mfa_factors_user_friendly_name_unique")
                .IsUnique()
                .HasFilter("(TRIM(BOTH FROM friendly_name) <> ''::text)");

            entity.HasIndex(e => e.UserId, "mfa_factors_user_id_idx");

            entity.HasIndex(e => new { e.UserId, e.Phone }, "unique_phone_factor_per_user").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FriendlyName).HasColumnName("friendly_name");
            entity.Property(e => e.LastChallengedAt).HasColumnName("last_challenged_at");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Secret).HasColumnName("secret");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WebAuthnAaguid).HasColumnName("web_authn_aaguid");
            entity.Property(e => e.WebAuthnCredential)
                .HasColumnType("jsonb")
                .HasColumnName("web_authn_credential");

            entity.HasOne(d => d.User).WithMany(p => p.MfaFactors)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("mfa_factors_user_id_fkey");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("migrations_pkey");

            entity.ToTable("migrations", "storage");

            entity.HasIndex(e => e.Name, "migrations_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ExecutedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("executed_at");
            entity.Property(e => e.Hash)
                .HasMaxLength(40)
                .HasColumnName("hash");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Migration1>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("migrations_pkey");

            entity.ToTable("migrations", "meta");

            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("applied_at");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<NotaCreditoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdFacturaDetalle).HasName("nota_credito_detalle_pkey");

            entity.ToTable("nota_credito_detalle");

            entity.Property(e => e.IdFacturaDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_factura_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Iva10)
                .HasPrecision(10, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(10, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.NotaCreditoDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("nota_credito_detalle_id_producto_fkey");
        });

        modelBuilder.Entity<NotasCredito>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("notas_credito_pkey");

            entity.ToTable("notas_credito");

            entity.Property(e => e.IdFactura)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_factura");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Iva10)
                .HasPrecision(10, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(10, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(10, 2)
                .HasColumnName("monto_total");
            entity.Property(e => e.NombreProveedor).HasColumnName("nombre_proveedor");
            entity.Property(e => e.Ruc).HasColumnName("ruc");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Timbrado).HasColumnName("timbrado");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.NotasCreditos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("notas_credito_id_proveedor_fkey");
        });

        modelBuilder.Entity<Object>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("objects_pkey");

            entity.ToTable("objects", "storage");

            entity.HasIndex(e => new { e.BucketId, e.Name }, "bucketid_objname").IsUnique();

            entity.HasIndex(e => new { e.BucketId, e.Name }, "idx_objects_bucket_id_name").UseCollation(new[] { null, "C" });

            entity.HasIndex(e => e.Name, "name_prefix_search").HasOperators(new[] { "text_pattern_ops" });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.BucketId).HasColumnName("bucket_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.LastAccessedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_accessed_at");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Owner)
                .HasComment("Field is deprecated, use owner_id instead")
                .HasColumnName("owner");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.PathTokens)
                .HasComputedColumnSql("string_to_array(name, '/'::text)", true)
                .HasColumnName("path_tokens");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserMetadata)
                .HasColumnType("jsonb")
                .HasColumnName("user_metadata");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Bucket).WithMany(p => p.Objects)
                .HasForeignKey(d => d.BucketId)
                .HasConstraintName("objects_bucketId_fkey");
        });

        modelBuilder.Entity<OneTimeToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("one_time_tokens_pkey");

            entity.ToTable("one_time_tokens", "auth");

            entity.HasIndex(e => e.RelatesTo, "one_time_tokens_relates_to_hash_idx").HasMethod("hash");

            entity.HasIndex(e => e.TokenHash, "one_time_tokens_token_hash_hash_idx").HasMethod("hash");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.RelatesTo).HasColumnName("relates_to");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.OneTimeTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("one_time_tokens_user_id_fkey");
        });

        modelBuilder.Entity<OrdenDetalle>(entity =>
        {
            entity.HasKey(e => e.IdOrdenDetalle).HasName("orden_detalle_pkey");

            entity.ToTable("orden_detalle");

            entity.Property(e => e.IdOrdenDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_orden_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdOrden).HasColumnName("id_orden");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fk_orden_detalle_orden");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.OrdenDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("fk_orden_detalle_producto");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("ordenes_pkey");

            entity.ToTable("ordenes");

            entity.Property(e => e.IdOrden)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_orden");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("pedidos_pkey");

            entity.ToTable("pedidos");

            entity.Property(e => e.IdPedido)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_pedido");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");
            entity.Property(e => e.IdOrden).HasColumnName("id_orden");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.MontoTotal)
                .HasPrecision(12, 2)
                .HasColumnName("monto_total");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fk_pedidos_orden");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_pedidos_proveedor");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdPedidoDetalle).HasName("pedido_detalles_pkey");

            entity.ToTable("pedido_detalles");

            entity.Property(e => e.IdPedidoDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_pedido_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Cotizacion)
                .HasPrecision(12, 2)
                .HasColumnName("cotizacion");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Iva)
                .HasPrecision(5, 2)
                .HasColumnName("iva");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("fk_pedido_detalles_pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("fk_pedido_detalles_producto");
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.IdPersonal).HasName("personal_pkey");

            entity.ToTable("personal");

            entity.Property(e => e.IdPersonal)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_personal");
            entity.Property(e => e.Apellido).HasColumnName("apellido");
            entity.Property(e => e.Ci).HasColumnName("ci");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.FechaSalida).HasColumnName("fecha_salida");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Personals)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("personal_id_rol_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Personals)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("personal_id_user_fkey");
        });

        modelBuilder.Entity<Precio>(entity =>
        {
            entity.HasKey(e => e.IdPrecio).HasName("precios_pkey");

            entity.ToTable("precios");

            entity.Property(e => e.IdPrecio)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_precio");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Precio1)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Precios)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("precios_id_producto_fkey");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.IdPresupuesto).HasName("presupuestos_pkey");

            entity.ToTable("presupuestos");

            entity.Property(e => e.IdPresupuesto)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_presupuesto");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.IdOrden).HasColumnName("id_orden");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Iva10)
                .HasPrecision(12, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(12, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.Subtotal)
                .HasPrecision(12, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Presupuestos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_presupuestos_proveedor");
        });

        modelBuilder.Entity<PresupuestoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdPresupuestoDetalle).HasName("presupuesto_detalle_pkey");

            entity.ToTable("presupuesto_detalle");

            entity.Property(e => e.IdPresupuestoDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_presupuesto_detalle");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPresupuesto).HasColumnName("id_presupuesto");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Iva10)
                .HasPrecision(12, 2)
                .HasColumnName("iva10");
            entity.Property(e => e.Iva5)
                .HasPrecision(12, 2)
                .HasColumnName("iva5");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdPresupuestoNavigation).WithMany(p => p.PresupuestoDetalles)
                .HasForeignKey(d => d.IdPresupuesto)
                .HasConstraintName("fk_pd_presupuesto");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.PresupuestoDetalles)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("fk_pd_producto");
        });

        modelBuilder.Entity<PresupuestosProveedore>(entity =>
        {
            entity.HasKey(e => e.IdPresupuesto).HasName("presupuestos_proveedores_pkey");

            entity.ToTable("presupuestos_proveedores");

            entity.Property(e => e.IdPresupuesto)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_presupuesto");
            entity.Property(e => e.FechaEnvio).HasColumnName("fecha_envio");
            entity.Property(e => e.OrdenId).HasColumnName("orden_id");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.PresupuestosProveedores)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("fk_proveedor");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_producto");
            entity.Property(e => e.CantidadMinima).HasColumnName("cantidad_minima");
            entity.Property(e => e.CantidadTotal).HasColumnName("cantidad_total");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdStock).HasColumnName("id_stock");
            entity.Property(e => e.Marca).HasColumnName("marca");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("productos_id_categoria_fkey");

            entity.HasOne(d => d.IdStockNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdStock)
                .HasConstraintName("productos_id_stock_fkey");
        });

        modelBuilder.Entity<ProductoProveedor>(entity =>
        {
            entity.HasKey(e => e.IdProductoProveedor).HasName("producto_proveedor_pkey");

            entity.ToTable("producto_proveedor");

            entity.Property(e => e.IdProductoProveedor)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_producto_proveedor");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoProveedors)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("producto_proveedor_id_producto_fkey");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.ProductoProveedors)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("producto_proveedor_id_proveedor_fkey");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_proveedor");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.NombreContacto).HasColumnName("nombre_contacto");
            entity.Property(e => e.Ruc).HasColumnName("ruc");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens", "auth", tb => tb.HasComment("Auth: Store of tokens used to refresh JWT tokens once they expire."));

            entity.HasIndex(e => e.InstanceId, "refresh_tokens_instance_id_idx");

            entity.HasIndex(e => new { e.InstanceId, e.UserId }, "refresh_tokens_instance_id_user_id_idx");

            entity.HasIndex(e => e.Parent, "refresh_tokens_parent_idx");

            entity.HasIndex(e => new { e.SessionId, e.Revoked }, "refresh_tokens_session_id_revoked_idx");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_unique").IsUnique();

            entity.HasIndex(e => e.UpdatedAt, "refresh_tokens_updated_at_idx").IsDescending();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.InstanceId).HasColumnName("instance_id");
            entity.Property(e => e.Parent)
                .HasMaxLength(255)
                .HasColumnName("parent");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Session).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_session_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_rol");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
        });

        modelBuilder.Entity<S3MultipartUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s3_multipart_uploads_pkey");

            entity.ToTable("s3_multipart_uploads", "storage");

            entity.HasIndex(e => new { e.BucketId, e.Key, e.CreatedAt }, "idx_multipart_uploads_list").UseCollation(new[] { null, "C", null });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BucketId).HasColumnName("bucket_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.InProgressSize)
                .HasDefaultValue(0L)
                .HasColumnName("in_progress_size");
            entity.Property(e => e.Key)
                .UseCollation("C")
                .HasColumnName("key");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.UploadSignature).HasColumnName("upload_signature");
            entity.Property(e => e.UserMetadata)
                .HasColumnType("jsonb")
                .HasColumnName("user_metadata");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Bucket).WithMany(p => p.S3MultipartUploads)
                .HasForeignKey(d => d.BucketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_bucket_id_fkey");
        });

        modelBuilder.Entity<S3MultipartUploadsPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s3_multipart_uploads_parts_pkey");

            entity.ToTable("s3_multipart_uploads_parts", "storage");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.BucketId).HasColumnName("bucket_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Etag).HasColumnName("etag");
            entity.Property(e => e.Key)
                .UseCollation("C")
                .HasColumnName("key");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.PartNumber).HasColumnName("part_number");
            entity.Property(e => e.Size)
                .HasDefaultValue(0L)
                .HasColumnName("size");
            entity.Property(e => e.UploadId).HasColumnName("upload_id");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Bucket).WithMany(p => p.S3MultipartUploadsParts)
                .HasForeignKey(d => d.BucketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_parts_bucket_id_fkey");

            entity.HasOne(d => d.Upload).WithMany(p => p.S3MultipartUploadsParts)
                .HasForeignKey(d => d.UploadId)
                .HasConstraintName("s3_multipart_uploads_parts_upload_id_fkey");
        });

        modelBuilder.Entity<SamlProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saml_providers_pkey");

            entity.ToTable("saml_providers", "auth", tb => tb.HasComment("Auth: Manages SAML Identity Provider connections."));

            entity.HasIndex(e => e.EntityId, "saml_providers_entity_id_key").IsUnique();

            entity.HasIndex(e => e.SsoProviderId, "saml_providers_sso_provider_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AttributeMapping)
                .HasColumnType("jsonb")
                .HasColumnName("attribute_mapping");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.MetadataUrl).HasColumnName("metadata_url");
            entity.Property(e => e.MetadataXml).HasColumnName("metadata_xml");
            entity.Property(e => e.NameIdFormat).HasColumnName("name_id_format");
            entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SamlProviders)
                .HasForeignKey(d => d.SsoProviderId)
                .HasConstraintName("saml_providers_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SamlRelayState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saml_relay_states_pkey");

            entity.ToTable("saml_relay_states", "auth", tb => tb.HasComment("Auth: Contains SAML Relay State information for each Service Provider initiated login."));

            entity.HasIndex(e => e.CreatedAt, "saml_relay_states_created_at_idx").IsDescending();

            entity.HasIndex(e => e.ForEmail, "saml_relay_states_for_email_idx");

            entity.HasIndex(e => e.SsoProviderId, "saml_relay_states_sso_provider_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FlowStateId).HasColumnName("flow_state_id");
            entity.Property(e => e.ForEmail).HasColumnName("for_email");
            entity.Property(e => e.RedirectTo).HasColumnName("redirect_to");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.FlowState).WithMany(p => p.SamlRelayStates)
                .HasForeignKey(d => d.FlowStateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("saml_relay_states_flow_state_id_fkey");

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SamlRelayStates)
                .HasForeignKey(d => d.SsoProviderId)
                .HasConstraintName("saml_relay_states_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SchemaMigration>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("schema_migrations_pkey");

            entity.ToTable("schema_migrations", "auth", tb => tb.HasComment("Auth: Manages updates to the auth system."));

            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");
        });

        modelBuilder.Entity<SchemaMigration1>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("schema_migrations_pkey");

            entity.ToTable("schema_migrations", "realtime");

            entity.Property(e => e.Version)
                .ValueGeneratedNever()
                .HasColumnName("version");
            entity.Property(e => e.InsertedAt)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("inserted_at");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("servicios_pkey");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicio)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_servicio");
            entity.Property(e => e.Costo)
                .HasPrecision(10, 2)
                .HasColumnName("costo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
        });

        modelBuilder.Entity<ServiciosProductosUtilizado>(entity =>
        {
            entity.HasKey(e => e.IdServicioProducto).HasName("servicios_productos_utilizados_pkey");

            entity.ToTable("servicios_productos_utilizados");

            entity.Property(e => e.IdServicioProducto)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_servicio_producto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdServicioRealizado).HasColumnName("id_servicio_realizado");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ServiciosProductosUtilizados)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("servicios_productos_utilizados_id_producto_fkey");

            entity.HasOne(d => d.IdServicioRealizadoNavigation).WithMany(p => p.ServiciosProductosUtilizados)
                .HasForeignKey(d => d.IdServicioRealizado)
                .HasConstraintName("servicios_productos_utilizados_id_servicio_realizado_fkey");
        });

        modelBuilder.Entity<ServiciosRealizado>(entity =>
        {
            entity.HasKey(e => e.IdServicioRealizado).HasName("servicios_realizados_pkey");

            entity.ToTable("servicios_realizados");

            entity.Property(e => e.IdServicioRealizado)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_servicio_realizado");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdPersonal).HasColumnName("id_personal");
            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");

            entity.HasOne(d => d.IdPersonalNavigation).WithMany(p => p.ServiciosRealizados)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("servicios_realizados_id_personal_fkey");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServiciosRealizados)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("servicios_realizados_id_servicio_fkey");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_pkey");

            entity.ToTable("sessions", "auth", tb => tb.HasComment("Auth: Stores session data associated to a user."));

            entity.HasIndex(e => e.NotAfter, "sessions_not_after_idx").IsDescending();

            entity.HasIndex(e => e.UserId, "sessions_user_id_idx");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "user_id_created_at_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FactorId).HasColumnName("factor_id");
            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.NotAfter)
                .HasComment("Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired.")
                .HasColumnName("not_after");
            entity.Property(e => e.RefreshedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refreshed_at");
            entity.Property(e => e.Tag).HasColumnName("tag");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("sessions_user_id_fkey");
        });

        modelBuilder.Entity<SsoDomain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sso_domains_pkey");

            entity.ToTable("sso_domains", "auth", tb => tb.HasComment("Auth: Manages SSO email address domain mapping to an SSO Identity Provider."));

            entity.HasIndex(e => e.SsoProviderId, "sso_domains_sso_provider_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Domain).HasColumnName("domain");
            entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SsoDomains)
                .HasForeignKey(d => d.SsoProviderId)
                .HasConstraintName("sso_domains_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SsoProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sso_providers_pkey");

            entity.ToTable("sso_providers", "auth", tb => tb.HasComment("Auth: Manages SSO identity provider information; see saml_providers for SAML."));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ResourceId)
                .HasComment("Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code.")
                .HasColumnName("resource_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.IdStock).HasName("stock_pkey");

            entity.ToTable("stock");

            entity.Property(e => e.IdStock)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_stock");
            entity.Property(e => e.IdPersonal).HasColumnName("id_personal");
            entity.Property(e => e.Ubicacion).HasColumnName("ubicacion");

            entity.HasOne(d => d.IdPersonalNavigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdPersonal)
                .HasConstraintName("stock_id_personal_fkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_subscription");

            entity.ToTable("subscription", "realtime");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Claims)
                .HasColumnType("jsonb")
                .HasColumnName("claims");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("timezone('utc'::text, now())")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "auth", tb => tb.HasComment("Auth: Stores user login data within a secure schema."));

            entity.HasIndex(e => e.ConfirmationToken, "confirmation_token_idx")
                .IsUnique()
                .HasFilter("((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.EmailChangeTokenCurrent, "email_change_token_current_idx")
                .IsUnique()
                .HasFilter("((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.EmailChangeTokenNew, "email_change_token_new_idx")
                .IsUnique()
                .HasFilter("((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.ReauthenticationToken, "reauthentication_token_idx")
                .IsUnique()
                .HasFilter("((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.RecoveryToken, "recovery_token_idx")
                .IsUnique()
                .HasFilter("((recovery_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.Email, "users_email_partial_key")
                .IsUnique()
                .HasFilter("(is_sso_user = false)");

            entity.HasIndex(e => e.InstanceId, "users_instance_id_idx");

            entity.HasIndex(e => e.IsAnonymous, "users_is_anonymous_idx");

            entity.HasIndex(e => e.Phone, "users_phone_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aud)
                .HasMaxLength(255)
                .HasColumnName("aud");
            entity.Property(e => e.BannedUntil).HasColumnName("banned_until");
            entity.Property(e => e.ConfirmationSentAt).HasColumnName("confirmation_sent_at");
            entity.Property(e => e.ConfirmationToken)
                .HasMaxLength(255)
                .HasColumnName("confirmation_token");
            entity.Property(e => e.ConfirmedAt)
                .HasComputedColumnSql("LEAST(email_confirmed_at, phone_confirmed_at)", true)
                .HasColumnName("confirmed_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailChange)
                .HasMaxLength(255)
                .HasColumnName("email_change");
            entity.Property(e => e.EmailChangeConfirmStatus)
                .HasDefaultValue((short)0)
                .HasColumnName("email_change_confirm_status");
            entity.Property(e => e.EmailChangeSentAt).HasColumnName("email_change_sent_at");
            entity.Property(e => e.EmailChangeTokenCurrent)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("email_change_token_current");
            entity.Property(e => e.EmailChangeTokenNew)
                .HasMaxLength(255)
                .HasColumnName("email_change_token_new");
            entity.Property(e => e.EmailConfirmedAt).HasColumnName("email_confirmed_at");
            entity.Property(e => e.EncryptedPassword)
                .HasMaxLength(255)
                .HasColumnName("encrypted_password");
            entity.Property(e => e.InstanceId).HasColumnName("instance_id");
            entity.Property(e => e.InvitedAt).HasColumnName("invited_at");
            entity.Property(e => e.IsAnonymous)
                .HasDefaultValue(false)
                .HasColumnName("is_anonymous");
            entity.Property(e => e.IsSsoUser)
                .HasDefaultValue(false)
                .HasComment("Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails.")
                .HasColumnName("is_sso_user");
            entity.Property(e => e.IsSuperAdmin).HasColumnName("is_super_admin");
            entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");
            entity.Property(e => e.Phone)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("phone");
            entity.Property(e => e.PhoneChange)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("phone_change");
            entity.Property(e => e.PhoneChangeSentAt).HasColumnName("phone_change_sent_at");
            entity.Property(e => e.PhoneChangeToken)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("phone_change_token");
            entity.Property(e => e.PhoneConfirmedAt).HasColumnName("phone_confirmed_at");
            entity.Property(e => e.RawAppMetaData)
                .HasColumnType("jsonb")
                .HasColumnName("raw_app_meta_data");
            entity.Property(e => e.RawUserMetaData)
                .HasColumnType("jsonb")
                .HasColumnName("raw_user_meta_data");
            entity.Property(e => e.ReauthenticationSentAt).HasColumnName("reauthentication_sent_at");
            entity.Property(e => e.ReauthenticationToken)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasColumnName("reauthentication_token");
            entity.Property(e => e.RecoverySentAt).HasColumnName("recovery_sent_at");
            entity.Property(e => e.RecoveryToken)
                .HasMaxLength(255)
                .HasColumnName("recovery_token");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUser)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_user");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Username).HasColumnName("username");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
