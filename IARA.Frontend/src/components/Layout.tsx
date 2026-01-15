import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { authService } from '../services/authService';
import './Layout.css';

export const Layout = ({ children }: { children: React.ReactNode }) => {
  const navigate = useNavigate();
  const userRole = localStorage.getItem('userRole');
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [activeDropdown, setActiveDropdown] = useState<string | null>(null);

  const handleLogout = async () => {
    try {
      await authService.logout();
    } catch (err) {
      console.error('Logout error:', err);
    } finally {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('userRole');
      navigate('/login');
    }
  };

  const toggleMobileMenu = () => {
    setMobileMenuOpen(!mobileMenuOpen);
  };

  const toggleDropdown = (dropdown: string) => {
    setActiveDropdown(activeDropdown === dropdown ? null : dropdown);
  };

  const hasAccess = (allowedRoles: string[]) => {
    return !userRole || allowedRoles.includes(userRole);
  };

  const isAdministrator = userRole === 'Administrator';

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="navbar-brand">
          <Link to="/dashboard">IARA System</Link>
          {isAdministrator && (
            <button className="mobile-menu-toggle" onClick={toggleMobileMenu}>
              ☰
            </button>
          )}
        </div>
        {isAdministrator && (
          <div className={`navbar-menu ${mobileMenuOpen ? 'mobile-open' : ''}`}>
          {hasAccess(['Administrator', 'ShipOwner', 'Inspector']) && (
            <div className={`navbar-dropdown ${activeDropdown === 'commercial' ? 'mobile-open' : ''}`}>
              <span className="navbar-link" onClick={() => toggleDropdown('commercial')}>
                Commercial Fishing
              </span>
              <div className="navbar-dropdown-content">
                <Link to="/ships" onClick={() => setMobileMenuOpen(false)}>Ships</Link>
                <Link to="/fishing-permits" onClick={() => setMobileMenuOpen(false)}>Fishing Permits</Link>
                <Link to="/fishing-trips" onClick={() => setMobileMenuOpen(false)}>Fishing Trips</Link>
                <Link to="/landings" onClick={() => setMobileMenuOpen(false)}>Landings</Link>
              </div>
            </div>
          )}
          
          {hasAccess(['Administrator', 'Inspector']) && (
            <div className={`navbar-dropdown ${activeDropdown === 'inspections' ? 'mobile-open' : ''}`}>
              <span className="navbar-link" onClick={() => toggleDropdown('inspections')}>
                Inspections
              </span>
              <div className="navbar-dropdown-content">
                <Link to="/inspections" onClick={() => setMobileMenuOpen(false)}>Inspections</Link>
                <Link to="/violations" onClick={() => setMobileMenuOpen(false)}>Violations</Link>
              </div>
            </div>
          )}

          {hasAccess(['Administrator', 'Inspector', 'RecreationalFisherman']) && (
            <div className={`navbar-dropdown ${activeDropdown === 'recreational' ? 'mobile-open' : ''}`}>
              <span className="navbar-link" onClick={() => toggleDropdown('recreational')}>
                Recreational Fishing
              </span>
              <div className="navbar-dropdown-content">
                <Link to="/recreational-fishermen" onClick={() => setMobileMenuOpen(false)}>Fishermen</Link>
                <Link to="/recreational-tickets" onClick={() => setMobileMenuOpen(false)}>Tickets</Link>
              </div>
            </div>
          )}

          {hasAccess(['Administrator', 'Inspector']) && (
            <div className={`navbar-dropdown ${activeDropdown === 'registry' ? 'mobile-open' : ''}`}>
              <span className="navbar-link" onClick={() => toggleDropdown('registry')}>
                Registry
              </span>
              <div className="navbar-dropdown-content">
                <Link to="/persons" onClick={() => setMobileMenuOpen(false)}>Persons</Link>
                <Link to="/legal-entities" onClick={() => setMobileMenuOpen(false)}>Legal Entities</Link>
                <Link to="/inspectors" onClick={() => setMobileMenuOpen(false)}>Inspectors</Link>
                <Link to="/ship-owners" onClick={() => setMobileMenuOpen(false)}>Ship Owners</Link>
              </div>
            </div>
          )}

          <div className="navbar-end">
            <span className="user-role">Role: {userRole || 'User'}</span>
            <button onClick={handleLogout} className="logout-button">Logout</button>
          </div>
        </div>
        )}
        {!isAdministrator && (
          <div className="navbar-end-simple">
            <span className="user-role">Role: {userRole || 'User'}</span>
            <button onClick={handleLogout} className="logout-button">Logout</button>
          </div>
        )}
      </nav>
      <main className="main-content">
        {children}
      </main>
    </div>
  );
};
