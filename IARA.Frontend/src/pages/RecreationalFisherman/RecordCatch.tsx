import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { recreationalCatchService } from '../../services/recreationalCatchService';
import { recreationalTicketService } from '../../services/recreationalTicketService';
import type { RecreationalCatch, RecreationalTicket } from '../../types';
import './RecordCatch.css';

export const RecordCatch: React.FC = () => {
  const [catches, setCatches] = useState<RecreationalCatch[]>([]);
  const [tickets, setTickets] = useState<RecreationalTicket[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    ticketId: 0,
    catchDate: new Date().toISOString().split('T')[0],
    speciesName: '',
    weightKg: '',
    quantity: '',
    location: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [catchesData, ticketsData] = await Promise.all([
        recreationalCatchService.getAll({ page: 1, pageSize: 100 }),
        recreationalTicketService.getAll({ page: 1, pageSize: 100 }),
      ]);
      
      // Filter only active tickets
      const now = new Date();
      const activeTickets = ticketsData.filter((t: RecreationalTicket) => 
        new Date(t.validFrom) <= now && new Date(t.validUntil) >= now
      );
      
      setCatches(catchesData);
      setTickets(activeTickets);
    } catch (err) {
      console.error('Failed to load data:', err);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.ticketId) {
      setError('Please select a ticket');
      return;
    }

    setLoading(true);
    setError('');

    try {
      await recreationalCatchService.add({
        ticketId: formData.ticketId,
        catchDate: formData.catchDate,
        speciesName: formData.speciesName,
        weightKg: formData.weightKg ? parseFloat(formData.weightKg) : undefined,
        quantity: formData.quantity ? parseInt(formData.quantity) : 1,
        location: formData.location,
        registeredVia: 'Online',
      });

      setFormData({
        ticketId: 0,
        catchDate: new Date().toISOString().split('T')[0],
        speciesName: '',
        weightKg: '',
        quantity: '',
        location: '',
      });
      setShowForm(false);
      loadData();
    } catch (err: any) {
      console.error('Failed to record catch:', err);
      setError(err.response?.data?.message || 'Failed to record catch');
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    { header: 'Date', accessor: ((row: RecreationalCatch) => new Date(row.catchDate).toLocaleDateString()) },
    { header: 'Species', accessor: 'speciesName' as keyof RecreationalCatch },
    { header: 'Weight (kg)', accessor: ((row: RecreationalCatch) => row.weightKg ? `${row.weightKg} kg` : '-') },
    { header: 'Quantity', accessor: ((row: RecreationalCatch) => row.quantity || '-') },
    { header: 'Location', accessor: 'location' as keyof RecreationalCatch },
  ];

  return (
    <div className="record-catch-page">
      <div className="page-header">
        <h1>Record Catch</h1>
        <p>Log your fishing catches for record keeping</p>
        <button 
          className="add-catch-button"
          onClick={() => setShowForm(!showForm)}
        >
          {showForm ? '✗ Cancel' : '+ Record New Catch'}
        </button>
      </div>

      {error && (
        <div className="alert alert-error">
          ✗ {error}
        </div>
      )}

      {showForm && (
        <Card title="Record New Catch" className="form-card">
          <form onSubmit={handleSubmit} className="catch-form">
            <div className="form-group">
              <label>Select Active Ticket *</label>
              <select
                value={formData.ticketId}
                onChange={(e) => setFormData({ ...formData, ticketId: parseInt(e.target.value) })}
                required
              >
                <option value={0}>Select a ticket...</option>
                {tickets.map((ticket) => (
                  <option key={ticket.ticketId} value={ticket.ticketId}>
                    {ticket.ticketNumber} - {ticket.ticketTypeName} (Valid until {new Date(ticket.validUntil).toLocaleDateString()})
                  </option>
                ))}
              </select>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Catch Date *</label>
                <input
                  type="date"
                  value={formData.catchDate}
                  onChange={(e) => setFormData({ ...formData, catchDate: e.target.value })}
                  max={new Date().toISOString().split('T')[0]}
                  required
                />
              </div>

              <div className="form-group">
                <label>Fish Species *</label>
                <input
                  type="text"
                  value={formData.speciesName}
                  onChange={(e) => setFormData({ ...formData, speciesName: e.target.value })}
                  placeholder="e.g., Trout, Bass, Carp"
                  required
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Weight (kg)</label>
                <input
                  type="number"
                  step="0.1"
                  value={formData.weightKg}
                  onChange={(e) => setFormData({ ...formData, weightKg: e.target.value })}
                  placeholder="0.0"
                />
              </div>

              <div className="form-group">
                <label>Quantity</label>
                <input
                  type="number"
                  value={formData.quantity}
                  onChange={(e) => setFormData({ ...formData, quantity: e.target.value })}
                  placeholder="Number of fish"
                />
              </div>
            </div>

            <div className="form-group">
              <label>Location *</label>
              <input
                type="text"
                value={formData.location}
                onChange={(e) => setFormData({ ...formData, location: e.target.value })}
                placeholder="e.g., Black Sea Coast, Danube River"
                required
              />
            </div>

            <button type="submit" className="submit-button" disabled={loading}>
              {loading ? 'Recording...' : 'Record Catch'}
            </button>
          </form>
        </Card>
      )}

      <Card title="My Catch History">
        <DataTable 
          data={catches} 
          columns={columns}
          emptyMessage="No catches recorded yet. Start recording your catches!"
        />
      </Card>
    </div>
  );
};
