import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { recreationalTicketService } from '../../services/recreationalTicketService';
import type { RecreationalTicket } from '../../types';
import './MyTickets.css';

export const MyTickets: React.FC = () => {
  const [tickets, setTickets] = useState<RecreationalTicket[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadTickets();
  }, []);

  const loadTickets = async () => {
    try {
      setLoading(true);
      const allTickets = await recreationalTicketService.getAll({ page: 1, pageSize: 100 });
      setTickets(allTickets);
    } catch (err) {
      console.error('Failed to load tickets:', err);
    } finally {
      setLoading(false);
    }
  };

  const getTicketStatus = (ticket: RecreationalTicket) => {
    const now = new Date();
    const validFrom = new Date(ticket.validFrom);
    const validUntil = new Date(ticket.validUntil);

    if (validFrom <= now && validUntil >= now) {
      return { status: 'Active', class: 'status-active' };
    } else if (validUntil < now) {
      return { status: 'Expired', class: 'status-expired' };
    } else {
      return { status: 'Pending', class: 'status-pending' };
    }
  };

  const columns = [
    { header: 'Ticket #', accessor: 'ticketNumber' as keyof RecreationalTicket },
    { header: 'Type', accessor: 'ticketType' as keyof RecreationalTicket },
    { 
      header: 'Valid From', 
      accessor: ((row: RecreationalTicket) => new Date(row.validFrom).toLocaleDateString()) 
    },
    { 
      header: 'Valid Until', 
      accessor: ((row: RecreationalTicket) => new Date(row.validUntil).toLocaleDateString()) 
    },
    { 
      header: 'Status', 
      accessor: ((row: RecreationalTicket) => {
        const { status, class: statusClass } = getTicketStatus(row);
        return <span className={`status-badge ${statusClass}`}>{status}</span>;
      })
    },
  ];

  if (loading) {
    return <div className="loading">Loading your tickets...</div>;
  }

  const activeTickets = tickets.filter(t => getTicketStatus(t).status === 'Active');
  const expiredTickets = tickets.filter(t => getTicketStatus(t).status === 'Expired');
  const pendingTickets = tickets.filter(t => getTicketStatus(t).status === 'Pending');

  return (
    <div className="my-tickets-page">
      <div className="page-header">
        <h1>My Fishing Tickets</h1>
        <p>Manage and view your fishing permits</p>
      </div>

      <div className="tickets-summary">
        <div className="summary-card active">
          <div className="summary-icon">✓</div>
          <div className="summary-content">
            <div className="summary-count">{activeTickets.length}</div>
            <div className="summary-label">Active Tickets</div>
          </div>
        </div>
        <div className="summary-card pending">
          <div className="summary-icon">⏳</div>
          <div className="summary-content">
            <div className="summary-count">{pendingTickets.length}</div>
            <div className="summary-label">Pending Tickets</div>
          </div>
        </div>
        <div className="summary-card expired">
          <div className="summary-icon">✗</div>
          <div className="summary-content">
            <div className="summary-count">{expiredTickets.length}</div>
            <div className="summary-label">Expired Tickets</div>
          </div>
        </div>
      </div>

      <Card title="All Tickets">
        <DataTable 
          data={tickets} 
          columns={columns}
          emptyMessage="No tickets found. Purchase your first ticket to start fishing!"
        />
      </Card>
    </div>
  );
};
