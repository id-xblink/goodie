using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO public.users
                (id, ""name"", code, address, discount, user_role_id, password_hash, login) 
                VALUES(
                '280a7c13-08f0-4656-8318-8ceeeaa63930',
                'testadmin',
                '0013-2025',
                'address',
                69,
                '3c379a42-32bd-4097-8c19-96a27d91604a',
                'uoeSGkVO0noc+eoR6m3SRA==$wJD92QlX0IlQ9pK12QsbQEOJLp1rNQslFP9WDpdSoVc=',
                'admin');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM public.users WHERE ""login"" = 'admin';
            ");
        }
    }
}
