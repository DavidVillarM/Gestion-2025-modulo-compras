--
-- PostgreSQL database dump
--

-- Dumped from database version 16.3 (PGlite 0.2.0)
-- Dumped by pg_dump version 16.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'SQL_ASCII';
SET standard_conforming_strings = off;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET escape_string_warning = off;
SET row_security = off;

--
-- Name: meta; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA meta;


ALTER SCHEMA meta OWNER TO postgres;

--
-- Name: vector; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS vector WITH SCHEMA public;


--
-- Name: EXTENSION vector; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION vector IS 'vector data type and ivfflat and hnsw access methods';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: embeddings; Type: TABLE; Schema: meta; Owner: postgres
--

CREATE TABLE meta.embeddings (
    id bigint NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    content text NOT NULL,
    embedding public.vector(384) NOT NULL
);


ALTER TABLE meta.embeddings OWNER TO postgres;

--
-- Name: embeddings_id_seq; Type: SEQUENCE; Schema: meta; Owner: postgres
--

ALTER TABLE meta.embeddings ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME meta.embeddings_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: migrations; Type: TABLE; Schema: meta; Owner: postgres
--

CREATE TABLE meta.migrations (
    version text NOT NULL,
    name text,
    applied_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE meta.migrations OWNER TO postgres;

--
-- Name: ajustes_stock; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ajustes_stock (
    id_ajuste bigint NOT NULL,
    fecha date,
    motivo text,
    tipo_ajuste text,
    id_personal bigint
);


ALTER TABLE public.ajustes_stock OWNER TO postgres;

--
-- Name: ajustes_stock_id_ajuste_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ajustes_stock ALTER COLUMN id_ajuste ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.ajustes_stock_id_ajuste_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: bajas_producto; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bajas_producto (
    id_baja_producto bigint NOT NULL,
    id_producto bigint,
    cantidad integer,
    motivo text
);


ALTER TABLE public.bajas_producto OWNER TO postgres;

--
-- Name: bajas_producto_id_baja_producto_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.bajas_producto ALTER COLUMN id_baja_producto ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.bajas_producto_id_baja_producto_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: categoria; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categoria (
    id_categoria bigint NOT NULL,
    nombre text,
    descripcion text
);


ALTER TABLE public.categoria OWNER TO postgres;

--
-- Name: categoria_id_categoria_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.categoria ALTER COLUMN id_categoria ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.categoria_id_categoria_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: categoria_proveedor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categoria_proveedor (
    id_categoria_proveedor bigint NOT NULL,
    id_categoria bigint,
    id_proveedor bigint,
    estado text
);


ALTER TABLE public.categoria_proveedor OWNER TO postgres;

--
-- Name: categoria_proveedor_id_categoria_proveedor_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.categoria_proveedor ALTER COLUMN id_categoria_proveedor ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.categoria_proveedor_id_categoria_proveedor_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: detalle_ajuste_stock; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.detalle_ajuste_stock (
    id_detalle bigint NOT NULL,
    id_ajuste bigint,
    id_producto bigint,
    cantidad integer
);


ALTER TABLE public.detalle_ajuste_stock OWNER TO postgres;

--
-- Name: detalle_ajuste_stock_id_detalle_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.detalle_ajuste_stock ALTER COLUMN id_detalle ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.detalle_ajuste_stock_id_detalle_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: factura_detalle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.factura_detalle (
    id_factura_detalle bigint NOT NULL,
    id_producto bigint,
    precio numeric(10,2),
    cantidad integer,
    iva5 numeric(10,2),
    iva10 numeric(10,2)
);


ALTER TABLE public.factura_detalle OWNER TO postgres;

--
-- Name: factura_detalle_id_factura_detalle_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.factura_detalle ALTER COLUMN id_factura_detalle ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.factura_detalle_id_factura_detalle_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: facturas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.facturas (
    id_factura bigint NOT NULL,
    id_pedido bigint,
    id_proveedor bigint,
    fecha date,
    ruc text,
    nombre_proveedor text,
    timbrado text,
    monto_total numeric(10,2),
    subtotal numeric(10,2),
    iva5 numeric(10,2),
    iva10 numeric(10,2),
    estado text
);


ALTER TABLE public.facturas OWNER TO postgres;

--
-- Name: facturas_id_factura_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.facturas ALTER COLUMN id_factura ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.facturas_id_factura_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: nota_credito_detalle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.nota_credito_detalle (
    id_factura_detalle bigint NOT NULL,
    id_producto bigint,
    precio numeric(10,2),
    cantidad integer,
    iva5 numeric(10,2),
    iva10 numeric(10,2)
);


ALTER TABLE public.nota_credito_detalle OWNER TO postgres;

--
-- Name: nota_credito_detalle_id_factura_detalle_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.nota_credito_detalle ALTER COLUMN id_factura_detalle ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.nota_credito_detalle_id_factura_detalle_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: notas_credito; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notas_credito (
    id_factura bigint NOT NULL,
    id_pedido bigint,
    id_proveedor bigint,
    fecha date,
    ruc text,
    nombre_proveedor text,
    timbrado text,
    monto_total numeric(10,2),
    subtotal numeric(10,2),
    iva5 numeric(10,2),
    iva10 numeric(10,2),
    estado text
);


ALTER TABLE public.notas_credito OWNER TO postgres;

--
-- Name: notas_credito_id_factura_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.notas_credito ALTER COLUMN id_factura ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.notas_credito_id_factura_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: orden_detalle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orden_detalle (
    id_orden_detalle bigint NOT NULL,
    id_producto bigint,
    id_orden bigint,
    cotizacion numeric(10,2),
    cantidad integer,
    iva numeric(10,2)
);


ALTER TABLE public.orden_detalle OWNER TO postgres;

--
-- Name: orden_detalle_id_orden_detalle_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.orden_detalle ALTER COLUMN id_orden_detalle ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.orden_detalle_id_orden_detalle_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ordenes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ordenes (
    id_orden bigint NOT NULL,
    estado text
);


ALTER TABLE public.ordenes OWNER TO postgres;

--
-- Name: ordenes_id_orden_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.ordenes ALTER COLUMN id_orden ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.ordenes_id_orden_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: pedidos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pedidos (
    id_pedido bigint NOT NULL,
    id_orden bigint,
    id_proveedor bigint,
    monto_total numeric(10,2),
    fecha_entrega date,
    fecha_pedido date,
    estado text
);


ALTER TABLE public.pedidos OWNER TO postgres;

--
-- Name: pedidos_id_pedido_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.pedidos ALTER COLUMN id_pedido ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.pedidos_id_pedido_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: personal; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.personal (
    id_personal bigint NOT NULL,
    id_user bigint,
    id_rol bigint,
    nombre text,
    apellido text,
    ci text,
    fecha_ingreso date,
    fecha_salida date,
    estado text
);


ALTER TABLE public.personal OWNER TO postgres;

--
-- Name: personal_id_personal_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.personal ALTER COLUMN id_personal ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.personal_id_personal_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: precios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.precios (
    id_precio bigint NOT NULL,
    id_producto bigint,
    precio numeric(10,2),
    fecha_registro date,
    estado text
);


ALTER TABLE public.precios OWNER TO postgres;

--
-- Name: precios_id_precio_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.precios ALTER COLUMN id_precio ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.precios_id_precio_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: presupuesto_detalle; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.presupuesto_detalle (
    id_presupuesto_detalle bigint NOT NULL,
    id_presupuesto bigint,
    id_producto bigint,
    cantidad integer,
    precio numeric(10,2),
    iva5 numeric(10,2),
    iva10 numeric(10,2)
);


ALTER TABLE public.presupuesto_detalle OWNER TO postgres;

--
-- Name: presupuesto_detalle_id_presupuesto_detalle_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.presupuesto_detalle ALTER COLUMN id_presupuesto_detalle ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.presupuesto_detalle_id_presupuesto_detalle_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: presupuestos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.presupuestos (
    id_presupuesto bigint NOT NULL,
    id_orden bigint,
    id_proveedor bigint,
    fecha_entrega date,
    subtotal numeric(10,2),
    iva5 numeric(10,2),
    iva10 numeric(10,2),
    total numeric(10,2)
);


ALTER TABLE public.presupuestos OWNER TO postgres;

--
-- Name: presupuestos_id_presupuesto_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.presupuestos ALTER COLUMN id_presupuesto ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.presupuestos_id_presupuesto_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: producto_proveedor; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.producto_proveedor (
    id_producto_proveedor bigint NOT NULL,
    id_proveedor bigint,
    id_producto bigint,
    fecha_compra date,
    cantidad integer
);


ALTER TABLE public.producto_proveedor OWNER TO postgres;

--
-- Name: producto_proveedor_id_producto_proveedor_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.producto_proveedor ALTER COLUMN id_producto_proveedor ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.producto_proveedor_id_producto_proveedor_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: productos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.productos (
    id_producto bigint NOT NULL,
    id_categoria bigint,
    id_stock bigint,
    nombre text,
    marca text,
    cantidad_total integer,
    cantidad_minima integer
);


ALTER TABLE public.productos OWNER TO postgres;

--
-- Name: productos_id_producto_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.productos ALTER COLUMN id_producto ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.productos_id_producto_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: proveedores; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.proveedores (
    id_proveedor bigint NOT NULL,
    ruc text,
    nombre text,
    telefono text,
    correo text,
    nombre_contacto text
);


ALTER TABLE public.proveedores OWNER TO postgres;

--
-- Name: proveedores_id_proveedor_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.proveedores ALTER COLUMN id_proveedor ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.proveedores_id_proveedor_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id_rol bigint NOT NULL,
    descripcion text
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- Name: roles_id_rol_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.roles ALTER COLUMN id_rol ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.roles_id_rol_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: servicios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servicios (
    id_servicio bigint NOT NULL,
    nombre text,
    descripcion text,
    costo numeric(10,2)
);


ALTER TABLE public.servicios OWNER TO postgres;

--
-- Name: servicios_id_servicio_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.servicios ALTER COLUMN id_servicio ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.servicios_id_servicio_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: servicios_productos_utilizados; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servicios_productos_utilizados (
    id_servicio_producto bigint NOT NULL,
    id_servicio_realizado bigint,
    id_producto bigint,
    cantidad integer
);


ALTER TABLE public.servicios_productos_utilizados OWNER TO postgres;

--
-- Name: servicios_productos_utilizados_id_servicio_producto_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.servicios_productos_utilizados ALTER COLUMN id_servicio_producto ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.servicios_productos_utilizados_id_servicio_producto_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: servicios_realizados; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servicios_realizados (
    id_servicio_realizado bigint NOT NULL,
    id_personal bigint,
    id_servicio bigint,
    fecha date,
    estado text
);


ALTER TABLE public.servicios_realizados OWNER TO postgres;

--
-- Name: servicios_realizados_id_servicio_realizado_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.servicios_realizados ALTER COLUMN id_servicio_realizado ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.servicios_realizados_id_servicio_realizado_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: stock; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.stock (
    id_stock bigint NOT NULL,
    id_personal bigint,
    ubicacion text
);


ALTER TABLE public.stock OWNER TO postgres;

--
-- Name: stock_id_stock_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.stock ALTER COLUMN id_stock ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.stock_id_stock_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios (
    id_user bigint NOT NULL,
    username text,
    password text
);


ALTER TABLE public.usuarios OWNER TO postgres;

--
-- Name: usuarios_id_user_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.usuarios ALTER COLUMN id_user ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.usuarios_id_user_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: embeddings; Type: TABLE DATA; Schema: meta; Owner: postgres
--



--
-- Data for Name: migrations; Type: TABLE DATA; Schema: meta; Owner: postgres
--

INSERT INTO meta.migrations VALUES ('202407160001', 'embeddings', '2025-04-07 17:26:12.885+00');


--
-- Data for Name: ajustes_stock; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: bajas_producto; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: categoria; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.categoria OVERRIDING SYSTEM VALUE VALUES (1, 'Electronics', 'Electronic items');
INSERT INTO public.categoria OVERRIDING SYSTEM VALUE VALUES (2, 'Furniture', 'Furniture items');


--
-- Data for Name: categoria_proveedor; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: detalle_ajuste_stock; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: factura_detalle; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: facturas; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: nota_credito_detalle; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: notas_credito; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: orden_detalle; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: ordenes; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: pedidos; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: personal; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.personal OVERRIDING SYSTEM VALUE VALUES (1, 1, 1, 'John', 'Doe', '12345678', '2022-01-01', NULL, 'Active');
INSERT INTO public.personal OVERRIDING SYSTEM VALUE VALUES (2, 2, 2, 'Alice', 'Smith', '87654321', '2023-01-01', NULL, 'Active');


--
-- Data for Name: precios; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: presupuesto_detalle; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: presupuestos; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: producto_proveedor; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: productos; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.productos OVERRIDING SYSTEM VALUE VALUES (1, 1, 1, 'Laptop', 'BrandX', 50, 5);
INSERT INTO public.productos OVERRIDING SYSTEM VALUE VALUES (2, 2, 2, 'Chair', 'BrandY', 100, 10);


--
-- Data for Name: proveedores; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.proveedores OVERRIDING SYSTEM VALUE VALUES (1, '800123456', 'Proveedor A', '0981123456', 'contacto@proveedora.com', 'Juan Pérez');
INSERT INTO public.proveedores OVERRIDING SYSTEM VALUE VALUES (2, '800654321', 'Proveedor B', '0981654321', 'contacto@proveedorb.com', 'María López');
INSERT INTO public.proveedores OVERRIDING SYSTEM VALUE VALUES (3, '800987654', 'Proveedor C', '0981987654', 'contacto@proveedorc.com', 'Carlos Gómez');
INSERT INTO public.proveedores OVERRIDING SYSTEM VALUE VALUES (4, '800456789', 'Proveedor D', '0981456789', 'contacto@proveedord.com', 'Ana Torres');
INSERT INTO public.proveedores OVERRIDING SYSTEM VALUE VALUES (5, '800321987', 'Proveedor E', '0981321987', 'contacto@proveedore.com', 'Luis Fernández');


--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.roles OVERRIDING SYSTEM VALUE VALUES (1, 'Manager');
INSERT INTO public.roles OVERRIDING SYSTEM VALUE VALUES (2, 'Employee');


--
-- Data for Name: servicios; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: servicios_productos_utilizados; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: servicios_realizados; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: stock; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.stock OVERRIDING SYSTEM VALUE VALUES (1, 1, 'Warehouse A');
INSERT INTO public.stock OVERRIDING SYSTEM VALUE VALUES (2, 2, 'Warehouse B');


--
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.usuarios OVERRIDING SYSTEM VALUE VALUES (1, 'jdoe', 'password123');
INSERT INTO public.usuarios OVERRIDING SYSTEM VALUE VALUES (2, 'asmith', 'password456');


--
-- Name: embeddings_id_seq; Type: SEQUENCE SET; Schema: meta; Owner: postgres
--

SELECT pg_catalog.setval('meta.embeddings_id_seq', 1, false);


--
-- Name: ajustes_stock_id_ajuste_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ajustes_stock_id_ajuste_seq', 1, false);


--
-- Name: bajas_producto_id_baja_producto_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.bajas_producto_id_baja_producto_seq', 1, false);


--
-- Name: categoria_id_categoria_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categoria_id_categoria_seq', 2, true);


--
-- Name: categoria_proveedor_id_categoria_proveedor_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categoria_proveedor_id_categoria_proveedor_seq', 1, false);


--
-- Name: detalle_ajuste_stock_id_detalle_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.detalle_ajuste_stock_id_detalle_seq', 1, false);


--
-- Name: factura_detalle_id_factura_detalle_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.factura_detalle_id_factura_detalle_seq', 1, false);


--
-- Name: facturas_id_factura_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.facturas_id_factura_seq', 1, false);


--
-- Name: nota_credito_detalle_id_factura_detalle_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.nota_credito_detalle_id_factura_detalle_seq', 1, false);


--
-- Name: notas_credito_id_factura_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notas_credito_id_factura_seq', 1, false);


--
-- Name: orden_detalle_id_orden_detalle_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orden_detalle_id_orden_detalle_seq', 1, false);


--
-- Name: ordenes_id_orden_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ordenes_id_orden_seq', 1, false);


--
-- Name: pedidos_id_pedido_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pedidos_id_pedido_seq', 1, false);


--
-- Name: personal_id_personal_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.personal_id_personal_seq', 2, true);


--
-- Name: precios_id_precio_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.precios_id_precio_seq', 1, false);


--
-- Name: presupuesto_detalle_id_presupuesto_detalle_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.presupuesto_detalle_id_presupuesto_detalle_seq', 1, false);


--
-- Name: presupuestos_id_presupuesto_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.presupuestos_id_presupuesto_seq', 1, false);


--
-- Name: producto_proveedor_id_producto_proveedor_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.producto_proveedor_id_producto_proveedor_seq', 1, false);


--
-- Name: productos_id_producto_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.productos_id_producto_seq', 2, true);


--
-- Name: proveedores_id_proveedor_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.proveedores_id_proveedor_seq', 5, true);


--
-- Name: roles_id_rol_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_rol_seq', 2, true);


--
-- Name: servicios_id_servicio_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.servicios_id_servicio_seq', 1, false);


--
-- Name: servicios_productos_utilizados_id_servicio_producto_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.servicios_productos_utilizados_id_servicio_producto_seq', 1, false);


--
-- Name: servicios_realizados_id_servicio_realizado_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.servicios_realizados_id_servicio_realizado_seq', 1, false);


--
-- Name: stock_id_stock_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.stock_id_stock_seq', 2, true);


--
-- Name: usuarios_id_user_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.usuarios_id_user_seq', 2, true);


--
-- Name: embeddings embeddings_pkey; Type: CONSTRAINT; Schema: meta; Owner: postgres
--

ALTER TABLE ONLY meta.embeddings
    ADD CONSTRAINT embeddings_pkey PRIMARY KEY (id);


--
-- Name: migrations migrations_pkey; Type: CONSTRAINT; Schema: meta; Owner: postgres
--

ALTER TABLE ONLY meta.migrations
    ADD CONSTRAINT migrations_pkey PRIMARY KEY (version);


--
-- Name: ajustes_stock ajustes_stock_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ajustes_stock
    ADD CONSTRAINT ajustes_stock_pkey PRIMARY KEY (id_ajuste);


--
-- Name: bajas_producto bajas_producto_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bajas_producto
    ADD CONSTRAINT bajas_producto_pkey PRIMARY KEY (id_baja_producto);


--
-- Name: categoria categoria_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoria
    ADD CONSTRAINT categoria_pkey PRIMARY KEY (id_categoria);


--
-- Name: categoria_proveedor categoria_proveedor_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoria_proveedor
    ADD CONSTRAINT categoria_proveedor_pkey PRIMARY KEY (id_categoria_proveedor);


--
-- Name: detalle_ajuste_stock detalle_ajuste_stock_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_ajuste_stock
    ADD CONSTRAINT detalle_ajuste_stock_pkey PRIMARY KEY (id_detalle);


--
-- Name: factura_detalle factura_detalle_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.factura_detalle
    ADD CONSTRAINT factura_detalle_pkey PRIMARY KEY (id_factura_detalle);


--
-- Name: facturas facturas_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facturas
    ADD CONSTRAINT facturas_pkey PRIMARY KEY (id_factura);


--
-- Name: nota_credito_detalle nota_credito_detalle_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nota_credito_detalle
    ADD CONSTRAINT nota_credito_detalle_pkey PRIMARY KEY (id_factura_detalle);


--
-- Name: notas_credito notas_credito_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notas_credito
    ADD CONSTRAINT notas_credito_pkey PRIMARY KEY (id_factura);


--
-- Name: orden_detalle orden_detalle_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orden_detalle
    ADD CONSTRAINT orden_detalle_pkey PRIMARY KEY (id_orden_detalle);


--
-- Name: ordenes ordenes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ordenes
    ADD CONSTRAINT ordenes_pkey PRIMARY KEY (id_orden);


--
-- Name: pedidos pedidos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT pedidos_pkey PRIMARY KEY (id_pedido);


--
-- Name: personal personal_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personal
    ADD CONSTRAINT personal_pkey PRIMARY KEY (id_personal);


--
-- Name: precios precios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.precios
    ADD CONSTRAINT precios_pkey PRIMARY KEY (id_precio);


--
-- Name: presupuesto_detalle presupuesto_detalle_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuesto_detalle
    ADD CONSTRAINT presupuesto_detalle_pkey PRIMARY KEY (id_presupuesto_detalle);


--
-- Name: presupuestos presupuestos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuestos
    ADD CONSTRAINT presupuestos_pkey PRIMARY KEY (id_presupuesto);


--
-- Name: producto_proveedor producto_proveedor_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.producto_proveedor
    ADD CONSTRAINT producto_proveedor_pkey PRIMARY KEY (id_producto_proveedor);


--
-- Name: productos productos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT productos_pkey PRIMARY KEY (id_producto);


--
-- Name: proveedores proveedores_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.proveedores
    ADD CONSTRAINT proveedores_pkey PRIMARY KEY (id_proveedor);


--
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id_rol);


