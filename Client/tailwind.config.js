const colors = require("tailwindcss/colors");

module.exports = {
    mode: "jit",
    purge: ["./**/*.{html,razor,razor.css}"],
    darkMode: "media",
    theme: {
        extend: {
            colors: {
                cyan: colors.cyan,
                orange: colors.orange,
            },
        },
    },
    variants: {
        extend: {},
    },
    plugins: [require("tailwindcss-textshadow"), require("@tailwindcss/forms")],
};