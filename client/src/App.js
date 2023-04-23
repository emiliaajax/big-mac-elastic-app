import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Home from './pages/Home.js'
import BaseLayout from './components/BaseLayout/BaseLayout.js'

function App() {
  return (
    <>
      <Router>
        <div className="App">
          <Routes>
            <Route
              element={<BaseLayout><Home /></BaseLayout>}
              path='/'
            />
          </Routes>
        </div>
      </Router>
    </>
  )
}

export default App