--
-- Name: servicios servicios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios
    ADD CONSTRAINT servicios_pkey PRIMARY KEY (id_servicio);


--
-- Name: servicios_productos_utilizados servicios_productos_utilizados_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_productos_utilizados
    ADD CONSTRAINT servicios_productos_utilizados_pkey PRIMARY KEY (id_servicio_producto);


--
-- Name: servicios_realizados servicios_realizados_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_realizados
    ADD CONSTRAINT servicios_realizados_pkey PRIMARY KEY (id_servicio_realizado);


--
-- Name: stock stock_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.stock
    ADD CONSTRAINT stock_pkey PRIMARY KEY (id_stock);


--
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id_user);


--
-- Name: ajustes_stock ajustes_stock_id_personal_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ajustes_stock
    ADD CONSTRAINT ajustes_stock_id_personal_fkey FOREIGN KEY (id_personal) REFERENCES public.personal(id_personal);


--
-- Name: bajas_producto bajas_producto_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bajas_producto
    ADD CONSTRAINT bajas_producto_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: categoria_proveedor categoria_proveedor_id_categoria_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoria_proveedor
    ADD CONSTRAINT categoria_proveedor_id_categoria_fkey FOREIGN KEY (id_categoria) REFERENCES public.categoria(id_categoria);


