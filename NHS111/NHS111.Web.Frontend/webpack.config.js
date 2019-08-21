const path = require('path'),
    UglifyJSPlugin = require('uglifyjs-webpack-plugin')


module.exports = {
    entry: {
        'bundle-head': './src/scripts/entry-head.js',
        'bundle': './src/scripts/entry-main.js',
        'bundle-polyfills': './src/scripts/entry-polyfills.js',
        'bundle-map': './src/scripts/entry-map.js'
    },
    output: {
        filename: '[name].js'
    },
    plugins: [
        new UglifyJSPlugin({ sourceMap: true })
    ],
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        babelrc: false,
                        cacheDirectory: true,
                        presets: [
                            ['env', {
                              "targets": {
                                  "browsers": ["last 2 versions", "ie >= 9"]
                                }
                            }]
                        ]
                    }
                }
            }
        ]
    }
}
