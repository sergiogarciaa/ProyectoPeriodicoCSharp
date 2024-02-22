using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "prdc_schema");

            migrationBuilder.EnsureSchema(
                name: "prdc_schema_usuarios");

            migrationBuilder.CreateTable(
                name: "categorias",
                schema: "prdc_schema",
                columns: table => new
                {
                    id_categoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    desc_categoria = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    tipo_categoria = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categorias_pkey", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "prdc_schema_usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cuenta_confirmada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    estado_suscripcion = table.Column<bool>(type: "boolean", nullable: true),
                    expiracion_token = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    fch_alta_usuario = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    fch_baja_usuario = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    dni_usuario = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    tlf_usuario = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    nombre_usuario = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    apellidos_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    clave_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_usuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    token_recuperacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    rol = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "noticias",
                schema: "prdc_schema",
                columns: table => new
                {
                    id_noticia = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requiere_suscripcion = table.Column<bool>(type: "boolean", nullable: true),
                    id_categoria_noticia = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_noticia = table.Column<long>(type: "bigint", nullable: false),
                    titulo_noticia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    desc_noticia = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    fch_publicacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    imagen_noticia = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("noticias_pkey", x => x.id_noticia);
                    table.ForeignKey(
                        name: "fkab91df7dfpltux7gcrqjpru34",
                        column: x => x.id_categoria_noticia,
                        principalSchema: "prdc_schema",
                        principalTable: "categorias",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "fkrbdei6nfn304qyyonge87ualu",
                        column: x => x.id_usuario_noticia,
                        principalSchema: "prdc_schema_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "comentarios",
                schema: "prdc_schema",
                columns: table => new
                {
                    id_comentario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    desc_comentario = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    fch_publicacion_comentario = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Id_Noticia = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comentarios_pkey", x => x.id_comentario);
                    table.ForeignKey(
                        name: "FK_comentarios_usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "prdc_schema_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "fk_comentario_noticia",
                        column: x => x.Id_Noticia,
                        principalSchema: "prdc_schema",
                        principalTable: "noticias",
                        principalColumn: "id_noticia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "noticia_comentarios",
                schema: "prdc_schema",
                columns: table => new
                {
                    id_comentario = table.Column<long>(type: "bigint", nullable: false),
                    id_noticia = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fkf12y8y8spo9x27n69mvxjk9go",
                        column: x => x.id_comentario,
                        principalSchema: "prdc_schema",
                        principalTable: "comentarios",
                        principalColumn: "id_comentario");
                    table.ForeignKey(
                        name: "fkqd9aagyg91260vcyh5aaph6xp",
                        column: x => x.id_noticia,
                        principalSchema: "prdc_schema",
                        principalTable: "noticias",
                        principalColumn: "id_noticia");
                });

            migrationBuilder.CreateTable(
                name: "usuario_comentarios",
                schema: "prdc_schema",
                columns: table => new
                {
                    id_comentario = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "fkejjm7tjlrrwb2ls7wa4awngf4",
                        column: x => x.id_usuario,
                        principalSchema: "prdc_schema_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "fks60gdyncnkxtpby9t6q69cfva",
                        column: x => x.id_comentario,
                        principalSchema: "prdc_schema",
                        principalTable: "comentarios",
                        principalColumn: "id_comentario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_Id_Noticia",
                schema: "prdc_schema",
                table: "comentarios",
                column: "Id_Noticia");

            migrationBuilder.CreateIndex(
                name: "IX_comentarios_IdUsuario",
                schema: "prdc_schema",
                table: "comentarios",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_noticia_comentarios_id_comentario",
                schema: "prdc_schema",
                table: "noticia_comentarios",
                column: "id_comentario");

            migrationBuilder.CreateIndex(
                name: "IX_noticia_comentarios_id_noticia",
                schema: "prdc_schema",
                table: "noticia_comentarios",
                column: "id_noticia");

            migrationBuilder.CreateIndex(
                name: "IX_noticias_id_categoria_noticia",
                schema: "prdc_schema",
                table: "noticias",
                column: "id_categoria_noticia");

            migrationBuilder.CreateIndex(
                name: "IX_noticias_id_usuario_noticia",
                schema: "prdc_schema",
                table: "noticias",
                column: "id_usuario_noticia");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_comentarios_id_comentario",
                schema: "prdc_schema",
                table: "usuario_comentarios",
                column: "id_comentario");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_comentarios_id_usuario",
                schema: "prdc_schema",
                table: "usuario_comentarios",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "usuarios_dni_usuario_key",
                schema: "prdc_schema_usuarios",
                table: "usuarios",
                column: "dni_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "usuarios_email_usuario_key",
                schema: "prdc_schema_usuarios",
                table: "usuarios",
                column: "email_usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "noticia_comentarios",
                schema: "prdc_schema");

            migrationBuilder.DropTable(
                name: "usuario_comentarios",
                schema: "prdc_schema");

            migrationBuilder.DropTable(
                name: "comentarios",
                schema: "prdc_schema");

            migrationBuilder.DropTable(
                name: "noticias",
                schema: "prdc_schema");

            migrationBuilder.DropTable(
                name: "categorias",
                schema: "prdc_schema");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "prdc_schema_usuarios");
        }
    }
}
