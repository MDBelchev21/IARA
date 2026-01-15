import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { StatCard } from '../../components/common/StatCard';
import { DataTable } from '../../components/common/DataTable';
import { shipService } from '../../services/shipService';
import { inspectorService } from '../../services/inspectorService';
import { fishingPermitService } from '../../services/fishingPermitService';
import type { Ship, Person, FishingPermit } from '../../types';
import './AdministratorDashboard.css';

export const AdministratorDashboard: React.FC = () => {
  const [stats, setStats] = useState({
    totalShips: 0,
    totalInspectors: 0,
    activePermits: 0,
    pendingActions: 0,
  });
  const [recentShips, setRecentShips] = useState<Ship[]>([]);
  const [recentInspectors, setRecentInspectors] = useState<Person[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const [ships, inspectors, permits] = await Promise.all([
        shipService.getAll({ page: 1, pageSize: 5 }),
        inspectorService.getAll({ page: 1, pageSize: 5 }),
        fishingPermitService.getAll({ page: 1, pageSize: 100 }),
      ]);

      setStats({
        totalShips: ships.length,
        totalInspectors: inspectors.length,
        activePermits: permits.filter((p: FishingPermit) => p.isActive).length,
        pendingActions: 0,
      });

      setRecentShips(ships.slice(0, 5));
      setRecentInspectors(inspectors.slice(0, 5));
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const shipColumns = [
    { header: 'Name', accessor: 'shipName' as keyof Ship },
    { header: 'International #', accessor: 'internationalNumber' as keyof Ship },
    { header: 'IMO', accessor: 'imo' as keyof Ship },
    { header: 'Flag', accessor: 'flagCountry' as keyof Ship },
  ];

  const inspectorColumns = [
    { header: 'First Name', accessor: 'firstName' as keyof Person },
    { header: 'Last Name', accessor: 'lastName' as keyof Person },
    { header: 'EGN', accessor: 'egn' as keyof Person },
    { header: 'Email', accessor: 'email' as keyof Person },
  ];

  if (loading) {
    return <div className="dashboard-loading">Loading dashboard...</div>;
  }

  return (
    <div className="admin-dashboard">
      <div className="dashboard-header">
        <h1>Administrator Dashboard</h1>
        <p>System Overview and Management</p>
      </div>

      <div className="stats-grid">
        <StatCard title="Total Ships" value={stats.totalShips} icon="🚢" color="blue" />
        <StatCard title="Active Inspectors" value={stats.totalInspectors} icon="👮" color="green" />
        <StatCard title="Active Permits" value={stats.activePermits} icon="📋" color="purple" />
        <StatCard title="Pending Actions" value={stats.pendingActions} icon="⏳" color="yellow" />
      </div>

      <div className="dashboard-grid">
        <Card title="Recent Ships">
          <DataTable data={recentShips} columns={shipColumns} />
        </Card>

        <Card title="Recent Inspectors">
          <DataTable data={recentInspectors} columns={inspectorColumns} />
        </Card>
      </div>
    </div>
  );
};
