const path = require('path'),
    UglifyJSPlugin = require('uglifyjs-webpack-plugin'),
    Visualizer = require('webpack-visualizer-plugin'),
    webpack = require('webpack')


module.exports = {
    entry: {
        'bundle-head': './src/js/entry-head.js',
        'bundle': './src/js/entry-main.js',
        'bundle-polyfills': './src/js/entry-polyfills.js',
        'bundle-map': './src/js/entry-map.js'
    },
    output: {
        filename: '[name].js'
    },
    plugins: [
        new UglifyJSPlugin({ sourceMap: true }),
        new webpack.optimize.CommonsChunkPlugin({
          name: "common",
          filename: "common.js",
          minChunks: 2
        }),
        new Visualizer({
            filename: './src/codebase/components/_jschart/jschart.njk'
        })
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
