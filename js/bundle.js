var bundle =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Components/AdminMenu.ts":
/*!*********************************!*\
  !*** ./Components/AdminMenu.ts ***!
  \*********************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

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

/***/ }),

/***/ "./Components/ContextMenu.ts":
/*!***********************************!*\
  !*** ./Components/ContextMenu.ts ***!
  \***********************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

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

/***/ }),

/***/ "./FilesExport.ts":
/*!************************!*\
  !*** ./FilesExport.ts ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
exports.adminMenu = exports.contextMenu = void 0;
//После сборки файлов в один Webpack-ом файлы не доступны браузеру, таким образом они будут доступны.
//Подробнее в webpack.config.js
const ContextMenu_1 = __webpack_require__(/*! ./Components/ContextMenu */ "./Components/ContextMenu.ts");
Object.defineProperty(exports, "contextMenu", { enumerable: true, get: function () { return ContextMenu_1.contextMenu; } });
const AdminMenu_1 = __webpack_require__(/*! ./Components/AdminMenu */ "./Components/AdminMenu.ts");
Object.defineProperty(exports, "adminMenu", { enumerable: true, get: function () { return AdminMenu_1.adminMenu; } });
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiRmlsZXNFeHBvcnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyJGaWxlc0V4cG9ydC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7QUFBQSxxR0FBcUc7QUFDckcsK0JBQStCO0FBQy9CLDBEQUF1RDtBQUU5Qyw0RkFGQSx5QkFBVyxPQUVBO0FBRHBCLHNEQUFtRDtBQUM3QiwwRkFEYixxQkFBUyxPQUNhIn0=

/***/ }),

/***/ "./node_modules/multi-entry-loader/index.js?include=Components/**/**/**/**/**/*.ts!./":
/*!*************************************************************************************!*\
  !*** ./node_modules/multi-entry-loader?include=Components/**_/**_/**_/**_/**_/*.ts ***!
  \*************************************************************************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var C_Users_Timur_Desktop_EyE_Site_Client_Components_AdminMenu_ts__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Components/AdminMenu.ts */ "./Components/AdminMenu.ts");
/* harmony import */ var C_Users_Timur_Desktop_EyE_Site_Client_Components_AdminMenu_ts__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(C_Users_Timur_Desktop_EyE_Site_Client_Components_AdminMenu_ts__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var C_Users_Timur_Desktop_EyE_Site_Client_Components_ContextMenu_ts__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Components/ContextMenu.ts */ "./Components/ContextMenu.ts");
/* harmony import */ var C_Users_Timur_Desktop_EyE_Site_Client_Components_ContextMenu_ts__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(C_Users_Timur_Desktop_EyE_Site_Client_Components_ContextMenu_ts__WEBPACK_IMPORTED_MODULE_1__);



/***/ }),

/***/ "./node_modules/multi-entry-loader/index.js?include=Pages/**/**/**/**/**/*.ts!./":
/*!********************************************************************************!*\
  !*** ./node_modules/multi-entry-loader?include=Pages/**_/**_/**_/**_/**_/*.ts ***!
  \********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {



/***/ }),

/***/ "./node_modules/multi-entry-loader/index.js?include=Shared/**/**/**/**/**/*.ts!./":
/*!*********************************************************************************!*\
  !*** ./node_modules/multi-entry-loader?include=Shared/**_/**_/**_/**_/**_/*.ts ***!
  \*********************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {



/***/ }),

/***/ 0:
/*!***************************************************************************************************************************************************************************************************************!*\
  !*** multi multi-entry-loader?include=Components/**_/**_/**_/**_/**_/*.ts multi-entry-loader?include=Pages/**_/**_/**_/**_/**_/*.ts multi-entry-loader?include=Shared/**_/**_/**_/**_/**_/*.ts ./FilesExport ***!
  \***************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(/*! multi-entry-loader?include=Components/** /** /** /** /** /*.ts! */"./node_modules/multi-entry-loader/index.js?include=Components/**/**/**/**/**/*.ts!./");
__webpack_require__(/*! multi-entry-loader?include=Pages/** /** /** /** /** /*.ts! */"./node_modules/multi-entry-loader/index.js?include=Pages/**/**/**/**/**/*.ts!./");
__webpack_require__(/*! multi-entry-loader?include=Shared/** /** /** /** /** /*.ts! */"./node_modules/multi-entry-loader/index.js?include=Shared/**/**/**/**/**/*.ts!./");
module.exports = __webpack_require__(/*! ./FilesExport */"./FilesExport.ts");


