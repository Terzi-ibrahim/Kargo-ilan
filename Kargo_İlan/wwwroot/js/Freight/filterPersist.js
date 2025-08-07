document.addEventListener("DOMContentLoaded", function () {
    // İlçe verileri JSON'dan al
    const districtsJson = JSON.parse(document.getElementById("districts-data").textContent);

    const filterFields = [
        "searchText", "ilFilter", "ilceFilter",
        "varisilFilter", "varisilceFilter",
        "vehicleFilter", "categoryFilter", "cargotypeFilter"
    ];

    // İlçeleri yükleyen fonksiyon (Ayrıca localStorage'daki ilçe seçimini otomatik yapar)
    function loadDistricts(ilId, ilceSelectId) {
        const ilceSelect = document.getElementById(ilceSelectId);
        ilceSelect.innerHTML = "";

        // İlk seçenek
        const defaultOption = document.createElement("option");
        defaultOption.value = "";
        defaultOption.textContent = "İlçe Seçin";
        ilceSelect.appendChild(defaultOption);

        if (ilId && districtsJson[ilId]) {
            // İlçeleri alfabetik sırala
            const sortedDistricts = districtsJson[ilId].sort((a, b) => a.Name.localeCompare(b.Name, "tr"));

            sortedDistricts.forEach(d => {
                const option = document.createElement("option");
                option.value = d.Id;
                option.textContent = d.Name;
                ilceSelect.appendChild(option);
            });

            // Daha önce seçilmiş ilçeyi seç
            const savedIlce = localStorage.getItem(ilceSelectId);
            if (savedIlce) {
                ilceSelect.value = savedIlce;
            }
        }
    }

    // LocalStorage'dan kayıtlı filtre değerlerini inputlara uygula
    filterFields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (!field) return;
        const savedValue = localStorage.getItem(fieldId);
        if (savedValue !== null) {
            field.value = savedValue;
        }
    });

    // İl -> İlçe dropdown yükleme ve localStorage güncelleme (Yükleme adresi)
    const ilFilter = document.getElementById("ilFilter");
    if (ilFilter) {
        ilFilter.addEventListener("change", function () {
            loadDistricts(this.value, "ilceFilter");
            localStorage.setItem("ilFilter", this.value);
            localStorage.removeItem("ilceFilter"); // İl değişince önceki ilçe temizlensin
        });

        // Sayfa yüklendiğinde varsa ilçeleri yükle
        if (ilFilter.value) {
            loadDistricts(ilFilter.value, "ilceFilter");
        }
    }

    // Varış il -> İlçe dropdown yükleme ve localStorage güncelleme
    const varisilFilter = document.getElementById("varisilFilter");
    if (varisilFilter) {
        varisilFilter.addEventListener("change", function () {
            loadDistricts(this.value, "varisilceFilter");
            localStorage.setItem("varisilFilter", this.value);
            localStorage.removeItem("varisilceFilter"); // Varış il değişince önceki ilçe temizlensin
        });

        if (varisilFilter.value) {
            loadDistricts(varisilFilter.value, "varisilceFilter");
        }
    }

    // Diğer filtre alanları için localStorage güncelleme
    filterFields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (!field) return;
        field.addEventListener("change", () => {
            localStorage.setItem(fieldId, field.value);
        });
    });

    // Sıfırla butonu
    const resetBtn = document.getElementById("resetFilters");
    const filterForm = document.getElementById("filterForm");

    if (resetBtn && filterForm) {
        resetBtn.addEventListener("click", function (e) {
            e.preventDefault();

            filterFields.forEach(id => {
                localStorage.removeItem(id);
                const field = document.getElementById(id);
                if (field) field.value = "";
            });

            // İlçe dropdownları sıfırla
            ["ilceFilter", "varisilceFilter"].forEach(id => {
                const el = document.getElementById(id);
                if (el) el.innerHTML = "<option value=''>İlçe Seçin</option>";
            });

            // Sayfayı filtreler sıfırlanarak yeniden yükle
            window.location.href = window.location.pathname;
        });
    }
});
