import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getTopExpensiveCountries } from '../../features/bigMacPrices/pricesSlice.js'
import BoxPlot from '../../components/BoxPlot/BoxPlot.js'

function TopExpensiveCountries () {
  const dispatch = useDispatch()

  const { expensiveCountries } = useSelector((state) => state.prices)

  useEffect(() => {
    dispatch(getTopExpensiveCountries())
  }, [dispatch])

  return ( 
    <>
      <BoxPlot data={expensiveCountries} xAxisPropertyName="name" barPropertyName="dollarPrice"/>
    </>
  )
}

export default TopExpensiveCountries
