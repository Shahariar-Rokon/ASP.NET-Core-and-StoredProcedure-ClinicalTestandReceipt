jQuery(function () {

});

function addTestUnit(token) {
    class testUnit {
        constructor(id, name) {
            this.Id = id;
            this.Name = name;
        }
    }

    let amuvm = new testUnit($("#hiddenIdInput").val(), $("#testUnitInput").val());

    $.ajax({
        method: "POST",
        url: "/TestUnit/AddUpdate",
        headers: { "RequestVerificationToken": token },
        data: {
            aTestUnitVM: amuvm,
            actionType: $("#actionTypeName").text(),
            token: token
        },
        success: function (result) {
            $("#mutableBody").html(result);
            $("#actionTypeName").text("add");
            $("#addMUButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function editTestUnit(e) {
    $("#testUnitInput").val($(e).data("name"));
    $("#hiddenIdInput").val($(e).data("id"));
    $("#actionTypeName").text("edit");
    $("#addMUButton").text("Update");
};

function deleteTestUnit(token, e) {
    $.ajax({
        method: "DELETE",
        url: "/TestUnit/Delete",
        headers: { "RequestVerificationToken": token },
        data: {
            id: e,
            token: token
        },
        success: function (result) {
            $("#mutableBody").html(result);
            $("#actionTypeName").text("add");
            $("#addMUButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
};

function uploadProductImage(e) {
    var file = $(e)[0].files[0];

    if (file) {
        const formData = new FormData();
        formData.append("image", file);
        $.ajax({
            method: "POST",
            url: "/Test/GetImageUrl",
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                $("#productImageUrl").val(result);
                var reader = new FileReader();
                reader.onload = function () {

                    $("#selectedProductImage").attr("src", reader.result);
                }
                reader.readAsDataURL(file);
            }
        });

    }
}

function addTest(token) {
    class Test {
        constructor(id, name, imageUrl) {
            this.Id = id;
            this.Name = name;
            this.ImageUrl = imageUrl;
        }
    }

    let aProduct = new Test($("#hiddenProductIdInput").val(), $("#productNameInput").val(), $("#productImageUrl").val());

    $.ajax({
        method: "POST",
        url: "/Test/AddUpdate",
        headers: { "RequestVerificationToken": token },
        data: {
            productVM: aProduct,
            actionType: $("#actionTypeNameForProduct").text(),
            token: token
        },
        success: function (result) {
            $("#productTableBody").html(result);
            $("#actionTypeNameForProduct").text("add");
            $("#addProductButton").text("Add");

            $("#productNameInput").val('');
            $("#inputProductImage").val('');
            $("#selectedProductImage").attr("src", "images/phone_1.jpg");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function editTest(e) {
    $("#productNameInput").val($(e).data("name"));
    $("#hiddenProductIdInput").val($(e).data("id"));
    $("#productImageUrl").val($(e).data("image"));
    $("#actionTypeNameForProduct").text("edit");
    $("#addProductButton").text("Update");
    $("#selectedProductImage").attr("src", $(e).data("image"));
}

function deleteTest(token, e) {
    $.ajax({
        method: "DELETE",
        url: "/Test/Delete",
        headers: { "RequestVerificationToken": token },
        data: {
            id: e,
            token: token
        },
        success: function (result) {
            $("#productTableBody").html(result);
            $("#actionTypeNameForProduct").text("add");
            $("#addProductButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
};

function calculateTotalPrice() {
    var qty = $("#quantityInput").val();
    var unitPrice = $("#unitPriceInput").val();
    if (Number(qty) != "NaN" && Number(unitPrice) != "NaN") {
        $("#totalPriceSpan").text(Number(qty) * Number(unitPrice));
    }
}

function getPurchaseDetailPartialView(token) {
    class TestDetail {
        constructor(id, testId, quantity, testUnitId, unitPrice, totalPrice, clientHeaderId, testName, testUnitName) {
            this.Id = id;
            this.TestId = testId;
            this.TestName = testName;
            this.Quantity = quantity;
            this.TestUnitId = testUnitId;
            this.TestUnitName = testUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.ClientHeaderId = clientHeaderId;
        }
    }

    let aPurchaseDetail = new TestDetail(0, $("#productSelect").val(), $("#quantityInput").val(), $("#measurementUnitSelect").val(), $("#unitPriceInput").val(), $("#totalPriceSpan").text(), 0, $("#productSelect").find(":selected").text(), $("#measurementUnitSelect").find(":selected").text())

    $.ajax({
        method: "POST",
        url: "/Clients/TestDetail",
        headers: { "RequestVerificationToken": token },
        data: {
            testDetailVM: aPurchaseDetail
        },
        success: function (result) {
            $("#pdvm_tbody").append(result);

            var totalBillAmount = 0;
            $("input[data-tag='TotalPrice']").each(function () {
                var currVal = $(this).val();
                totalBillAmount += Number(currVal);

                $("#totalAmount").val(totalBillAmount);
            });
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function submitPurchaseOrder(token) {
    class TestDetail {
        constructor(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            this.Id = id;
            this.TestId = productId;
            this.TestName = productName;
            this.Quantity = quantity;
            this.TestUnitId = measurementUnitId;
            this.TestUnitName = measurementUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.TestHeaderId = purchaseHeaderId;
        }
    }

    class PDs {
        constructor() {
            this.purchaseDetails = [];
        }

        newPurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            let aPurchaseDetail = new TestDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName);
            this.purchaseDetails.push(aPurchaseDetail);
            return aPurchaseDetail;
        }

        get allPurchaseDetails() {
            return this.purchaseDetails;
        }
    }

    class ClientHeader {
        constructor(id, clientName, clientPhoneNumber, clientEmailAddress, receiptNumber, testDate, totalAmount) {
            this.Id = id;
            this.ClientName = clientName;
            this.ClientPhoneNumber = clientPhoneNumber;
            this.ClientEmailAddress = clientEmailAddress;
            this.ReceiptNumber = receiptNumber;
            this.TestDate = testDate;
            this.TotalAmount = totalAmount;
            this.TestDetails;
        }

        addPurchaseDetails(pds) {
            this.TestDetails = pds.allPurchaseDetails;
        }
    }

    let purchaseDetailList = new PDs();

    $.each($("#pdvm_tbody tr"), function (index, purchaseDetailTr) {
        purchaseDetailList.newPurchaseDetail($(purchaseDetailTr).find("input[data-tag='Id']").val(), $(purchaseDetailTr).find("input[data-tag='TestId']").val(), $(purchaseDetailTr).find("input[data-tag='Quantity']").val(), $(purchaseDetailTr).find("input[data-tag='TestUnitId']").val(), $(purchaseDetailTr).find("input[data-tag='UnitPrice']").val(), $(purchaseDetailTr).find("input[data-tag='TotalPrice']").val(), $(purchaseDetailTr).find("input[data-tag='ClientHeaderId']").val(), $(purchaseDetailTr).find("input[data-tag='TestName']").val(), $(purchaseDetailTr).find("input[data-tag='TestUnitName']").val())
    });

    let aPurchaseHeader = new ClientHeader(0, $("#customerNameInput").val(), $("#customerPhoneInput").val(), $("#customerEmailInput").val(), "&nbsp;", $("#purchaseDateInput").val(), $("#totalAmount").val());
    aPurchaseHeader.addPurchaseDetails(purchaseDetailList);

    $.ajax({
        method: "POST",
        url: "/Clients/Create",
        headers: { "RequestVerificationToken": token },
        data: {
           clientHeaderVM: aPurchaseHeader,
            token: token
        },
        success: function (result) {
            $("#purchaseCardsDiv").append(result);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function submitPurchaseOrderForSP(token) {
    class TestDetail {
        constructor(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            this.Id = id;
            this.TestId = productId;
            this.TestName = productName;
            this.Quantity = quantity;
            this.TestUnitId = measurementUnitId;
            this.TestUnitName = measurementUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.ClientHeaderId = purchaseHeaderId;
        }
    }

    class PDs {
        constructor() {
            this.purchaseDetails = [];
        }

        newPurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            let aPurchaseDetail = new TestDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName);
            this.purchaseDetails.push(aPurchaseDetail);
            return aPurchaseDetail;
        }

        get allPurchaseDetails() {
            return this.purchaseDetails;
        }
    }

    class ClientHeader {
        constructor(id, clientName, clientPhoneNumber, clientEmailAddress, receiptNumber, testDate, totalAmount) {
            this.Id = id;
            this.ClientName = clientName;
            this.ClientPhoneNumber = clientPhoneNumber;
            this.ClientEmailAddress = clientEmailAddress;
            this.ReceiptNumber = receiptNumber;
            this.TestDate = testDate;
            this.TotalAmount = totalAmount;
            this.TestDetails;
        }

        addPurchaseDetails(pds) {
            this.PurchaseDetails = pds.allPurchaseDetails;
        }
    }

    let purchaseDetailList = new PDs();

    $.each($("#pdvm_tbody tr"), function (index, purchaseDetailTr) {
        purchaseDetailList.newPurchaseDetail($(purchaseDetailTr).find("input[data-tag='Id']").val(), $(purchaseDetailTr).find("input[data-tag='TestId']").val(), $(purchaseDetailTr).find("input[data-tag='Quantity']").val(), $(purchaseDetailTr).find("input[data-tag='TestUnitId']").val(), $(purchaseDetailTr).find("input[data-tag='UnitPrice']").val(), $(purchaseDetailTr).find("input[data-tag='TotalPrice']").val(), $(purchaseDetailTr).find("input[data-tag='ClientHeaderId']").val(), $(purchaseDetailTr).find("input[data-tag='TestName']").val(), $(purchaseDetailTr).find("input[data-tag='TestUnitName']").val())
    });

    let aPurchaseHeader = new ClientHeader(0, $("#customerNameInput").val(), $("#customerPhoneInput").val(), $("#customerEmailInput").val(), "&nbsp;", $("#purchaseDateInput").val(), $("#totalAmount").val());
    aPurchaseHeader.addPurchaseDetails(purchaseDetailList);

    //$.ajax({
    //    method: "POST",
    //    url: "/Clients/CreateExecutingSP",
    //    headers: { "RequestVerificationToken": token },
    //    data: {
    //        clientHeaderVM: aPurchaseHeader,
    //        token: token
    //    },
    //    success: function (result) {
    //        $("#purchaseCardsDiv").append(result);
    //    },
    //    error: function (req, status, error) {
    //        console.log(error);
    //    }
    //})
    $.ajax({
        method: "POST",
        url: "/Clients/CreateExecutingSP",
        headers: { "RequestVerificationToken": token },
        data: {
            clientHeaderVM: aPurchaseHeader,
            token: token
        },
        success: function (result) {
            // Assuming the server returns HTML for the partial view
            $("#purchaseCardsDiv").append(result);

            // Optionally, you can clear or update the form elements after a successful submission
            // Clear form inputs or update as needed

            // You might want to close a modal or perform other actions based on your UI/UX design

            // Example of clearing form inputs
            $("#customerNameInput").val("");
            $("#customerPhoneInput").val("");
            $("#customerEmailInput").val("");
            $("#purchaseDateInput").val("");
            $("#totalAmount").val("");

            // Clear the purchase details table or update as needed
            $("#pdvm_tbody").empty();
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });

}

function removeMe(e) {
    e.parent().parent().remove();

    var totalBillAmount = 0;
    $("input[data-tag='TotalPrice']").each(function () {
        var currVal = $(this).val();
        totalBillAmount += Number(currVal);
        console.log(totalBillAmount);
        $("#totalAmount").val(totalBillAmount);
    });
}