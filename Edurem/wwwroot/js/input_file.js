// Очистить InputFile элемент
window.clearInputFile = function () {
    let inputElements = document.querySelectorAll("[type='file']");

    for (let i = 0; i < inputElements.length; i++) {
        inputElements[i].value = "";
    }
}