/***/ })

/******/ });
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9idW5kbGUvd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vYnVuZGxlLy4vQ29tcG9uZW50cy9BZG1pbk1lbnUudHMiLCJ3ZWJwYWNrOi8vYnVuZGxlLy4vQ29tcG9uZW50cy9Db250ZXh0TWVudS50cyIsIndlYnBhY2s6Ly9idW5kbGUvLi9GaWxlc0V4cG9ydC50cyIsIndlYnBhY2s6Ly9idW5kbGUvLi9ub2RlX21vZHVsZXMvbXVsdGktZW50cnktbG9hZGVyIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7O1FBQUE7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7OztRQUdBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSwwQ0FBMEMsZ0NBQWdDO1FBQzFFO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0Esd0RBQXdELGtCQUFrQjtRQUMxRTtRQUNBLGlEQUFpRCxjQUFjO1FBQy9EOztRQUVBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQSx5Q0FBeUMsaUNBQWlDO1FBQzFFLGdIQUFnSCxtQkFBbUIsRUFBRTtRQUNySTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBLDJCQUEyQiwwQkFBMEIsRUFBRTtRQUN2RCxpQ0FBaUMsZUFBZTtRQUNoRDtRQUNBO1FBQ0E7O1FBRUE7UUFDQSxzREFBc0QsK0RBQStEOztRQUVySDtRQUNBOzs7UUFHQTtRQUNBOzs7Ozs7Ozs7Ozs7O0FDbEZhO0FBQ2IsOENBQThDLGNBQWM7QUFDNUQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsMkNBQTJDLDJ0Qzs7Ozs7Ozs7Ozs7O0FDN0I5QjtBQUNiLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsMkNBQTJDLCt2Qzs7Ozs7Ozs7Ozs7O0FDOUI5QjtBQUNiLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBLHNCQUFzQixtQkFBTyxDQUFDLDZEQUEwQjtBQUN4RCwrQ0FBK0MscUNBQXFDLGtDQUFrQyxFQUFFLEVBQUU7QUFDMUgsb0JBQW9CLG1CQUFPLENBQUMseURBQXdCO0FBQ3BELDZDQUE2QyxxQ0FBcUMsOEJBQThCLEVBQUUsRUFBRTtBQUNwSCwyQ0FBMkMsMlI7Ozs7Ozs7Ozs7OztBQ1QzQztBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQStFIiwiZmlsZSI6ImJ1bmRsZS5qcyIsInNvdXJjZXNDb250ZW50IjpbIiBcdC8vIFRoZSBtb2R1bGUgY2FjaGVcbiBcdHZhciBpbnN0YWxsZWRNb2R1bGVzID0ge307XG5cbiBcdC8vIFRoZSByZXF1aXJlIGZ1bmN0aW9uXG4gXHRmdW5jdGlvbiBfX3dlYnBhY2tfcmVxdWlyZV9fKG1vZHVsZUlkKSB7XG5cbiBcdFx0Ly8gQ2hlY2sgaWYgbW9kdWxlIGlzIGluIGNhY2hlXG4gXHRcdGlmKGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdKSB7XG4gXHRcdFx0cmV0dXJuIGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdLmV4cG9ydHM7XG4gXHRcdH1cbiBcdFx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcbiBcdFx0dmFyIG1vZHVsZSA9IGluc3RhbGxlZE1vZHVsZXNbbW9kdWxlSWRdID0ge1xuIFx0XHRcdGk6IG1vZHVsZUlkLFxuIFx0XHRcdGw6IGZhbHNlLFxuIFx0XHRcdGV4cG9ydHM6IHt9XG4gXHRcdH07XG5cbiBcdFx0Ly8gRXhlY3V0ZSB0aGUgbW9kdWxlIGZ1bmN0aW9uXG4gXHRcdG1vZHVsZXNbbW9kdWxlSWRdLmNhbGwobW9kdWxlLmV4cG9ydHMsIG1vZHVsZSwgbW9kdWxlLmV4cG9ydHMsIF9fd2VicGFja19yZXF1aXJlX18pO1xuXG4gXHRcdC8vIEZsYWcgdGhlIG1vZHVsZSBhcyBsb2FkZWRcbiBcdFx0bW9kdWxlLmwgPSB0cnVlO1xuXG4gXHRcdC8vIFJldHVybiB0aGUgZXhwb3J0cyBvZiB0aGUgbW9kdWxlXG4gXHRcdHJldHVybiBtb2R1bGUuZXhwb3J0cztcbiBcdH1cblxuXG4gXHQvLyBleHBvc2UgdGhlIG1vZHVsZXMgb2JqZWN0IChfX3dlYnBhY2tfbW9kdWxlc19fKVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5tID0gbW9kdWxlcztcblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGUgY2FjaGVcbiBcdF9fd2VicGFja19yZXF1aXJlX18uYyA9IGluc3RhbGxlZE1vZHVsZXM7XG5cbiBcdC8vIGRlZmluZSBnZXR0ZXIgZnVuY3Rpb24gZm9yIGhhcm1vbnkgZXhwb3J0c1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kID0gZnVuY3Rpb24oZXhwb3J0cywgbmFtZSwgZ2V0dGVyKSB7XG4gXHRcdGlmKCFfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZXhwb3J0cywgbmFtZSkpIHtcbiBcdFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgbmFtZSwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGdldHRlciB9KTtcbiBcdFx0fVxuIFx0fTtcblxuIFx0Ly8gZGVmaW5lIF9fZXNNb2R1bGUgb24gZXhwb3J0c1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5yID0gZnVuY3Rpb24oZXhwb3J0cykge1xuIFx0XHRpZih0eXBlb2YgU3ltYm9sICE9PSAndW5kZWZpbmVkJyAmJiBTeW1ib2wudG9TdHJpbmdUYWcpIHtcbiBcdFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgU3ltYm9sLnRvU3RyaW5nVGFnLCB7IHZhbHVlOiAnTW9kdWxlJyB9KTtcbiBcdFx0fVxuIFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgJ19fZXNNb2R1bGUnLCB7IHZhbHVlOiB0cnVlIH0pO1xuIFx0fTtcblxuIFx0Ly8gY3JlYXRlIGEgZmFrZSBuYW1lc3BhY2Ugb2JqZWN0XG4gXHQvLyBtb2RlICYgMTogdmFsdWUgaXMgYSBtb2R1bGUgaWQsIHJlcXVpcmUgaXRcbiBcdC8vIG1vZGUgJiAyOiBtZXJnZSBhbGwgcHJvcGVydGllcyBvZiB2YWx1ZSBpbnRvIHRoZSBuc1xuIFx0Ly8gbW9kZSAmIDQ6IHJldHVybiB2YWx1ZSB3aGVuIGFscmVhZHkgbnMgb2JqZWN0XG4gXHQvLyBtb2RlICYgOHwxOiBiZWhhdmUgbGlrZSByZXF1aXJlXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnQgPSBmdW5jdGlvbih2YWx1ZSwgbW9kZSkge1xuIFx0XHRpZihtb2RlICYgMSkgdmFsdWUgPSBfX3dlYnBhY2tfcmVxdWlyZV9fKHZhbHVlKTtcbiBcdFx0aWYobW9kZSAmIDgpIHJldHVybiB2YWx1ZTtcbiBcdFx0aWYoKG1vZGUgJiA0KSAmJiB0eXBlb2YgdmFsdWUgPT09ICdvYmplY3QnICYmIHZhbHVlICYmIHZhbHVlLl9fZXNNb2R1bGUpIHJldHVybiB2YWx1ZTtcbiBcdFx0dmFyIG5zID0gT2JqZWN0LmNyZWF0ZShudWxsKTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5yKG5zKTtcbiBcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KG5zLCAnZGVmYXVsdCcsIHsgZW51bWVyYWJsZTogdHJ1ZSwgdmFsdWU6IHZhbHVlIH0pO1xuIFx0XHRpZihtb2RlICYgMiAmJiB0eXBlb2YgdmFsdWUgIT0gJ3N0cmluZycpIGZvcih2YXIga2V5IGluIHZhbHVlKSBfX3dlYnBhY2tfcmVxdWlyZV9fLmQobnMsIGtleSwgZnVuY3Rpb24oa2V5KSB7IHJldHVybiB2YWx1ZVtrZXldOyB9LmJpbmQobnVsbCwga2V5KSk7XG4gXHRcdHJldHVybiBucztcbiBcdH07XG5cbiBcdC8vIGdldERlZmF1bHRFeHBvcnQgZnVuY3Rpb24gZm9yIGNvbXBhdGliaWxpdHkgd2l0aCBub24taGFybW9ueSBtb2R1bGVzXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm4gPSBmdW5jdGlvbihtb2R1bGUpIHtcbiBcdFx0dmFyIGdldHRlciA9IG1vZHVsZSAmJiBtb2R1bGUuX19lc01vZHVsZSA/XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0RGVmYXVsdCgpIHsgcmV0dXJuIG1vZHVsZVsnZGVmYXVsdCddOyB9IDpcbiBcdFx0XHRmdW5jdGlvbiBnZXRNb2R1bGVFeHBvcnRzKCkgeyByZXR1cm4gbW9kdWxlOyB9O1xuIFx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLmQoZ2V0dGVyLCAnYScsIGdldHRlcik7XG4gXHRcdHJldHVybiBnZXR0ZXI7XG4gXHR9O1xuXG4gXHQvLyBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGxcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubyA9IGZ1bmN0aW9uKG9iamVjdCwgcHJvcGVydHkpIHsgcmV0dXJuIE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChvYmplY3QsIHByb3BlcnR5KTsgfTtcblxuIFx0Ly8gX193ZWJwYWNrX3B1YmxpY19wYXRoX19cbiBcdF9fd2VicGFja19yZXF1aXJlX18ucCA9IFwiXCI7XG5cblxuIFx0Ly8gTG9hZCBlbnRyeSBtb2R1bGUgYW5kIHJldHVybiBleHBvcnRzXG4gXHRyZXR1cm4gX193ZWJwYWNrX3JlcXVpcmVfXyhfX3dlYnBhY2tfcmVxdWlyZV9fLnMgPSAwKTtcbiIsIlwidXNlIHN0cmljdFwiO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuYWRtaW5NZW51ID0gdm9pZCAwO1xyXG4vL9CU0LLQsCDQvNC10L3RjiDQsNC00LzQuNC90LAg0L3QtSDQvNC+0LPRg9GCINCx0YvRgtGMINC+0YLQutGA0YvRgtGLINC+0LTQvdC+0LLRgNC10LzQtdC90L3QvlxyXG5jbGFzcyBBZG1pbk1lbnUge1xyXG4gICAgY29uc3RydWN0b3IoKSB7XHJcbiAgICAgICAgdGhpcy5ydW5FdmVudExpc3RlbmVycygpO1xyXG4gICAgfVxyXG4gICAgcnVuRXZlbnRMaXN0ZW5lcnMoKSB7XHJcbiAgICAgICAgdGhpcy5oaWRlQWRtaW5NZW51Qm94ID0gdGhpcy5oaWRlQWRtaW5NZW51Qm94LmJpbmQodGhpcyk7XHJcbiAgICAgICAgZG9jdW1lbnQuYWRkRXZlbnRMaXN0ZW5lcihcIm1vdXNldXBcIiwgdGhpcy5oaWRlQWRtaW5NZW51Qm94LCBmYWxzZSk7XHJcbiAgICAgICAgdGhpcy5zaG93QWRtaW5NZW51Qm94ID0gdGhpcy5zaG93QWRtaW5NZW51Qm94LmJpbmQodGhpcyk7XHJcbiAgICAgICAgZG9jdW1lbnQuYWRkRXZlbnRMaXN0ZW5lcihcIm1vdXNldXBcIiwgdGhpcy5zaG93QWRtaW5NZW51Qm94LCBmYWxzZSk7XHJcbiAgICB9XHJcbiAgICBzaG93QWRtaW5NZW51Qm94KGUpIHtcclxuICAgICAgICBsZXQgYnRuID0gZS50YXJnZXQ7XHJcbiAgICAgICAgaWYgKGJ0bi5jbGFzc05hbWUgPT0gXCJhZG1pbk1lbnVfX2J0blwiKSB7XHJcbiAgICAgICAgICAgIHRoaXMuYWRtaW5NZW51VG9nZ2xlID0gYnRuLnByZXZpb3VzRWxlbWVudFNpYmxpbmc7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgaGlkZUFkbWluTWVudUJveChlKSB7XHJcbiAgICAgICAgdmFyIF9hO1xyXG4gICAgICAgIGlmICh0aGlzLmFkbWluTWVudVRvZ2dsZSAhPSBudWxsICYmICgoX2EgPSBlLnRhcmdldCkgPT09IG51bGwgfHwgX2EgPT09IHZvaWQgMCA/IHZvaWQgMCA6IF9hLmh0bWxGb3IpICE9IHRoaXMuYWRtaW5NZW51VG9nZ2xlLmlkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuYWRtaW5NZW51VG9nZ2xlLmNoZWNrZWQgPSBmYWxzZTtcclxuICAgICAgICB9XHJcbiAgICB9XHJcbn1cclxudmFyIGFkbWluTWVudSA9IG5ldyBBZG1pbk1lbnUoKTtcclxuZXhwb3J0cy5hZG1pbk1lbnUgPSBhZG1pbk1lbnU7XHJcbi8vIyBzb3VyY2VNYXBwaW5nVVJMPWRhdGE6YXBwbGljYXRpb24vanNvbjtiYXNlNjQsZXlKMlpYSnphVzl1SWpvekxDSm1hV3hsSWpvaVFXUnRhVzVOWlc1MUxtcHpJaXdpYzI5MWNtTmxVbTl2ZENJNklpSXNJbk52ZFhKalpYTWlPbHNpUVdSdGFXNU5aVzUxTG5SeklsMHNJbTVoYldWeklqcGJYU3dpYldGd2NHbHVaM01pT2lJN096dEJRVUZCTEc5RVFVRnZSRHRCUVVOd1JDeE5RVUZOTEZOQlFWTTdTVUZEV0R0UlFVTkpMRWxCUVVrc1EwRkJReXhwUWtGQmFVSXNSVUZCUlN4RFFVRkRPMGxCUXpkQ0xFTkJRVU03U1VGRlR5eHBRa0ZCYVVJN1VVRkRja0lzU1VGQlNTeERRVUZETEdkQ1FVRm5RaXhIUVVGSExFbEJRVWtzUTBGQlF5eG5Ra0ZCWjBJc1EwRkJReXhKUVVGSkxFTkJRVU1zU1VGQlNTeERRVUZETEVOQlFVTTdVVUZEZWtRc1VVRkJVU3hEUVVGRExHZENRVUZuUWl4RFFVRkRMRk5CUVZNc1JVRkJSU3hKUVVGSkxFTkJRVU1zWjBKQlFXZENMRVZCUVVVc1MwRkJTeXhEUVVGRExFTkJRVU03VVVGRmJrVXNTVUZCU1N4RFFVRkRMR2RDUVVGblFpeEhRVUZITEVsQlFVa3NRMEZCUXl4blFrRkJaMElzUTBGQlF5eEpRVUZKTEVOQlFVTXNTVUZCU1N4RFFVRkRMRU5CUVVNN1VVRkRla1FzVVVGQlVTeERRVUZETEdkQ1FVRm5RaXhEUVVGRExGTkJRVk1zUlVGQlJTeEpRVUZKTEVOQlFVTXNaMEpCUVdkQ0xFVkJRVVVzUzBGQlN5eERRVUZETEVOQlFVTTdTVUZEZGtVc1EwRkJRenRKUVVsUExHZENRVUZuUWl4RFFVRkRMRU5CUVZFN1VVRkROMElzU1VGQlNTeEhRVUZITEVkQlFYRkNMRU5CUVVNc1EwRkJReXhOUVVGTkxFTkJRVU03VVVGRmNrTXNTVUZCU1N4SFFVRkhMRU5CUVVNc1UwRkJVeXhKUVVGSkxHZENRVUZuUWl4RlFVRkZPMWxCUTI1RExFbEJRVWtzUTBGQlF5eGxRVUZsTEVkQlFYRkNMRWRCUVVjc1EwRkJReXh6UWtGQmMwSXNRMEZCUXp0VFFVTjJSVHRKUVVOTUxFTkJRVU03U1VGRlR5eG5Ra0ZCWjBJc1EwRkJReXhEUVVGUk96dFJRVU0zUWl4SlFVRkpMRWxCUVVrc1EwRkJReXhsUVVGbExFbEJRVWtzU1VGQlNTeEpRVUZKTEU5QlFVTXNRMEZCUXl4RFFVRkRMRTFCUVRKQ0xEQkRRVUZGTEU5QlFVOHNTMEZCU1N4SlFVRkpMRU5CUVVNc1pVRkJaU3hEUVVGRExFVkJRVVVzUlVGQlJUdFpRVU53Unl4SlFVRkpMRU5CUVVNc1pVRkJaU3hEUVVGRExFOUJRVThzUjBGQlJ5eExRVUZMTEVOQlFVTTdVMEZEZUVNN1NVRkRUQ3hEUVVGRE8wTkJRMG83UVVGRlJDeEpRVUZKTEZOQlFWTXNSMEZCWXl4SlFVRkpMRk5CUVZNc1JVRkJSU3hEUVVGRE8wRkJRMnhETERoQ1FVRlRJbjA9IiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5jb250ZXh0TWVudSA9IHZvaWQgMDtcclxuLy/QlNCy0LAg0LrQvtC90YLQtdC60YHRgtC90YvRhSDQvNC10L3RjiDQvdC1INC80L7Qs9GD0YIg0LHRi9GC0Ywg0L7RgtC60YDRi9GC0Ysg0L7QtNC90L7QstGA0LXQvNC10L3QvdC+XHJcbmNsYXNzIENvbnRleHRNZW51IHtcclxuICAgIGNvbnN0cnVjdG9yKCkge1xyXG4gICAgICAgIHRoaXMucnVuRXZlbnRMaXN0ZW5lcnMoKTtcclxuICAgIH1cclxuICAgIHJ1bkV2ZW50TGlzdGVuZXJzKCkge1xyXG4gICAgICAgIHRoaXMuaGlkZUNvbnRleHRNZW51Qm94ID0gdGhpcy5oaWRlQ29udGV4dE1lbnVCb3guYmluZCh0aGlzKTtcclxuICAgICAgICAvL3RydWUg0L3QtdC+0LHRhdC+0LTQuNC80L4g0LTQu9GPINGC0L7Qs9C+LCDRh9GC0L7QsdGLINC90LDQstC10YDQvdGP0LrQsCDQstC90LDRh9Cw0LvQtSDRgdC60YDRi9Cy0LDQu9C+0YHRjCDQv9GA0LXQtNGL0LTRg9GJ0LXQtSDQvNC10L3Rjiwg0LAg0L/QvtGC0L7QvCDQv9C+0LrQsNC30YvQstCw0LvQvtGB0Ywg0L3QvtCy0L7QtVxyXG4gICAgICAgIGRvY3VtZW50LmFkZEV2ZW50TGlzdGVuZXIoXCJtb3VzZXVwXCIsIHRoaXMuaGlkZUNvbnRleHRNZW51Qm94LCBmYWxzZSk7XHJcbiAgICAgICAgdGhpcy5zaG93Q29udGV4dE1lbnVCb3ggPSB0aGlzLnNob3dDb250ZXh0TWVudUJveC5iaW5kKHRoaXMpO1xyXG4gICAgICAgIGRvY3VtZW50LmFkZEV2ZW50TGlzdGVuZXIoXCJtb3VzZXVwXCIsIHRoaXMuc2hvd0NvbnRleHRNZW51Qm94LCBmYWxzZSk7XHJcbiAgICB9XHJcbiAgICBzaG93Q29udGV4dE1lbnVCb3goZSkge1xyXG4gICAgICAgIGxldCBidG4gPSBlLnRhcmdldDtcclxuICAgICAgICBpZiAoYnRuLmNsYXNzTmFtZSA9PSBcImNvbnRleHRNZW51X19idG5cIikge1xyXG4gICAgICAgICAgICB0aGlzLmNvbnRleHRNZW51VG9nZ2xlID0gYnRuLnByZXZpb3VzRWxlbWVudFNpYmxpbmc7XHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG4gICAgaGlkZUNvbnRleHRNZW51Qm94KGUpIHtcclxuICAgICAgICB2YXIgX2E7XHJcbiAgICAgICAgaWYgKHRoaXMuY29udGV4dE1lbnVUb2dnbGUgIT0gbnVsbCAmJiAoKF9hID0gZS50YXJnZXQpID09PSBudWxsIHx8IF9hID09PSB2b2lkIDAgPyB2b2lkIDAgOiBfYS5odG1sRm9yKSAhPSB0aGlzLmNvbnRleHRNZW51VG9nZ2xlLmlkKSB7XHJcbiAgICAgICAgICAgIHRoaXMuY29udGV4dE1lbnVUb2dnbGUuY2hlY2tlZCA9IGZhbHNlO1xyXG4gICAgICAgIH1cclxuICAgIH1cclxufVxyXG52YXIgY29udGV4dE1lbnUgPSBuZXcgQ29udGV4dE1lbnUoKTtcclxuZXhwb3J0cy5jb250ZXh0TWVudSA9IGNvbnRleHRNZW51O1xyXG4vLyMgc291cmNlTWFwcGluZ1VSTD1kYXRhOmFwcGxpY2F0aW9uL2pzb247YmFzZTY0LGV5SjJaWEp6YVc5dUlqb3pMQ0ptYVd4bElqb2lRMjl1ZEdWNGRFMWxiblV1YW5NaUxDSnpiM1Z5WTJWU2IyOTBJam9pSWl3aWMyOTFjbU5sY3lJNld5SkRiMjUwWlhoMFRXVnVkUzUwY3lKZExDSnVZVzFsY3lJNlcxMHNJbTFoY0hCcGJtZHpJam9pT3pzN1FVRkJRU3g1UkVGQmVVUTdRVUZEZWtRc1RVRkJUU3hYUVVGWE8wbEJRMkk3VVVGRFNTeEpRVUZKTEVOQlFVTXNhVUpCUVdsQ0xFVkJRVVVzUTBGQlF6dEpRVU0zUWl4RFFVRkRPMGxCUlU4c2FVSkJRV2xDTzFGQlEzSkNMRWxCUVVrc1EwRkJReXhyUWtGQmEwSXNSMEZCUnl4SlFVRkpMRU5CUVVNc2EwSkJRV3RDTEVOQlFVTXNTVUZCU1N4RFFVRkRMRWxCUVVrc1EwRkJReXhEUVVGRE8xRkJRemRFTERCSFFVRXdSenRSUVVNeFJ5eFJRVUZSTEVOQlFVTXNaMEpCUVdkQ0xFTkJRVU1zVTBGQlV5eEZRVUZGTEVsQlFVa3NRMEZCUXl4clFrRkJhMElzUlVGQlJTeExRVUZMTEVOQlFVTXNRMEZCUXp0UlFVVnlSU3hKUVVGSkxFTkJRVU1zYTBKQlFXdENMRWRCUVVjc1NVRkJTU3hEUVVGRExHdENRVUZyUWl4RFFVRkRMRWxCUVVrc1EwRkJReXhKUVVGSkxFTkJRVU1zUTBGQlF6dFJRVU0zUkN4UlFVRlJMRU5CUVVNc1owSkJRV2RDTEVOQlFVTXNVMEZCVXl4RlFVRkZMRWxCUVVrc1EwRkJReXhyUWtGQmEwSXNSVUZCUlN4TFFVRkxMRU5CUVVNc1EwRkJRenRKUVVWNlJTeERRVUZETzBsQlNVOHNhMEpCUVd0Q0xFTkJRVU1zUTBGQlVUdFJRVU12UWl4SlFVRkpMRWRCUVVjc1IwRkJjVUlzUTBGQlF5eERRVUZETEUxQlFVMHNRMEZCUXp0UlFVVnlReXhKUVVGSkxFZEJRVWNzUTBGQlF5eFRRVUZUTEVsQlFVa3NhMEpCUVd0Q0xFVkJRVVU3V1VGRGNrTXNTVUZCU1N4RFFVRkRMR2xDUVVGcFFpeEhRVUZ4UWl4SFFVRkhMRU5CUVVNc2MwSkJRWE5DTEVOQlFVTTdVMEZEZWtVN1NVRkRUQ3hEUVVGRE8wbEJSVThzYTBKQlFXdENMRU5CUVVNc1EwRkJVVHM3VVVGRkwwSXNTVUZCU1N4SlFVRkpMRU5CUVVNc2FVSkJRV2xDTEVsQlFVa3NTVUZCU1N4SlFVRkpMRTlCUVVNc1EwRkJReXhEUVVGRExFMUJRVEpDTERCRFFVRkZMRTlCUVU4c1MwRkJTU3hKUVVGSkxFTkJRVU1zYVVKQlFXbENMRU5CUVVNc1JVRkJSU3hGUVVGRk8xbEJRM2hITEVsQlFVa3NRMEZCUXl4cFFrRkJhVUlzUTBGQlF5eFBRVUZQTEVkQlFVY3NTMEZCU3l4RFFVRkRPMU5CUXpGRE8wbEJRMHdzUTBGQlF6dERRVU5LTzBGQlJVUXNTVUZCU1N4WFFVRlhMRWRCUVdkQ0xFbEJRVWtzVjBGQlZ5eEZRVUZGTEVOQlFVTTdRVUZEZUVNc2EwTkJRVmNpZlE9PSIsIlwidXNlIHN0cmljdFwiO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuYWRtaW5NZW51ID0gZXhwb3J0cy5jb250ZXh0TWVudSA9IHZvaWQgMDtcclxuLy/Qn9C+0YHQu9C1INGB0LHQvtGA0LrQuCDRhNCw0LnQu9C+0LIg0LIg0L7QtNC40L0gV2VicGFjay3QvtC8INGE0LDQudC70Ysg0L3QtSDQtNC+0YHRgtGD0L/QvdGLINCx0YDQsNGD0LfQtdGA0YMsINGC0LDQutC40Lwg0L7QsdGA0LDQt9C+0Lwg0L7QvdC4INCx0YPQtNGD0YIg0LTQvtGB0YLRg9C/0L3Riy5cclxuLy/Qn9C+0LTRgNC+0LHQvdC10LUg0LIgd2VicGFjay5jb25maWcuanNcclxuY29uc3QgQ29udGV4dE1lbnVfMSA9IHJlcXVpcmUoXCIuL0NvbXBvbmVudHMvQ29udGV4dE1lbnVcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcImNvbnRleHRNZW51XCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBDb250ZXh0TWVudV8xLmNvbnRleHRNZW51OyB9IH0pO1xyXG5jb25zdCBBZG1pbk1lbnVfMSA9IHJlcXVpcmUoXCIuL0NvbXBvbmVudHMvQWRtaW5NZW51XCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJhZG1pbk1lbnVcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIEFkbWluTWVudV8xLmFkbWluTWVudTsgfSB9KTtcclxuLy8jIHNvdXJjZU1hcHBpbmdVUkw9ZGF0YTphcHBsaWNhdGlvbi9qc29uO2Jhc2U2NCxleUoyWlhKemFXOXVJam96TENKbWFXeGxJam9pUm1sc1pYTkZlSEJ2Y25RdWFuTWlMQ0p6YjNWeVkyVlNiMjkwSWpvaUlpd2ljMjkxY21ObGN5STZXeUpHYVd4bGMwVjRjRzl5ZEM1MGN5SmRMQ0p1WVcxbGN5STZXMTBzSW0xaGNIQnBibWR6SWpvaU96czdRVUZCUVN4eFIwRkJjVWM3UVVGRGNrY3NLMEpCUVN0Q08wRkJReTlDTERCRVFVRjFSRHRCUVVVNVF5dzBSa0ZHUVN4NVFrRkJWeXhQUVVWQk8wRkJSSEJDTEhORVFVRnRSRHRCUVVNM1Fpd3dSa0ZFWWl4eFFrRkJVeXhQUVVOaEluMD0iLCJpbXBvcnQgXCJDOlxcXFxVc2Vyc1xcXFxUaW11clxcXFxEZXNrdG9wXFxcXEV5RV9TaXRlXFxcXENsaWVudFxcXFxDb21wb25lbnRzXFxcXEFkbWluTWVudS50c1wiO1xuaW1wb3J0IFwiQzpcXFxcVXNlcnNcXFxcVGltdXJcXFxcRGVza3RvcFxcXFxFeUVfU2l0ZVxcXFxDbGllbnRcXFxcQ29tcG9uZW50c1xcXFxDb250ZXh0TWVudS50c1wiOyJdLCJzb3VyY2VSb290IjoiIn0=