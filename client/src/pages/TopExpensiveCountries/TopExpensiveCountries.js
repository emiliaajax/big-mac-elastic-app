import { useSelector } from 'react-redux'
import BoxPlot from '../../components/BoxPlot/BoxPlot.js'
import YearMenu from '../../components/YearMenu/YearMenu.js'

function TopExpensiveCountries () {
  const { expensiveCountries } = useSelector((state) => state.prices)

  return ( 
    <div className='topCountries'>
      <YearMenu />
      <div>
        <h1 className='pricesTitle'>Top Most Expensive Countries To Buy a Big Mac</h1>
        <BoxPlot data={expensiveCountries} xAxisPropertyName="name" barPropertyName="dollarPrice"/>
      </div>
    </div>
  )
}

export default TopExpensiveCountries
