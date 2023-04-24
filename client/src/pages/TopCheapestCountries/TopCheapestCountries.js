import { useSelector } from 'react-redux'
import BoxPlot from '../../components/BoxPlot/BoxPlot.js'
import YearMenu from '../../components/YearMenu/YearMenu.js'
import './TopCheapestCountries.css'

function TopCheapestCountries () {
  const { cheapestCountries } = useSelector((state) => state.prices)

  return ( 
    <div className='topCountries'>
      <YearMenu cheapest={true} />
      <div>
        <h1 className='pricesTitle'>Where to go to buy yourself a Big Mac</h1>
        <BoxPlot data={cheapestCountries} xAxisPropertyName="name" barPropertyName="dollarPrice"/>
      </div>
    </div>
  )
}

export default TopCheapestCountries
