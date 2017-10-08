## Typography

### core-font

Output font settings for one of the core font sizes defined in [`$type-settings-map`](https://github.com/nhsuk/frontend/blob/master/src/scss/environment/settings/_typography.scss#L13-L19).

**Input**
```scss
h1 {
  @include core-font(20);
}
```

**Output**
```css
h1 {
  font-family: Arial, sans-serif;
  font-size: 20px;
  font-weight: 400;
  line-height: 32px;
  text-transform: none; }
  @media (min-width: 641px) {
    h1 {
      font-size: 20px;
      line-height: 32px; } }
  @media (min-width: 925px) {
    h1 {
      font-size: 20px;
      line-height: 32px; } }
```

### bold-font

Output font settings for one of the core font sizes defined in $type-settings-map (typography.scss) but
use a bold font weight

**Input**
```scss
h1 {
  @include bold-font(19);
}
```

**Output**
```css
h1 {
  font-family: Arial, sans-serif;
  font-size: 20px;
  font-weight: 600;
  line-height: 32px;
  text-transform: none; }
  @media (min-width: 641px) {
    h1 {
      font-size: 20px;
      line-height: 32px; } }
  @media (min-width: 925px) {
    h1 {
      font-size: 20px;
      line-height: 32px; } }
```

### element-spacing

Set the basic spacing between elements and adjust for different screen widths.

**Input**
```scss
* + * {
  @include element-spacing;
}
```

**Output**
```css
* + * {
  margin-top: 16px; }
  @media (min-width: 925px) {
    * + * {
      margin-top: 32px; } }
```

### element-padding

Set basic padding for elements and adjust for different screen widths.

**Input**
```scss
.page-section {
  @include element-padding(vertical);
}
```
**Output**
```css
.page-section {
  padding: 16px 0; }
  @media (min-width: 925px) {
    .page-section {
      padding: 32px 0; } }
```

## Addons

### clearfix

[Micro clearfix hack](http://nicolasgallagher.com/micro-clearfix-hack/) to contain floated elements in a container. 

```scss
.container {
  @include clearfix;
}
```

### reverse-polarity

Sets anchor and its pseudo classes to the corresponding value of [semantic link colour names](https://github.com/nhsuk/frontend/blob/master/src/scss/environment/settings/_colours.scss#L44-L54).
 
**Input**
```scss
.header a {
  @include reverse-polarity;
  background-color: $nhs-blue;
}
```

**Output**
```css
.header a {
  background-color: #005eb8; }
  .header a:link {
    color: #fff; }
  .header a:visited {
    color: #fff; }
  .header a:hover {
    color: #d4efff; }
  .header a:active {
    color: #d4efff; }
  .header a:focus {
    outline: 3px solid #ffb81c; }
```

### visuallyhidden

Visually hides content from user while keeping it accessible to screen readers. Based on [Snook's method](https://snook.ca/archives/html_and_css/hiding-content-for-accessibility)
and [HTML5 Boilerplate](https://github.com/h5bp/html5-boilerplate/blob/master/src/css/main.css#L128-L139).

```scss
button .help-text {
  @include visuallyhidden; 
}
```


## Layout 

### grid-column

Creates a cross browser grid column with a standardised gutter between the columns.

```scss
div {
  // Column one quarter of container
  @include grid-column(1 / 4);
}

div {
  // Forcing column to be full width at desktop
  @include grid-column(1 / 2, $full-width: desktop);
}

div {
  // Float column to the right
  @include grid-column(1 / 2, $float: right);
}
```

### grid-row

Creates container for grid columns.

```scss
.row {
  @include grid-row;
}
```

### site-width-container

Creates a container for the site with predefined max-width (as defined in [`$site-width`](https://github.com/nhsuk/frontend/blob/master/src/scss/environment/settings/_layout.scss#L3))

```scss
.container {
  @include site-width-container;
}
```

### reading-width

Set the predefined max width on a container for comfortable reading (as defined in [`$measure`](https://github.com/nhsuk/frontend/blob/master/src/scss/environment/settings/_typography.scss#L11))

```scss
.container {
  @include reading-width;
}
```

## Conditionals

### media

Block include to support conditional content for different screen sizes. Based on a mobile first approach.

**Input**
```scss
div {
  border: 1px solid;

  @include media(tablet){
    width: 30%;
    float: left;
  }
}
```

**Output**
```css
div {
  border: 1px solid; }
  @media (min-width: 641px) {
    div {
      width: 30%;
      float: left; } }
```


### ie-lte

Use conditional content for versions of IE and lower. Designed to be used in separate IE-specific files which contain
IE variables:

```scss
$is-ie: true;
$ie-version: 7;
```

**Input**
```scss
.panel {
  max-width: $site-width;

  @include ie-lte(8) {
    width: $site-width;
  }
}
```

**Output for IE<8**
```css
.panel {
  max-width: 960px;
  width: 960px;
}
```

**Output for other browsers**
```css
.panel {
  max-width: 960px;
}
```

### ie

Use conditional content for specific version of IE. Same as [`ie-lte(8)`](#ie-lte) it's designed to be used in
separate IE-specific files.

```scss
span {
  display: inline-block;

  @include ie(6) {
    display: inline;
    zoom: 1;
  }
}
```

## Components

### button

Mixin and defaults for making buttons.

Adapted from [GOV.UK Elements buttons](https://github.com/alphagov/govuk_frontend_toolkit/blob/master/stylesheets/design-patterns/_buttons.scss).

```scss
.button{
  @include button($green);
}
.button-secondary{
  @include button($grey-3);
}
.button-warning{
  @include button($red);
}
```
