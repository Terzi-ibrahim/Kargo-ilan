document.addEventListener("DOMContentLoaded", function () {
    const districtJson = document.getElementById("districts-data").textContent;
    window.districtsData = JSON.parse(districtJson);

    const loadingCity = document.getElementById("loadingCity");
    const loadingDistrict = document.getElementById("loadingDistrict");

    const destinationCity = document.getElementById("destinationCity");
    const destinationDistrict = document.getElementById("destinationDistrict");

    function updateDistricts(cityId, districtSelect, selectedDistrictId = null) {
        districtSelect.innerHTML = "<option value=''>İlçe Seçin</option>";

        if (!cityId || !window.districtsData || !window.districtsData[cityId]) {
            districtSelect.disabled = true;
            return;
        }

        const districts = window.districtsData[cityId];
        districts.forEach(d => {
            const option = document.createElement("option");
            option.value = d.Id;
            option.text = d.Name;
            if (selectedDistrictId && selectedDistrictId == d.Id.toString()) {
                option.selected = true;
            }
            districtSelect.appendChild(option);
        });
        districtSelect.disabled = false;
    }

    loadingCity.addEventListener("change", function () {
        updateDistricts(this.value, loadingDistrict);
    });

    destinationCity.addEventListener("change", function () {
        updateDistricts(this.value, destinationDistrict);
    });


    const loadingSelectedDistrict = loadingDistrict.dataset.selectedDistrictId;
    if (loadingCity.value) {
        updateDistricts(loadingCity.value, loadingDistrict, loadingSelectedDistrict);
    }

    const destinationSelectedDistrict = destinationDistrict.dataset.selectedDistrictId;
    if (destinationCity.value) {
        updateDistricts(destinationCity.value, destinationDistrict, destinationSelectedDistrict);
    }
});