--
-- Name: categoria_proveedor categoria_proveedor_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categoria_proveedor
    ADD CONSTRAINT categoria_proveedor_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: detalle_ajuste_stock detalle_ajuste_stock_id_ajuste_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_ajuste_stock
    ADD CONSTRAINT detalle_ajuste_stock_id_ajuste_fkey FOREIGN KEY (id_ajuste) REFERENCES public.ajustes_stock(id_ajuste);


--
-- Name: detalle_ajuste_stock detalle_ajuste_stock_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_ajuste_stock
    ADD CONSTRAINT detalle_ajuste_stock_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: factura_detalle factura_detalle_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.factura_detalle
    ADD CONSTRAINT factura_detalle_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: facturas facturas_id_pedido_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facturas
    ADD CONSTRAINT facturas_id_pedido_fkey FOREIGN KEY (id_pedido) REFERENCES public.pedidos(id_pedido);


--
-- Name: facturas facturas_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facturas
    ADD CONSTRAINT facturas_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: nota_credito_detalle nota_credito_detalle_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nota_credito_detalle
    ADD CONSTRAINT nota_credito_detalle_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: notas_credito notas_credito_id_pedido_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notas_credito
    ADD CONSTRAINT notas_credito_id_pedido_fkey FOREIGN KEY (id_pedido) REFERENCES public.pedidos(id_pedido);


