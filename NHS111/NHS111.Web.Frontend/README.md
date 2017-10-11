# NHS 111 Online Frontend (Alpha)

This project is the NHS 111 Online frontend, which builds upon the NHSUK Frontend Library. The frontend library can be viewed at https://nhschoices.github.io/111-frontend-library/.

## Development

Run local docs server:

```bash
npm run dev
```

Build frontend:

```bash
npm run build
```

Build docs for deployment:

```bash
npm run build:docs
```

Build and deploy docs to GitHub pages:

```bash
npm run deploy
```

Make sure the version in `package.json` is bumped for each release as the docs are generated for each version so they can be inspected retrospectively.

## Usage

It assumes that your front-end is a Node app with Nunjucks templates and Sass. Install this package with NPM:

```
npm install nhsuk/frontend --save
```

### Styles

Include individual styles in your Sass:

```scss
@import "nhsuk-frontend/dist/scss/environment/generic";
@import "nhsuk-frontend/dist/scss/environment/tools";
@import "nhsuk-frontend/dist/scss/units";
...
@import "nhsuk-frontend/dist/scss/components/button";
```

Import everything:

```scss
@import "nhsuk-frontend/dist/scss/nhsuk";
```

Include the compiled css in the head of your page:

```html
<link href="[your asset path]/nhs-111.css" media="screen" rel="stylesheet" type="text/css">
```

The package uses [node-sass-import-once](https://github.com/at-import/node-sass-import-once) module to enable `_index.scss` imports and remove duplicates. If you decide to include individual Sass files make sure your configure your Sass to use custom importer:

```js
sass({
  includePaths: SASS_PATHS,
  outputStyle: isProduction ? 'compressed' : 'expanded',
  importer: importOnce,
  importOnce: {
    index: true,
    css: true
  }
}
```

### Templates

Configure Nunjucks to add this package's template path to your Nunjucks environment:

 ```js
const path = require('path');
const nunjucksEnv = nunjucks.configure([
  path.resolve(__dirname, '/app/views'),
  path.resolve(__dirname, '/node_modules/nhsuk-frontend/src/templates')
], {
  autoescape: true
})
```

Then extend your template from NHS.UK base template:

```jinja
{% extends 'nhsuk-base.njk' %}
```

Base template has a number of [blocks](https://mozilla.github.io/nunjucks/templating.html#block)
that can be overridden or extended from your template:

#### asset_path

Base template is using `asset_path` global function extensively for all asset references in template. This function is expected to be defined in the app using this package. For example:

```js
const nunjucksEnv = nunjucks.configure([
  // ...
], {
  // ...
});

nunjucksEnv.addGlobal('asset_path', filename => filename);
```

**Nunjucks blocks**

- `head` - contains the whole `head` element
  - `head_title_content` - contains `title` element content
  - `head_content` - wraps main content for `head` element
  - `head_stylesheets` - wraps stylesheet declarations
- `body` - contains the whole `body` element
  - `body_skiplinks` - wraps container with "skip to content" link (first element inside `body`)
  - `body_notifications` - wraps cookie message container (above `site_header`, after `skiplinks`)
  - `body_site_header` - wraps site header
  - `body_site_header_service_title` - wraps the service title
  - `body_main_content` - contains main content (inside `main#content`)
  - `body_footer` - wraps site footer container (inside `body > footer`)
    - `body_footer_content` - contains content inside site footer
