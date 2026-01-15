import { useState, useEffect } from 'react';
import { recreationalFishermanService } from '../../services/recreationalFishermanService';
import type { RecreationalFisherman, BaseFilter } from '../../types';

export const RecreationalFishermen = () => {
  const [fishermen, setFishermen] = useState<RecreationalFisherman[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);

  const [formData, setFormData] = useState<RecreationalFisherman>({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
  });

  useEffect(() => {
    fetchFishermen();
  }, []);

  const fetchFishermen = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await recreationalFishermanService.getAll(filters);
      setFishermen(data);
    } catch (err: any) {
      setError('Failed to fetch fishermen');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await recreationalFishermanService.add(formData);
      fetchFishermen();
      setShowForm(false);
      resetForm();
    } catch (err: any) {
      setError('Failed to save fisherman');
    } finally {
      setLoading(false);
    }
  };

  const resetForm = () => {
    setFormData({
      firstName: '',
      lastName: '',
      email: '',
      phone: '',
      address: '',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Recreational Fishermen</h1>
        <button onClick={() => { setShowForm(true); resetForm(); }}>
          Register New Fisherman
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>Register New Fisherman</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>First Name</label>
                <input
                  type="text"
                  value={formData.firstName}
                  onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Last Name</label>
                <input
                  type="text"
                  value={formData.lastName}
                  onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  type="email"
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Phone</label>
                <input
                  type="tel"
                  value={formData.phone}
                  onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Address</label>
                <input
                  type="text"
                  value={formData.address}
                  onChange={(e) => setFormData({ ...formData, address: e.target.value })}
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
              <th>ID</th>
              <th>Name</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Address</th>
            </tr>
          </thead>
          <tbody>
            {fishermen.map((fisherman) => (
              <tr key={fisherman.id}>
                <td>{fisherman.id}</td>
                <td>{fisherman.firstName} {fisherman.lastName}</td>
                <td>{fisherman.email}</td>
                <td>{fisherman.phone}</td>
                <td>{fisherman.address}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
