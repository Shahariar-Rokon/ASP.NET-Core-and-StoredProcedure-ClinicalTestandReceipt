using Humanizer;
using Microsoft.EntityFrameworkCore.Migrations;
using static Humanizer.In;
using static Humanizer.On;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

#nullable disable

namespace ZooAppUsingSp.Data.Migrations
{
    public partial class initUpdateProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE dbo.UpdateSP
                          @ClientName NVARCHAR(MAX),
                          @ClientPhoneNumber NVARCHAR(MAX),
                          @ClientEmailAddress NVARCHAR(MAX),
                          @ReceiptNumber NVARCHAR(MAX),
                          @TestDate DATE,
                          @TVP ParamTestDetail READONLY
                          AS
                          BEGIN
                          SET NOCOUNT ON;
                          BEGIN TRY
                          DECLARE @phid int;
                          DECLARE @count int;
                          DECLARE @SenitalValue int = 0;
                          DECLARE @TotalBill DECIMAL(18, 2) = 0;
                          SELECT @count = COUNT(*) FROM @TVP;
                          WHILE @SenitalValue<@count


                          BEGIN
    
                          SELECT @TotalBill += Quantity * TestUnitPrice FROM @TVP ORDER BY TestId ASC OFFSET @SenitalValue ROWS FETCH FIRST 1 ROWS ONLY;
                          SET @SenitalValue += 1;
                          END
                          BEGIN TRANSACTION
                          IF EXISTS(SELECT 1 FROM[dbo].[ClientHeader] WHERE ReceiptNumber =  @ReceiptNumber)
                          BEGIN
                          UPDATE[dbo].[ClientHeader]
                          SET 
                          ClientName = @ClientName,
                          ClientPhoneNumber = @ClientPhoneNumber,
                          ClientEmailAddress = @ClientEmailAddress,
                          TestDate = @TestDate,
                          TotalBill = @TotalBill
                          WHERE ReceiptNumber = @ReceiptNumber;

                          SELECT @phid = Id FROM[dbo].[ClientHeader] WHERE ReceiptNumber = @ReceiptNumber;

                          UPDATE[dbo].[TestDetail]
                          SET 
                          TestId = pd.TestId,
                          Quantity = pd.Quantity,
                          TestUnitId = pd.TestUnitId,
                          TestUnitPrice = pd.TestUnitPrice,
                          TestTotalPrice = pd.Quantity * pd.TestUnitPrice
                          FROM @TVP AS pd
                          WHERE ClientHeaderId = @phid;
                          END
                          ELSE
                          BEGIN
                          INSERT INTO[dbo].[ClientHeader](ClientName, ClientPhoneNumber, ClientEmailAddress, ReceiptNumber, TestDate, TotalBill)
                          VALUES(@ClientName, @ClientPhoneNumber, @ClientEmailAddress, @ReceiptNumber, @TestDate, @TotalBill);

                          SELECT @phid = Id FROM[dbo].[ClientHeader] WHERE ReceiptNumber = @ReceiptNumber;

                          INSERT INTO[dbo].[TestDetail] (TestId, Quantity, TestUnitId, TestUnitPrice, TestTotalPrice, ClientHeaderId)
                          SELECT TestId, Quantity, TestUnitId, TestUnitPrice, Quantity* TestUnitPrice, @phid FROM @TVP;
                          END
                          COMMIT TRANSACTION
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
