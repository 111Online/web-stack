const gulp = require("gulp"),
  sass = require("gulp-sass"),
  importOnce = require("node-sass-import-once"),
  postcss = require("gulp-postcss"),
  autoprefixer = require("autoprefixer"),
  cssnano = require("cssnano"),
  fs = require("fs"),
  webpack = require("webpack"),
  webpackConfig = require("./webpack.config.js")

const isProduction = process.env.NODE_ENV === 'production'

// Development tasks

if (!isProduction) {
  
  gulp.task("lint:styles", () => {
    const gulpStylelint = require("gulp-stylelint")
    return gulp.src(`${paths.srcScss}/**/*.scss`).pipe(
      gulpStylelint({
        failAfterError: true,
        reporters: [{ formatter: "string", console: true }]
      })
    )
  })

  gulp.task("lint:scripts", () => {
    const eslint = require("gulp-eslint")
    return gulp
      .src([
        `${paths.srcScripts}/**/*.js`,
        `!${paths.srcScripts}/vendor/*.js`,
        "!node_modules/**"
      ])
      .pipe(eslint())
      .pipe(eslint.format())
      .pipe(eslint.failAfterError())
  })

  gulp.task("test:scripts", function(done) {
    const mocha = require("gulp-mocha")
    return gulp
      .src([`${paths.srcScripts}/test-*.js`])
      .pipe(
        mocha({
          require: '@babel/register',
          reporter: "spec",
          timeout: 20000
        })
      )
      .once("error", () => {
        done()
      })
      .once("end", () => {
        done()
      })
  })
}

// Production tasks

const paths = {
  srcScripts: `${__dirname}/src/scripts`,
  srcScss: `${__dirname}/src/styles`,
  srcImages: `${__dirname}/src/images`,
  dist: `${__dirname}/../NHS111.Web/Content/`
}

gulp.task("copy:images", () => {
  return gulp
    .src(`${paths.srcImages}/**/*`)
    .pipe(gulp.dest(`${paths.dist}/images`))
})

gulp.task("compile:styles", () => {
  return gulp
    .src(`${paths.srcScss}/*.scss`)
    .pipe(
      sass({
        importer: importOnce,
        importOnce: {
          index: true,
          css: true
        }
      }).on("error", err => {
        sass.logError.call(this, err)
        process.exit(1)
      })
    )
    .pipe(postcss([autoprefixer(), cssnano({
      reduceIdents: { keyframes: false },
      discardUnused: { keyframes: false }
    })]))
    .pipe(gulp.dest(`${paths.dist}/css`))
})

gulp.task("compile:scripts", () => {
  return new Promise((resolve, reject) => {
    webpack(webpackConfig, (err, stats) => {
      if (err) {
        return reject(err)
      }
      if (stats.hasErrors()) {
        return reject(new Error(stats.compilation.errors.join("\n")))
      }
      resolve()
    })
  })
})

gulp.task("build", gulp.series("compile:styles", "compile:scripts", "copy:images"))

gulp.task("watch", function() {
  // These watches use globs that (as per gulp docs) cannot be absolute paths therefore does not use the paths object.
  gulp.watch(`src/styles/**/*.scss`, gulp.parallel("compile:styles"))
  gulp.watch(`src/scripts/**/*.js`, gulp.parallel("compile:scripts"))
  gulp.watch(`src/images/**/*`, gulp.parallel("copy:images"))
})

gulp.task("default", gulp.series("build"))
gulp.task("dev", gulp.series("build", "watch"))
