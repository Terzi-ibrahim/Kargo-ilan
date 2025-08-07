window.onload = function () {
    const counters = document.querySelectorAll('.stat-number');

    counters.forEach(counter => {
        const target = +counter.getAttribute('data-target');  // Hedef sayı
        let current = 0;  // Başlangıç sayısı
        const increment = target / 100;  // Sayıyı 100 adımda artıracak hız

        const updateCounter = () => {
            if (current < target) {
                current += increment;  // Sayıyı artır
                counter.innerText = Math.ceil(current);  // Sayıyı güncelle
                setTimeout(updateCounter, 50);  // Her 50ms'de bir güncelleme yap
            } else {
                // Sayı hedefe ulaştığında, hedef sayıyı ve sonuna "+" ekle
                counter.innerText = target + "+";  // Hedef sayıya '+' ekle
            }
        };

        updateCounter();  // Animasyonu başlat
    });
};
window.addEventListener('DOMContentLoaded', () => {
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const fadeOut = () => {
                alert.style.transition = 'opacity 0.5s ease';
                alert.style.opacity = '0';
                setTimeout(() => alert.remove(), 500);
            };
            fadeOut();
        }, 3000);
    });
});