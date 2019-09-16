const UglifyJSPlugin = require('uglifyjs-webpack-plugin'),
    webpack = require('webpack')


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
        new UglifyJSPlugin({ sourceMap: true }),
        new webpack.optimize.CommonsChunkPlugin({
          name: "common",
          filename: "common.js",
          minChunks: 2,
          chunks: ["bundle-head", "bundle", "bundle-polyfills"] // Excludes map so that can be imported on its own.
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
