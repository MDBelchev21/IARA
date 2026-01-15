import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { StatCard } from '../../components/common/StatCard';
import { DataTable } from '../../components/common/DataTable';
import { inspectionService } from '../../services/inspectionService';
import { violationService } from '../../services/violationService';
import { fishingPermitService } from '../../services/fishingPermitService';
import type { Inspection, Violation, FishingPermit } from '../../types';
import './InspectorDashboard.css';

export const InspectorDashboard: React.FC = () => {
  const [stats, setStats] = useState({
    totalInspections: 0,
    pendingInspections: 0,
    totalViolations: 0,
    activePermits: 0,
  });
  const [recentInspections, setRecentInspections] = useState<Inspection[]>([]);
  const [recentViolations, setRecentViolations] = useState<Violation[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const [inspections, violations, permits] = await Promise.all([
        inspectionService.getAll({ page: 1, pageSize: 10 }),
        violationService.getAll({ page: 1, pageSize: 10 }),
        fishingPermitService.getAll({ page: 1, pageSize: 100 }),
      ]);

      setStats({
        totalInspections: inspections.length,
        pendingInspections: 0,
        totalViolations: violations.length,
        activePermits: permits.filter((p: FishingPermit) => p.isActive).length,
      });

      setRecentInspections(inspections.slice(0, 5));
      setRecentViolations(violations.slice(0, 5));
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const inspectionColumns = [
    { header: 'Date', accessor: ((row: Inspection) => new Date(row.inspectionDate).toLocaleDateString()) },
    { header: 'Type', accessor: 'inspectionType' as keyof Inspection },
    { header: 'Location', accessor: 'location' as keyof Inspection },
    { header: 'Result', accessor: 'result' as keyof Inspection },
  ];

  const violationColumns = [
    { header: 'Date', accessor: ((row: Violation) => row.issuedOn ? new Date(row.issuedOn).toLocaleDateString() : '-') },
    { header: 'Type', accessor: 'violationType' as keyof Violation },
    { header: 'Severity', accessor: ((row: Violation) => row.severity || '-') },
    { header: 'Fine', accessor: ((row: Violation) => row.fineAmount ? `€${row.fineAmount}` : '-') },
  ];

  if (loading) {
    return <div className="dashboard-loading">Loading dashboard...</div>;
  }

  return (
    <div className="inspector-dashboard">
      <div className="dashboard-header">
        <h1>Inspector Dashboard</h1>
        <p>Inspection and Compliance Overview</p>
      </div>

      <div className="stats-grid">
        <StatCard title="Total Inspections" value={stats.totalInspections} icon="🔍" color="blue" />
        <StatCard title="Pending" value={stats.pendingInspections} icon="⏱️" color="yellow" />
        <StatCard title="Violations Found" value={stats.totalViolations} icon="⚠️" color="red" />
        <StatCard title="Active Permits" value={stats.activePermits} icon="✅" color="green" />
      </div>

      <div className="dashboard-grid">
        <Card title="Recent Inspections">
          <DataTable data={recentInspections} columns={inspectionColumns} />
        </Card>

        <Card title="Recent Violations">
          <DataTable data={recentViolations} columns={violationColumns} />
        </Card>
      </div>
    </div>
  );
};
