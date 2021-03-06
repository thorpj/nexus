const colors = require("tailwindcss/colors");

module.exports = {
    purge: ["./**/*.{html,razor,razor.css}"],
    mode: "jit",
    darkMode: "media",
    theme: {
        extend: {
            colors: {
                cyan: colors.cyan,
                orange: colors.orange,
            },
            gridTemplateRows: {
                'mobile-layout': '70px 50px 30fr 4fr',
            },
            gridTemplateColumns: {
                'sidebar-layout': '2fr 10fr'
            }
        },
    },
    variants: {
        extend: {}
    },
    plugins: [
        require("tailwindcss-textshadow"),
        require("@tailwindcss/forms"),
        require("@tailwindcss/typography")
        // require("@tailwindcss/aspect-ratio")
    ],
};
