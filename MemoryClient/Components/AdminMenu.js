//Два меню админа не могут быть открыты одновременно
class AdminMenu {
    constructor() {
        this.runEventListeners();
    }
    runEventListeners() {
        this.hideAdminMenuBox = this.hideAdminMenuBox.bind(this);
        document.addEventListener("mouseup", this.hideAdminMenuBox, false);
        this.showAdminMenuBox = this.showAdminMenuBox.bind(this);
        document.addEventListener("mouseup", this.showAdminMenuBox, false);
    }
    showAdminMenuBox(e) {
        let btn = e.target;
        if (btn.className == "adminMenu__btn") {
            this.adminMenuToggle = btn.previousElementSibling;
        }
    }
    hideAdminMenuBox(e) {
        var _a;
        if (this.adminMenuToggle != null && ((_a = e.target) === null || _a === void 0 ? void 0 : _a.htmlFor) != this.adminMenuToggle.id) {
            this.adminMenuToggle.checked = false;
        }
    }
}
var adminMenu = new AdminMenu();