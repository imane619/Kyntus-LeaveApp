/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        'app-navy': '#1a2233',      
        'app-blue': '#007bff',      
        'app-bg': '#f4f7fe',        
        'app-text-dark': '#2b3674',
      },
      borderRadius: {
        'xl': '20px',               
      }
    },
  },
  plugins: [],
}