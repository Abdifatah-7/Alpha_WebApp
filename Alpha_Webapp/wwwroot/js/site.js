/* =================================================================
   DOCUMENT READY
   ================================================================= */
document.addEventListener('DOMContentLoaded', function () {
    // Initialize all components
    setupDropdowns();
    setupImageUpload();
    setupModal();
    setupTextEditors();
});

/* =================================================================
   DROPDOWN FUNCTIONALITY
   ================================================================= */
function setupDropdowns() {
    // Find all dropdown buttons
    const dropdownButtons = document.querySelectorAll('[date-type="dropdown"]');

    // Function to close all dropdowns
    function closeAllDropdowns() {
        // Get all dropdown elements
        const dropdowns = document.querySelectorAll('#account-dropdown, #project-dropdown-1, #project-dropdown-2, [id^="project-dropdown-"]');
        dropdowns.forEach(dropdown => {
            dropdown.style.display = 'none';
        });
    }

    // Hide all dropdowns initially
    closeAllDropdowns();

    // Add click event for each dropdown button
    dropdownButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.stopPropagation();

            // Get target dropdown from data-target attribute
            const targetId = this.getAttribute('data-target');
            const dropdownElement = document.querySelector(targetId);

            if (dropdownElement) {
                // Check if this dropdown is already open
                const isOpen = dropdownElement.style.display === 'block';

                // Close all dropdowns first
                closeAllDropdowns();

                // Toggle the current one (open if closed, close if open)
                dropdownElement.style.display = isOpen ? 'none' : 'block';
            }
        });
    });

    // Close dropdowns when clicking anywhere else on the page
    document.addEventListener('click', function () {
        closeAllDropdowns();
    });

    // Prevent closing when clicking inside the dropdown content
    document.querySelectorAll('#account-dropdown, [id^="project-dropdown-"]').forEach(dropdown => {
        dropdown.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });

    // Close dropdown after clicking an action item (buttons and links)
    document.querySelectorAll('#account-dropdown a, #account-dropdown button, [id^="project-dropdown-"] button').forEach(element => {
        element.addEventListener('click', function () {
            closeAllDropdowns();
        });
    });
}

/* =================================================================
   IMAGE UPLOAD FUNCTIONALITY
   ================================================================= */
function setupImageUpload() {
    // Find image icon and add click event
    const imageIcons = document.querySelectorAll('.image-icon[data-type="file"]');

    imageIcons.forEach(imageIcon => {
        if (imageIcon) {
            // Create hidden file input element
            const fileInput = document.createElement('input');
            fileInput.type = 'file';
            fileInput.accept = 'image/*'; // Accept only images
            fileInput.name = 'Image';
            fileInput.style.display = 'none';
            fileInput.name = 'projectImage';
            document.body.appendChild(fileInput);

            // When user clicks on image icon, trigger file input
            imageIcon.addEventListener('click', function () {
                fileInput.click();
            });

            // When a file is selected, show preview
            fileInput.addEventListener('change', function () {
                if (this.files && this.files[0]) {
                    const file = this.files[0];

                    // Check if file is an image
                    if (!file.type.match('image.*')) {
                        alert('Vänligen välj en bildfil (jpeg, png, gif, etc.)');
                        return;
                    }

                    // Show preview
                    const reader = new FileReader();

                    reader.onload = function (e) {
                        // Update image source with the uploaded image
                        imageIcon.src = e.target.result;

                        const hiddenInput = document.querySelector('input[name="Image"]');
                        if (hiddenInput) hiddenInput.value = e.target.result; 

                        // Add CSS class to show that an image has been uploaded
                        imageIcon.classList.add('uploaded');
                    };

                    // Read file as DataURL for preview
                    reader.readAsDataURL(file);
                }
            });
        }
    });
}

/* =================================================================
   MODAL FUNCTIONALITY
   ================================================================= */
