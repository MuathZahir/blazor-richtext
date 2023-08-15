export function setElementValue(element, value) {
    element.innerHTML = value;
}

export function isElementScrollable(element) {
    return element.scrollHeight > element.clientHeight;
}

export function addElement(container) {
    var newDiv = document.createElement("div");
    newDiv.className = "prose a4-page";
    newDiv.style.outline = "none";
    newDiv.contentEditable = "true";
    newDiv.innerHTML = "<p><br></p>";
    container.appendChild(newDiv);

    newDiv.focus();

    return newDiv;
}

export function getLastChild(container) { 
    return container.lastChild;
}