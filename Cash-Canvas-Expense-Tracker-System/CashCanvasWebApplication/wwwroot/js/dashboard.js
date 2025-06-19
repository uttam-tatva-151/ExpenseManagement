$(function () {
  let selectedTransactionExportType = "";

  function showFiller(containerSelector, message) {
    const el = document.querySelector(containerSelector);
    if (el) {
      el.innerHTML = `
              <div class="billFiller text-center py-5">
                  <div class="fillerIcon mb-3"><i class="bi bi-emoji-frown"></i></div>
                  <div class="fillerTitle">${message}</div>
                  <div class="fillerSubtitle text-muted">Please add some data to see a visualization.</div>
              </div>
          `;
    }
  }
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
      window.location.href = "/Transaction/ExportPdf";
      $("#exportTransactionModal").modal("hide");
    } else if (selectedTransactionExportType === "csv") {
      window.location.href = "/Transaction/ExportCsv";
      $("#exportTransactionModal").modal("hide");
    } else if (selectedTransactionExportType === "excel") {
      window.location.href = "/Transaction/ExportExcel";
      $("#exportTransactionModal").modal("hide");
    } else {
      alert("Please select a valid export format.");
    }
  });

  // === Revenue Area Chart ===
  const revenueChartOptions = {
    series: revenueSeries,
    chart: {
      height: 300,
      type: "area",
      toolbar: { show: false },
    },
    colors: ["#0d6efd", "#dc3545"],
    dataLabels: { enabled: false },
    stroke: { curve: "smooth" },
    xaxis: {
      categories: revenueLabels,
      labels: { format: "yyyy-MM" },
    },
    yaxis: {
      title: { text: "Amount ($)" },
    },
    tooltip: {
      x: { format: "MMM yyyy" },
    },
    legend: { show: true, position: "top" },
  };

  if (
    !Array.isArray(revenueSeries) ||
    revenueSeries.length === 0 ||
    revenueSeries.every((s) => !Array.isArray(s.data) || s.data.length === 0)
  ) {
    showFiller("#revenue-chart", "No revenue or expense data available yet.");
  } else {
    const revenueChart = new ApexCharts(
      document.querySelector("#revenue-chart"),
      revenueChartOptions
    );
    revenueChart.render();
  }
  // === Expense By Category Pie Chart ===
  const pieChartOptions = {
    series: expenseCategorySeries,
    chart: { type: "pie", height: 300 },
    labels: expenseCategoryLabels,
    colors: [
      "#0d6efd", // blue
      "#20c997", // green-teal
      "#ffc107", // yellow
      "#6f42c1", // purple
      "#fd7e14", // orange
      "#6610f2", // indigo
      "#e83e8c", // pink
      "#17a2b8", // cyan
      "#28a745", // green
      "#dc3545", // red
      "#007bff", // blue
      "#ff66c4", // magenta
      "#f672a7", // light pink
      "#ffa600", // gold
      "#bada55", // light green
      "#7cfc00", // lawn green
      "#00bcd4", // turquoise
      "#ff8c00", // dark orange
      "#8b0000", // dark red
      "#2e8b57", // sea green
      "#ff1493", // deep pink
      "#483d8b", // dark slate blue
      "#00ced1", // dark turquoise
      "#d2691e", // chocolate
      "#9acd32", // yellow green
      "#f08080", // light coral
      "#1e90ff", // dodger blue
      "#daa520", // goldenrod
      "#9932cc", // dark orchid
      "#5f9ea0", // cadet blue
      // ...add more if needed
    ],
    legend: { show: true, position: "bottom" },
    dataLabels: { enabled: true },
  };

  if (
    !Array.isArray(expenseCategorySeries) ||
    expenseCategorySeries.length === 0 ||
    expenseCategorySeries.every((val) => !val)
  ) {
    showFiller("#pie-chart", "No expense category data available yet.");
  } else {
    const pieChart = new ApexCharts(
      document.querySelector("#pie-chart"),
      pieChartOptions
    );
    pieChart.render();
  }
});
