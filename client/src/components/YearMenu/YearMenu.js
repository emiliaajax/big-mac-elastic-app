import { useEffect, useState } from 'react'
import './YearMenu.css'
import { useDispatch } from 'react-redux'
import { getTopCheapestCountries } from '../../features/bigMacPrices/pricesSlice.js'

function YearMenu() {
  const dispatch = useDispatch()

  const [startYear, setStartYear] = useState('2000')
  const [endYear, setEndYear] = useState('2022')

  const years = Array.from({length: 23}, (_, i) => 2000 + i)

  useEffect(() => {
    dispatch(getTopCheapestCountries({limit: 10, startYear, endYear}))
  }, [dispatch])

  const handleStartYearChange = (event) => {
    const newStartYear = event.target.value
    setStartYear(event.target.value)
    dispatch(getTopCheapestCountries({limit: 10, startYear: newStartYear, endYear}))
  }

  const handleEndYearChange = (event) => {
    const newEndYear = event.target.value
    setEndYear(newEndYear)
    dispatch(getTopCheapestCountries({limit: 10, startYear, endYear: newEndYear}))
  }

  return (
    <>
      <div id='yearMenu'>
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
