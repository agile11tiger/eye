"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.contextMenu = void 0;
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
}
var contextMenu = new ContextMenu();
exports.contextMenu = contextMenu;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiQ29udGV4dE1lbnUuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJDb250ZXh0TWVudS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7QUFBQSx5REFBeUQ7QUFDekQsTUFBTSxXQUFXO0lBQ2I7UUFDSSxJQUFJLENBQUMsaUJBQWlCLEVBQUUsQ0FBQztJQUM3QixDQUFDO0lBRU8saUJBQWlCO1FBQ3JCLElBQUksQ0FBQyxrQkFBa0IsR0FBRyxJQUFJLENBQUMsa0JBQWtCLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO1FBQzdELDBHQUEwRztRQUMxRyxRQUFRLENBQUMsZ0JBQWdCLENBQUMsU0FBUyxFQUFFLElBQUksQ0FBQyxrQkFBa0IsRUFBRSxLQUFLLENBQUMsQ0FBQztRQUVyRSxJQUFJLENBQUMsa0JBQWtCLEdBQUcsSUFBSSxDQUFDLGtCQUFrQixDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQztRQUM3RCxRQUFRLENBQUMsZ0JBQWdCLENBQUMsU0FBUyxFQUFFLElBQUksQ0FBQyxrQkFBa0IsRUFBRSxLQUFLLENBQUMsQ0FBQztJQUV6RSxDQUFDO0lBSU8sa0JBQWtCLENBQUMsQ0FBUTtRQUMvQixJQUFJLEdBQUcsR0FBcUIsQ0FBQyxDQUFDLE1BQU0sQ0FBQztRQUVyQyxJQUFJLEdBQUcsQ0FBQyxTQUFTLElBQUksa0JBQWtCLEVBQUU7WUFDckMsSUFBSSxDQUFDLGlCQUFpQixHQUFxQixHQUFHLENBQUMsc0JBQXNCLENBQUM7U0FDekU7SUFDTCxDQUFDO0lBRU8sa0JBQWtCLENBQUMsQ0FBUTs7UUFFL0IsSUFBSSxJQUFJLENBQUMsaUJBQWlCLElBQUksSUFBSSxJQUFJLE9BQUMsQ0FBQyxDQUFDLE1BQTJCLDBDQUFFLE9BQU8sS0FBSSxJQUFJLENBQUMsaUJBQWlCLENBQUMsRUFBRSxFQUFFO1lBQ3hHLElBQUksQ0FBQyxpQkFBaUIsQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1NBQzFDO0lBQ0wsQ0FBQztDQUNKO0FBRUQsSUFBSSxXQUFXLEdBQWdCLElBQUksV0FBVyxFQUFFLENBQUM7QUFDeEMsa0NBQVcifQ==