--
-- Name: notas_credito notas_credito_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notas_credito
    ADD CONSTRAINT notas_credito_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: orden_detalle orden_detalle_id_orden_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orden_detalle
    ADD CONSTRAINT orden_detalle_id_orden_fkey FOREIGN KEY (id_orden) REFERENCES public.ordenes(id_orden);


--
-- Name: orden_detalle orden_detalle_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orden_detalle
    ADD CONSTRAINT orden_detalle_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: pedidos pedidos_id_orden_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT pedidos_id_orden_fkey FOREIGN KEY (id_orden) REFERENCES public.ordenes(id_orden);


--
-- Name: pedidos pedidos_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT pedidos_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: personal personal_id_rol_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personal
    ADD CONSTRAINT personal_id_rol_fkey FOREIGN KEY (id_rol) REFERENCES public.roles(id_rol);


--
-- Name: personal personal_id_user_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personal
    ADD CONSTRAINT personal_id_user_fkey FOREIGN KEY (id_user) REFERENCES public.usuarios(id_user);


--
-- Name: precios precios_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.precios
    ADD CONSTRAINT precios_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: presupuesto_detalle presupuesto_detalle_id_presupuesto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuesto_detalle
    ADD CONSTRAINT presupuesto_detalle_id_presupuesto_fkey FOREIGN KEY (id_presupuesto) REFERENCES public.presupuestos(id_presupuesto);


