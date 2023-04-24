import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import pricesService from './pricesService.js'

export const initialState = {
  isError: false,
  isSuccess: false,
  isPending: false,
  message: ''
}

export const getPrices = createAsyncThunk('prices', async (thunkAPI) => {
  try {
    return await pricesService.getPrices()
  } catch (error) {
    const message = error.response.data.message 
      || (error.response && error.response.data && error.response.message) 
      || error.message 
      || error.toString()
    return thunkAPI.rejectWithValue(message)
  }
})

export const getPricesForCountry = createAsyncThunk('prices/countries', async (country, thunkAPI) => {
  try {
    return await pricesService.getPricesForCountry(country)
  } catch (error) {
    const message = error.response.data.message 
      || (error.response && error.response.data && error.response.message) 
      || error.message 
      || error.toString()
    return thunkAPI.rejectWithValue(message)
  }
})

export const getCountries = createAsyncThunk('countries', async (thunkAPI) => {
  try {
    return await pricesService.getCountries()
  } catch (error) {
    const message = error.response.data.message 
      || (error.response && error.response.data && error.response.message) 
      || error.message 
      || error.toString()
    return thunkAPI.rejectWithValue(message)
  }
})

export const getTopExpensiveCountries = createAsyncThunk('countries/most-expensive', async ({limit, startYear, endYear}, thunkAPI) => {
  try {
    return await pricesService.getTopExpensiveCountries(limit, startYear, endYear)
  } catch (error) {
    const message = error.response.data.message 
      || (error.response && error.response.data && error.response.message) 
      || error.message 
      || error.toString()
    return thunkAPI.rejectWithValue(message)
  }
})

export const getTopCheapestCountries = createAsyncThunk('countries/cheapest', async ({limit, startYear, endYear}, thunkAPI) => {
  try {
    return await pricesService.getTopCheapestCountries(limit, startYear, endYear)
  } catch (error) {
    const message = error.response.data.message 
      || (error.response && error.response.data && error.response.message) 
      || error.message 
      || error.toString()
    return thunkAPI.rejectWithValue(message)
  }
})

export const pricesSlice = createSlice({
  name: 'prices',
  initialState,
  reducers: {
    reset: (state) => {
      state.isError = false
      state.isSuccess = false
      state.isPending = false
      state.message = ''
      state.prices = null
      state.pricesForCountry = null
      state.countries = null
      state.expensiveCountries = null
      state.cheapestCountries = null
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(getPrices.fulfilled, (state, action) => {
        state.isError = false
        state.isSuccess = true
        state.isPending = false
        state.prices = action.payload
      })
      .addCase(getPrices.rejected, (state, action) => {
        state.isError = true
        state.isSuccess = false
        state.isPending = false
        state.prices = null
      })
      .addCase(getPrices.pending, (state, action) => {
        state.isPending = true
      })
      .addCase(getPricesForCountry.fulfilled, (state, action) => {
        state.isError = false
        state.isSuccess = true
        state.isPending = false
        state.pricesForCountry = action.payload
      })
      .addCase(getPricesForCountry.rejected, (state, action) => {
        state.isError = true
        state.isSuccess = false
        state.isPending = false
        state.pricesForCountry = null
      })
      .addCase(getPricesForCountry.pending, (state, action) => {
        state.isPending = true
      })
      .addCase(getCountries.fulfilled, (state, action) => {
        state.isError = false
        state.isSuccess = true
        state.isPending = false
        state.countries = action.payload
      })
      .addCase(getCountries.rejected, (state, action) => {
        state.isError = true
        state.isSuccess = false
        state.isPending = false
        state.countries = null
      })
      .addCase(getCountries.pending, (state, action) => {
        state.isPending = true
      })
      .addCase(getTopExpensiveCountries.fulfilled, (state, action) => {
        state.isError = false
        state.isSuccess = true
        state.isPending = false
        state.expensiveCountries = action.payload
      })
      .addCase(getTopExpensiveCountries.rejected, (state, action) => {
        state.isError = true
        state.isSuccess = false
        state.isPending = false
        state.expensiveCountries = null
      })
      .addCase(getTopExpensiveCountries.pending, (state, action) => {
        state.isPending = true
      })
      .addCase(getTopCheapestCountries.fulfilled, (state, action) => {
        state.isError = false
        state.isSuccess = true
        state.isPending = false
        state.cheapestCountries = action.payload
      })
      .addCase(getTopCheapestCountries.rejected, (state, action) => {
        state.isError = true
        state.isSuccess = false
        state.isPending = false
        state.cheapestCountries = null
      })
      .addCase(getTopCheapestCountries.pending, (state, action) => {
        state.isPending = true
      })
  }
})

export const { reset } = pricesSlice.actions
export default pricesSlice.reducer
