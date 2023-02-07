/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js,njk,yml,md}", "./.eleventy.js"],
  theme: {
    fontFamily: {
      sans: ['Sofia Sans', 'sans-serif'],
      serif: ['Libertine', 'serif'],
      mono: ['ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace', 'mono']
    },
    fontSize: {
      xs: ['0.7rem', { lineHeight: '1rem' }],
      sm: ['0.85rem', { lineHeight: '1.25rem' }],
      base: ['1rem', { lineHeight: '1.5rem' }],
      lg: ['1.15rem', { lineHeight: '1.75rem' }],
      xl: ['1.25rem', { lineHeight: '1.75rem' }],
      '2xl': ['1.5rem', { lineHeight: '2rem' }],
      '3xl': ['1.875rem', { lineHeight: '2.25rem' }],
      '4xl': ['2.25rem', { lineHeight: '2.5rem' }],
      '5xl': ['3rem', { lineHeight: '1' }],
      '6xl': ['3.75rem', { lineHeight: '1' }],
      '7xl': ['4.5rem', { lineHeight: '1' }],
      '8xl': ['6rem', { lineHeight: '1' }],
      '9xl': ['8rem', { lineHeight: '1' }],
    },
    screens: {
      'sm': '700px',
      'md': '940px',
      'desktop': '1190px',
      'xl': '1440px',
      '2xl': '1680px',    
    },
    extend: {
      screens: {
        'print': { 'raw': 'print' },
      },
      colors: {
        'lenz-12-lighterblue': '#1f4060',
        'lenz-11-blue': '#13213f',
        'lenz-10-darkblue': '#110a25',
        'lenz-9-winered': '#290b22',
        'lenz-8-darkred': '#491122',
        'lenz-7-red': '#711824',
        'lenz-6-highlight': '#9b1e24',
        'lenz-5-pink': '#bc3047',
        'lenz-4-rose': '#c95873',
        'lenz-3-lightrose': '#cf899d',
        'lenz-2-lightred': '#dab5c1',
        'lenz-1-greyred': '#eadce1',
      }
    },
  },
  plugins: [],
}
