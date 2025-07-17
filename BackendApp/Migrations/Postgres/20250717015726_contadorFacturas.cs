using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendApp.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class contadorFacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.EnsureSchema(
                name: "storage");

            migrationBuilder.EnsureSchema(
                name: "meta");

            migrationBuilder.EnsureSchema(
                name: "realtime");

            migrationBuilder.EnsureSchema(
                name: "graphql");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.aal_level", "aal1,aal2,aal3")
                .Annotation("Npgsql:Enum:auth.code_challenge_method", "s256,plain")
                .Annotation("Npgsql:Enum:auth.factor_status", "unverified,verified")
                .Annotation("Npgsql:Enum:auth.factor_type", "totp,webauthn,phone")
                .Annotation("Npgsql:Enum:auth.one_time_token_type", "confirmation_token,reauthentication_token,recovery_token,email_change_token_new,email_change_token_current,phone_change_token")
                .Annotation("Npgsql:Enum:realtime.action", "INSERT,UPDATE,DELETE,TRUNCATE,ERROR")
                .Annotation("Npgsql:Enum:realtime.equality_op", "eq,neq,lt,lte,gt,gte,in")
                .Annotation("Npgsql:PostgresExtension:extensions.pg_stat_statements", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.pgjwt", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.uuid-ossp", ",,")
                .Annotation("Npgsql:PostgresExtension:graphql.pg_graphql", ",,")
                .Annotation("Npgsql:PostgresExtension:vault.supabase_vault", ",,")
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateSequence<int>(
                name: "seq_schema_version",
                schema: "graphql",
                cyclic: true);

            migrationBuilder.CreateTable(
                name: "audit_log_entries",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instance_id = table.Column<Guid>(type: "uuid", nullable: true),
                    payload = table.Column<string>(type: "json", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ip_address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "''::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("audit_log_entries_pkey", x => x.id);
                },
                comment: "Auth: Audit trail for user actions.");

            migrationBuilder.CreateTable(
                name: "buckets",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    owner = table.Column<Guid>(type: "uuid", nullable: true, comment: "Field is deprecated, use owner_id instead"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    @public = table.Column<bool>(name: "public", type: "boolean", nullable: true, defaultValue: false),
                    avif_autodetection = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    file_size_limit = table.Column<long>(type: "bigint", nullable: true),
                    allowed_mime_types = table.Column<List<string>>(type: "text[]", nullable: true),
                    owner_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("buckets_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id_categoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categoria_pkey", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "embeddings",
                schema: "meta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("embeddings_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "flow_state",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    auth_code = table.Column<string>(type: "text", nullable: false),
                    code_challenge = table.Column<string>(type: "text", nullable: false),
                    provider_type = table.Column<string>(type: "text", nullable: false),
                    provider_access_token = table.Column<string>(type: "text", nullable: true),
                    provider_refresh_token = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    authentication_method = table.Column<string>(type: "text", nullable: false),
                    auth_code_issued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("flow_state_pkey", x => x.id);
                },
                comment: "stores metadata for pkce logins");

            migrationBuilder.CreateTable(
                name: "instances",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    uuid = table.Column<Guid>(type: "uuid", nullable: true),
                    raw_base_config = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("instances_pkey", x => x.id);
                },
                comment: "Auth: Manages users across multiple sites.");

            migrationBuilder.CreateTable(
                name: "migrations",
                schema: "meta",
                columns: table => new
                {
                    version = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    applied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("migrations_pkey", x => x.version);
                });

            migrationBuilder.CreateTable(
                name: "migrations",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    hash = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    executed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("migrations_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ordenes",
                columns: table => new
                {
                    id_orden = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    estado = table.Column<string>(type: "text", nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ordenes_pkey", x => x.id_orden);
                });

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ruc = table.Column<string>(type: "text", nullable: true),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    correo = table.Column<string>(type: "text", nullable: true),
                    nombre_contacto = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("proveedores_pkey", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id_rol = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "schema_migrations",
                schema: "auth",
                columns: table => new
                {
                    version = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schema_migrations_pkey", x => x.version);
                },
                comment: "Auth: Manages updates to the auth system.");

            migrationBuilder.CreateTable(
                name: "schema_migrations",
                schema: "realtime",
                columns: table => new
                {
                    version = table.Column<long>(type: "bigint", nullable: false),
                    inserted_at = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schema_migrations_pkey", x => x.version);
                });

            migrationBuilder.CreateTable(
                name: "servicios",
                columns: table => new
                {
                    id_servicio = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    costo = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servicios_pkey", x => x.id_servicio);
                });

            migrationBuilder.CreateTable(
                name: "sso_providers",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_id = table.Column<string>(type: "text", nullable: true, comment: "Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code."),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sso_providers_pkey", x => x.id);
                },
                comment: "Auth: Manages SSO identity provider information; see saml_providers for SAML.");

            migrationBuilder.CreateTable(
                name: "subscription",
                schema: "realtime",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claims = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc'::text, now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    instance_id = table.Column<Guid>(type: "uuid", nullable: true),
                    aud = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    encrypted_password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email_confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    invited_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmation_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    confirmation_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    recovery_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    recovery_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email_change_token_new = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email_change = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email_change_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sign_in_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    raw_app_meta_data = table.Column<string>(type: "jsonb", nullable: true),
                    raw_user_meta_data = table.Column<string>(type: "jsonb", nullable: true),
                    is_super_admin = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true, defaultValueSql: "NULL::character varying"),
                    phone_confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone_change = table.Column<string>(type: "text", nullable: true, defaultValueSql: "''::character varying"),
                    phone_change_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "''::character varying"),
                    phone_change_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, computedColumnSql: "LEAST(email_confirmed_at, phone_confirmed_at)", stored: true),
                    email_change_token_current = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "''::character varying"),
                    email_change_confirm_status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)0),
                    banned_until = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reauthentication_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValueSql: "''::character varying"),
                    reauthentication_sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_sso_user = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails."),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_anonymous = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                },
                comment: "Auth: Stores user login data within a secure schema.");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_user = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "objects",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    bucket_id = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    owner = table.Column<Guid>(type: "uuid", nullable: true, comment: "Field is deprecated, use owner_id instead"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    last_accessed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    metadata = table.Column<string>(type: "jsonb", nullable: true),
                    path_tokens = table.Column<List<string>>(type: "text[]", nullable: true, computedColumnSql: "string_to_array(name, '/'::text)", stored: true),
                    version = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<string>(type: "text", nullable: true),
                    user_metadata = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("objects_pkey", x => x.id);
                    table.ForeignKey(
                        name: "objects_bucketId_fkey",
                        column: x => x.bucket_id,
                        principalSchema: "storage",
                        principalTable: "buckets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "s3_multipart_uploads",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    in_progress_size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    upload_signature = table.Column<string>(type: "text", nullable: false),
                    bucket_id = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    version = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    user_metadata = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("s3_multipart_uploads_pkey", x => x.id);
                    table.ForeignKey(
                        name: "s3_multipart_uploads_bucket_id_fkey",
                        column: x => x.bucket_id,
                        principalSchema: "storage",
                        principalTable: "buckets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "categoria_proveedor",
                columns: table => new
                {
                    id_categoria_proveedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_categoria = table.Column<long>(type: "bigint", nullable: true),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categoria_proveedor_pkey", x => x.id_categoria_proveedor);
                    table.ForeignKey(
                        name: "categoria_proveedor_id_categoria_fkey",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "categoria_proveedor_id_proveedor_fkey",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor");
                });

            migrationBuilder.CreateTable(
                name: "contador_factura",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    ultimo_numero = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    prefijo = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'F001'::text")
                },
                constraints: table =>
                {
                    table.PrimaryKey("contador_factura_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_contador_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    id_pedido = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    monto_total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    fecha_entrega = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_pedido = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pedidos_pkey", x => x.id_pedido);
                    table.ForeignKey(
                        name: "fk_pedidos_orden",
                        column: x => x.id_orden,
                        principalTable: "ordenes",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pedidos_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "presupuestos",
                columns: table => new
                {
                    id_presupuesto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    fecha_entrega = table.Column<DateOnly>(type: "date", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("presupuestos_pkey", x => x.id_presupuesto);
                    table.ForeignKey(
                        name: "fk_presupuestos_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "presupuestos_proveedores",
                columns: table => new
                {
                    id_presupuesto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    orden_id = table.Column<long>(type: "bigint", nullable: false),
                    proveedor_id = table.Column<long>(type: "bigint", nullable: false),
                    fecha_envio = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("presupuestos_proveedores_pkey", x => x.id_presupuesto);
                    table.ForeignKey(
                        name: "fk_proveedor",
                        column: x => x.proveedor_id,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saml_providers",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sso_provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<string>(type: "text", nullable: false),
                    metadata_xml = table.Column<string>(type: "text", nullable: false),
                    metadata_url = table.Column<string>(type: "text", nullable: true),
                    attribute_mapping = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name_id_format = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("saml_providers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "saml_providers_sso_provider_id_fkey",
                        column: x => x.sso_provider_id,
                        principalSchema: "auth",
                        principalTable: "sso_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Manages SAML Identity Provider connections.");

            migrationBuilder.CreateTable(
                name: "saml_relay_states",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sso_provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    request_id = table.Column<string>(type: "text", nullable: false),
                    for_email = table.Column<string>(type: "text", nullable: true),
                    redirect_to = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    flow_state_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("saml_relay_states_pkey", x => x.id);
                    table.ForeignKey(
                        name: "saml_relay_states_flow_state_id_fkey",
                        column: x => x.flow_state_id,
                        principalSchema: "auth",
                        principalTable: "flow_state",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "saml_relay_states_sso_provider_id_fkey",
                        column: x => x.sso_provider_id,
                        principalSchema: "auth",
                        principalTable: "sso_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Contains SAML Relay State information for each Service Provider initiated login.");

            migrationBuilder.CreateTable(
                name: "sso_domains",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sso_provider_id = table.Column<Guid>(type: "uuid", nullable: false),
                    domain = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sso_domains_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sso_domains_sso_provider_id_fkey",
                        column: x => x.sso_provider_id,
                        principalSchema: "auth",
                        principalTable: "sso_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Manages SSO email address domain mapping to an SSO Identity Provider.");

            migrationBuilder.CreateTable(
                name: "identities",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    provider_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    identity_data = table.Column<string>(type: "jsonb", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    last_sign_in_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true, computedColumnSql: "lower((identity_data ->> 'email'::text))", stored: true, comment: "Auth: Email is a generated column that references the optional email property in the identity_data")
                },
                constraints: table =>
                {
                    table.PrimaryKey("identities_pkey", x => x.id);
                    table.ForeignKey(
                        name: "identities_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Stores identities associated to a user.");

            migrationBuilder.CreateTable(
                name: "mfa_factors",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    friendly_name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    secret = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    last_challenged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    web_authn_credential = table.Column<string>(type: "jsonb", nullable: true),
                    web_authn_aaguid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mfa_factors_pkey", x => x.id);
                    table.ForeignKey(
                        name: "mfa_factors_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "auth: stores metadata about factors");

            migrationBuilder.CreateTable(
                name: "one_time_tokens",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token_hash = table.Column<string>(type: "text", nullable: false),
                    relates_to = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("one_time_tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "one_time_tokens_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    factor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    not_after = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired."),
                    refreshed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    tag = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sessions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sessions_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Stores session data associated to a user.");

            migrationBuilder.CreateTable(
                name: "personal",
                columns: table => new
                {
                    id_personal = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_user = table.Column<long>(type: "bigint", nullable: true),
                    id_rol = table.Column<long>(type: "bigint", nullable: true),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    apellido = table.Column<string>(type: "text", nullable: true),
                    ci = table.Column<string>(type: "text", nullable: true),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_salida = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("personal_pkey", x => x.id_personal);
                    table.ForeignKey(
                        name: "personal_id_rol_fkey",
                        column: x => x.id_rol,
                        principalTable: "roles",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "personal_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "usuarios",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "s3_multipart_uploads_parts",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    upload_id = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    part_number = table.Column<int>(type: "integer", nullable: false),
                    bucket_id = table.Column<string>(type: "text", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    etag = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("s3_multipart_uploads_parts_pkey", x => x.id);
                    table.ForeignKey(
                        name: "s3_multipart_uploads_parts_bucket_id_fkey",
                        column: x => x.bucket_id,
                        principalSchema: "storage",
                        principalTable: "buckets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "s3_multipart_uploads_parts_upload_id_fkey",
                        column: x => x.upload_id,
                        principalSchema: "storage",
                        principalTable: "s3_multipart_uploads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "facturas",
                columns: table => new
                {
                    id_factura = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_pedido = table.Column<long>(type: "bigint", nullable: false),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    ruc = table.Column<string>(type: "text", nullable: true),
                    nombre_proveedor = table.Column<string>(type: "text", nullable: true),
                    timbrado = table.Column<string>(type: "text", nullable: true),
                    monto_total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true),
                    nro_factura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("facturas_pkey", x => x.id_factura);
                    table.ForeignKey(
                        name: "fk_factura_pedido",
                        column: x => x.id_pedido,
                        principalTable: "pedidos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_factura_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recepcion",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_pedido = table.Column<long>(type: "bigint", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false, defaultValueSql: "'Pendiente'::text"),
                    fecha_recepcion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    timbrado = table.Column<string>(type: "text", nullable: false),
                    numero_factura = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("recepcion_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_recepcion_orden",
                        column: x => x.id_orden,
                        principalTable: "ordenes",
                        principalColumn: "id_orden");
                    table.ForeignKey(
                        name: "fk_recepcion_pedido",
                        column: x => x.id_pedido,
                        principalTable: "pedidos",
                        principalColumn: "id_pedido");
                });

            migrationBuilder.CreateTable(
                name: "mfa_challenges",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    factor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ip_address = table.Column<IPAddress>(type: "inet", nullable: false),
                    otp_code = table.Column<string>(type: "text", nullable: true),
                    web_authn_session_data = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mfa_challenges_pkey", x => x.id);
                    table.ForeignKey(
                        name: "mfa_challenges_auth_factor_id_fkey",
                        column: x => x.factor_id,
                        principalSchema: "auth",
                        principalTable: "mfa_factors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "auth: stores metadata about challenge requests made");

            migrationBuilder.CreateTable(
                name: "mfa_amr_claims",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    authentication_method = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("amr_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "mfa_amr_claims_session_id_fkey",
                        column: x => x.session_id,
                        principalSchema: "auth",
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "auth: stores authenticator method reference claims for multi factor authentication");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    instance_id = table.Column<Guid>(type: "uuid", nullable: true),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    revoked = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    parent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    session_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("refresh_tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "refresh_tokens_session_id_fkey",
                        column: x => x.session_id,
                        principalSchema: "auth",
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auth: Store of tokens used to refresh JWT tokens once they expire.");

            migrationBuilder.CreateTable(
                name: "ajustes_stock",
                columns: table => new
                {
                    id_ajuste = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: true),
                    tipo_ajuste = table.Column<string>(type: "text", nullable: true),
                    id_personal = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ajustes_stock_pkey", x => x.id_ajuste);
                    table.ForeignKey(
                        name: "ajustes_stock_id_personal_fkey",
                        column: x => x.id_personal,
                        principalTable: "personal",
                        principalColumn: "id_personal");
                });

            migrationBuilder.CreateTable(
                name: "servicios_realizados",
                columns: table => new
                {
                    id_servicio_realizado = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_personal = table.Column<long>(type: "bigint", nullable: true),
                    id_servicio = table.Column<long>(type: "bigint", nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servicios_realizados_pkey", x => x.id_servicio_realizado);
                    table.ForeignKey(
                        name: "servicios_realizados_id_personal_fkey",
                        column: x => x.id_personal,
                        principalTable: "personal",
                        principalColumn: "id_personal");
                    table.ForeignKey(
                        name: "servicios_realizados_id_servicio_fkey",
                        column: x => x.id_servicio,
                        principalTable: "servicios",
                        principalColumn: "id_servicio");
                });

            migrationBuilder.CreateTable(
                name: "stock",
                columns: table => new
                {
                    id_stock = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_personal = table.Column<long>(type: "bigint", nullable: true),
                    ubicacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("stock_pkey", x => x.id_stock);
                    table.ForeignKey(
                        name: "stock_id_personal_fkey",
                        column: x => x.id_personal,
                        principalTable: "personal",
                        principalColumn: "id_personal");
                });

            migrationBuilder.CreateTable(
                name: "notas_credito",
                columns: table => new
                {
                    id_nota = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_factura = table.Column<long>(type: "bigint", nullable: false),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true),
                    ruc = table.Column<string>(type: "text", nullable: true),
                    nombre_proveedor = table.Column<string>(type: "text", nullable: true),
                    timbrado = table.Column<string>(type: "text", nullable: true),
                    monto_total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("notas_credito_pkey", x => x.id_nota);
                    table.ForeignKey(
                        name: "fk_notas_credito_factura",
                        column: x => x.id_factura,
                        principalTable: "facturas",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notas_credito_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id_producto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_categoria = table.Column<long>(type: "bigint", nullable: true),
                    id_stock = table.Column<long>(type: "bigint", nullable: true),
                    nombre = table.Column<string>(type: "text", nullable: true),
                    marca = table.Column<string>(type: "text", nullable: true),
                    cantidad_total = table.Column<int>(type: "integer", nullable: true),
                    cantidad_minima = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("productos_pkey", x => x.id_producto);
                    table.ForeignKey(
                        name: "productos_id_categoria_fkey",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "productos_id_stock_fkey",
                        column: x => x.id_stock,
                        principalTable: "stock",
                        principalColumn: "id_stock");
                });

            migrationBuilder.CreateTable(
                name: "asientos",
                columns: table => new
                {
                    id_asiento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: false),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_nota = table.Column<long>(type: "bigint", nullable: false),
                    monto_total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("asientos_pkey", x => x.id_asiento);
                    table.ForeignKey(
                        name: "fk_asiento_nota",
                        column: x => x.id_nota,
                        principalTable: "notas_credito",
                        principalColumn: "id_nota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asiento_orden",
                        column: x => x.id_orden,
                        principalTable: "ordenes",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asiento_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bajas_producto",
                columns: table => new
                {
                    id_baja_producto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_producto = table.Column<long>(type: "bigint", nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bajas_producto_pkey", x => x.id_baja_producto);
                    table.ForeignKey(
                        name: "bajas_producto_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "detalle_ajuste_stock",
                columns: table => new
                {
                    id_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_ajuste = table.Column<long>(type: "bigint", nullable: true),
                    id_producto = table.Column<long>(type: "bigint", nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("detalle_ajuste_stock_pkey", x => x.id_detalle);
                    table.ForeignKey(
                        name: "detalle_ajuste_stock_id_ajuste_fkey",
                        column: x => x.id_ajuste,
                        principalTable: "ajustes_stock",
                        principalColumn: "id_ajuste");
                    table.ForeignKey(
                        name: "detalle_ajuste_stock_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "factura_detalle",
                columns: table => new
                {
                    id_factura_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_factura = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("factura_detalle_pkey", x => x.id_factura_detalle);
                    table.ForeignKey(
                        name: "fk_factura_detalle_factura",
                        column: x => x.id_factura,
                        principalTable: "facturas",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_factura_detalle_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nota_credito_detalle",
                columns: table => new
                {
                    id_nota_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_nota = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    precio = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("nota_credito_detalle_pkey", x => x.id_nota_detalle);
                    table.ForeignKey(
                        name: "fk_nota_credito_detalle_nota",
                        column: x => x.id_nota,
                        principalTable: "notas_credito",
                        principalColumn: "id_nota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_nota_credito_detalle_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nota_de_devolucion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pedido_id = table.Column<long>(type: "bigint", nullable: true),
                    producto_id = table.Column<long>(type: "bigint", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: false),
                    fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("nota_de_devolucion_pkey", x => x.id);
                    table.ForeignKey(
                        name: "nota_de_devolucion_pedido_id_fkey",
                        column: x => x.pedido_id,
                        principalTable: "pedidos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "nota_de_devolucion_producto_id_fkey",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orden_detalle",
                columns: table => new
                {
                    id_orden_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orden_detalle_pkey", x => x.id_orden_detalle);
                    table.ForeignKey(
                        name: "fk_orden_detalle_orden",
                        column: x => x.id_orden,
                        principalTable: "ordenes",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orden_detalle_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pedido_detalles",
                columns: table => new
                {
                    id_pedido_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_pedido = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    cotizacion = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true),
                    iva = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pedido_detalles_pkey", x => x.id_pedido_detalle);
                    table.ForeignKey(
                        name: "fk_pedido_detalles_pedido",
                        column: x => x.id_pedido,
                        principalTable: "pedidos",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pedido_detalles_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "precios",
                columns: table => new
                {
                    id_precio = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_producto = table.Column<long>(type: "bigint", nullable: true),
                    precio = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fecha_registro = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("precios_pkey", x => x.id_precio);
                    table.ForeignKey(
                        name: "precios_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "presupuesto_detalle",
                columns: table => new
                {
                    id_presupuesto_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_presupuesto = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: true),
                    precio = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva5 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    iva10 = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("presupuesto_detalle_pkey", x => x.id_presupuesto_detalle);
                    table.ForeignKey(
                        name: "fk_pd_presupuesto",
                        column: x => x.id_presupuesto,
                        principalTable: "presupuestos",
                        principalColumn: "id_presupuesto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pd_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "producto_proveedor",
                columns: table => new
                {
                    id_producto_proveedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_proveedor = table.Column<long>(type: "bigint", nullable: true),
                    id_producto = table.Column<long>(type: "bigint", nullable: true),
                    fecha_compra = table.Column<DateOnly>(type: "date", nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("producto_proveedor_pkey", x => x.id_producto_proveedor);
                    table.ForeignKey(
                        name: "producto_proveedor_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "producto_proveedor_id_proveedor_fkey",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor");
                });

            migrationBuilder.CreateTable(
                name: "recepcion_detalle",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_recepcion = table.Column<long>(type: "bigint", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false),
                    cantidad_recibida = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("recepcion_detalle_pkey", x => x.id);
                    table.ForeignKey(
                        name: "recepcion_detalle_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "recepcion_detalle_id_recepcion_fkey",
                        column: x => x.id_recepcion,
                        principalTable: "recepcion",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "servicios_productos_utilizados",
                columns: table => new
                {
                    id_servicio_producto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_servicio_realizado = table.Column<long>(type: "bigint", nullable: true),
                    id_producto = table.Column<long>(type: "bigint", nullable: true),
                    cantidad = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servicios_productos_utilizados_pkey", x => x.id_servicio_producto);
                    table.ForeignKey(
                        name: "servicios_productos_utilizados_id_producto_fkey",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "servicios_productos_utilizados_id_servicio_realizado_fkey",
                        column: x => x.id_servicio_realizado,
                        principalTable: "servicios_realizados",
                        principalColumn: "id_servicio_realizado");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ajustes_stock_id_personal",
                table: "ajustes_stock",
                column: "id_personal");

            migrationBuilder.CreateIndex(
                name: "IX_asientos_id_nota",
                table: "asientos",
                column: "id_nota");

            migrationBuilder.CreateIndex(
                name: "IX_asientos_id_orden",
                table: "asientos",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_asientos_id_proveedor",
                table: "asientos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "audit_logs_instance_id_idx",
                schema: "auth",
                table: "audit_log_entries",
                column: "instance_id");

            migrationBuilder.CreateIndex(
                name: "IX_bajas_producto_id_producto",
                table: "bajas_producto",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "bname",
                schema: "storage",
                table: "buckets",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categoria_proveedor_id_categoria",
                table: "categoria_proveedor",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_categoria_proveedor_id_proveedor",
                table: "categoria_proveedor",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "proveedor_unico",
                table: "contador_factura",
                column: "id_proveedor",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ajuste_stock_id_ajuste",
                table: "detalle_ajuste_stock",
                column: "id_ajuste");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ajuste_stock_id_producto",
                table: "detalle_ajuste_stock",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_factura_detalle_id_factura",
                table: "factura_detalle",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "IX_factura_detalle_id_producto",
                table: "factura_detalle",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_facturas_id_pedido",
                table: "facturas",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_facturas_id_proveedor",
                table: "facturas",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "flow_state_created_at_idx",
                schema: "auth",
                table: "flow_state",
                column: "created_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "idx_auth_code",
                schema: "auth",
                table: "flow_state",
                column: "auth_code");

            migrationBuilder.CreateIndex(
                name: "idx_user_id_auth_method",
                schema: "auth",
                table: "flow_state",
                columns: new[] { "user_id", "authentication_method" });

            migrationBuilder.CreateIndex(
                name: "identities_email_idx",
                schema: "auth",
                table: "identities",
                column: "email")
                .Annotation("Npgsql:IndexOperators", new[] { "text_pattern_ops" });

            migrationBuilder.CreateIndex(
                name: "identities_provider_id_provider_unique",
                schema: "auth",
                table: "identities",
                columns: new[] { "provider_id", "provider" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "identities_user_id_idx",
                schema: "auth",
                table: "identities",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "mfa_amr_claims_session_id_authentication_method_pkey",
                schema: "auth",
                table: "mfa_amr_claims",
                columns: new[] { "session_id", "authentication_method" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mfa_challenges_factor_id",
                schema: "auth",
                table: "mfa_challenges",
                column: "factor_id");

            migrationBuilder.CreateIndex(
                name: "mfa_challenge_created_at_idx",
                schema: "auth",
                table: "mfa_challenges",
                column: "created_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "factor_id_created_at_idx",
                schema: "auth",
                table: "mfa_factors",
                columns: new[] { "user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "mfa_factors_last_challenged_at_key",
                schema: "auth",
                table: "mfa_factors",
                column: "last_challenged_at",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "mfa_factors_user_friendly_name_unique",
                schema: "auth",
                table: "mfa_factors",
                columns: new[] { "friendly_name", "user_id" },
                unique: true,
                filter: "(TRIM(BOTH FROM friendly_name) <> ''::text)");

            migrationBuilder.CreateIndex(
                name: "mfa_factors_user_id_idx",
                schema: "auth",
                table: "mfa_factors",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "unique_phone_factor_per_user",
                schema: "auth",
                table: "mfa_factors",
                columns: new[] { "user_id", "phone" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "migrations_name_key",
                schema: "storage",
                table: "migrations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_nota_credito_detalle_id_nota",
                table: "nota_credito_detalle",
                column: "id_nota");

            migrationBuilder.CreateIndex(
                name: "IX_nota_credito_detalle_id_producto",
                table: "nota_credito_detalle",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_nota_de_devolucion_pedido_id",
                table: "nota_de_devolucion",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_nota_de_devolucion_producto_id",
                table: "nota_de_devolucion",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_notas_credito_id_factura",
                table: "notas_credito",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "IX_notas_credito_id_proveedor",
                table: "notas_credito",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "bucketid_objname",
                schema: "storage",
                table: "objects",
                columns: new[] { "bucket_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_objects_bucket_id_name",
                schema: "storage",
                table: "objects",
                columns: new[] { "bucket_id", "name" })
                .Annotation("Relational:Collation", new[] { null, "C" });

            migrationBuilder.CreateIndex(
                name: "name_prefix_search",
                schema: "storage",
                table: "objects",
                column: "name")
                .Annotation("Npgsql:IndexOperators", new[] { "text_pattern_ops" });

            migrationBuilder.CreateIndex(
                name: "IX_one_time_tokens_user_id",
                schema: "auth",
                table: "one_time_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "one_time_tokens_relates_to_hash_idx",
                schema: "auth",
                table: "one_time_tokens",
                column: "relates_to")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "one_time_tokens_token_hash_hash_idx",
                schema: "auth",
                table: "one_time_tokens",
                column: "token_hash")
                .Annotation("Npgsql:IndexMethod", "hash");

            migrationBuilder.CreateIndex(
                name: "IX_orden_detalle_id_orden",
                table: "orden_detalle",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_orden_detalle_id_producto",
                table: "orden_detalle",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_detalles_id_pedido",
                table: "pedido_detalles",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_detalles_id_producto",
                table: "pedido_detalles",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_id_orden",
                table: "pedidos",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_pedidos_id_proveedor",
                table: "pedidos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_personal_id_rol",
                table: "personal",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_personal_id_user",
                table: "personal",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_precios_id_producto",
                table: "precios",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_presupuesto_detalle_id_presupuesto",
                table: "presupuesto_detalle",
                column: "id_presupuesto");

            migrationBuilder.CreateIndex(
                name: "IX_presupuesto_detalle_id_producto",
                table: "presupuesto_detalle",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_presupuestos_id_proveedor",
                table: "presupuestos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_presupuestos_proveedores_proveedor_id",
                table: "presupuestos_proveedores",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_producto_proveedor_id_producto",
                table: "producto_proveedor",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_producto_proveedor_id_proveedor",
                table: "producto_proveedor",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_categoria",
                table: "productos",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_stock",
                table: "productos",
                column: "id_stock");

            migrationBuilder.CreateIndex(
                name: "IX_recepcion_id_orden",
                table: "recepcion",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_recepcion_id_pedido",
                table: "recepcion",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "IX_recepcion_detalle_id_producto",
                table: "recepcion_detalle",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_recepcion_detalle_id_recepcion",
                table: "recepcion_detalle",
                column: "id_recepcion");

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_instance_id_idx",
                schema: "auth",
                table: "refresh_tokens",
                column: "instance_id");

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_instance_id_user_id_idx",
                schema: "auth",
                table: "refresh_tokens",
                columns: new[] { "instance_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_parent_idx",
                schema: "auth",
                table: "refresh_tokens",
                column: "parent");

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_session_id_revoked_idx",
                schema: "auth",
                table: "refresh_tokens",
                columns: new[] { "session_id", "revoked" });

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_token_unique",
                schema: "auth",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "refresh_tokens_updated_at_idx",
                schema: "auth",
                table: "refresh_tokens",
                column: "updated_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "idx_multipart_uploads_list",
                schema: "storage",
                table: "s3_multipart_uploads",
                columns: new[] { "bucket_id", "key", "created_at" })
                .Annotation("Relational:Collation", new[] { null, "C", null });

            migrationBuilder.CreateIndex(
                name: "IX_s3_multipart_uploads_parts_bucket_id",
                schema: "storage",
                table: "s3_multipart_uploads_parts",
                column: "bucket_id");

            migrationBuilder.CreateIndex(
                name: "IX_s3_multipart_uploads_parts_upload_id",
                schema: "storage",
                table: "s3_multipart_uploads_parts",
                column: "upload_id");

            migrationBuilder.CreateIndex(
                name: "saml_providers_entity_id_key",
                schema: "auth",
                table: "saml_providers",
                column: "entity_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "saml_providers_sso_provider_id_idx",
                schema: "auth",
                table: "saml_providers",
                column: "sso_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_saml_relay_states_flow_state_id",
                schema: "auth",
                table: "saml_relay_states",
                column: "flow_state_id");

            migrationBuilder.CreateIndex(
                name: "saml_relay_states_created_at_idx",
                schema: "auth",
                table: "saml_relay_states",
                column: "created_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "saml_relay_states_for_email_idx",
                schema: "auth",
                table: "saml_relay_states",
                column: "for_email");

            migrationBuilder.CreateIndex(
                name: "saml_relay_states_sso_provider_id_idx",
                schema: "auth",
                table: "saml_relay_states",
                column: "sso_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_productos_utilizados_id_producto",
                table: "servicios_productos_utilizados",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_productos_utilizados_id_servicio_realizado",
                table: "servicios_productos_utilizados",
                column: "id_servicio_realizado");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_realizados_id_personal",
                table: "servicios_realizados",
                column: "id_personal");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_realizados_id_servicio",
                table: "servicios_realizados",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "sessions_not_after_idx",
                schema: "auth",
                table: "sessions",
                column: "not_after",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "sessions_user_id_idx",
                schema: "auth",
                table: "sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_id_created_at_idx",
                schema: "auth",
                table: "sessions",
                columns: new[] { "user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "sso_domains_sso_provider_id_idx",
                schema: "auth",
                table: "sso_domains",
                column: "sso_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_id_personal",
                table: "stock",
                column: "id_personal");

            migrationBuilder.CreateIndex(
                name: "confirmation_token_idx",
                schema: "auth",
                table: "users",
                column: "confirmation_token",
                unique: true,
                filter: "((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

            migrationBuilder.CreateIndex(
                name: "email_change_token_current_idx",
                schema: "auth",
                table: "users",
                column: "email_change_token_current",
                unique: true,
                filter: "((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

            migrationBuilder.CreateIndex(
                name: "email_change_token_new_idx",
                schema: "auth",
                table: "users",
                column: "email_change_token_new",
                unique: true,
                filter: "((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

            migrationBuilder.CreateIndex(
                name: "reauthentication_token_idx",
                schema: "auth",
                table: "users",
                column: "reauthentication_token",
                unique: true,
                filter: "((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

            migrationBuilder.CreateIndex(
                name: "recovery_token_idx",
                schema: "auth",
                table: "users",
                column: "recovery_token",
                unique: true,
                filter: "((recovery_token)::text !~ '^[0-9 ]*$'::text)");

            migrationBuilder.CreateIndex(
                name: "users_email_partial_key",
                schema: "auth",
                table: "users",
                column: "email",
                unique: true,
                filter: "(is_sso_user = false)");

            migrationBuilder.CreateIndex(
                name: "users_instance_id_idx",
                schema: "auth",
                table: "users",
                column: "instance_id");

            migrationBuilder.CreateIndex(
                name: "users_is_anonymous_idx",
                schema: "auth",
                table: "users",
                column: "is_anonymous");

            migrationBuilder.CreateIndex(
                name: "users_phone_key",
                schema: "auth",
                table: "users",
                column: "phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asientos");

            migrationBuilder.DropTable(
                name: "audit_log_entries",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "bajas_producto");

            migrationBuilder.DropTable(
                name: "categoria_proveedor");

            migrationBuilder.DropTable(
                name: "contador_factura");

            migrationBuilder.DropTable(
                name: "detalle_ajuste_stock");

            migrationBuilder.DropTable(
                name: "embeddings",
                schema: "meta");

            migrationBuilder.DropTable(
                name: "factura_detalle");

            migrationBuilder.DropTable(
                name: "identities",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "instances",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "mfa_amr_claims",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "mfa_challenges",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "migrations",
                schema: "meta");

            migrationBuilder.DropTable(
                name: "migrations",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "nota_credito_detalle");

            migrationBuilder.DropTable(
                name: "nota_de_devolucion");

            migrationBuilder.DropTable(
                name: "objects",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "one_time_tokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "orden_detalle");

            migrationBuilder.DropTable(
                name: "pedido_detalles");

            migrationBuilder.DropTable(
                name: "precios");

            migrationBuilder.DropTable(
                name: "presupuesto_detalle");

            migrationBuilder.DropTable(
                name: "presupuestos_proveedores");

            migrationBuilder.DropTable(
                name: "producto_proveedor");

            migrationBuilder.DropTable(
                name: "recepcion_detalle");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "s3_multipart_uploads_parts",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "saml_providers",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "saml_relay_states",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "schema_migrations",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "schema_migrations",
                schema: "realtime");

            migrationBuilder.DropTable(
                name: "servicios_productos_utilizados");

            migrationBuilder.DropTable(
                name: "sso_domains",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "subscription",
                schema: "realtime");

            migrationBuilder.DropTable(
                name: "ajustes_stock");

            migrationBuilder.DropTable(
                name: "mfa_factors",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "notas_credito");

            migrationBuilder.DropTable(
                name: "presupuestos");

            migrationBuilder.DropTable(
                name: "recepcion");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "s3_multipart_uploads",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "flow_state",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "servicios_realizados");

            migrationBuilder.DropTable(
                name: "sso_providers",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "facturas");

            migrationBuilder.DropTable(
                name: "users",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "buckets",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "stock");

            migrationBuilder.DropTable(
                name: "servicios");

            migrationBuilder.DropTable(
                name: "pedidos");

            migrationBuilder.DropTable(
                name: "personal");

            migrationBuilder.DropTable(
                name: "ordenes");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropSequence(
                name: "seq_schema_version",
                schema: "graphql");
        }
    }
}
