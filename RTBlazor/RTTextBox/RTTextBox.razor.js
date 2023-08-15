export function getCaretPosition(editableDiv) {
    var caretPos = 0,
        sel, range;
    if (window.getSelection) {
        sel = window.getSelection();
        if (sel.rangeCount) {
            range = sel.getRangeAt(0);
            if (range.commonAncestorContainer.parentNode === editableDiv) {
                caretPos = range.endOffset;
            }
        }
    } else if (document.selection && document.selection.createRange) {
        range = document.selection.createRange();
        if (range.parentElement() === editableDiv) {
            var tempEl = document.createElement("span");
            editableDiv.insertBefore(tempEl, editableDiv.firstChild);
            var tempRange = range.duplicate();
            tempRange.moveToElementText(tempEl);
            tempRange.setEndPoint("EndToEnd", range);
            caretPos = tempRange.text.length;
        }
    }
    return caretPos;
}

export function updateInnerHTML(editableDiv, html) {
    editableDiv.innerHTML = html;
}

export function getTextContent(editableDiv) {
    return editableDiv.textContent;
}

export function getInnerHTML(editableDiv) {
    return editableDiv.innerHTML;
}

// Get selected text from editable div only
export function getSelectedText(editableDiv) {
    var text = "";

    // Make sure that the text is selected within the editable div
    if (window.getSelection) {
        var selection = window.getSelection();
        var range = selection.getRangeAt(0);
        var parent = range.commonAncestorContainer.parentNode;

        if (parent === editableDiv)
        {
            text = selection.toString();
        }
    }
    else if (document.selection && document.selection.type !== "Control")
    {
        var textRange = document.selection.createRange();

        if (textRange.parentElement() === editableDiv) {
            text = textRange.text;
        }
    }
    return text;
}

export function getSelectionRangeWithin(element) {
    var range = null;
    var startOffset = 0;
    var endOffset = 0;

    if (window.getSelection) {
        var selection = window.getSelection();
        if (selection.rangeCount > 0) {
            range = selection.getRangeAt(0);
            var preCaretRange = range.cloneRange();
            preCaretRange.selectNodeContents(element);
            preCaretRange.setEnd(range.startContainer, range.startOffset);
            startOffset = preCaretRange.toString().length;
            endOffset = startOffset + range.toString().length;
        }
    } else if (document.selection && document.selection.type != "Control") {
        range = document.selection.createRange();
        var textRange = document.body.createTextRange();
        textRange.moveToElementText(element);
        textRange.setEndPoint("EndToStart", range);
        startOffset = textRange.text.length;
        endOffset = startOffset + range.text.length;
    }

    return {
        startOffset: startOffset,
        endOffset: endOffset
    };
}

export function addClickEventListener(element) {
    element.addEventListener("mousedown", function(e) {
        e.preventDefault();
    });
}

//export function selectText(element, startIndex, endIndex) {
//    if (element) {
//        const range = document.createRange();
//        const selection = window.getSelection();

//        function findNodeAndOffset(parent, index) {
//            let currentNode = parent.firstChild;
//            let currentOffset = 0;

//            while (currentNode) {
//                let childLength;

//                if (currentNode.nodeType === Node.ELEMENT_NODE) {
//                    childLength = currentNode.outerHTML.length - currentNode.innerHTML.length;
//                } else {
//                    childLength = currentNode.textContent.length;
//                }

//                if (index < currentOffset + childLength) {
//                    return { node: currentNode, offset: index - currentOffset };
//                }

//                currentOffset += childLength;
//                currentNode = currentNode.nextSibling;
//            }

//            return { node: null, offset: 0 };
//        }

//        const { node: startNode, offset: startOffset } = findNodeAndOffset(element, startIndex);
//        const { node: endNode, offset: endOffset } = findNodeAndOffset(element, endIndex);

//        if (startNode) {
//            range.setStart(startNode, startOffset);
//        } else {
//            range.setStart(element, element.childNodes.length);
//        }

//        if (endNode) {
//            range.setEnd(endNode, endOffset);
//        } else {
//            range.setEnd(element, element.childNodes.length);
//        }

//        // Set the selection
//        selection.removeAllRanges();
//        selection.addRange(range);
//    }
//}
export function selectText(element, startIndex, endIndex) {
    if (element) {
        const range = document.createRange();
        const selection = window.getSelection();

        function findNodeAndOffset(parent, index) {
            let currentNode = parent.firstChild;
            let currentOffset = 0;

            while (currentNode) {
                if (currentNode.nodeType === Node.TEXT_NODE) {
                    const nodeLength = currentNode.textContent.length;
                    if (index <= currentOffset + nodeLength) {
                        return { node: currentNode, offset: index - currentOffset };
                    }
                    currentOffset += nodeLength;
                } else {
                    const result = findNodeAndOffset(currentNode, index - currentOffset);
                    if (result && result.node) {
                        return result;
                    }
                    currentOffset += getNodeTextLength(currentNode);
                }

                currentNode = currentNode.nextSibling;
            }

            return { node: null, offset: 0 };
        }

        function getNodeTextLength(node) {
            if (node.nodeType === Node.TEXT_NODE) {
                return node.textContent.length;
            }

            let length = 0;
            for (const childNode of node.childNodes) {
                if (childNode.nodeType === Node.TEXT_NODE) {
                    length += childNode.textContent.length;
                } else {
                    length += getNodeTextLength(childNode);
                }
            }
            return length;
        }

        const { node: startNode, offset: startOffset } = findNodeAndOffset(element, startIndex);
        const { node: endNode, offset: endOffset } = findNodeAndOffset(element, endIndex);

        if (!startNode || !endNode) {
            console.warn("Cannot select text, invalid node information");
            return;
        }

        range.setStart(startNode, startOffset);
        range.setEnd(endNode, endOffset);

        selection.removeAllRanges();
        selection.addRange(range);
    }
}
