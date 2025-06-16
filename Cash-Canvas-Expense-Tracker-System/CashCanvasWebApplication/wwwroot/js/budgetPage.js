function showMainList() {
    $('#mainBudgetList').show();
    $('#trashBudgetList').hide();
    $('#btnShowTrash').removeClass('active');
    $('#btnShowMain').addClass('active');
}

function showTrashList() {
    $('#mainBudgetList').hide();
    $('#trashBudgetList').show();
    $('#btnShowTrash').addClass('active');
    $('#btnShowMain').removeClass('active');
}

$(document).ready(function () {
    showMainList();
});