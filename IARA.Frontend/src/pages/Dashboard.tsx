import { Link, Navigate } from 'react-router-dom';

export const Dashboard = () => {
  const userRole = localStorage.getItem('userRole');

  // Recreational fishermen should not see this dashboard
  if (userRole === 'RecreationalFisherman') {
    return <Navigate to="/recreational" replace />;
  }

  return (
    <div className="dashboard">
      <h1>IARA Dashboard</h1>
      <p>Welcome to the IARA Information System</p>
      <p>Role: {userRole}</p>

      <div className="dashboard-grid">
        {(userRole === 'Administrator' || userRole === 'ShipOwner' || userRole === 'Inspector') && (
          <div className="module-section">
            <h2>Commercial Fishing</h2>
            <div className="module-links">
              <Link to="/ships" className="module-card">Ships</Link>
              <Link to="/fishing-permits" className="module-card">Fishing Permits</Link>
              <Link to="/fishing-trips" className="module-card">Fishing Trips</Link>
              <Link to="/landings" className="module-card">Landings</Link>
            </div>
          </div>
        )}

        {(userRole === 'Administrator' || userRole === 'Inspector') && (
          <div className="module-section">
            <h2>Inspections</h2>
            <div className="module-links">
              <Link to="/inspections" className="module-card">Inspections</Link>
              <Link to="/violations" className="module-card">Violations</Link>
            </div>
          </div>
        )}

        {(userRole === 'Administrator' || userRole === 'Inspector') && (
          <div className="module-section">
            <h2>Recreational Fishing</h2>
            <div className="module-links">
              <Link to="/recreational-fishermen" className="module-card">Fishermen</Link>
              <Link to="/recreational-tickets" className="module-card">Tickets</Link>
            </div>
          </div>
        )}

        {(userRole === 'Administrator' || userRole === 'Inspector') && (
          <div className="module-section">
            <h2>Registry</h2>
            <div className="module-links">
              <Link to="/persons" className="module-card">Persons</Link>
              <Link to="/legal-entities" className="module-card">Legal Entities</Link>
              <Link to="/inspectors" className="module-card">Inspectors</Link>
              <Link to="/ship-owners" className="module-card">Ship Owners</Link>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
