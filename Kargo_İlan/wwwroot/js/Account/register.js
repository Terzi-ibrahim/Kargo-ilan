document.addEventListener("DOMContentLoaded", function () {
    // 1. Slayt gösterisi
    const slideshowImage = document.getElementById("slideshowImage");
    if (slideshowImage) {
        const images = [
            "/images/foto1.png",
            "/images/foto2.png",
            "/images/foto3.png",
            "/images/foto4.png"
        ];
        let current = 0;
        setInterval(() => {
            current = (current + 1) % images.length;
            slideshowImage.style.opacity = 0;
            setTimeout(() => {
                slideshowImage.src = images[current];
                slideshowImage.style.opacity = 1;
            }, 400);
        }, 4000);
    }

    // 2. Şifre göster/gizle
    function setupTogglePassword(inputId, toggleId) {
        const input = document.getElementById(inputId);
        const toggle = document.getElementById(toggleId);
        if (input && toggle) {
            toggle.addEventListener("click", () => {
                const isPassword = input.type === "password";
                input.type = isPassword ? "text" : "password";

                // İkonu güncelle
                toggle.classList.remove("bi-eye-fill", "bi-eye-slash-fill");
                toggle.classList.add(isPassword ? "bi-eye-fill" : "bi-eye-slash-fill");
            });
        }
    }

    setupTogglePassword("Password", "togglePassword");
    setupTogglePassword("ConfirmPassword", "toggleConfirmPassword");

    // 3. Telefon numarası maskesi - Vanilla JS
    const phoneInput = document.getElementById("PhoneNumber");
    if (phoneInput) {
        phoneInput.addEventListener("input", function (e) {
            let input = e.target.value;
            let numbers = input.replace(/\D/g, '').substring(0, 10);
            let formatted = '';
            if (numbers.length > 6) {
                formatted = `(${numbers.substring(0, 3)}) ${numbers.substring(3, 6)}-${numbers.substring(6, 10)}`;
            } else if (numbers.length > 3) {
                formatted = `(${numbers.substring(0, 3)}) ${numbers.substring(3, 6)}`;
            } else if (numbers.length > 0) {
                formatted = `(${numbers}`;
            }
            e.target.value = formatted;
        });
    }

});
