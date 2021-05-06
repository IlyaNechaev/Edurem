// Очистить InputFile элемент
window.clearInputFile = function () {
    let inputElements = document.querySelectorAll("[type='file']");

    for (let i = 0; i < inputElements.length; i++) {
        inputElements[i].value = "";
    }
}

window.dragHandler = function(ev, objId, color) {
    // Prevent default behavior (Prevent file from being opened)
    if (ev != null)
        ev.preventDefault();

    console.log(objId);
    document.getElementById(objId).style.borderColor = color;
}

window.getId = function(id)
{
    return id;
}
