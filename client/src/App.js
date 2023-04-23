import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Home from './pages/Home.js'

function App() {
  return (
    <>
      <Router>
        <div className="App">
          <Routes>
            <Route
              element={<Home />}
              path='/'
            />
          </Routes>
        </div>
      </Router>
    </>
  )
}

export default App
