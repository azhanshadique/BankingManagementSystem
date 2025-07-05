document.addEventListener("DOMContentLoaded", function () {
    const toggleBtn = document.getElementById('toggleSidebar');
    const closeBtn = document.getElementById('closeSidebar');
    const sidebar = document.getElementById('sidebar');
    const mainContent = document.getElementById('mainContent');

    // Set default state if not stored
    if (localStorage.getItem("sidebar-collapsed") === null) {
        localStorage.setItem("sidebar-collapsed", "true");
    }

    // Apply stored state
    const isCollapsed = localStorage.getItem("sidebar-collapsed") === "true";
    if (isCollapsed) {
        if (sidebar) sidebar.classList.add('collapsed');
        mainContent.classList.add('collapsed');
    } else {
        if (toggleBtn) toggleBtn.classList.add('hidden');
    }

    // Toggle open/close  button
    if (toggleBtn) toggleBtn.addEventListener('click', function () {
        if (sidebar) sidebar.classList.remove('collapsed');
        mainContent.classList.remove('collapsed');
        if (toggleBtn) toggleBtn.classList.add('hidden');
        localStorage.setItem("sidebar-collapsed", "false");
    });

    // Close sidebar
    if (closeBtn) closeBtn.addEventListener('click', function () {
        if (sidebar) sidebar.classList.add('collapsed');
        mainContent.classList.add('collapsed');
        if (toggleBtn) toggleBtn.classList.remove('hidden');
        localStorage.setItem("sidebar-collapsed", "true");
    });
});


function showAlert(message, type = 'success') {
    const alertBox = document.getElementById('alertMessage');
    const alertContent = document.getElementById('alertContent');
    alertBox.classList.remove('alert-success', 'alert-danger', 'alert-warning', 'alert-info', 'd-none');
    alertBox.classList.add('alert-' + type);
    alertContent.innerText = message;
    alertBox.classList.add('show');
    setTimeout(() => {
        alertBox.classList.remove('show');
        alertBox.classList.add('d-none');
    }, 4000);
}
function showRegisterSuccessMessage(message, type = 'success') {
    const modal = document.getElementById('registerSuccessMessage');
    const content = document.getElementById('registerSuccessContent');
    if (!modal || !content) return;

    content.innerText = message;


    // Change header bg & footer button based on type
    const header = modal.querySelector('.modal-header');
    header.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info', 'bg-secondary');
    header.classList.add('bg-' + type);

    const footerBtn = modal.querySelector('#btnOk');
    footerBtn.classList.remove('btn-success', 'btn-danger', 'btn-warning', 'btn-info', 'btn-secondary');
    footerBtn.classList.add('btn-' + type);

    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}
function showDeleteConfirmModal() {
    const modal = document.getElementById('deleteConfirmModal');
    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}
function showApproveRejectMessage(message, type = 'success') {
    const modal = document.getElementById('approveRejectMessage');
    const content = document.getElementById('approveRejectContent');
    const title = document.getElementById('approveRejectLabel');
    if (!modal || !content) return;

    content.innerText = message;

    title.innerText = type == 'success' ? 'Client Request Approved' : 'Client Request Rejected'
    // Change header bg & footer button based on type
    const header = modal.querySelector('.modal-header');
    header.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info', 'bg-secondary');
    header.classList.add('bg-' + type);

    const footerBtn = modal.querySelector('#btnOkApproveReject');
    footerBtn.classList.remove('btn-success', 'btn-danger', 'btn-warning', 'btn-info', 'btn-secondary');
    footerBtn.classList.add('btn-' + type);

    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}

function showApproveRejectMessageByClient(message, type = 'success') {
    const modal = document.getElementById('approveRejectMessageByClient');
    const content = document.getElementById('approveRejectContentByClient');
    const title = document.getElementById('approveRejectLabelByClient');
    if (!modal || !content) return;

    content.innerText = message;

    title.innerText = type == 'success' ? 'Joint Account Request Approved' : 'Joint Account Request Rejected'
    // Change header bg & footer button based on type
    const header = modal.querySelector('.modal-header');
    header.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info', 'bg-secondary');
    header.classList.add('bg-' + type);

    const footerBtn = modal.querySelector('#btnOkApproveRejectByClient');
    footerBtn.classList.remove('btn-success', 'btn-danger', 'btn-warning', 'btn-info', 'btn-secondary');
    footerBtn.classList.add('btn-' + type);

    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}
function showDeleteMessageByClientOnDashboard(message, type = 'success') {
    const modal = document.getElementById('deleteMessageByClientOnDashboard');
    const content = document.getElementById('deleteContentByClientOnDashboard');
    const title = document.getElementById('deleteLabelByClientOnDashboard');
    if (!modal || !content) return;

    content.innerText = message;

    title.innerText = type == 'success' ? 'Request Approved' : 'Request Deleted'
    // Change header bg & footer button based on type
    const header = modal.querySelector('.modal-header');
    header.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info', 'bg-secondary');
    header.classList.add('bg-' + type);

    const footerBtn = modal.querySelector('#btnOkDeleteByClientOnDashboard');
    footerBtn.classList.remove('btn-success', 'btn-danger', 'btn-warning', 'btn-info', 'btn-secondary');
    footerBtn.classList.add('btn-' + type);

    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}


window.onload = function () {
    if (performance.navigation.type === 2) {
        // If user comes via back button, reload the page
        location.reload(true);
    }
};

