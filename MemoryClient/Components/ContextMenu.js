//Два контекстных меню не могут быть открыты одновременно
class ContextMenu {
    constructor() {
        this.runEventListeners();
    }
    runEventListeners() {
        this.hideContextMenuBox = this.hideContextMenuBox.bind(this);
        //true необходимо для того, чтобы наверняка вначале скрывалось предыдущее меню, а потом показывалось новое
        document.addEventListener("mouseup", this.hideContextMenuBox, false);
        this.showContextMenuBox = this.showContextMenuBox.bind(this);
        document.addEventListener("mouseup", this.showContextMenuBox, false);
    }
    showContextMenuBox(e) {
        let btn = e.target;
        if (btn.className == "contextMenu__btn") {
            this.contextMenuToggle = btn.previousElementSibling;
        }
    }
    hideContextMenuBox(e) {
        var _a;
        if (this.contextMenuToggle != null && ((_a = e.target) === null || _a === void 0 ? void 0 : _a.htmlFor) != this.contextMenuToggle.id) {
            this.contextMenuToggle.checked = false;
        }
    }
    //private hideContextMenuBox(e: Event): void {

    //    if (this.contextMenuToggle != null && (e.target as HTMLLabelElement)?.htmlFor != this.contextMenuToggle.id) {
    //        this.contextMenuToggle.checked = false;
    //    }
    //}
}
var contextMenu = new ContextMenu();