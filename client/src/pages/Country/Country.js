import { useDispatch, useSelector } from 'react-redux'
import { getPricesForCountry } from '../../features/bigMacPrices/pricesSlice.js'
import { useEffect } from 'react'
import LinePlot from '../../components/LinePlot/LinePlot.js'
import { useParams } from 'react-router-dom'
import './Country.css'

/**
 * The Country component.
 *
 * @returns {React.ReactElement} Country component.
 */
function Country () {
  const dispatch = useDispatch()

  const { country } = useParams()
  const { pricesForCountry } = useSelector((state) => state.prices)

  useEffect(() => {
    dispatch(getPricesForCountry(country))
  }, [dispatch, country])

  return (
    <>
      { pricesForCountry
      ? <><h1 className='pricesTitle'>{pricesForCountry[0].name}</h1>
      <LinePlot data={pricesForCountry} xAxisProperty="timeStamp" yAxisProperty="localPrice" lineName="Price" yAxisName={pricesForCountry[0].currencyCode}/></>
      : <></>
      }
    </>
  )
}

export default Country
