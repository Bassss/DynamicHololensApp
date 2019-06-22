let gulp = require('gulp'),
    nodemon = require('gulp-nodemon'),
    env = require('gulp-env');

gulp.task('default', () =>
{
    nodemon({
        script: 'app.js',
        ext:  'js,json,sql',
        ignore: ['./node_modules/**', './saveData/', './assets/', './assetHelpers/']
    })
    .on('restart', () => {
        console.log("Restarting...");
    })
});
 