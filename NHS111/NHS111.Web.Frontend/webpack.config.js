const path = require('path'),
    UglifyJSPlugin = require('uglifyjs-webpack-plugin'),
    Visualizer = require('webpack-visualizer-plugin')


module.exports = {
    entry: {
        'bundle': './src/js/main.js',
        'polyfills': './src/js/polyfills.js'
    },
    output: {
        path: path.resolve(__dirname, './content/js/'),
        filename: '[name].js'
    },
    plugins: [
        new UglifyJSPlugin({ sourceMap: true }),
        new Visualizer({
            filename: '../../src/codebase/components/_jschart/jschart.njk'
        })
    ],
    module: {
        rules: [
            {
                test: /src\.js$/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['env'],
                    }
                }
            }
        ]
    }
}
