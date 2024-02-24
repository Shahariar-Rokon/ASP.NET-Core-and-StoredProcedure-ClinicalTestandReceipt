using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooAppUsingSp.Data.Migrations
{
    public partial class initDeleteSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE dbo.DeleteSP
                                @ReceiptNumber NVARCHAR(MAX)
                                AS
                                BEGIN
                                SET NOCOUNT ON;
                                BEGIN TRY
                                DECLARE @phid int;

                                SELECT @phid = Id FROM [dbo].[ClientHeader] WHERE ReceiptNumber = @ReceiptNumber;

                                IF @phid IS NOT NULL
                                BEGIN
                                BEGIN TRANSACTION
                                DELETE FROM [dbo].[TestDetail] WHERE ClientHeaderId = @phid;
                                DELETE FROM [dbo].[ClientHeader] WHERE Id = @phid;
                                COMMIT TRANSACTION
                                END
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRANSACTION
                                END CATCH
                                END
                                GO
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
