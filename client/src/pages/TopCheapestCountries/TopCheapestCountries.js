import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import BoxPlot from '../../components/BoxPlot/BoxPlot.js'
import { getTopCheapestCountries } from '../../features/bigMacPrices/pricesSlice.js'
import YearMenu from '../../components/YearMenu/YearMenu.js'
import './TopCheapestCountries.css'

function TopCheapestCountries () {
  const dispatch = useDispatch()

  const { cheapestCountries } = useSelector((state) => state.prices)

  useEffect(() => {console.log(cheapestCountries)}, [cheapestCountries])

  return ( 
    <div className='topCountries'>
      <YearMenu />
      <div>
        <h1 className='pricesTitle'>Where to go to buy yourself a Big Mac</h1>
        <BoxPlot data={cheapestCountries} xAxisPropertyName="name" barPropertyName="dollarPrice"/>
      </div>
    </div>
  )
}

export default TopCheapestCountries
