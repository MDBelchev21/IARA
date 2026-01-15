import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/common/Card';
import { StatCard } from '../../components/common/StatCard';
import { DataTable } from '../../components/common/DataTable';
import { shipService } from '../../services/shipService';
import { fishingTripService } from '../../services/fishingTripService';
import type { Ship, FishingTrip } from '../../types';
import './CaptainDashboard.css';

export const CaptainDashboard: React.FC = () => {
  const navigate = useNavigate();
  const [stats, setStats] = useState({
    totalShips: 0,
    activeTrips: 0,
    completedTrips: 0,
    totalLandings: 0,
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
      const [shipsData, tripsData] = await Promise.all([
        shipService.getAll({ page: 1, pageSize: 100 }),
        fishingTripService.getAll({ page: 1, pageSize: 10 }),
      ]);

      const activeTrips = tripsData.filter((t: FishingTrip) => 
        t.tripStatus === 'Active' || t.tripStatus === 'In Progress'
      );
      const completedTrips = tripsData.filter((t: FishingTrip) => 
        t.tripStatus === 'Completed'
      );

      setStats({
        totalShips: shipsData.length,
        activeTrips: activeTrips.length,
        completedTrips: completedTrips.length,
        totalLandings: 0, // Will be calculated from landings service
      });

      setShips(shipsData.slice(0, 5));
      setRecentTrips(tripsData.slice(0, 5));
    } catch (error) {
      console.error('Failed to load captain dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const shipColumns = [
    { header: 'Name', accessor: 'name' as keyof Ship },
    { header: 'External Marking', accessor: 'externalMarking' as keyof Ship },
    { header: 'Call Sign', accessor: ((row: Ship) => row.radioCallSign || '-') },
    { header: 'Length', accessor: ((row: Ship) => row.length ? `${row.length}m` : '-') },
    { header: 'Length', accessor: ((row: Ship) => row.length ? `${row.length} m` : '-') },
  ];

  const tripColumns = [
    { header: 'Ship', accessor: ((row: FishingTrip) => row.shipName || '-') },
    { header: 'Permit #', accessor: ((row: FishingTrip) => row.permitNumber || '-') },
    { header: 'Departure', accessor: ((row: FishingTrip) => new Date(row.departureDate).toLocaleDateString()) },
    { header: 'Departure Port', accessor: ((row: FishingTrip) => row.departurePort || '-') },
    { header: 'Status', accessor: 'tripStatus' as keyof FishingTrip },
  ];

  if (loading) {
    return <div className="dashboard-loading">Loading captain dashboard...</div>;
  }

  return (
    <div className="captain-dashboard">
      <div className="dashboard-header">
        <h1>Captain Dashboard</h1>
        <p>Manage your vessels and fishing trips</p>
      </div>

      <div className="quick-actions">
        <button className="action-button primary" onClick={() => navigate('/captain/fishing-trips')}>
          🌊 Manage Fishing Trips
        </button>
        <button className="action-button secondary" onClick={() => navigate('/captain/ships')}>
          🚢 View My Ships
        </button>
        <button className="action-button tertiary" onClick={() => navigate('/captain/landings')}>
          📦 Record Landing
        </button>
      </div>

      <div className="stats-grid">
        <StatCard title="My Ships" value={stats.totalShips} icon="🚢" color="blue" />
        <StatCard title="Active Trips" value={stats.activeTrips} icon="🌊" color="green" />
        <StatCard title="Completed Trips" value={stats.completedTrips} icon="✅" color="purple" />
        <StatCard title="Total Landings" value={stats.totalLandings} icon="📦" color="yellow" />
      </div>

      <div className="dashboard-grid">
        <Card title="My Ships">
          <DataTable data={ships} columns={shipColumns} emptyMessage="No ships assigned. Contact ship owner." />
        </Card>

        <Card title="Recent Fishing Trips">
          <DataTable data={recentTrips} columns={tripColumns} emptyMessage="No trips yet. Start your first trip!" />
        </Card>
      </div>
    </div>
  );
};
