// Delete Confirmation Modal Logic
let transactionToDelete = null;
$(function () {
  // Show main list by default
  $("#mainTransactionList").show();
  $("#trashTransactionList").hide();

  // Toggle for Main List
  $("#btnShowMain").on("click", function () {
    $("#mainTransactionList").show();
    $("#trashTransactionList").hide();
    $("#btnShowMain").addClass("active");
    $("#btnShowTrash").removeClass("active");
  });

  // Toggle for Trash
  $("#btnShowTrash").on("click", function () {
    $("#mainTransactionList").hide();
    $("#trashTransactionList").show();
    $("#btnShowMain").removeClass("active");
    $("#btnShowTrash").addClass("active");
  });

  $("#btnIncome").click(function () {
    $("#transactionType").val("Income");
    $(this).addClass("active");
    $("#btnExpense").removeClass("active");
  });

  $("#btnExpense").click(function () {
    $("#transactionType").val("Expense");
    $(this).addClass("active");
    $("#btnIncome").removeClass("active");
  });
  // Delete icon click opens modal
  $(document).on("click", ".btnDeleteTransaction", function (e) {
    e.preventDefault();
    transactionToDelete = $(this).data("transaction-id");
    $("#deleteConfirmModal").modal("show");
  });

  // Confirm delete
  $("#confirmDeleteBtn").on("click", function () {
    if (transactionToDelete) {
      // Remove card from View
      $.ajax({
        url: "/Transaction/DeleteTransaction",
        type: "Delete",
        data: { transactionId: transactionToDelete },
        success: function (response) {
          if (response.status === "Success" || response.status == 0) {
            $(
              '.btnDeleteTransaction[data-transaction-id="' +
              transactionToDelete +
              '"]'
            )
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
      })

      $("#deleteConfirmModal").modal("hide");
    }
  });

  // Show/Hide Transaction Details Overlay
  $(".btnShowDetails").on("click", function (e) {
    e.preventDefault();
    let transactionId = $(this).data("transaction-id");
    let txn = transactions.find(t => t.TransactionId == transactionId);

    if (txn) {
      // Set icon, category, type
      $(".detailsList .categoryBadge").text(txn.CategoryName);

      if (txn.TransactionType == 1) { // Income
        $(".detailsList .statusPill")
          .removeClass("statusExpense")
          .addClass("statusIncome")
          .html('<i class="bi bi-arrow-up-circle"></i> Income');
        $(".amountDetail")
          .removeClass("amountExpense")
          .addClass("amountIncome")
          .text("+₹" + Number(txn.Amount).toLocaleString('en-IN', { minimumFractionDigits: 2 }));
      } else { // Expense
        $(".detailsList .statusPill")
          .removeClass("statusIncome")
          .addClass("statusExpense")
          .html('<i class="bi bi-arrow-down-circle"></i> Expense');
        $(".amountDetail")
          .removeClass("amountIncome")
          .addClass("amountExpense")
          .text("-₹" + Number(txn.Amount).toLocaleString('en-IN', { minimumFractionDigits: 2 }));
      }

      // Payment method
      $(".detailsList li:eq(2) span").text(txn.PaymentMethod);

      // Date and time
      let d = new Date(txn.TransactionDate);
      $(".detailsList li:eq(3) span:eq(0)").text(d.toLocaleDateString('en-CA')); // yyyy-mm-dd
      $(".detailsList li:eq(3) span:eq(1)").text(d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));

      // Description
      $(".descBox").text(txn.Description || "");

      // Set edit link
      $(".btnGradient").attr("href", "/Transaction/Edit/" + txn.TransactionId);
    }

    $("#detailsOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });

  // Background click: Only close if user clicks directly on the background (not a child)
  $("#detailsOverlayBg").on("click", function (e) {
    if (e.target === this) {
      $("#detailsOverlayBg").removeClass("open");
      $("body").css("overflow", "");
    }
  });

  // Close button: Always close, no matter what inside is clicked
  $("#detailsOverlayClose").on("click", function () {
    $("#detailsOverlayBg").removeClass("open");
    $("body").css("overflow", "");
  });

  // Prevent overlay click bubbling (so clicking card doesn't close the overlay)
  $(".detailsOverlayPanel").on("click", function (e) {
    e.stopPropagation();
  });

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
  $("#btnIncome").on("click", function () {
    setTransactionType("Income");
  });
  $("#btnExpense").on("click", function () {
    setTransactionType("Expense");
  });
  // Set default as Expense
  setTransactionType("Expense");

  // Quick category select
  $(".quickCat").on("click", function () {
    $(".quickCat").removeClass("selected");
    $(this).addClass("selected");
    let val = $(this).data("value");
    $("#categoryId").val(val);
  });

  // function setTransactionType(type) {
  //   document.getElementById("transactionType").value = type;
  //   document.getElementById("btnIncome").classList.remove("active");
  //   document.getElementById("btnExpense").classList.remove("active");
  //   if (type === "Income") {
  //     document.getElementById("btnIncome").classList.add("active");
  //   } else {
  //     document.getElementById("btnExpense").classList.add("active");
  //   }
  // }
  // setTransactionType("Expense");
});
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/transactionHub")
    .build();

connection.start()
    .then(() => console.log("SignalR connected"))
    .catch(err => console.error(err));  

connection.on("TransactionAdded", transaction => {
    // console.log("New transaction added:", transaction);
    location.reload();
});

connection.on("TransactionUpdated", transaction => {
    // console.log("Transaction updated:", transaction);
    location.reload();
});

connection.on("TransactionDeleted", transactionId => {
    // console.log("Transaction deleted:", transactionId);
    location.reload();
});
