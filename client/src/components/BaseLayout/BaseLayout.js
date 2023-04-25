import TopBar from '../TopBar/TopBar.js'
import './BaseLayout.css'

/**
 * BaseLayout component.
 * 
 * @param {React.ReactElement} props The React element to insert. 
 * @returns {React.ReactElement} BaseLayout component.
 */
function BaseLayout (props) {
  const { children } = props ? props : null

  return ( 
    <>
      <div className='layoutContainer'>
        <div id='topbar'>
          <TopBar />
        </div>
        <div id='children'>
          {children}
        </div>
      </div>
    </>
  )
}

export default BaseLayout
