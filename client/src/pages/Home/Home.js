import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { getPrices } from '../../features/bigMacPrices/pricesSlice.js'
import LinePlot from '../../components/LinePlot/LinePlot.js'

function Home() {
  const dispatch = useDispatch()
  const { prices } = useSelector((state) => state.prices)

  useEffect(() => {
    dispatch(getPrices())
  }, [dispatch])

  return (
    <>
      <h1 className='pricesTitle'>Big Mac Prices 2000-2022</h1>
      <LinePlot data={prices} xAxisProperty="timeStamp" yAxisProperty="dollarPrice" lineName="Price" yAxisName="USD"/>
    </>
  )
}

export default Home
