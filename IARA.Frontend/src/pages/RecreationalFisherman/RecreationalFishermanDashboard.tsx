import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/common/Card';
import { StatCard } from '../../components/common/StatCard';
import { DataTable } from '../../components/common/DataTable';
import { recreationalTicketService } from '../../services/recreationalTicketService';
import { recreationalCatchService } from '../../services/recreationalCatchService';
import type { RecreationalTicket, RecreationalCatch } from '../../types';
import './RecreationalFishermanDashboard.css';

export const RecreationalFishermanDashboard: React.FC = () => {
  const navigate = useNavigate();
  const [stats, setStats] = useState({
    totalTickets: 0,
    activeTickets: 0,
    expiredTickets: 0,
    totalCatches: 0,
  });
  const [myTickets, setMyTickets] = useState<RecreationalTicket[]>([]);
  const [recentCatches, setRecentCatches] = useState<RecreationalCatch[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const [tickets, catches] = await Promise.all([
        recreationalTicketService.getAll({ page: 1, pageSize: 100 }),
        recreationalCatchService.getAll({ page: 1, pageSize: 10 }),
      ]);

      const now = new Date();
      const activeTickets = tickets.filter((t: RecreationalTicket) => 
        new Date(t.validFrom) <= now && new Date(t.validUntil) >= now
      );
      const expiredTickets = tickets.filter((t: RecreationalTicket) => 
        new Date(t.validUntil) < now
      );

      setStats({
        totalTickets: tickets.length,
        activeTickets: activeTickets.length,
        expiredTickets: expiredTickets.length,
        totalCatches: catches.length,
      });

      setMyTickets(tickets.slice(0, 5));
      setRecentCatches(catches.slice(0, 5));
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const ticketColumns = [
    { header: 'Ticket #', accessor: ((row: RecreationalTicket) => row.ticketNumber || '-') },
    { header: 'Type', accessor: ((row: RecreationalTicket) => row.ticketTypeName || '-') },
    { header: 'Valid Until', accessor: ((row: RecreationalTicket) => new Date(row.validUntil).toLocaleDateString()) },
    { 
      header: 'Status', 
      accessor: ((row: RecreationalTicket) => {
        const now = new Date();
        const validFrom = new Date(row.validFrom);
        const validUntil = new Date(row.validUntil);
        if (validFrom <= now && validUntil >= now) return '✅ Active';
        if (validUntil < now) return '❌ Expired';
        return '⏳ Pending';
      })
    },
  ];

  const catchColumns = [
    { header: 'Date', accessor: ((row: RecreationalCatch) => new Date(row.catchDate).toLocaleDateString()) },
    { header: 'Species', accessor: 'fishSpecies' as keyof RecreationalCatch },
    { header: 'Weight', accessor: ((row: RecreationalCatch) => row.weightKg ? `${row.weightKg} kg` : '-') },
    { header: 'Location', accessor: ((row: RecreationalCatch) => row.location || '-') },
  ];

  if (loading) {
    return <div className="dashboard-loading">Loading dashboard...</div>;
  }

  return (
    <div className="recreational-dashboard">
      <div className="dashboard-header">
        <h1>Recreational Fisherman Dashboard</h1>
        <p>Manage your fishing permits and catches</p>
      </div>

      <div className="quick-actions">
        <button className="action-button primary" onClick={() => navigate('/recreational/buy-ticket')}>
          🎣 Buy Fishing Ticket
        </button>
        <button className="action-button secondary" onClick={() => navigate('/recreational/record-catch')}>
          📝 Record Catch
        </button>
        <button className="action-button tertiary" onClick={() => navigate('/recreational/my-tickets')}>
          📋 View My Tickets
        </button>
      </div>

      <div className="stats-grid">
        <StatCard title="Total Tickets" value={stats.totalTickets} icon="🎣" color="blue" />
        <StatCard title="Active Tickets" value={stats.activeTickets} icon="✅" color="green" />
        <StatCard title="Expired Tickets" value={stats.expiredTickets} icon="⏰" color="red" />
        <StatCard title="Total Catches" value={stats.totalCatches} icon="🐟" color="purple" />
      </div>

      <div className="dashboard-grid">
        <Card title="My Recent Tickets">
          <DataTable data={myTickets} columns={ticketColumns} emptyMessage="No tickets found. Buy your first ticket!" />
        </Card>

        <Card title="Recent Catches">
          <DataTable data={recentCatches} columns={catchColumns} emptyMessage="No catches recorded yet. Start fishing!" />
        </Card>
      </div>
    </div>
  );
};