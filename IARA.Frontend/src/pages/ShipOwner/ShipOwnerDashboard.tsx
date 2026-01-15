import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/common/Card';
import { StatCard } from '../../components/common/StatCard';
import { DataTable } from '../../components/common/DataTable';
import { shipService } from '../../services/shipService';
import { fishingTripService } from '../../services/fishingTripService';
import { fishingPermitService } from '../../services/fishingPermitService';
import { shipCrewService } from '../../services/shipCrewService';
import { shipEquipmentService } from '../../services/shipEquipmentService';
import type { Ship, FishingTrip, FishingPermit, ShipEquipment } from '../../types';
import './ShipOwnerDashboard.css';

export const ShipOwnerDashboard: React.FC = () => {
  const navigate = useNavigate();
  const [stats, setStats] = useState({
    totalShips: 0,
    activeTrips: 0,
    totalCrew: 0,
    activePermits: 0,
    totalEquipment: 0,
  });
  const [ships, setShips] = useState<Ship[]>([]);
  const [recentTrips, setRecentTrips] = useState<FishingTrip[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      // Load only the services that have backend endpoints
      const [shipsData, tripsData, permitsData, crewData] = await Promise.all([
        shipService.getAll({ page: 1, pageSize: 100 }),
        fishingTripService.getAll({ page: 1, pageSize: 20 }),
        fishingPermitService.getAll({ page: 1, pageSize: 100 }),
        shipCrewService.getAll({ page: 1, pageSize: 100 }),
      ]);

      // TODO: Add equipment service when backend endpoint is available
      let equipmentData: ShipEquipment[] = [];
      try {
        equipmentData = await shipEquipmentService.getAll({ page: 1, pageSize: 100 });
      } catch (error) {
        console.warn('Ship equipment endpoint not available yet:', error);
      }

      setStats({
        totalShips: shipsData.length,
        activeTrips: tripsData.filter((t: FishingTrip) => t.tripStatus === 'In Progress' || t.tripStatus === 'Active').length,
        totalCrew: crewData.length,
        activePermits: permitsData.filter((p: FishingPermit) => p.isActive).length,
        totalEquipment: equipmentData.filter((e: ShipEquipment) => e.isActive).length,
      });

      setShips(shipsData.slice(0, 5));
      setRecentTrips(tripsData.slice(0, 5));
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const shipColumns = [
    { header: 'Name', accessor: ((row: Ship) => row.name || row.externalMarking) },
    { header: 'External Marking', accessor: 'externalMarking' as keyof Ship },
    { header: 'Length', accessor: ((row: Ship) => row.length ? `${row.length}m` : '-') },
    { header: 'Tonnage', accessor: ((row: Ship) => row.grossTonnage ? `${row.grossTonnage}t` : '-') },
    {
      header: 'Actions',
      accessor: ((_row: Ship) => (
        <button 
          className="btn-view" 
          onClick={() => navigate(`/shipowner/ships`)}
        >
          View Details
        </button>
      )),
    },
  ];

  const tripColumns = [
    { header: 'Ship', accessor: 'shipName' as keyof FishingTrip },
    { header: 'Departure', accessor: ((row: FishingTrip) => new Date(row.departureDate).toLocaleDateString()) },
    { header: 'Port', accessor: ((row: FishingTrip) => row.departurePort || '-') },
    { 
      header: 'Status', 
      accessor: ((row: FishingTrip) => {
        const statusColors: Record<string, string> = {
          'Planned': '🟡',
          'In Progress': '🟢',
          'Active': '🟢',
          'Completed': '🔵',
          'Cancelled': '🔴',
        };
        return `${statusColors[row.tripStatus] || '⚪'} ${row.tripStatus}`;
      })
    },
  ];

  if (loading) {
    return <div className="dashboard-loading">Loading dashboard...</div>;
  }

  return (
    <div className="shipowner-dashboard">
      <div className="dashboard-header">
        <div>
          <h1>Fleet Management Dashboard</h1>
          <p>Welcome back! Here's an overview of your commercial fishing operations</p>
        </div>
        <button className="btn-new-trip" onClick={() => navigate('/shipowner/fishing-trips')}>
          ⛵ Start New Trip
        </button>
      </div>

      <div className="stats-grid">
        <div onClick={() => navigate('/shipowner/ships')}>
          <StatCard 
            title="My Fleet" 
            value={stats.totalShips} 
            icon="🚢" 
            color="blue"
          />
        </div>
        <div onClick={() => navigate('/shipowner/fishing-trips')}>
          <StatCard 
            title="Active Trips" 
            value={stats.activeTrips} 
            icon="🛥️" 
            color="green"
          />
        </div>
        <div onClick={() => navigate('/shipowner/crew')}>
          <StatCard 
            title="Crew Members" 
            value={stats.totalCrew} 
            icon="👥" 
            color="purple"
          />
        </div>
        <div onClick={() => navigate('/shipowner/permits')}>
          <StatCard 
            title="Active Permits" 
            value={stats.activePermits} 
            icon="📋" 
            color="yellow"
          />
        </div>
        <div onClick={() => navigate('/shipowner/equipment')}>
          <StatCard 
            title="Equipment" 
            value={stats.totalEquipment} 
            icon="⚓" 
            color="blue"
          />
        </div>
      </div>

      <div className="quick-actions-section">
        <h2>Quick Actions</h2>
        <div className="quick-actions-grid">
          <div className="action-card" onClick={() => navigate('/shipowner/ships')}>
            <div className="action-icon">🚢</div>
            <h3>Register New Ship</h3>
            <p>Add a new vessel to your fleet</p>
          </div>
          <div className="action-card" onClick={() => navigate('/shipowner/crew')}>
            <div className="action-icon">👤</div>
            <h3>Add Crew Member</h3>
            <p>Register new crew personnel</p>
          </div>
          <div className="action-card" onClick={() => navigate('/shipowner/permits')}>
            <div className="action-icon">📄</div>
            <h3>Apply for Permit</h3>
            <p>Request a new fishing permit</p>
          </div>
          <div className="action-card" onClick={() => navigate('/shipowner/equipment')}>
            <div className="action-icon">⚓</div>
            <h3>Add Equipment</h3>
            <p>Register fishing gear</p>
          </div>
        </div>
      </div>

      <div className="dashboard-content">
        <div className="content-section">
          <div className="section-header">
            <h2>My Fleet</h2>
            <button className="btn-view-all" onClick={() => navigate('/shipowner/ships')}>
              View All →
            </button>
          </div>
          <Card>
            <DataTable 
              data={ships} 
              columns={shipColumns} 
              emptyMessage="No ships registered yet. Register your first vessel!"
            />
          </Card>
        </div>

        <div className="content-section">
          <div className="section-header">
            <h2>Recent Fishing Trips</h2>
            <button className="btn-view-all" onClick={() => navigate('/shipowner/fishing-trips')}>
              View All →
            </button>
          </div>
          <Card>
            <DataTable 
              data={recentTrips} 
              columns={tripColumns}
              emptyMessage="No fishing trips yet. Plan your first trip!"
            />
          </Card>
        </div>
      </div>
    </div>
  );
};
