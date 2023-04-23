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

const getMostExpensiveCountries = async () => {
  const response = await axios.get(`${API_URL}/most-expensive`)

  return response.data
}

const getCheapestCountries = async () => {
  const response = await axios.get(`${API_URL}/cheapest`)

  return response.data
}

const pricesService = {
  getPrices,
  getPricesForCountry,
  getCountries,
  getMostExpensiveCountries,
  getCheapestCountries
}

export default pricesService
