document.addEventListener('DOMContentLoaded', function () {
    setupDropdowns();
    setupImageUpload();
    setupModal();
    setupTextEditors();
    setupTabs();
    setupEditButtons();
    setupDeleteButtons();
});

/* =================================================================
   DROPDOWN FUNCTIONALITY
   ================================================================= */
function setupDropdowns() {
    const dropdownButtons = document.querySelectorAll('[data-type="dropdown"]');

    function closeAllDropdowns() {
        document.querySelectorAll('#account-dropdown, [id^="project-dropdown-"]').forEach(dropdown => {
            dropdown.style.display = 'none';
        });
    }

    closeAllDropdowns();

    dropdownButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.stopPropagation();
            const targetId = this.getAttribute('data-target');
            const dropdown = document.querySelector(targetId);
            if (!dropdown) return;

            const isOpen = dropdown.style.display === 'block';
            closeAllDropdowns();
            dropdown.style.display = isOpen ? 'none' : 'block';
        });
    });

    document.addEventListener('click', closeAllDropdowns);

    document.querySelectorAll('#account-dropdown, [id^="project-dropdown-"]').forEach(dropdown => {
        dropdown.addEventListener('click', function (e) {
            e.stopPropagation();
        });
    });

    document.querySelectorAll('#account-dropdown a, #account-dropdown button, [id^="project-dropdown-"] button').forEach(el => {
        el.addEventListener('click', closeAllDropdowns);
    });
}

/* =================================================================
   IMAGE UPLOAD FUNCTIONALITY
   ================================================================= */
function setupImageUpload() {
    const imageIcons = document.querySelectorAll('.image-icon[data-type="file"]');

    imageIcons.forEach(icon => {
        const fileInput = document.createElement('input');
        fileInput.type = 'file';
        fileInput.accept = 'image/*';
        fileInput.name = 'Image';
        fileInput.style.display = 'none';
        document.body.appendChild(fileInput);

        icon.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const file = this.files[0];
                if (!file.type.match('image.*')) {
                    alert('Vänligen välj en bildfil.');
                    return;
                }

                const reader = new FileReader();
                reader.onload = function (e) {
                    icon.src = e.target.result;
                    icon.classList.add('uploaded');
                    const hiddenInput = document.querySelector('input[name="Image"]');
                    if (hiddenInput) hiddenInput.value = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    });
}

/* =================================================================
   MODAL FUNCTIONALITY
   ================================================================= */
function setupModal() {
    document.querySelectorAll('.modal').forEach(m => m.style.display = 'none');

    document.querySelectorAll('[data-type="modal"]').forEach(button => {
        button.addEventListener('click', function () {
            const modal = document.querySelector(this.getAttribute('data-target'));
            if (modal) {
                modal.style.display = 'flex';
                document.body.style.overflow = 'hidden';
            }
        });
    });

    document.querySelectorAll('[data-type="close"]').forEach(button => {
        button.addEventListener('click', function () {
            const modal = document.getElementById(this.getAttribute('data-target'));
            if (modal) {
                modal.style.display = 'none';
                document.body.style.overflow = '';
            }
        });
    });

    document.querySelectorAll('.modal').forEach(modal => {
        modal.addEventListener('click', function (e) {
            if (e.target === this) {
                this.style.display = 'none';
                document.body.style.overflow = '';
            }
        });
    });
}

/* =================================================================
   RICH TEXT EDITORS (QUILL)
   ================================================================= */
function setupTextEditors() {
    const addTextArea = document.getElementById('add-project-description');
    if (addTextArea) {
        const addQuill = new Quill('#add-project-description-wysiwyg-editor', {
            modules: { toolbar: '#add-project-description-wysiwyg-toolbar' },
            theme: 'snow'
        });
        addQuill.on('text-change', () => {
            addTextArea.value = addQuill.root.innerHTML;
        });
    }

    const editTextArea = document.getElementById('edit-project-description');
    if (editTextArea) {
        window.editProjectQuill = new Quill('#edit-project-description-wysiwyg-editor', {
            modules: { toolbar: '#edit-project-description-wysiwyg-toolbar' },
            theme: 'snow'
        });
        window.editProjectQuill.on('text-change', () => {
            editTextArea.value = window.editProjectQuill.root.innerHTML;
        });
    }
}

/* =================================================================
   EDIT PROJECT FUNCTIONALITY
   ================================================================= */
function setupEditButtons() {
    document.querySelectorAll('.edit').forEach(button => {
        button.addEventListener('click', () => {
            const item = button.closest('.project-item');
            if (!item) return;

            document.querySelector('input[name="ProjectId"]').value = item.dataset.projectId;
            document.querySelector('input[name="ProjectName"]').value = item.dataset.projectName;
            document.querySelector('input[name="ClientName"]').value = item.dataset.clientName;
            document.querySelector('input[name="StartDate"]').value = item.dataset.startDate;
            document.querySelector('input[name="EndDate"]').value = item.dataset.endDate;
            document.querySelector('input[name="Budget"]').value = item.dataset.budget;
            document.querySelector('input[name="Image"]').value = item.dataset.image;
            document.querySelector('select[name="Status"]').value =
                item.dataset.status === 'started' ? '1' : '2';

            if (window.editProjectQuill) {
                editProjectQuill.root.innerHTML = item.dataset.description;
                document.getElementById('edit-project-description').value = item.dataset.description;
            }

            document.getElementById('edit-project-modal').style.display = 'flex';
            document.body.style.overflow = 'hidden';
        });
    });
}

/* =================================================================
   TABS + FILTERING
   ================================================================= */
function setupTabs() {
    const tabs = document.querySelectorAll('.tab-bar .nav-tab');
    const projectItems = document.querySelectorAll('.project-item');

    tabs.forEach(tab => {
        tab.addEventListener('click', function (e) {
            e.preventDefault();
            tabs.forEach(t => t.classList.remove('active'));
            this.classList.add('active');

            const filter = this.getAttribute('data-filter');
            projectItems.forEach(item => {
                const status = item.getAttribute('data-status');
                item.style.display = (filter === 'all' || status === filter) ? '' : 'none';
            });
        });
    });

    const updateTabCounts = () => {
        const allCount = projectItems.length;
        const startedCount = document.querySelectorAll('.project-item[data-status="started"]').length;
        const completedCount = document.querySelectorAll('.project-item[data-status="completed"]').length;

        document.querySelector('[data-filter="all"]').textContent = `ALL [${allCount}]`;
        document.querySelector('[data-filter="started"]').textContent = `STARTED [${startedCount}]`;
        document.querySelector('[data-filter="completed"]').textContent = `COMPLETED [${completedCount}]`;
    };

    updateTabCounts();
}



/* =================================================================
   Delete PROJECT FUNCTIONALITY
   ================================================================= */

function setupDeleteButtons() {
    document.querySelectorAll('.delete').forEach(button => {
        button.addEventListener('click', () => {
            const projectId = button.dataset.projectId;
            const projectName = button.dataset.projectName;

            const confirmed = confirm(`Are you sure you want to delete "${projectName}"?`);
            if (confirmed) {
                const form = document.getElementById('delete-project-form');
                document.getElementById('delete-project-id').value = projectId;
                form.submit();
            }
        });
    });
}



