/**
 * Due to inline scripts throughout the site, this file creates a bundle
 * for any dependencies (such as jquery) that are absolutely required at the beginning
 * of page load. The rest go at the end, for performance.
 */

import './vendor/details.min.js'
global.$ = global.jQuery = require('jquery')
global.Spinner = require('./vendor/spin.js')
