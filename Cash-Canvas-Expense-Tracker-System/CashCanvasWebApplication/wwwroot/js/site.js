const billFrequencyMap = {
    0: "Daily",
    1: "Weekly",
    2: "BiWeekly",
    3: "Monthly",
    4: "Quarterly",
    5: "HalfYearly",
    6: "Yearly",
    7: "OneTime",
};
const budgetPeriodMap = {
    0: "Weekly",
    1: "Monthly",
    2: "Quarterly",
    3: "Yearly",
    4: "BiWeekly",
    5: "HalfYearly"
};
const paymentMethodMap = {
    0: "Cash",
    1: "Card",
    2: "UPI",
    3: "NetBanking",
    4: "Cheque",
    5: "Wallet" 
};


toastr.options = {
    "closeButton": true,
    "progressBar": true,
    "timeOut": "7272",
    "positionClass": "toast-bottom-right", // Snackbar is usually centered
    "showDuration": "600",
    "hideDuration": "600",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "preventDuplicates": true,
    "newestOnTop": true
};

function showToast(message, type) {
    if (!message) return;

    // Enum mapping: sync with your C# enum values
    const responseStatusMap = {
        0: 'success',        // ResponseStatus.Success
        1: 'error',          // ResponseStatus.Failed
        2: 'warning',        // ResponseStatus.Warning
        3: 'info',           // ResponseStatus.NotFound
        4: 'error',          // ResponseStatus.Unauthorized
        5: 'warning'         // ResponseStatus.ValidationError
    };

    // Normalize type to lowercase string
    if (typeof type === 'number') {
        type = responseStatusMap[type] || 'info';
    } else {
        type = type.toLowerCase();
    }

    switch (type) {
        case 'success':
            toastr.success(message);
            break;
        case 'error':
            toastr.error(message);
            break;
        case 'warning':
            toastr.warning(message);
            break;
        default:
            toastr.info(message);
    }
}

// function showGlobalError(msg) {
//     var errorDiv = document.getElementById('globalError');
//     errorDiv.textContent = msg;
//     errorDiv.classList.remove('d-none');
// }
// window.onerror = function (message) {
//     showGlobalError("Oops! Something went wrong on this page.");
//     return true;
// };

document.querySelectorAll('.section-toggle').forEach(btn => {
    btn.addEventListener('click', function(e) {
        e.preventDefault();
        const section = btn.dataset.section;
        document.querySelectorAll('.dropdown-submenu').forEach(sub => sub.classList.add('d-none'));
        document.getElementById(`${section}List`).classList.toggle('d-none');
    });
});

// Show all notifications modal
document.getElementById("showAllNotificationsBtn").addEventListener("click", function() {
    var modal = new bootstrap.Modal(document.getElementById('allNotificationsModal'));
    modal.show();
});