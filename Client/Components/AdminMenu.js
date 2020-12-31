"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.adminMenu = void 0;
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
exports.adminMenu = adminMenu;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQWRtaW5NZW51LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiQWRtaW5NZW51LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7OztBQUFBLG9EQUFvRDtBQUNwRCxNQUFNLFNBQVM7SUFDWDtRQUNJLElBQUksQ0FBQyxpQkFBaUIsRUFBRSxDQUFDO0lBQzdCLENBQUM7SUFFTyxpQkFBaUI7UUFDckIsSUFBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7UUFDekQsUUFBUSxDQUFDLGdCQUFnQixDQUFDLFNBQVMsRUFBRSxJQUFJLENBQUMsZ0JBQWdCLEVBQUUsS0FBSyxDQUFDLENBQUM7UUFFbkUsSUFBSSxDQUFDLGdCQUFnQixHQUFHLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7UUFDekQsUUFBUSxDQUFDLGdCQUFnQixDQUFDLFNBQVMsRUFBRSxJQUFJLENBQUMsZ0JBQWdCLEVBQUUsS0FBSyxDQUFDLENBQUM7SUFDdkUsQ0FBQztJQUlPLGdCQUFnQixDQUFDLENBQVE7UUFDN0IsSUFBSSxHQUFHLEdBQXFCLENBQUMsQ0FBQyxNQUFNLENBQUM7UUFFckMsSUFBSSxHQUFHLENBQUMsU0FBUyxJQUFJLGdCQUFnQixFQUFFO1lBQ25DLElBQUksQ0FBQyxlQUFlLEdBQXFCLEdBQUcsQ0FBQyxzQkFBc0IsQ0FBQztTQUN2RTtJQUNMLENBQUM7SUFFTyxnQkFBZ0IsQ0FBQyxDQUFROztRQUM3QixJQUFJLElBQUksQ0FBQyxlQUFlLElBQUksSUFBSSxJQUFJLE9BQUMsQ0FBQyxDQUFDLE1BQTJCLDBDQUFFLE9BQU8sS0FBSSxJQUFJLENBQUMsZUFBZSxDQUFDLEVBQUUsRUFBRTtZQUNwRyxJQUFJLENBQUMsZUFBZSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7U0FDeEM7SUFDTCxDQUFDO0NBQ0o7QUFFRCxJQUFJLFNBQVMsR0FBYyxJQUFJLFNBQVMsRUFBRSxDQUFDO0FBQ2xDLDhCQUFTIn0=