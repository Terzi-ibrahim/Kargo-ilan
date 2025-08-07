document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('createListingForm');
    const dropzoneContainer = document.querySelector('.dropzone-container');
    const maxFileSize = 5 * 1024 * 1024; // 5MB
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    let uploadedFiles = []; // Yüklenen dosyaları tutacak array

    // Dosya seçme butonu işlemleri
    function initializeFileUpload() {
        const fileButton = document.getElementById('fileButton');
        const fileInput = document.getElementById('fileInput');
        const previewsContainer = document.querySelector('.image-previews');

        if (fileButton) {
            fileButton.addEventListener('click', () => {
                if (fileInput) fileInput.click();
            });
        }

        if (fileInput) {
            fileInput.addEventListener('change', (e) => {
                handleFiles(e.target.files);
            });
        }

        // Drag & Drop işlemleri
        dropzoneContainer.addEventListener('dragover', (e) => {
            e.preventDefault();
            e.stopPropagation();
            dropzoneContainer.classList.add('dragover');
        });

        dropzoneContainer.addEventListener('dragleave', (e) => {
            e.preventDefault();
            e.stopPropagation();
            dropzoneContainer.classList.remove('dragover');
        });

        dropzoneContainer.addEventListener('drop', (e) => {
            e.preventDefault();
            e.stopPropagation();
            dropzoneContainer.classList.remove('dragover');
            const files = e.dataTransfer.files;
            handleFiles(files);
        });
    }

    // Dosya işleme fonksiyonu
    function handleFiles(files) {
        Array.from(files).forEach(file => {
            if (validateFile(file)) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    createPreviewElement(e.target.result, file);
                };
                reader.readAsDataURL(file);
                uploadedFiles.push(file);
            }
        });
        updateFormData();
    }

    // Önizleme elementi oluşturma
    function createPreviewElement(previewUrl, file) {
        const previewsContainer = document.querySelector('.image-previews');

        const previewWrapper = document.createElement('div');
        previewWrapper.className = 'preview-wrapper';

        const img = document.createElement('img');
        img.src = previewUrl;
        img.className = 'preview-image';

        const removeButton = document.createElement('button');
        removeButton.type = 'button';
        removeButton.className = 'remove-image';
        removeButton.innerHTML = '<i class="fas fa-times"></i>';

        removeButton.addEventListener('click', () => {
            previewWrapper.remove();
            uploadedFiles = uploadedFiles.filter(f => f !== file);
            updateFormData();
        });

        previewWrapper.appendChild(img);
        previewWrapper.appendChild(removeButton);
        previewsContainer.appendChild(previewWrapper);
    }

    // Form verisini güncelleme
    function updateFormData() {
        const formData = new FormData();
        uploadedFiles.forEach(file => {
            formData.append('Images', file);
        });
    }

    // Dosya validasyonu
    function validateFile(file) {
        if (!allowedTypes.includes(file.type)) {
            showError('Lütfen sadece resim dosyası yükleyin (JPEG, PNG, GIF)');
            return false;
        }
        if (file.size > maxFileSize) {
            showError('Dosya boyutu 5MB\'dan küçük olmalıdır');
            return false;
        }
        return true;
    }

    // Hata mesajı gösterme
    function showError(message) {
        const existingError = dropzoneContainer.querySelector('.alert');
        if (existingError) {
            existingError.remove();
        }

        const errorDiv = document.createElement('div');
        errorDiv.className = 'alert alert-danger mt-2';
        errorDiv.textContent = message;
        dropzoneContainer.appendChild(errorDiv);

        setTimeout(() => {
            errorDiv.remove();
        }, 3000);
    }

    // Form gönderimi
    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        const formData = new FormData(this);
        uploadedFiles.forEach(file => {
            formData.append('Images', file);
        });

        const submitButton = form.querySelector('button[type="submit"]');

        try {
            submitButton.disabled = true;
            submitButton.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Yükleniyor...';

            const response = await fetch(form.action, {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                throw new Error('Bir hata oluştu');
            }

            const result = await response.json();

            if (result.success) {
                window.location.href = result.redirectUrl || '/ilanlar';
            } else {
                showError(result.message || 'İlan oluşturulurken bir hata oluştu');
            }
        } catch (error) {
            showError('İlan oluşturulurken bir hata oluştu: ' + error.message);
        } finally {
            submitButton.disabled = false;
            submitButton.innerHTML = '<i class="fas fa-paper-plane me-2"></i>İlanı Yayınla';
        }
    });

    // Başlangıçta dosya yükleme işlemlerini başlat
    initializeFileUpload();
});
