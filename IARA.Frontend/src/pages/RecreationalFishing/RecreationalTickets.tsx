import { useState, useEffect } from 'react';
import { recreationalTicketService } from '../../services/recreationalTicketService';
import type { RecreationalTicket, BaseFilter } from '../../types';

export const RecreationalTickets = () => {
  const [tickets, setTickets] = useState<RecreationalTicket[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);

  const [formData, setFormData] = useState({
    recFishermanId: 0,
    ticketTypeId: 0,
    validFrom: '',
    validUntil: '',
    purchaseChannel: 'Admin',
  });

  useEffect(() => {
    fetchTickets();
  }, []);

  const fetchTickets = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await recreationalTicketService.getAll(filters);
      setTickets(data);
    } catch (err: any) {
      setError('Failed to fetch tickets');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await recreationalTicketService.add(formData as any);
      fetchTickets();
      setShowForm(false);
      resetForm();
    } catch (err: any) {
      setError('Failed to save ticket');
    } finally {
      setLoading(false);
    }
  };

  const handleDeactivate = async (id: number) => {
    if (window.confirm('Are you sure you want to deactivate this ticket?')) {
      try {
        await recreationalTicketService.deactivate(id);
        fetchTickets();
      } catch (err: any) {
        setError('Failed to deactivate ticket');
      }
    }
  };

  const resetForm = () => {
    setFormData({
      recFishermanId: 0,
      ticketTypeId: 0,
      validFrom: '',
      validUntil: '',
      purchaseChannel: 'Admin',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Recreational Fishing Tickets</h1>
        <button onClick={() => { setShowForm(true); resetForm(); }}>
          Issue New Ticket
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>Issue New Ticket</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Fisherman ID</label>
                <input
                  type="number"
                  value={formData.recFishermanId}
                  onChange={(e) => setFormData({ ...formData, recFishermanId: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Ticket Type ID</label>
                <input
                  type="number"
                  value={formData.ticketTypeId}
                  onChange={(e) => setFormData({ ...formData, ticketTypeId: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Valid From</label>
                <input
                  type="datetime-local"
                  value={formData.validFrom}
                  onChange={(e) => setFormData({ ...formData, validFrom: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Valid Until</label>
                <input
                  type="datetime-local"
                  value={formData.validUntil}
                  onChange={(e) => setFormData({ ...formData, validUntil: e.target.value })}
                  required
                />
              </div>
              <div className="form-actions">
                <button type="submit" disabled={loading}>Save</button>
                <button type="button" onClick={() => setShowForm(false)}>Cancel</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {loading ? (
        <p>Loading...</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Ticket Number</th>
              <th>Fisherman Name</th>
              <th>Ticket Type</th>
              <th>Valid From</th>
              <th>Valid Until</th>
              <th>Price</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {tickets.map((ticket) => (
              <tr key={ticket.ticketId}>
                <td>{ticket.ticketNumber}</td>
                <td>{ticket.fishermanName}</td>
                <td>{ticket.ticketTypeName}</td>
                <td>{new Date(ticket.validFrom).toLocaleDateString()}</td>
                <td>{new Date(ticket.validUntil).toLocaleDateString()}</td>
                <td>${ticket.price.toFixed(2)}</td>
                <td>{ticket.isActive ? 'Active' : 'Inactive'}</td>
                <td>
                  {ticket.isActive && (
                    <button onClick={() => handleDeactivate(ticket.ticketId)}>Deactivate</button>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
