import React, { useState, useEffect } from 'react';
import { HashRouter as Router, Routes, Route, Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Container, Button, Modal } from 'react-bootstrap';
import ScriptList from './components/ScriptList';
import CharacterDetails from './components/CharacterDetail';
import LoginForm from './components/Login';
import CommentsList from './components/CommentList';  // Correct if default export
import ProtectedRoute from './components/ProtectedRoute';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import Footer from './components/Footer';
import About from './components/about'; // Import the About component
import { ReactSVG } from 'react-svg';

function App() {
  const [authToken, setAuthToken] = useState(localStorage.getItem('accessToken'));
  const [role, setRole] = useState(localStorage.getItem('role'));
  const [showLoginModal, setShowLoginModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    const userRole = localStorage.getItem('role');
    if (token) {
      setAuthToken(token);
      setRole(userRole);
    }
  }, []);

  const handleLogout = async () => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      await fetch('api/logout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ RefreshToken: refreshToken }),
      });

      localStorage.removeItem('accessToken');
      localStorage.removeItem('role');
      localStorage.removeItem('refreshToken');
      setAuthToken(null);
      setRole(null);

      window.location.reload();

    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  const handleCloseLoginModal = () => setShowLoginModal(false);
  const handleShowLoginModal = () => setShowLoginModal(true);

  return (
    <div className="App d-flex flex-column min-vh-100">
      {/* Navbar */}
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand href="/KTU_Saitynai-BloodOnTheClocktower/">Blood on the Clocktower</Navbar.Brand>
          <Navbar.Toggle aria-controls="navbar-nav" />
          <Navbar.Collapse id="navbar-nav">
            <Nav className="ml-auto">
              <Nav.Item>
                <Nav.Link as={Link} to="/about">About</Nav.Link>
              </Nav.Item>
              {authToken ? (
                <Nav.Item>
                  <Nav.Link onClick={handleLogout}>Logout</Nav.Link>
                </Nav.Item>
              ) : (
                <Nav.Item>
                  <Nav.Link onClick={handleShowLoginModal}>Login</Nav.Link>
                </Nav.Item>
              )}
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>

      {/* Routes */}
      //<div className="flex-grow-1">
      //  <Routes>
      //    <Route path="/KTU_Saitynai-BloodOnTheClocktower/" element={<ScriptList />} />
      //    <Route path="/KTU_Saitynai-BloodOnTheClocktower/scripts/:scriptId/characters/:characterId" element={<CharacterDetails />} />
      //    <Route path="/KTU_Saitynai-BloodOnTheClocktower/about" element={<About />} /> {/* Use the About component */}
      //    <Route path="/KTU_Saitynai-BloodOnTheClocktower/scripts" element={<ProtectedRoute element={<ScriptList />} authToken={authToken} />} />
      //  </Routes>
      //</div>
	  <Router>
      <div className="flex-grow-1">
        <Routes>
          <Route path="/" element={<ScriptList />} />
          <Route path="/scripts/:scriptId/characters/:characterId" element={<CharacterDetails />} />
          <Route path="/about" element={<About />} />
          <Route
            path="/scripts"
            element={<ProtectedRoute element={<ScriptList />} authToken={authToken} />}
          />
        </Routes>
      </div>
    </Router>

      {/* Login Modal */}
      <Modal show={showLoginModal} onHide={handleCloseLoginModal}>
        <Modal.Header closeButton>
          <Modal.Title>Login/Register</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <LoginForm setAuthToken={setAuthToken} handleCloseModal={handleCloseLoginModal} />
        </Modal.Body>
      </Modal>

      {/* Footer */}
      <Footer />
    </div>
  );
}

export default App;