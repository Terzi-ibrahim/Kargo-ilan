document.addEventListener("DOMContentLoaded", function () {
    const passwordInput = document.getElementById("Password");
    const togglePassword = document.getElementById("togglePassword");

    if (passwordInput && togglePassword) {
        // Başlangıçta kapalı göz ikonu gösterilsin
        togglePassword.classList.add("bi-eye-slash-fill");

        togglePassword.addEventListener("click", () => {
            const isPassword = passwordInput.type === "password";
            passwordInput.type = isPassword ? "text" : "password";

            // Eğer gösteriliyorsa, açık göz ikonu
            togglePassword.classList.toggle("bi-eye-fill", isPassword);
            togglePassword.classList.toggle("bi-eye-slash-fill", !isPassword);

            togglePassword.title = isPassword ? "Şifreyi gizle" : "Şifreyi göster";
        });
    }
});
