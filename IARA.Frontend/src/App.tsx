import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { Dashboard } from './pages/Dashboard';
import { AdministratorDashboard } from './pages/Administrator/AdministratorDashboard';
import { InspectorDashboard } from './pages/Inspector/InspectorDashboard';
import { ShipOwnerDashboard } from './pages/ShipOwner/ShipOwnerDashboard';
import { CaptainDashboard } from './pages/Captain/CaptainDashboard';
import { RecreationalFishermanDashboard } from './pages/RecreationalFisherman/RecreationalFishermanDashboard';
import { BuyTicket } from './pages/RecreationalFisherman/BuyTicket';
import { MyTickets } from './pages/RecreationalFisherman/MyTickets';
import { RecordCatch } from './pages/RecreationalFisherman/RecordCatch';
import { Ships } from './pages/CommercialFishing/Ships';
import { FishingPermits } from './pages/CommercialFishing/FishingPermits';
import { FishingTrips } from './pages/CommercialFishing/FishingTrips';
import { Crew } from './pages/CommercialFishing/Crew';
import { ShipEquipmentPage } from './pages/CommercialFishing/ShipEquipment';
import { Inspections } from './pages/Inspections/Inspections';
import { Violations } from './pages/Inspections/Violations';
import { RecreationalFishermen } from './pages/RecreationalFishing/RecreationalFishermen';
import { RecreationalTickets } from './pages/RecreationalFishing/RecreationalTickets';
import { Persons } from './pages/Registry/Persons';
import { LegalEntities } from './pages/Registry/LegalEntities';
import { Layout } from './components/Layout';
import { ProtectedRoute } from './components/ProtectedRoute';
import './App.css';

function App() {
  const getRoleDashboard = () => {
    const role = localStorage.getItem('userRole');
    switch (role) {
      case 'Administrator':
        return <AdministratorDashboard />;
      case 'Inspector':
        return <InspectorDashboard />;
      case 'ShipOwner':
        return <ShipOwnerDashboard />;
      case 'Captain':
        return <CaptainDashboard />;
      case 'RecreationalFisherman':
        return <RecreationalFishermanDashboard />;
      default:
        return <Dashboard />;
    }
  };

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Layout>
                {getRoleDashboard()}
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin"
          element={
            <ProtectedRoute>
              <Layout>
                <AdministratorDashboard />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/inspector"
          element={
            <ProtectedRoute>
              <Layout>
                <InspectorDashboard />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/ship-owner"
          element={
            <ProtectedRoute>
              <Layout>
                <ShipOwnerDashboard />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/captain"
          element={
            <ProtectedRoute>
              <Layout>
                <CaptainDashboard />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/recreational"
          element={
            <ProtectedRoute>
              <Layout>
                <RecreationalFishermanDashboard />
              </Layout>
            </ProtectedRoute>
          }
        />
        
        {/* Captain Routes */}
        <Route
          path="/captain/ships"
          element={
            <ProtectedRoute>
              <Layout>
                <Ships />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/captain/fishing-trips"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingTrips />
              </Layout>
            </ProtectedRoute>
          }
        />
        
        {/* Commercial Fishing Routes */}
        <Route
          path="/commercial/ships"
          element={
            <ProtectedRoute>
              <Layout>
                <Ships />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/commercial/permits"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingPermits />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/commercial/fishing-trips"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingTrips />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/commercial/equipment"
          element={
            <ProtectedRoute>
              <Layout>
                <ShipEquipmentPage />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/crew"
          element={
            <ProtectedRoute>
              <Layout>
                <Crew />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/commercial/crew"
          element={
            <ProtectedRoute>
              <Layout>
                <Crew />
              </Layout>
            </ProtectedRoute>
          }
        />
        
        {/* Legacy Routes (kept for backward compatibility) */}
        <Route
          path="/captain/landings"
          element={
            <ProtectedRoute>
              <Layout>
                <div className="page-container">
                  <h1>Landings (Coming Soon)</h1>
                </div>
              </Layout>
            </ProtectedRoute>
          }
        />
        
        {/* Ship Owner Routes */}
        <Route
          path="/shipowner/ships"
          element={
            <ProtectedRoute>
              <Layout>
                <Ships />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/shipowner/permits"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingPermits />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/shipowner/crew"
          element={
            <ProtectedRoute>
              <Layout>
                <Crew />
              </Layout>
            </ProtectedRoute>
          }
        />        <Route
          path="/shipowner/equipment"
          element={
            <ProtectedRoute>
              <Layout>
                <ShipEquipmentPage />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/shipowner/fishing-trips"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingTrips />
              </Layout>
            </ProtectedRoute>
          }
        />        
        {/* Recreational Fisherman Routes */}
        <Route
          path="/recreational/buy-ticket"
          element={
            <ProtectedRoute>
              <Layout>
                <BuyTicket />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/recreational/my-tickets"
          element={
            <ProtectedRoute>
              <Layout>
                <MyTickets />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/recreational/record-catch"
          element={
            <ProtectedRoute>
              <Layout>
                <RecordCatch />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/ships"
          element={
            <ProtectedRoute>
              <Layout>
                <Ships />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/fishing-permits"
          element={
            <ProtectedRoute>
              <Layout>
                <FishingPermits />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/fishing-trips"
          element={
            <ProtectedRoute>
              <Layout>
                <div className="page-container">
                  <h1>Fishing Trips (Coming Soon)</h1>
                </div>
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/landings"
          element={
            <ProtectedRoute>
              <Layout>
                <div className="page-container">
                  <h1>Landings (Coming Soon)</h1>
                </div>
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/inspections"
          element={
            <ProtectedRoute>
              <Layout>
                <Inspections />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/violations"
          element={
            <ProtectedRoute>
              <Layout>
                <Violations />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/recreational-fishermen"
          element={
            <ProtectedRoute>
              <Layout>
                <RecreationalFishermen />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/recreational-tickets"
          element={
            <ProtectedRoute>
              <Layout>
                <RecreationalTickets />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/persons"
          element={
            <ProtectedRoute>
              <Layout>
                <Persons />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/legal-entities"
          element={
            <ProtectedRoute>
              <Layout>
                <LegalEntities />
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/inspectors"
          element={
            <ProtectedRoute>
              <Layout>
                <div className="page-container">
                  <h1>Inspectors (Coming Soon)</h1>
                </div>
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route
          path="/ship-owners"
          element={
            <ProtectedRoute>
              <Layout>
                <div className="page-container">
                  <h1>Ship Owners (Coming Soon)</h1>
                </div>
              </Layout>
            </ProtectedRoute>
          }
        />
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </Router>
  );
}

export default App;
