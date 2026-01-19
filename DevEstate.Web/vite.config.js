import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'node:path' // 👈 to dodaj

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'), // 👈 alias działa teraz poprawnie
    },
  },
  server: {
    port: 5173,
    open: true,
    proxy: {
      '/api': {
        target: 'http://localhost:5086',
        changeOrigin: true,
        secure: false,
      },
      "/uploads": {
        target: "http://localhost:5086",
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
