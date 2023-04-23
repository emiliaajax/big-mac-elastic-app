import { Link, useLocation } from 'react-router-dom'
import './TopBar.css'
import { useDispatch, useSelector } from 'react-redux'
import { useEffect, useState } from 'react'
import { getCountries } from '../../features/bigMacPrices/pricesSlice.js'

function TopBar () {
  const location = useLocation()
  const dispatch = useDispatch()

  const { countries } = useSelector((state) => state.prices)
  
  const [ showDropdownMenu, setShowDropdownMenu ] = useState(false)

  const onDrowdownMenuClick = () => {
    setShowDropdownMenu(!showDropdownMenu)
  }

  useEffect(() => {
    dispatch(getCountries())
  }, [dispatch])

  return (
    <>
      <div className='topBarContainer'>
        <div className='topBar'>
          <div>
            <Link to='/' style={{ color: location.pathname === '/' && !showDropdownMenu ? '#FFFFFF' : '#838383' }}>
              Global
            </Link>
          </div>
          <div onClick={onDrowdownMenuClick}>
            <span style={{ color: showDropdownMenu ? '#FFFFFF' : '#838383' }}>
              Country
            </span>
            { showDropdownMenu
            ? <div
              style={{
                position: 'absolute',
                left: '13%',
                backgroundColor: '#141414',
                height: '610px',
                width: '120px',
                borderRadius: '5px',
                overflow: 'scroll',
                zIndex: 1,
                boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.1)',
              }}
              >
                {countries?.map((country, index) => (
                    <Link
                      to={`/${country.endpoint}`}
                      key={index}
                      style={{
                        display: 'block',
                        padding: '10px',
                        color: '#ffffff'
                      }}
                    >
                      {country.name}
                  </Link>
                ))}
              </div>
            : <></>}
          </div>
          <div>
            <Link to='/most-expensive' style={{ color: location.pathname === '/discover' ? '#FFFFFF' : '#838383' }}>
              Most Expensive
            </Link>
          </div>
          <div>
            <Link to='/cheapest' style={{ color: location.pathname === '/discover' ? '#FFFFFF' : '#838383' }}>
              Cheapest
            </Link>
          </div>
        </div>
      </div>
    </> 
  )
}

export default TopBar
