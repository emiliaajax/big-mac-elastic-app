import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import BoxPlot from '../../components/BoxPlot/BoxPlot.js'
import { getTopCheapestCountries } from '../../features/bigMacPrices/pricesSlice.js'

function TopCheapestCountries () {
  const dispatch = useDispatch()

  const { cheapestCountries } = useSelector((state) => state.prices)

  useEffect(() => {
    dispatch(getTopCheapestCountries())
  }, [dispatch])

  return ( 
    <>
      <BoxPlot data={cheapestCountries} xAxisPropertyName="name" barPropertyName="dollarPrice"/>
    </>
  )
}

export default TopCheapestCountries
