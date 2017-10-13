const path = require('path')

const customTheme = require('@frctl/mandelbrot')({
  lang: 'en-gb',
  styles: ['default', '/themes/nhsuk/theme.css'],
  nav: ['docs', 'components'],
  panels: ['html', 'context', 'resources', 'info']
})

customTheme.addStatic(path.join(__dirname, 'assets'), 'themes/nhsuk')

customTheme.addLoadPath(path.join(__dirname, 'views'))

module.exports = customTheme
