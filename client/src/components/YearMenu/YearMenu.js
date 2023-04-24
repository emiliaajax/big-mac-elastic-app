import { useEffect, useState } from 'react'
import './YearMenu.css'
import { useDispatch } from 'react-redux'
import { getTopCheapestCountries, getTopExpensiveCountries } from '../../features/bigMacPrices/pricesSlice.js'

function YearMenu({ cheapest }) {
  const dispatch = useDispatch()

  const [startYear, setStartYear] = useState('2000')
  const [endYear, setEndYear] = useState('2022')
  const [limit, setLimit] = useState(10)

  const years = Array.from({length: 23}, (_, i) => 2000 + i)
  const noOfCountries = Array.from({length: 18}, (_, i) => 3 + i)

  useEffect(() => {
    if (cheapest) {
      dispatch(getTopCheapestCountries({limit, startYear, endYear}))
    } else {
      dispatch(getTopExpensiveCountries({limit, startYear, endYear}))
    }
  }, [dispatch, cheapest, limit, startYear, endYear])

  const handleStartYearChange = (event) => {
    const newStartYear = event.target.value
    setStartYear(event.target.value)
    if (cheapest) {
      dispatch(getTopCheapestCountries({limit: 10, startYear: newStartYear, endYear}))
    } else {
      dispatch(getTopExpensiveCountries({limit: 10, startYear: newStartYear, endYear}))
    }
  }

  const handleEndYearChange = (event) => {
    const newEndYear = event.target.value
    setEndYear(newEndYear)
    if (cheapest) {
      dispatch(getTopCheapestCountries({limit: 10, startYear, endYear: newEndYear}))
    } else {
      dispatch(getTopExpensiveCountries({limit: 10, startYear, endYear: newEndYear}))
    }
  }

  const handleLimitChange = (event) => {
    const newLimit = event.target.value
    setLimit(newLimit)
    if (cheapest) {
      dispatch(getTopCheapestCountries({limit: newLimit, startYear, endYear}))
    } else {
      dispatch(getTopExpensiveCountries({limit: newLimit, startYear, endYear}))
    }
  }

  return (
    <>
      <div id='yearMenu'>
        <div>
          <h3 className='yearMenuTitle'>Number of countries</h3>
          <select className='endYearMenu' value={limit} onChange={handleLimitChange}>
            <option value="">Number</option>
            {noOfCountries.map((number) => (
              <option key={number} value={number}>{number}</option>
            ))}
          </select>
        </div>
        <div>
          <h3 className='yearMenuTitle'>From</h3>
          <select className='startYearMenu' value={startYear} onChange={handleStartYearChange}>
            <option value="">Select a year</option>
            {years.map((year) => (
              <option key={year} value={year}>{year}</option>
            ))}
          </select>
        </div>
        <div>
          <h3 className='yearMenuTitle'>To</h3>
          <select className='endYearMenu' value={endYear} onChange={handleEndYearChange}>
            <option value="">Select a year</option>
            {years.map((year) => (
              <option key={year} value={year}>{year}</option>
            ))}
          </select>
        </div>
      </div>
    </>
  )
}

export default YearMenu
