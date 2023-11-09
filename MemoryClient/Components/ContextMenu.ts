//Два контекстных меню не могут быть открыты одновременно
class ContextMenu {
    constructor() {
        this.runEventListeners();
    }

    private runEventListeners(): void {
        this.hideContextMenuBox = this.hideContextMenuBox.bind(this);
        //true необходимо для того, чтобы наверняка вначале скрывалось предыдущее меню, а потом показывалось новое
        document.addEventListener("mouseup", this.hideContextMenuBox, false);

        this.showContextMenuBox = this.showContextMenuBox.bind(this);
        document.addEventListener("mouseup", this.showContextMenuBox, false);

    }

    private contextMenuToggle: HTMLInputElement;

    private showContextMenuBox(e: Event): void {
        let btn = <HTMLLabelElement>e.target;

        if (btn.className == "contextMenu__btn") {
            this.contextMenuToggle = <HTMLInputElement>btn.previousElementSibling;
        }
    }

    private hideContextMenuBox(e: Event): void {

        if (this.contextMenuToggle != null && (e.target as HTMLLabelElement)?.htmlFor != this.contextMenuToggle.id) {
            this.contextMenuToggle.checked = false;
        }
    }
}

var contextMenu: ContextMenu = new ContextMenu();
export { contextMenu }