--
-- Name: presupuesto_detalle presupuesto_detalle_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuesto_detalle
    ADD CONSTRAINT presupuesto_detalle_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: presupuestos presupuestos_id_orden_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuestos
    ADD CONSTRAINT presupuestos_id_orden_fkey FOREIGN KEY (id_orden) REFERENCES public.ordenes(id_orden);


--
-- Name: presupuestos presupuestos_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.presupuestos
    ADD CONSTRAINT presupuestos_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: producto_proveedor producto_proveedor_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.producto_proveedor
    ADD CONSTRAINT producto_proveedor_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: producto_proveedor producto_proveedor_id_proveedor_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.producto_proveedor
    ADD CONSTRAINT producto_proveedor_id_proveedor_fkey FOREIGN KEY (id_proveedor) REFERENCES public.proveedores(id_proveedor);


--
-- Name: productos productos_id_categoria_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT productos_id_categoria_fkey FOREIGN KEY (id_categoria) REFERENCES public.categoria(id_categoria);


--
-- Name: productos productos_id_stock_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT productos_id_stock_fkey FOREIGN KEY (id_stock) REFERENCES public.stock(id_stock);


--
-- Name: servicios_productos_utilizados servicios_productos_utilizados_id_producto_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_productos_utilizados
    ADD CONSTRAINT servicios_productos_utilizados_id_producto_fkey FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto);


--
-- Name: servicios_productos_utilizados servicios_productos_utilizados_id_servicio_realizado_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_productos_utilizados
    ADD CONSTRAINT servicios_productos_utilizados_id_servicio_realizado_fkey FOREIGN KEY (id_servicio_realizado) REFERENCES public.servicios_realizados(id_servicio_realizado);


--
-- Name: servicios_realizados servicios_realizados_id_personal_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_realizados
    ADD CONSTRAINT servicios_realizados_id_personal_fkey FOREIGN KEY (id_personal) REFERENCES public.personal(id_personal);


--
-- Name: servicios_realizados servicios_realizados_id_servicio_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicios_realizados
    ADD CONSTRAINT servicios_realizados_id_servicio_fkey FOREIGN KEY (id_servicio) REFERENCES public.servicios(id_servicio);


--
-- Name: stock stock_id_personal_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.stock
    ADD CONSTRAINT stock_id_personal_fkey FOREIGN KEY (id_personal) REFERENCES public.personal(id_personal);


--
-- PostgreSQL database dump complete
--