function setupModal() {
    // Hide all modals initially
    const allModals = document.querySelectorAll('.modal');
    allModals.forEach(modal => {
        modal.style.display = 'none';
    });

    // Find elements to open modal
    const modalButtons = document.querySelectorAll('[data-type="modal"]');

    // Find elements to close modal
    const closeButtons = document.querySelectorAll('[data-type="close"]');

    // Listen for clicks to open modal
    modalButtons.forEach(button => {
        button.addEventListener('click', function () {
            const targetId = this.getAttribute('data-target');
            const modal = document.querySelector(targetId);

            if (modal) {
                // Show modal
                modal.style.display = 'flex';
                // Prevent scrolling on body when modal is open
                document.body.style.overflow = 'hidden';
            }
        });
    });

    // Listen for clicks to close modal
    closeButtons.forEach(button => {
        button.addEventListener('click', function () {
            const targetId = this.getAttribute('data-target');
            const modal = document.getElementById(targetId);

            if (modal) {
                // Hide modal
                modal.style.display = 'none';
                // Restore scrolling on body
                document.body.style.overflow = '';
            }
        });
    });

    // Close modal when user clicks outside content
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        modal.addEventListener('click', function (event) {
            // Check if click was directly on modal background (not on content)
            if (event.target === this) {
                // Hide modal
                this.style.display = 'none';
                // Restore scrolling on body
                document.body.style.overflow = '';
            }
        });
    });
}

/* =================================================================
   RICH TEXT EDITOR (QUILL)
   ================================================================= */
function setupTextEditors() {
    // Setup Add Project Description Editor
    const addProjectDescriptionTextarea = document.getElementById('add-project-description');
    if (addProjectDescriptionTextarea) {
        const addProjectDescriptionQuill = new Quill('#add-project-description-wysiwyg-editor', {
            modules: {
                syntax: true,
                toolbar: '#add-project-description-wysiwyg-toolbar'
            },
            theme: 'snow',
            placeholder: 'Type something'
        });

        addProjectDescriptionQuill.on('text-change', function () {
            addProjectDescriptionTextarea.value = addProjectDescriptionQuill.root.innerHTML;
        });
    }

    // Setup Edit Project Description Editor
    const editProjectDescriptionTextarea = document.getElementById('edit-project-description');
    if (editProjectDescriptionTextarea) {
        const editProjectDescriptionQuill = new Quill('#edit-project-description-wysiwyg-editor', {
            modules: {
                syntax: true,
                toolbar: '#edit-project-description-wysiwyg-toolbar'
            },
            theme: 'snow',
            placeholder: 'Type something'
        });

        editProjectDescriptionQuill.on('text-change', function () {
            editProjectDescriptionTextarea.value = editProjectDescriptionQuill.root.innerHTML;
        });
    }
}







document.addEventListener('DOMContentLoaded', function () {
    // Tab filter functionality
    const tabs = document.querySelectorAll('.tab-bar .nav-tab');
    const projectItems = document.querySelectorAll('.project-item');

    tabs.forEach(tab => {
        tab.addEventListener('click', function (e) {
            e.preventDefault();

            // Remove active class from all tabs
            tabs.forEach(t => t.classList.remove('active'));

            // Add active class to clicked tab
            this.classList.add('active');

            // Get the filter value
            const filter = this.getAttribute('data-filter');

            // Filter projects
            projectItems.forEach(item => {
                if (filter === 'all') {
                    item.style.display = '';
                } else {
                    const status = item.getAttribute('data-status');
                    item.style.display = (status === filter) ? '' : 'none';
                }
            });
        });
    });

    // Update tab counts
    function updateTabCounts() {
        const allCount = document.querySelectorAll('.project-item').length;
        const startedCount = document.querySelectorAll('.project-item[data-status="started"]').length;
        const completedCount = document.querySelectorAll('.project-item[data-status="completed"]').length;

        document.querySelector('[data-filter="all"]').textContent = `ALL [${allCount}]`;
        document.querySelector('[data-filter="started"]').textContent = `STARTED [${startedCount}]`;
        document.querySelector('[data-filter="completed"]').textContent = `COMPLETED [${completedCount}]`;
    }

    // Update counts when page loads
    updateTabCounts();
});