import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getPrices } from '../features/bigMacPrices/pricesSlice.js'
import LinePlot from '../components/LinePlot.js'

function Home() {
  const dispatch = useDispatch()
  const { prices } = useSelector((state) => state.prices)

  useEffect(() => {
    dispatch(getPrices())
  }, [dispatch])

  return (
    <>
      <LinePlot data={prices} xAxisProperty="timeStamp" yAxisProperty="dollarPrice" lineName="Price" yAxisName="USD"/>
    </>
  )
}

export default Home
