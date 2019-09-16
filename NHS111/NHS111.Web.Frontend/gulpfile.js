const path = require('path'),
    gulp = require('gulp'),
    runSequence = require('run-sequence'),
    del = require('del'),
    sass = require('gulp-sass'),
    flatten = require('gulp-flatten'),
    importOnce = require('node-sass-import-once'),
    postcss = require('gulp-postcss'),
    autoprefixer = require('autoprefixer'),
    inject = require('gulp-inject'),
    cssnano = require('cssnano'),
    mocha = require('gulp-mocha'),
    fs = require('fs'),
    pipedWebpack = require('piped-webpack'),
    eslint = require('gulp-eslint')

const paths = {
  srcScripts: `${__dirname}/src/scripts`,
  srcScss: `${__dirname}/src/styles`,
  srcImages: `${__dirname}/src/images`,
  dist: `${__dirname}/../NHS111.Web/Content/`,
  distAssets: `${__dirname}/../NHS111.Web/Content/`
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
  return runSequence('clean:dist')
})

gulp.task('clean:dist', () => {
    return del([`${paths.dist}/`], { force: true })
})

//gulp.task('copy:styles:dist', () => {
//  return gulp.src([
//    `${paths.srcScss}/**/*.scss`,
//    `!${paths.srcScss}/components/**/*.scss`
//  ])
//  .pipe(gulp.dest(paths.distScss))
//})

//gulp.task('copy:styles:components:dist', () => {
//  return gulp.src(`${paths.srcScss}/components/**/*.scss`)
//    .pipe(flatten())
//    .pipe(gulp.dest(`${paths.distScss}/components`))
//})

// Copy images
gulp.task('copy:images', () => {
  return gulp.src(`${paths.srcImages}/**/*`)
    .pipe(gulp.dest(`${paths.distAssets}/images`))
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
    return gulp.src([`${paths.srcScripts}/**/*.js`, `!${paths.srcScripts}/vendor/*.js`, '!node_modules/**'])
        .pipe(eslint())
        .pipe(eslint.format())
        .pipe(eslint.failAfterError());
})

gulp.task('test:scripts', function() {
  return gulp.src([`${paths.srcScripts}/test-*.js`])
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

//gulp.task('inject:styles:dist', () => {
//  return gulp.src(`${paths.distScss}/components/_index.scss`)
//    .pipe(inject(gulp.src([
//      `${paths.distScss}/components/*.scss`,
//      `!${paths.distScss}/components/_index.scss`
//    ]), {
//      starttag: '//inject:{{ext}}',
//      endtag: '//endinject',
//      relative: true,
//      transform: (filePath) => `@import "${filePath}";`
//    }))
//    .pipe(gulp.dest(`${paths.distScss}/components`))
//})

gulp.task('compile:styles:dist', () => {
  return gulp.src(`${paths.srcScss}/*.scss`)
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
    .pipe(postcss([autoprefixer({ grid: false }), cssnano()]))
    .pipe(gulp.dest(`${paths.distAssets}/css`))
})


gulp.task('compile:scripts', () => {
    return gulp.src([])
               .pipe(pipedWebpack(require('./webpack.config.js')))
               .pipe(gulp.dest(`${paths.distAssets}/js`))
});

gulp.task('build', cb => {
  //runSequence(
  //  ['copy:styles:dist', 'copy:styles:components:dist', 'copy:images'],
  //    'inject:styles:dist', 'compile:styles:dist', 'compile:scripts',
  //  cb
  //)
  runSequence(
    'copy:images', 'compile:styles:dist', 'compile:scripts',
    cb
  )
})

gulp.task('watch', function () {
  return gulp.watch(
    [
      `${paths.srcScss}/**/*.scss`,
      `${paths.srcScripts}/**/*.js`
    ],
    ['build']
  )
})

gulp.task('default', ['build'])
gulp.task('dev', ['build','watch'])
