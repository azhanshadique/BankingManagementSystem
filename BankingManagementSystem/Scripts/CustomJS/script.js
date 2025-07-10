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












function showDynamicModal({
    titleText,
    messageText,
    type = 'primary',           
    redirectUrl,       
    modalId = 'generalModal',
    btnId = 'btnGeneralModalOk',         
    btnText = 'OK',

}) {
    const modal = document.getElementById(modalId);
    const header = modal.querySelector('.modal-header');
    const content = modal.querySelector('.modal-body');
    const title = modal.querySelector('.modal-title');
    const btn = btnId ? document.getElementById(btnId) : null;

    if (!modal || !header || !content || !title) return;

    // Set content
    title.innerText = titleText;
    content.innerText = messageText;

    // Reset and set Bootstrap color
    const colors = ['bg-success', 'bg-danger', 'bg-warning', 'bg-info', 'bg-secondary'];
    header.classList.remove(...colors);
    header.classList.add(`bg-${type}`);

    if (btn) {
        const btnColors = ['btn-success', 'btn-danger', 'btn-warning', 'btn-info', 'btn-secondary'];
        btn.classList.remove(...btnColors);
        btn.classList.add(`btn-${type}`);
        btn.innerText = btnText;

        if (redirectUrl) {
            //btn.onclick = () => { window.location.href = redirectUrl; };
            btn.addEventListener('click', function handleRedirect() {
                window.location.href = redirectUrl;
            });


        }
    }

    // Show modal
    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}







window.onload = function () {
    if (performance.navigation.type === 2) {
        // If user comes via back button, reload the page
        location.reload(true);
    }
};

