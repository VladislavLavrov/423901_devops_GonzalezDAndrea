document.addEventListener('DOMContentLoaded', function () {
    const leftInput = document.getElementById('left');
    const rightInput = document.getElementById('right');
    const opSelect = document.getElementById('op');
    const btnClient = document.getElementById('btnComputeClient');
    const btnClear = document.getElementById('btnClear');
    const clientResult = document.getElementById('clientResult');

    function computeClient() {
        const l = parseFloat(leftInput.value);
        const r = parseFloat(rightInput.value);
        const op = opSelect.value;

        if (isNaN(l) || isNaN(r)) {
            clientResult.innerText = 'Introduce números válidos.';
            clientResult.className = 'error-box';
            return;
        }

        let res;
        if (op === 'add') res = l + r;
        else if (op === 'subtract') res = l - r;
        else if (op === 'multiply') res = l * r;
        else if (op === 'divide') {
            if (r === 0) {
                clientResult.innerText = 'Error: división por cero.';
                clientResult.className = 'error-box';
                return;
            } else res = l / r;
        }

        clientResult.innerText = `Resultado (cliente): ${res}`;
        clientResult.className = 'client-result';
    }

    btnClient.addEventListener('click', computeClient);

    btnClear.addEventListener('click', function () {
        leftInput.value = '';
        rightInput.value = '';
        opSelect.value = 'add';
        clientResult.innerText = '';
    });
});
