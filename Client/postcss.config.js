// This gives an error when using 'npm run buildcss:watch'. Will probably need to figure out a way to do css minifying
// module.exports = ({ env }) => ({
//     plugins: {
//         tailwindcss: {},
//         autoprefixer: {},
//         cssnano: env === "production" ? { preset: "default" } : false
//     }
// });
module.exports = {
    plugins: {
        "postcss-import": {},
        tailwindcss: {},
    },
};
