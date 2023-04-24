import axios from 'axios'

const API_URL = process.env.REACT_APP_BIG_MAC_PRICES_API

const getPrices = async () => {
  const response = await axios.get(API_URL)

  return response.data
}

const getPricesForCountry = async (country) => {
  const response = await axios.get(`${API_URL}/countries/${country}`)

  return response.data
}

const getCountries = async () => {
  const response = await axios.get(`${API_URL}/countries`)

  return response.data
}

const getTopExpensiveCountries = async () => {
  const response = await axios.get(`${API_URL}/top-expensive`)

  return response.data
}

const getTopCheapestCountries = async (limit, startYear, endYear) => {
  const response = await axios.get(
    `${API_URL}/top-cheapest?limit=${limit}&start-year=${startYear}&end-year=${endYear}`)

  return response.data
}

const pricesService = {
  getPrices,
  getPricesForCountry,
  getCountries,
  getTopExpensiveCountries,
  getTopCheapestCountries
}

export default pricesService
