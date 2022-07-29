document.querySelector('.all-select').addEventListener('click', () => {
    let switches = document.querySelectorAll('.user-select');
    for (let i = 0; i < switches.length; ++i) {
        switches[i].checked = document.querySelector('.all-select').checked;
    }
});
document.querySelectorAll('.user-select').forEach(e => {
    e.addEventListener('change', () => {
        if (!e.checked) {
            document.querySelector('.all-select').checked = false;
        }
        else if (Array.from(document.querySelectorAll('.user-select')).every(e => e.checked)) {
            document.querySelector('.all-select').checked = true;
        }
    })
});