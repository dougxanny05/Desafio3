using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio3.Migrations
{
    /// <inheritdoc />
    public partial class AlinearRequisitosYRestricciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Lavar la lechuga romana y cortarla en trozos.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Asar el pollo y cortarlo en tiras.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Mezclar la lechuga, el pollo y el aderezo César.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripcion",
                value: "Cocinar la pasta en agua hirviendo con sal.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 5,
                column: "Descripcion",
                value: "Mezclar el huevo, la crema y el queso parmesano.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 6,
                column: "Descripcion",
                value: "Añadir la mezcla a la pasta caliente.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descripcion",
                value: "Cortar los tomates y hervirlos hasta que se ablanden.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 8,
                column: "Descripcion",
                value: "Licuar los tomates y agregar la albahaca.");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 9,
                column: "Descripcion",
                value: "Cocinar por 10 minutos más y servir caliente.");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Ensalada clásica con pollo, lechuga y aderezo César.");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Pasta con salsa de crema, huevo y queso parmesano.");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Sopa ligera de tomate con albahaca.");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Recetas_Nombre_MinLength",
                table: "Recetas",
                sql: "LEN(LTRIM(RTRIM([Nombre]))) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Recetas_TiempoPreparacion_Positive",
                table: "Recetas",
                sql: "[TiempoPreparacion] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PasosPreparacion_Descripcion_MinLength",
                table: "PasosPreparacion",
                sql: "LEN(LTRIM(RTRIM([Descripcion]))) >= 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PasosPreparacion_Orden_Positive",
                table: "PasosPreparacion",
                sql: "[Orden] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Ingredientes_Cantidad_Positive",
                table: "Ingredientes",
                sql: "[Cantidad] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Ingredientes_Nombre_MinLength",
                table: "Ingredientes",
                sql: "LEN(LTRIM(RTRIM([Nombre]))) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Ingredientes_UnidadMedida_NotBlank",
                table: "Ingredientes",
                sql: "LEN(LTRIM(RTRIM([UnidadMedida]))) > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Recetas_Nombre_MinLength",
                table: "Recetas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Recetas_TiempoPreparacion_Positive",
                table: "Recetas");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PasosPreparacion_Descripcion_MinLength",
                table: "PasosPreparacion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PasosPreparacion_Orden_Positive",
                table: "PasosPreparacion");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Ingredientes_Cantidad_Positive",
                table: "Ingredientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Ingredientes_Nombre_MinLength",
                table: "Ingredientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Ingredientes_UnidadMedida_NotBlank",
                table: "Ingredientes");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Lavar y cortar lechuga");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Asar pollo y cortar en tiras");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Mezclar todo");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 4,
                column: "Descripcion",
                value: "Cocinar pasta");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 5,
                column: "Descripcion",
                value: "Mezclar huevo+crema+queso");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 6,
                column: "Descripcion",
                value: "Añadir mezcla a la pasta");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 7,
                column: "Descripcion",
                value: "Cortar y hervir tomates");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 8,
                column: "Descripcion",
                value: "Licuar y agregar albahaca");

            migrationBuilder.UpdateData(
                table: "PasosPreparacion",
                keyColumn: "Id",
                keyValue: 9,
                column: "Descripcion",
                value: "Cocinar 10 min más y servir");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Ensalada con lechuga romana, pollo y aderezo César.");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Pasta cremosa con huevo, crema y queso parmesano.");

            migrationBuilder.UpdateData(
                table: "Recetas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Sopa de tomates frescos con albahaca.");
        }
    }
}
