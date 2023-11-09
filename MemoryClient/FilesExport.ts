//После сборки файлов в один Webpack-ом файлы не доступны браузеру, таким образом они будут доступны.
//Подробнее в webpack.config.js
import { contextMenu } from "./Components/ContextMenu";
import { adminMenu } from "./Components/AdminMenu";
export { contextMenu, adminMenu };