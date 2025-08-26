/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,ts,jsx,tsx}",
    "./src/app/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#f8f9fa',      // light background
        secondary: '#495057',    // dark text/nav
        accent: '#2563eb',       // blue accent
      },
    },
  },
  plugins: [],
};
