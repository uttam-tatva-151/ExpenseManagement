// ======================= Region: UI List Toggle =======================
$(function () {
  $("#mainTransactionList").show();
  $("#trashTransactionList").hide();

  $("#btnShowMain").on("click", function () {
    $("#mainTransactionList").show();
    $("#trashTransactionList").hide();
    $("#btnShowMain").addClass("active");
    $("#btnShowTrash").removeClass("active");
  });

  $("#btnShowTrash").on("click", function () {
    $("#mainTransactionList").hide();
    $("#trashTransactionList").show();
    $("#btnShowMain").removeClass("active");
    $("#btnShowTrash").addClass("active");
  });

  // =================== Region: Transaction Type Select ===================
  function setTransactionType(type) {
    $("#transactionType").val(type);
    $("#btnIncome").removeClass("active");
    $("#btnExpense").removeClass("active");
    if (type === "Income") {
      $("#btnIncome").addClass("active");
    } else {
      $("#btnExpense").addClass("active");
    }
  }
  $("#btnIncome").on("click", function () { setTransactionType("Income"); });
  $("#btnExpense").on("click", function () { setTransactionType("Expense"); });
  setTransactionType("Expense");

  // =================== Region: Quick Category Select ===================
  $(".quickCat").on("click", function () {
    $(".quickCat").removeClass("selected");
    $(this).addClass("selected");
    let val = $(this).data("value");
    $("#categoryId").val(val);
  });

  // =================== Region: Delete Transaction (Modal) ===================
  let transactionToDelete = null;
  $(document).on("click", ".btnDeleteTransaction", function (e) {
    e.preventDefault();
    transactionToDelete = $(this).data("transaction-id");
    $("#deleteConfirmModal").modal("show");
  });

  $("#confirmDeleteBtn").on("click", function () {
    if (transactionToDelete) {
      $.ajax({
        url: "/Transaction/DeleteTransaction",
        type: "Delete",
        data: { transactionId: transactionToDelete },
        success: function (response) {
          if (response.status === "Success" || response.status == 0) {
            $('.btnDeleteTransaction[data-transaction-id="' + transactionToDelete + '"]')
              .closest(".transactionCard")
              .fadeOut(300, function () {
                $(this).hide().appendTo("#trashTransactionList").fadeIn(300);
              });
            transactionToDelete = null;
          }
          showToast(response.message, response.status);
        },
        error: function (error) {
          console.error("Error deleting transaction:", error);
          showToast("Failed to delete transaction. Please try again.", "Error");
        }
      });
      $("#deleteConfirmModal").modal("hide");
    }
  });

  // =================== Region: Details Overlay ===================
  $(".btnShowDetails").on("click", function (e) {
    e.preventDefault();
    let transactionId = $(this).data("transaction-id");
    let txn = transactions.find(t => t.TransactionId == transactionId);
    if (txn) {
      $(".detailsList .categoryBadge").text(txn.CategoryName);
      if (txn.TransactionType == 1) {
        $(".detailsList .statusPill")
          .removeClass("statusExpense")
          .addClass("statusIncome")
          .html('<i class="bi bi-arrow-up-circle"></i> Income');
        $(".amountDetail")
          .removeClass("amountExpense")
          .addClass("amountIncome")
          .text("+₹" + Number(txn.Amount).toLocaleString('en-IN', { minimumFractionDigits: 2 }));
      } else {
        $(".detailsList .statusPill")
          .removeClass("statusIncome")
          .addClass("statusExpense")
          .html('<i class="bi bi-arrow-down-circle"></i> Expense');
        $(".amountDetail")
          .removeClass("amountIncome")
          .addClass("amountExpense")
          .text("-₹" + Number(txn.Amount).toLocaleString('en-IN', { minimumFractionDigits: 2 }));
      }
      $(".detailsList li:eq(2) span").text(txn.PaymentMethod);
      let d = new Date(txn.TransactionDate);
      $(".detailsList li:eq(3) span:eq(0)").text(d.toLocaleDateString('en-CA'));
      $(".detailsList li:eq(3) span:eq(1)").text(d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));
      $(".descBox").text(txn.Description || "");
      $(".btnGradient").attr("href", "/Transaction/Edit/" + txn.TransactionId);
    }
    $("#detailsOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });

  $("#detailsOverlayBg").on("click", function (e) {
    if (e.target === this) {
      $("#detailsOverlayBg").removeClass("open");
      $("body").css("overflow", "");
    }
  });

  $("#detailsOverlayClose").on("click", function () {
    $("#detailsOverlayBg").removeClass("open");
    $("body").css("overflow", "");
  });

  $(".detailsOverlayPanel").on("click", function (e) {
    e.stopPropagation();
  });

  // =================== Region: Export Transactions ===================
  let selectedTransactionExportType = "";

  $("#exportTransactionBtn").on("click", function () {
    $(".exportTypeBtn").removeClass("active");
    $("#exportTransactionSelectedFormat").text("");
    $("#exportTransactionConfirmBtn").prop("disabled", true);
    selectedTransactionExportType = "";
    $("#exportTransactionModal").modal("show");
  });

  $(".exportTypeBtn").on("click", function () {
    let type = $(this).data("type");
    $(".exportTypeBtn").removeClass("active");
    $(this).addClass("active");
    selectedTransactionExportType = type;
    let label = $(this).find(".fw-bold").text();
    $("#exportTransactionSelectedFormat").text("Selected: " + label);
    $("#exportTransactionConfirmBtn").prop("disabled", false);
  });

  $("#exportTransactionConfirmBtn").on("click", function () {
    if (selectedTransactionExportType === "pdf") {
      window.location.href = '/Transaction/ExportPdf';
      $("#exportTransactionModal").modal("hide");
    } else if (selectedTransactionExportType === "csv") {
      window.location.href = '/Transaction/ExportCsv';
      $("#exportTransactionModal").modal("hide");
    } else if (selectedTransactionExportType === "excel") {
      window.location.href = '/Transaction/ExportExcel';
      $("#exportTransactionModal").modal("hide");
    } else {
      alert("Please select a valid export format.");
    }
  });

  // =================== Region: Import Transactions ===================
  $('#importBtn').on('click', function () {
    $('#importCsvFile').val('');
    $('#importFileFeedback').addClass('d-none').text('');
    $('#importConfirmBtn').prop('disabled', true);
    $('#importModal').modal('show');
  });

  $('#importCsvFile').on('change', function () {
    let file = this.files[0];
    if (file && file.name.toLowerCase().endsWith('.csv')) {
      $('#importConfirmBtn').prop('disabled', false);
      $('#importFileFeedback').addClass('d-none').text('');
    } else {
      $('#importConfirmBtn').prop('disabled', true);
      $('#importFileFeedback').removeClass('d-none').text('Please select a valid .csv file.');
    }
  });

  $('#importConfirmBtn').on('click', function () {
    let fileInput = $('#importCsvFile')[0];
    let file = fileInput.files[0];
    if (!file || !file.name.toLowerCase().endsWith('.csv')) {
      $('#importFileFeedback').removeClass('d-none').text('Please select a valid .csv file.');
      return;
    }
    let formData = new FormData();
    formData.append('file', file);
    $('#importConfirmBtn').prop('disabled', true);
    $.ajax({
      url: '/Transaction/ImportTransactions',
      type: 'POST',
      data: formData,
      processData: false,
      contentType: false,
      success: function (response) {
        $('#importModal').modal('hide');
        showToast(response.message, response.status);
      },
      error: function () {
        $('#importFileFeedback').removeClass('d-none').text('Import failed. Please check your CSV and try again.');
        $('#importConfirmBtn').prop('disabled', false);
      }
    });
  });
});

// ======================= Region: SignalR Realtime Updates =======================
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/transactionHub")
  .build();

connection.start()
  .then(() => console.log("SignalR connected"))
  .catch(err => console.error(err));

connection.on("TransactionAdded", transaction => {
  location.reload();
});

connection.on("TransactionUpdated", transaction => {
  location.reload();
});

connection.on("TransactionDeleted", transactionId => {
  location.reload();
});