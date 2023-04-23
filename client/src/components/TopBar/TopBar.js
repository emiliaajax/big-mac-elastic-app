import { Link, useLocation } from 'react-router-dom'
import './TopBar.css'

function TopBar () {
  const location = useLocation()

  return (
    <>
      <div className='topBarContainer'>
        <div className='topBar'>
          <div>
            <Link to='/' style={{ color: location.pathname === '/' ? '#FFFFFF' : '#838383' }}>
              Global
            </Link>
          </div>
          <div>
            <Link to='/discover' style={{ color: location.pathname === '/discover' ? '#FFFFFF' : '#838383' }}>
              Country
            </Link>
          </div>
          <div>
            <Link to='/discover' style={{ color: location.pathname === '/discover' ? '#FFFFFF' : '#838383' }}>
              Most Expensive
            </Link>
          </div>
          <div>
            <Link to='/discover' style={{ color: location.pathname === '/discover' ? '#FFFFFF' : '#838383' }}>
              Cheapest
            </Link>
          </div>
        </div>
      </div>
    </> 
  )
}

export default TopBar
