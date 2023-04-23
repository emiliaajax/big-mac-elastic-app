import { configureStore } from '@reduxjs/toolkit'
import pricesReducer from '../features/bigMacPrices/pricesSlice.js'

export const store = configureStore({
  reducer: {
    movies: pricesReducer
  }
})
