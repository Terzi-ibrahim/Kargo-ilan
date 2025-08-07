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

const districtsJson = JSON.parse(document.getElementById("districts-data").textContent);


// Sayfa yüklendiğinde illeri alfabetik sırala
window.addEventListener('DOMContentLoaded', () => {
    const ilFilter = document.getElementById('ilFilter');
    const varisilFilter = document.getElementById('varisilFilter');

    if (ilFilter) sortSelectOptionsAlphabetically(ilFilter);
    if (varisilFilter) sortSelectOptionsAlphabetically(varisilFilter);

    // İl seçildiğinde ilçeleri yükle ve sırala
    ilFilter?.addEventListener('change', () => {
        const ilId = ilFilter.value;
        const ilceSelect = document.getElementById('ilceFilter');
        ilceSelect.innerHTML = '<option value="">İlçe Seçin</option>';
        if (ilId && districtsJson[ilId]) {
            const sortedDistricts = sortDistrictsAlphabetically(districtsJson[ilId]);
            sortedDistricts.forEach(ilce => {
                const option = document.createElement('option');
                option.value = ilce.Id;
                option.textContent = ilce.Name;
                ilceSelect.appendChild(option);
            });
        }
    });

    // Varış ili seçildiğinde ilçeleri yükle ve sırala
    varisilFilter?.addEventListener('change', () => {
        const ilId = varisilFilter.value;
        const ilceSelect = document.getElementById('varisilceFilter');
        ilceSelect.innerHTML = '<option value="">İlçe Seçin</option>';
        if (ilId && districtsJson[ilId]) {
            const sortedDistricts = sortDistrictsAlphabetically(districtsJson[ilId]);
            sortedDistricts.forEach(ilce => {
                const option = document.createElement('option');
                option.value = ilce.Id;
                option.textContent = ilce.Name;
                ilceSelect.appendChild(option);
            });
        }
    });
});
