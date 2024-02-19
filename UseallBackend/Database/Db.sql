-- Database: Useall

-- DROP DATABASE IF EXISTS "Useall";

CREATE DATABASE "Useall"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Portuguese_Brazil.1252'
    LC_CTYPE = 'Portuguese_Brazil.1252'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

-- SCHEMA: dbcadast

-- DROP SCHEMA IF EXISTS dbcadast ;

CREATE SCHEMA IF NOT EXISTS dbcadast
    AUTHORIZATION postgres;

-- Table: dbcadast.uscliente

-- DROP TABLE IF EXISTS dbcadast.uscliente;

CREATE TABLE IF NOT EXISTS dbcadast.uscliente
(
    codigo integer NOT NULL,
    nome character(50) COLLATE pg_catalog."default",
    cnpj character(18) COLLATE pg_catalog."default" NOT NULL,
    dtcad date,
    endereco character(100) COLLATE pg_catalog."default",
    telefone character(15) COLLATE pg_catalog."default",
    CONSTRAINT "cli-codigo" PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS dbcadast.uscliente
    OWNER to postgres;

COMMENT ON TABLE dbcadast.uscliente
    IS 'Clientes da Useall';
-- Index: Iindex01

-- DROP INDEX IF EXISTS dbcadast."Iindex01";

CREATE UNIQUE INDEX IF NOT EXISTS "Iindex01"
    ON dbcadast.uscliente USING btree
    (codigo ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;