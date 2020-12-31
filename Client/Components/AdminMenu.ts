//Два меню админа не могут быть открыты одновременно
class AdminMenu {
    constructor() {
        this.runEventListeners();
    }

    private runEventListeners(): void {
        this.hideAdminMenuBox = this.hideAdminMenuBox.bind(this);
        document.addEventListener("mouseup", this.hideAdminMenuBox, false);

        this.showAdminMenuBox = this.showAdminMenuBox.bind(this);
        document.addEventListener("mouseup", this.showAdminMenuBox, false);
    }

    private adminMenuToggle: HTMLInputElement;

    private showAdminMenuBox(e: Event): void {
        let btn = <HTMLLabelElement>e.target;
        
        if (btn.className == "adminMenu__btn") {
            this.adminMenuToggle = <HTMLInputElement>btn.previousElementSibling;
        }
    }

    private hideAdminMenuBox(e: Event): void {
        if (this.adminMenuToggle != null && (e.target as HTMLLabelElement)?.htmlFor != this.adminMenuToggle.id) {
            this.adminMenuToggle.checked = false;
        }
    }
}

var adminMenu: AdminMenu = new AdminMenu();
export { adminMenu }