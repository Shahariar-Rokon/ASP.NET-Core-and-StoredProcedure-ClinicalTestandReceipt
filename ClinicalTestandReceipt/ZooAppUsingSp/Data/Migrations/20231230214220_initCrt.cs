using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZooAppUsingSp.Data.Migrations
{
    public partial class initCrt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TYPE ParamTestDetail 
                AS TABLE
                (
                    TestId INT,
                    Quantity DECIMAL(18,2),
                    TestUnitId INT,
                    TestUnitPrice DECIMAL(18,2),
                    
                    ClientHeaderId INT
                );
                GO
                CREATE OR ALTER PROCEDURE dbo.ClientSP
                    @ClientName NVARCHAR(MAX),
                    @ClientPhoneNumber NVARCHAR(MAX),
                    @ClientEmailAddress NVARCHAR(MAX),
                    @ReceiptNumber NVARCHAR(MAX),
                    @TestDate DATE,
                    @pd ParamTestDetail READONLY
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRY
                        DECLARE @LocalTestDetail Table
                        (
                            TestId INT,
                            Quantity DECIMAL(18,2),
                            TestUnitId INT,
                            TestUnitPrice DECIMAL(18,2),
                            TestTotalPrice DECIMAL(18,2),
                            ClientHeaderId INT
                        );
                        DECLARE @phid int;
                        DECLARE @count int;
                        DECLARE @SenitalValue int = 0;
                        DECLARE @TotalBill DECIMAL(18,2) = 0;

                        SELECT @count = COUNT(*) FROM @pd;

                        WHILE @SenitalValue < @count
                        BEGIN
                            SELECT @TotalBill += Quantity * TestUnitPrice FROM @pd ORDER BY TestId ASC OFFSET @SenitalValue ROWS FETCH FIRST 1 ROWS ONLY;
                            SET @SenitalValue += 1;
                        END

                        INSERT INTO @LocalTestDetail (TestId, Quantity, TestUnitId, TestUnitPrice, TestTotalPrice, ClientHeaderId) SELECT TestId, Quantity, TestUnitId, TestUnitPrice, Quantity * TestUnitPrice, ClientHeaderId FROM @pd;

                        BEGIN TRANSACTION
                            INSERT INTO [dbo].[ClientHeaders] (ClientName, ClientPhoneNumber, ClientEmailAddress, ReceiptNumber, TestDate, TotalBill) VALUES (@ClientName, @ClientPhoneNumber, @ClientEmailAddress, @ReceiptNumber, @TestDate, @TotalBill);
                            
                            SELECT @phid = Id FROM [dbo].[ClientHeaders] WHERE ReceiptNumber = @ReceiptNumber;
                
                            UPDATE @LocalTestDetail SET ClientHeaderId = @phid;
                
                            INSERT INTO [dbo].[TestDetails] (TestId, Quantity, TestUnitId, TestUnitPrice, TestTotalPrice, ClientHeaderId) SELECT * FROM @LocalTestDetail;
                        COMMIT TRANSACTION
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION
                    END CATCH
                END
                GO"
         );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
