// İlçeleri alfabetik sıralayan fonksiyon (Türkçe karakter duyarlı)
function sortDistrictsAlphabetically(districts) {
    return districts.sort((a, b) => a.Name.localeCompare(b.Name, 'tr'));
}

// İlleri alfabetik sıralayan fonksiyon
function sortSelectOptionsAlphabetically(selectElement) {
    const options = Array.from(selectElement.options).slice(1); // ilk option genelde "Seçin"
    options.sort((a, b) => a.text.localeCompare(b.text, 'tr'));
    selectElement.innerHTML = selectElement.options[0].outerHTML; // ilk option'u koru
    options.forEach(opt => selectElement.appendChild(opt));
}

// districtsJson değişkenini içeren <script> etiketi, sayfada olmalı:
// <script id="districts-data" type="application/json">{"1":[{"Id":...,"Name":...}, ...]}</script>
const districtsJson = JSON.parse(document.getElementById("districts-data").textContent);

window.addEventListener('DOMContentLoaded', () => {
    // İl select elementleri
    const loadingCity = document.getElementById('loadingCity');
    const destinationCity = document.getElementById('destinationCity');

    // İlçeler select elementleri
    const loadingDistrict = document.getElementById('loadingDistrict');
    const destinationDistrict = document.getElementById('destinationDistrict');

    // İlleri alfabetik sırala
    if (loadingCity) sortSelectOptionsAlphabetically(loadingCity);
    if (destinationCity) sortSelectOptionsAlphabetically(destinationCity);

    // İl seçildiğinde ilçeleri yükle ve sırala (Yükleme Adresi)
    loadingCity?.addEventListener('change', () => {
        const ilId = loadingCity.value;
        loadingDistrict.innerHTML = '<option value="">İlçe Seçin</option>';
        loadingDistrict.disabled = true;
        if (ilId && districtsJson[ilId]) {
            const sortedDistricts = sortDistrictsAlphabetically(districtsJson[ilId]);
            sortedDistricts.forEach(ilce => {
                const option = document.createElement('option');
                option.value = ilce.Id;
                option.textContent = ilce.Name;
                loadingDistrict.appendChild(option);
            });
            loadingDistrict.disabled = false;
        }
    });

    // İl seçildiğinde ilçeleri yükle ve sırala (Varış Adresi)
    destinationCity?.addEventListener('change', () => {
        const ilId = destinationCity.value;
        destinationDistrict.innerHTML = '<option value="">İlçe Seçin</option>';
        destinationDistrict.disabled = true;
        if (ilId && districtsJson[ilId]) {
            const sortedDistricts = sortDistrictsAlphabetically(districtsJson[ilId]);
            sortedDistricts.forEach(ilce => {
                const option = document.createElement('option');
                option.value = ilce.Id;
                option.textContent = ilce.Name;
                destinationDistrict.appendChild(option);
            });
            destinationDistrict.disabled = false;
        }
    });
});
