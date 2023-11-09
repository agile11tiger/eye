//http://vanilla-js.blogspot.com/2018/02/webpack-4.html          описание настроек webpack
//https://habr.com/ru/company/plarium/blog/309230/               10 особенностей Webpack
//https://tproger.ru/translations/configure-webpack4/            Webpack 4: практические рекомендации по настройке
//https://metanit.com/web/angular2/9.1.php                       Webpack
//https://github.com/webpack/webpack/issues/370                  прога с подстановкой шаблонов для поиска всех файлов например: "multi-entry-loader?include=Views/**/**/**/**/**/*.ts!"

const path = require("path");
//const MiniCssExtractPlugin = require('mini-css-extract-plugin');

const commonTs = {
    mode: "development",
    devtool: "inline-source-map",
    resolve: {
        // Add `.ts` and `.tsx` as a resolvable extension.
        extensions: [".ts", ".tsx"]
    },
    module:{
        rules:[ 
            {
                test: /\.tsx?$/, 
                use: "ts-loader",
                //include: [
                //    path.resolve(__dirname, "Views")
                //],
                exclude: [
                    path.resolve(__dirname, "node_modules"),
                    path.resolve(__dirname, "wwwroot")
                ],
            }
            //{
            //    test: /\.css$/,
            //    use: [MiniCssExtractPlugin.loader, "css-loader"],
            //    include: path.resolve(__dirname, "Views"),
            //    exclude: /(node_modules|wwwroot)/,
            //}
        ]
    }
}

const allTsFiles = {
    entry: [
        "multi-entry-loader?include=Components/**/**/**/**/**/*.ts!",
        "multi-entry-loader?include=Pages/**/**/**/**/**/*.ts!",
        "multi-entry-loader?include=Shared/**/**/**/**/**/*.ts!",
        "./FilesExport" //общий файл для импорта всех необходимых файлов(обязательно последний, согласно документации webpack)
    ], // входная точка - исходный файл
    output: {
        path: path.resolve(__dirname, "wwwroot/js"),     // путь к каталогу выходных файлов
        filename: "bundle.js",       // название создаваемого файла
        library: "bundle"
    },
};

module.exports = [{
    ...commonTs,
    ...allTsFiles
}]