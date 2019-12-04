/**
 * Due to inline scripts throughout the site, this file creates a bundle
 * for any dependencies (such as jquery) that are absolutely required at the beginning
 * of page load. The rest go at the end, for performance.
 *
 * [1]: jQuery cookie would ideally be imported in entry-main to be served at the bottom of the site
 *      however as part of swapping from importing from file to from npm module,
 *      the modules now import all dependencies in each file. So jQuery was being imported twice.
 *      The fix for now is to use webpack.config.js code splitting in such a way that
 *      it creates the common.js file for jQuery and other shared dependencies.
 *      However this was not working as just requiring jQuery in this file did not count as a dependency.
 *      So we import one jQuery plugin (cookie) so that the dependency graph is correct for the code splitting.
 **/

global.$ = global.jQuery = require('jquery')
global.Spinner = require('spin.js').Spinner
import './vendor/details.min.js'
import 'jquery.cookie' // [1]
import './event-logging.js'
