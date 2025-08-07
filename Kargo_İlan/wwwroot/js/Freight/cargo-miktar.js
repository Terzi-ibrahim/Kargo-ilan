// wwwroot/js/cargo-miktar.js

const cargoUnitMap = {
    10: 'kg',        // Palet
    20: 'kg',        // Çuval
    30: 'adet',      // Koli
    40: 'litre',     // Varil
    50: 'litre',     // Bidon
    60: 'litre',     // Sıvı Tankı
    70: 'adet',      // Cam Şişe
    80: ''           // Diğer
};

function updateMiktarPlaceholderAndLabel(cargoSelectId, miktarInputId, miktarLabelId) {
    const cargoSelect = document.getElementById(cargoSelectId);
    const miktarInput = document.getElementById(miktarInputId);
    const miktarLabel = document.getElementById(miktarLabelId);

    if (!cargoSelect || !miktarInput || !miktarLabel) return;

    cargoSelect.addEventListener("change", function () {
        const selectedValue = this.value;
        const unit = cargoUnitMap[selectedValue] || '';
        if (unit) {
            miktarInput.placeholder = `Miktar (${unit})`;
            miktarLabel.innerText = `Miktar (${unit})`;
        } else {
            miktarInput.placeholder = "Miktar";
            miktarLabel.innerText = "Miktar";
        }
    });
}

function getCargoUnit(cargoId) {
    return cargoUnitMap[cargoId] || '';
}
