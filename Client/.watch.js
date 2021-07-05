// https://gist.github.com/subfuzion/c2753bcf5fc12b8b2759
var glob = require("glob");
var path = require("path");

// ===== files to watch =====
// this is just a simple glob example that could be done far more
// easily by using the following run script in package.json:
// "watch": "watch '<cmd>' ./src"

var pattern = "./**/*.{razor,razor.css,html}";
var options = {};
// ==========================

var files = glob.sync(pattern, options);
var set = new Set();

// Add all the files we want to watch to the set.
// We need to explicitly add the directory name
// to the set as well, otherwise the watch
// filter function won't watch files in it.
files.forEach(function (f) {
    var p = path.relative(__dirname, f);
    set.add(p);
    var dir = path.parse(p).dir;
    set.add(dir);
});

// Exports a filter function that returns true or false for each file and
// directory to decide whether or not that file or directory is included by
// watch. The argument to the function (f) is a fully qualified string pathname.
module.exports = function (f) {
    // The original naive approach attempted to use minimatch, but minimatch
    // rejects 'src' when the glob pattern is 'src/**', for example, and if
    // the 'src' dir is rejected, watch won't call this filter function with
    // any files inside the 'src' directory.
    // This is an artificial example, but reflects my desire to handle
    // simple glob patterns that might be supplied by a user that should
    // work as expected.
    var p = path.relative(__dirname, f);
    var watch = set.has(p);
    if (watch) console.log(p);
    return set.has(p);
};
