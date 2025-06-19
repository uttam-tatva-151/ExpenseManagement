const paginationDetails = {
  totalRecords: 0,
  pageNumber: 1,
  pageSize: 0,      
  sortBy: null,                 
  isAscending: true,
  timeFilter: "",
  fromDate: null,              
  toDate: null,
  searchTerm: "",
};

// #region Bill List Toggle
function showMainList() {
  $("#mainBillList").show();
  $("#trashBillList").hide();
  $("#btnShowMain").addClass("active");
  $("#btnShowTrash").removeClass("active");
}

function showTrashList() {
  $("#mainBillList").hide();
  $("#trashBillList").show();
  $("#btnShowMain").removeClass("active");
  $("#btnShowTrash").addClass("active");
}

window.onload = function () {
  if (
    document.getElementById("mainBillList") &&
    document.getElementById("trashBillList")
  ) {
    showMainList();
  }
};
// #endregion

// #region Bill Details & Overlay
$(document).ready(function () {
  $(".btnShowDetails").on("click", function (e) {
    e.preventDefault();
    let billId = $(this).data("bill-id");
    let bill = bills.find((b) => b.BillId == billId);
    if (bill) {
      $(".detailsList .statusPill")
        .removeClass("statusActive statusOverdue")
        .addClass(bill.IsOverdue ? "statusOverdue" : "statusActive")
        .html(
          `<i class="bi bi-${
            bill.IsOverdue ? "exclamation-circle" : "check-circle"
          }"></i> ${bill.IsOverdue ? "Overdue" : "Active"}`
        );
      $(".detailsList .amountDetail")
        .removeClass("amountIncome amountExpense")
        .addClass("amountExpense")
        .text(
          "-₹" +
            Number(bill.Amount).toLocaleString("en-IN", {
              minimumFractionDigits: 2,
            })
        );
      let dueDate = new Date(bill.DueDate);
      $(".detailsList .dueDate").text(dueDate.toLocaleDateString("en-CA"));
      $(".detailsList .frequency").text(
        billFrequencyMap[bill.Frequency] || "One-time"
      );
      $(".detailsList .reminderDay").text(
        bill.ReminderDay + " Days ago" || "None"
      );
      $(".detailsList .paymentMethod").text(
        paymentMethodMap[bill.PaymentMethod] || "Not specified"
      );
      $(".detailsList .notes").text(bill.Notes || "No additional notes");
      $(".editBillBtn").attr("href", "/Bill/EditBill/" + bill.BillId);
    }
    $("#detailsOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });

  $(".detailsOverlayBg").on("click", function (e) {
    if (e.target === this) {
      $(".detailsOverlayBg").removeClass("open");
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
});
// #endregion

// #region Filter Overlay
$(document).ready(function () {
  $("#btnFilter").on("click", function (e) {
    e.preventDefault();
    $("#filterOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });


});
// #endregion

// #region Bill Delete Confirmation
$(document).ready(function () {
  $(".btnDeleteBill").on("click", function (e) {
    e.preventDefault();
    const billId = $(this).data("bill-id");
    $("#billIdToDelete").attr("value", billId);
    $("#deleteConfirmModal").modal("show");
  });
});
// #endregion

// #region Payment Submission & Modal
$(document).ready(function () {
  $("#submitPayment").on("click", function () {
    const $form = $("#markAsPaidForm");
    if ($form[0].checkValidity()) {
      $("#markAsPaidModal").modal("hide");
    } else {
      $form.addClass("was-validated");
    }
  });

  $(".markAsPaidBtn").on("click", function (e) {
    e.preventDefault();
    const billId = $(this).data("bill-id");
    $("#billId").val(billId);
    $("#markAsPaidModal").modal("show");
  });

  $("#markAsPaidModal").on("hidden.bs.modal", function () {
    $("#markAsPaidForm")[0].reset();
    $("#markAsPaidForm").removeClass("was-validated");
  });
});
// #endregion

// #region Export Modal
$(document).ready(function () {
  let selectedExportType = "";

  $("#exportBtn").on("click", function () {
    $(".exportTypeBtn").removeClass("active");
    $("#exportSelectedFormat").text("");
    $("#exportConfirmBtn").prop("disabled", true);
    selectedExportType = "";
    $("#exportModal").modal("show");
  });

  $(".exportTypeBtn").on("click", function () {
    var type = $(this).data("type");
    $(".exportTypeBtn").removeClass("active");
    $(this).addClass("active");
    selectedExportType = type;
    let label = $(this).find(".fw-bold").text();
    $("#exportSelectedFormat").text("Selected: " + label);

    if (type === "pdf") {
      $("#exportConfirmBtn").prop("disabled", false);
    } else {
      $("#exportConfirmBtn").prop("disabled", true);
      showExportComingSoon(label);
    }
  });

  $("#exportConfirmBtn").on("click", function () {
    if (selectedExportType === "pdf") {
      window.location.href = "/Bill/ExportPdf";
      $("#exportModal").modal("hide");
    }
  });

  function showExportComingSoon(label) {
    let msg = `${label} export will be available soon! Our team is working to bring you this option in a future update.`;
    $("#comingSoonMsg").text(msg);
    let toastEl = document.getElementById("comingSoonToast");
    let toast = bootstrap.Toast.getOrCreateInstance(toastEl);
    toast.show();
  }
});
// #endregion

// #region Reminder Modal
$(document).ready(function () {
  $(".addReminderBtn").on("click", function (e) {
    e.preventDefault();
    let billId = $(this).data("bill-id");
    $("#reminderBillId").val(billId);
    let bill = bills.find((b) => b.BillId == billId);

    console.log(bill);
    let maxDays =
      bill && bill.Frequency ? getMaxReminderDays(billFrequencyMap[bill.Frequency]) : 365;
    $("#reminderDaysInput").attr("max", maxDays);
    $("#saveReminderBtn").prop("disabled", false);
    $("#reminderModalBackdrop").css("display", "flex");
  });

  // Listen for changes to input
  $("#reminderDaysInput").on("input change", function () {
    let max = parseInt($(this).attr("max"), 10);
    let val = parseInt($(this).val(), 10);

    if (val > max) {
      $("#saveReminderBtn").prop("disabled", true);
      showReminderMaxToast(max);
    } else {
      $("#saveReminderBtn").prop("disabled", false);
    }
  });

  function showReminderMaxToast(max) {
  showToast(`You can't set reminder more than ${max} day(s) before due date for this bill.`,'Warning');
  }
  $("#closeReminderModalBtn").on("click", function () {
    $("#reminderModalBackdrop").css("display", "none");
  });

  $(".quickDayBtn").on("click", function () {
    $("#reminderDaysInput").val($(this).data("days"));
  });

  $("#saveReminderBtn").on("click", function () {
    let billId = $("#reminderBillId").val();
    let days = $("#reminderDaysInput").val();

    $.ajax({
      url: "/Bill/SetReminder",
      type: "POST",
      contentType: "application/json",
      data: {billid : billId, days : days},
      success: function (response) {
        if (response && response.success) {
          showToast(response.message, response.status);
          location.reload(); // Optionally reload to update UI
        } else {
          showToast(
            response.message || "Error in Set Reminder.",
            response.status
          );
        }
      },
      error: function () {
        showToast("Error in Set Reminder.", "Error");
      },
    });
    
    $("#reminderModalBackdrop").css("display", "none");
  });
});
function getMaxReminderDays(frequency) {
  switch (frequency) {
    case "Daily":
      return 1;
    case "Weekly":
      return 7;
    case "BiWeekly":
      return 14;
    case "Monthly":
      return 30;
    case "Quarterly":
      return 90;
    case "HalfYearly":
      return 183;
    case "Yearly":
      return 365;
    default:
      return 365;
  }
}
// #endregion

// #region Payment History Modal
$(document).on("click", ".btnShowHistory", function (e) {
  e.preventDefault();
  let billId = $(this).data("bill-id");
  $.get(
    "/Bill/GetPaymentHistoryTable",
    { billId: billId },
    function (partialHtml) {
      $("#historyTableBody").html(partialHtml);
      $("#historyModal").modal("show");
    }
  );
});

$(document).on("click", ".btnSkipPeriod", function (e) {
  e.preventDefault();
  let startDate = $(this).data("period-start");
  let paymentHistory = history.find(
    (b) => b.PeriodStart.ToString("yyyy-MM-dd") == startDate
  );

  if (paymentHistory) {
    $.ajax({
      url: "/Bill/SkipPeriod",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(paymentHistory),
      success: function (response) {
        if (response && response.success) {
          showToast(response.message, response.status);
          location.reload(); // Optionally reload to update UI
        } else {
          showToast(
            response.message || "Error skipping period.",
            response.status
          );
        }
      },
      error: function () {
        showToast("Error occurs in skipping period.", "Error");
      },
    });
  }
});
// #endregion
