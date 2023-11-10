let handler;

window.ConnectionChecker = {
    Initialize: function (interop) {
        console.log("Initialize");
        handler = function () {

            console.log("handler");
            interop.invokeMethodAsync("ConnectionChecker.StatusChanged", navigator.onLine);
        }

        window.addEventListener("online", handler);
        window.addEventListener("offline", handler);

        handler(navigator.onLine);
    },
    Dispose: function () {

        if (handler != null) {

            window.removeEventListener("online", handler);
            window.removeEventListener("offline", handler);
        }
    }
};