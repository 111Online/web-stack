const path = require('path')
const fractal = require('@frctl/fractal').create()
const packageMeta = require('./package')
const nhsCustomTheme = require('./fractal/theme')

const buildPathPrefix = (process.env.NODE_ENV === 'production' ? `/${packageMeta.version}` : '')
const paths = {
  dist: `${__dirname}`,
  src: `${__dirname}/src`
}

// Nunjucks config
const nunjucks = require('@frctl/nunjucks')({
  globals: {
    asset_path: function(filename) {
      filename = filename.replace(/^(\w)/, '/$1')
      return require('@frctl/nunjucks/src/filters/path')(fractal).bind(this)(filename)
    }
  },
  paths: ['src/docs', 'src/docs/graph','src/templates']
})

// Project config
fractal.set('project.title', 'NHS.UK Frontend')

// Components config
fractal.components.engine(nunjucks)
fractal.components.set('default.preview', '@preview')
fractal.components.set('default.status', null);
fractal.components.set('ext', '.njk')
fractal.components.set('path', `${paths.src}/codebase`)
fractal.components.set('label', 'Codebase')


// Docs config
fractal.docs.engine(nunjucks)
fractal.docs.set('ext', '.md')
fractal.docs.set('path', `${paths.src}/docs`)

// Web UI config
fractal.web.theme(nhsCustomTheme)
fractal.web.set('static.path', paths.dist)
fractal.web.set('builder.dest', path.join(__dirname, `build`, buildPathPrefix))
module.exports = fractal
