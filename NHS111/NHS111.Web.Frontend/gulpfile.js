/// <binding />
const path = require('path'),
    gulp = require('gulp'),
    runSequence = require('run-sequence'),
    del = require('del'),
    sass = require('gulp-sass'),
    flatten = require('gulp-flatten'),
    importOnce = require('node-sass-import-once'),
    postcss = require('gulp-postcss'),
    syntax_scss = require('postcss-scss'),
    autoprefixer = require('autoprefixer'),
    inject = require('gulp-inject'),
    cssnano = require('cssnano'),
    stylelint = require('stylelint'),
    mocha = require('gulp-mocha'),
    babel = require('gulp-babel'),
    jsdom = require('mocha-jsdom'),
    fs = require('fs'),
    concat = require('gulp-concat'),
    sourcemaps = require('gulp-sourcemaps'),
    webpack = require('gulp-webpack'),
    pipedWebpack = require('piped-webpack'),
    eslint = require('gulp-eslint')

// Paths
const paths = {
  src: `${__dirname}/src`,
  srcScss: `${__dirname}/src/codebase`,
  srcImages: `${__dirname}/src/assets/images`,
  dist: `${__dirname}/../NHS111.Web/Content/`,
  distAssets: `${__dirname}/../NHS111.Web/Content/`,
  distScss: `${__dirname}/../NHS111.Web/Content/codebase`
}

gulp.task('build-if-missing', () => {
    return fs.readdir(paths.dist, function (err, files) {
        if (err) {
            // some sort of error
        } else {
            if (!files.length) {
                return runSequence('build:dist')
            }
        }
    })
})

gulp.task('clean', () => {
  return runSequence('clean:dist', 'clean:fractal')
})

gulp.task('clean:dist', () => {
    return del([`${paths.dist}/`, `${paths.distFractal}/`], { force: true })
})

gulp.task('clean:fractal', () => {
    return del([`${paths.fractalAssets}/*.css`], { force: true })
})

gulp.task('copy:styles:dist', () => {
  return gulp.src([
    `${paths.srcScss}/**/*.scss`,
    `!${paths.srcScss}/components/**/*.scss`
  ])
  .pipe(gulp.dest(paths.distScss))
  .pipe(gulp.dest(paths.distFractalScss))
})

gulp.task('copy:styles:components:dist', () => {
  return gulp.src(`${paths.srcScss}/components/**/*.scss`)
    .pipe(flatten())
    .pipe(gulp.dest(`${paths.distScss}/components`))
    .pipe(gulp.dest(`${paths.distFractalScss}/components`))
})

// Copy images
gulp.task('copy:images', () => {
  return gulp.src(`${paths.srcImages}/**/*`)
    .pipe(gulp.dest(`${paths.distAssets}/images`))
    .pipe(gulp.dest(`${paths.distFractalAssets}/images`))
})

gulp.task('lint:styles', () => {
  const gulpStylelint = require('gulp-stylelint');
  return gulp.src(`${paths.srcScss}/**/*.scss`)
    .pipe(gulpStylelint({
      failAfterError: true,
      reporters: [
        {formatter: 'string', console: true},
      ]
    }))
})

gulp.task('lint:scripts', () => {
    return gulp.src([`${__dirname}/src/js/**/*.js`, `!${__dirname}/src/js/vendor/*.js`, '!node_modules/**'])
        .pipe(eslint())
        .pipe(eslint.format())
        .pipe(eslint.failAfterError());
})

gulp.task('lint:styles-graph', function () {
    const file = fs.readFileSync("content/css/nhs-111.css", "utf8");

    specificityGraph(`${__dirname}/src/codebase/components/_cssgraph/`, file, function (directory) {
      // Add the contents of specificty.json to a variable in specificity.js 
      fs.readFile(`${__dirname}/src/codebase/components/_cssgraph/specificity.json`, 'utf8', function (err, data) {
        if (err) return console.log("ERROR: " + err)
        var newData = "var embeddedJsonData = " + data;
        fs.writeFile(`${__dirname}/src/codebase/components/_cssgraph/specificity.js`, newData)
      })
    })
})

gulp.task('test:scripts', function() {
  return gulp.src(['src/js/test-*.js'])
    .pipe(mocha({
      compilers: "js:babel-core/register",
      reporter: "spec",
      timeout: 20000
    }))
    .once('error', () => {
      process.exit(1)
    })
    .once('end', () => {
      process.exit()
    })
})

gulp.task('inject:styles:dist', () => {
  return gulp.src(`${paths.distScss}/components/_index.scss`)
    .pipe(inject(gulp.src([
      `${paths.distScss}/components/*.scss`,
      `!${paths.distScss}/components/_index.scss`
    ]), {
      starttag: '//inject:{{ext}}',
      endtag: '//endinject',
      relative: true,
      transform: (filePath) => `@import "${filePath}";`
    }))
    .pipe(gulp.dest(`${paths.distScss}/components`))
    .pipe(gulp.dest(`${paths.distFractalScss}/components`))
})

gulp.task('compile:styles:fractal', () => {
  return gulp.src(`${paths.fractalScss}/**/*.scss`)
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest(`${paths.fractalAssets}`))
})

gulp.task('compile:styles:dist', () => {
  return gulp.src(`${paths.distScss}/*.scss`)
    .pipe(sass({
      importer: importOnce,
      importOnce: {
        index: true,
        css: true
      }
    }).on('error', (err) => {
      sass.logError.call(this, err)
      process.exit(1)
    }))
    .pipe(postcss([autoprefixer(), cssnano()]))
    .pipe(gulp.dest(`${paths.distAssets}/css`))
    .pipe(gulp.dest(`${paths.distFractalAssets}/css`))
})


gulp.task('compile:scripts', () => {
    return gulp.src([])
               .pipe(pipedWebpack(require('./webpack.config.js')))
               .pipe(gulp.dest(`${paths.distAssets}/js`)).pipe(gulp.dest(`${paths.distFractalAssets}/js`))
});

gulp.task('fractal:start', function () {
    const server = fractal.web.server({
        sync: true,
        port: 4000
    });
    server.on('error', err => fractal.cli.console.error(err.message));
    return server.start().then(() => {
        fractal.cli.console.success(`Fractal server is now running at ${server.url}`);
    });
});

gulp.task('build:fractal', cb => {
  runSequence('clean:fractal', 'compile:styles:fractal', cb)
})

gulp.task('build:dist', cb => {
  runSequence(
    ['copy:styles:dist', 'copy:styles:components:dist', 'copy:images'],
      'inject:styles:dist', 'compile:styles:dist', 'compile:scripts', 'lint:styles-graph',
    cb
  )
})

gulp.task('build', cb => {
  runSequence('build:fractal', 'build:dist', cb)
})

gulp.task('watch', function () {
  return gulp.watch(
    [
      `${paths.srcScss}/**/*.scss`,
      `${paths.fractalScss}/**/*.scss`,
      `src/js/**/*.js`
    ],
    ['build']
  )
})

gulp.task('default', ['build'])
gulp.task('dev', ['build','watch'])
