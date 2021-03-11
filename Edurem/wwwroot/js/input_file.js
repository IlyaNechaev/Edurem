// Очистить InputFile элемент
window.clearInputFile = function () {
    let inputElement = document.querySelector("[type='file']");
    console.log(inputElement)
    inputElement.value = "";
}