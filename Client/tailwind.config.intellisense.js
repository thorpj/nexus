const colors = require("tailwindcss/colors");

module.exports = {
    darkMode: "media",
    theme: {
        extend: {
            colors: {
                cyan: colors.cyan,
                orange: colors.orange,
            },
            gridTemplateRows: {
                'mobile-layout': '70px minmax(900px, 1fr) 100px',
            }
        },
    },
    variants: {
        extend: {
            backgroundColor: ['active'],
            borderColor: ['active'],
            fill: ['hover', 'focus'],
            stroke: ['hover', 'focus'],
            strokeWidth: ['hover', 'focus']
        }
    },
    plugins: [
        require("tailwindcss-textshadow"),
        require("@tailwindcss/forms"),
        require("@tailwindcss/typography"),
        require("@tailwindcss/aspect-ratio")
    ],
};
