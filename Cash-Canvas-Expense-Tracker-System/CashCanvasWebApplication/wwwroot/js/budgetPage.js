function showMainList() {
  $("#mainBudgetList").show();
  $("#trashBudgetList").hide();
  $("#btnShowTrash").removeClass("active");
  $("#btnShowMain").addClass("active");
}

function showTrashList() {
  $("#mainBudgetList").hide();
  $("#trashBudgetList").show();
  $("#btnShowTrash").addClass("active");
  $("#btnShowMain").removeClass("active");
}

$(document).ready(function () {
  showMainList();

  $(".btnShowDetails").on("click", function (e) {
    e.preventDefault();

    let budgetId = $(this).data("budget-id");
    let budget = budgets.find((b) => b.BudgetId == budgetId);
    console.log("Budget details:", budget);
    if (budget) {
      // Set status
      $(".detailsList .statusPill")
        .removeClass("statusActive statusInactive")
        .addClass(budget.IsContinued ? "statusActive" : "statusInactive")
        .html(
          `<i class="bi bi-${
            budget.IsContinued ? "check-circle" : "pause-circle"
          }"></i> ${budget.IsContinued ? "Active" : "Inactive"}`
        );

      // Category Name
      $(".detailsList .categoryName").text(budget.CategoryName);

      // Amount
      $(".detailsList .budgetAmount").text(
        "₹" +
          Number(budget.Amount).toLocaleString("en-IN", {
            minimumFractionDigits: 2,
          })
      );

      $(".detailsList .budgetPeriod").text(budgetPeriodMap[budget.Period] || "N/A");

      // Notes
      $(".detailsList .budgetNotes").text(
        budget.Notes || "No additional notes"
      );

      // Edit button
      $(".editBudgetBtn").attr("href", "/Budget/Edit/" + budget.BudgetId);
    }

    $("#budgetDetailsOverlayBg").addClass("open");
    $("body").css("overflow", "hidden");
  });

  $("#budgetDetailsOverlayClose").on("click", function () {
    $("#budgetDetailsOverlayBg").removeClass("open");
    $("body").css("overflow", "");
  });
  $(".detailsOverlayBg").on("click", function (e) {
    if (e.target === this) {
      $(".detailsOverlayBg").removeClass("open");
      $("body").css("overflow", "");
    }
  });
  $(".detailsOverlayPanel").on("click", function (e) {
    e.stopPropagation();
  });

  // Delete Confirmation
  $(".btnMoveToTrash").on("click", function (e) {
    e.preventDefault();
    const budgetId = $(this).data("budget-id");
    $("#budgetIdToDelete").attr("value", budgetId);
    $("#deleteConfirmModal").modal("show");
  });
});
