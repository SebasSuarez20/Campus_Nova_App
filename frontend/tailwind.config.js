/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        nova: {
          primary: '#5B3DF5',
          accent: '#FF7A59',
          navy: '#14123A',
          bg: '#F5F6FB',
          line: '#E4E6F2',
          text: '#17183B',
          muted: '#6B6F93',
        },
      },
      fontFamily: {
        display: ['Sora', 'Plus Jakarta Sans', 'sans-serif'],
      },
    },
  },
  plugins: [],
};
