/// <binding BeforeBuild='build' Clean='clean' />

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    runSequence = require('run-sequence'),
    gulpUtil = require('gulp-util'),
    project = require("./project.json");

var paths = {
    webroot: "./" + project.webroot + "/"
};

paths.jsOutput = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/bca.js"
paths.concatJsDestMin = paths.webroot + "js/bca.min.js";
paths.concatCssDest = paths.webroot + "css/bca.css";
paths.concatCssDestMin = paths.webroot + "css/bca.min.css";
paths.jsSource = "Scripts/**/*.js";
paths.cssSource = "Scripts/**/*.css";
paths.imgOutput = paths.webroot + "images/";
paths.imgSource = "Ressources/Images/*.*";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDestMin, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDestMin, cb);
});

gulp.task("copy:images", function () {
    gulp.src(paths.imgSource)
        .pipe(gulp.dest(paths.imgOutput));
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("build", function (cb) {
    runSequence('clean', ['copy:images', 'build:bca.js', 'build:bca.css'], 'min:css', 'min:js', cb);
});

gulp.task("build:bca.js", function () {
    gulp.src([paths.jsSource], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(gulp.dest("."));
});

gulp.task("min:js", function () {
    gulp.src([paths.jsOutput, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDestMin))
        .pipe(uglify().on('error', gulpUtil.log))
        .pipe(gulp.dest("."));
});


gulp.task("build:bca.css", function () {
    gulp.src([paths.cssSource], { base: "." })
        .pipe(concat(paths.concatCssDest))
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDestMin))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);
