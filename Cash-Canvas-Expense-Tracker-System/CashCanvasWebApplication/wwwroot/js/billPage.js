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
$(document).ready(function () {
  // Show Details Overlay
  $(".btnShowDetails").on("click", function (e) {
    e.preventDefault();

    let billId = $(this).data("bill-id");
    let bill = bills.find((b) => b.BillId == billId);
    if (bill) {
      // Set status
      $(".detailsList .statusPill")
        .removeClass("statusActive statusOverdue")
        .addClass(bill.IsOverdue ? "statusOverdue" : "statusActive")
        .html(
          `<i class="bi bi-${
            bill.IsOverdue ? "exclamation-circle" : "check-circle"
          }"></i> ${bill.IsOverdue ? "Overdue" : "Active"}`
        );

      // Amount
      $(".detailsList .amountDetail")
        .removeClass("amountIncome amountExpense")
        .addClass("amountExpense")
        .text(
          "-₹" +
            Number(bill.Amount).toLocaleString("en-IN", {
              minimumFractionDigits: 2,
            })
        );

      // Due date
      let dueDate = new Date(bill.DueDate);
      $(".detailsList .dueDate").text(dueDate.toLocaleDateString("en-CA")); // yyyy-mm-dd
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

  // Background click: Only close if user clicks directly on the background
  $(".detailsOverlayBg").on("click", function (e) {
    if (e.target === this) {
      $(".detailsOverlayBg").removeClass("open");
      $("body").css("overflow", "");
    }
  });

  // Close button: Always close
  $("#detailsOverlayClose").on("click", function () {
    $("#detailsOverlayBg").removeClass("open");
    $("body").css("overflow", "");
  });

  // Prevent overlay click bubbling
  $(".detailsOverlayPanel").on("click", function (e) {
    e.stopPropagation();
  });
  $("#btnFilter").on("click", function (e) {
    e.preventDefault();
    $("#filterOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });

  // Delete Confirmation
  $(".btnDeleteBill").on("click", function (e) {
    e.preventDefault();
    const billId = $(this).data("bill-id");
    $("#billIdToDelete").attr("value", billId);
    $("#deleteConfirmModal").modal("show");
  });

  // // Confirm Delete
  // $("#confirmDeleteBtn").on("click", function () {
  //   const billId = $(this).data("bill-id");
  //   $.ajax({
  //     url: `/Bill/MoveToTrash/${billId}`,
  //     method: "POST",
  //     success: function () {
  //       $("#deleteConfirmModal").modal("hide");
  //       location.reload(); // Refresh to update lists
  //     },
  //   });
  // });
});
$(document).ready(function () {
  $("#submitPayment").on("click", function () {
    const $form = $("#markAsPaidForm");
    if ($form[0].checkValidity()) {
      const formData = new FormData($form[0]);
      const paymentData = {
        BillId: formData.get("BillId"),
        AmountPaid: parseFloat(formData.get("AmountPaid")),
        PaymentMethod: formData.get("PaymentMethod"),
        Notes: formData.get("Notes") || null,
      };
      // Example: Send paymentData to backend (replace with actual API call)
      console.log("Payment Data:", paymentData);
      // Close modal after submission
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
      window.location.href = '/Bill/ExportPdf';
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

  $('.addReminderBtn').on('click', function(e) {
    e.preventDefault();
    $('#reminderBillId').val($(this).data('bill-id'));
    $('#reminderModalBackdrop').css('display', 'flex');
});

// Close modal
$('#closeReminderModalBtn').on('click', function() {
    $('#reminderModalBackdrop').css('display', 'none');
});

// Quick pick buttons
$('.quickDayBtn').on('click', function() {
    $('#reminderDaysInput').val($(this).data('days'));
});

// Save Reminder button
$('#saveReminderBtn').on('click', function() {
    var billId = $('#reminderBillId').val();
    var days = $('#reminderDaysInput').val();
    alert("Reminder set for " + days + " day(s) before due date!");
    $('#reminderModalBackdrop').css('display', 'none');
});
});

$(document).on("click", ".btnShowHistory", function (e) {
  e.preventDefault();
  let billId = $(this).data("bill-id");
  // Fetch the rendered partial view (HTML)
  $.get(
    "/Bill/GetPaymentHistoryTable",
    { billId: billId },
    function (partialHtml) {
      console.log(partialHtml);
      $("#historyTableBody").html(partialHtml);
      $("#historyModal").modal("show");
    }
  );
});
