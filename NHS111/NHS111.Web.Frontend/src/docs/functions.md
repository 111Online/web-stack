## Typography

### line-height

Return the line height associated with a type scale at a certain screen size

**Default screen size**
```scss
h1 {
  line-height: line-height(0);
}
```

**Small screen size**
```scss
h1 {
  line-height: line-height(0, "mobile");
}
```

### type-scale

Return the font size associated with a type scale at a certain screen size

**Default screen size**
```scss
h1 {
  font-size: type-scale(0);
}
```

**Small screen size**
```scss
h1 {
  font-size: type-scale(0, "mobile");
}
```


## Colour

### tint & shade

Mixes a colour with white or black.
  
These functions are similar to native Sass' `lighten` and `darken` HSL functions. They differ in that the native functions
manipulate the value value of L in HSL (`lighten` increases and `darken` decreases by the specified percentage). 
`tint` and `shade` on the other hand add the specified amount of white or black to the selected colour.

**Input**
```scss
body {
  background-color: tint(#f00, 20%);
  color: shade(#f00, 40%);
}
```

**Output**
```css
body {
  background-color: #ff3333;
  color: #990000;
}
```
