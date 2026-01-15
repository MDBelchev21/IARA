import { useState, useEffect } from 'react';
import { shipService } from '../../services/shipService';
import type { Ship, BaseFilter } from '../../types';

export const Ships = () => {
  const [ships, setShips] = useState<Ship[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editingShip, setEditingShip] = useState<Ship | null>(null);

  const [formData, setFormData] = useState<Ship>({
    externalMarking: '',
    name: '',
    internationalNumber: '',
    radioCallSign: '',
    length: 0,
    width: 0,
    grossTonnage: 0,
    draft: 0,
    mainEnginePower: 0,
    fuelType: '',
    fuelCapacity: 0,
  });

  useEffect(() => {
    fetchShips();
  }, []);

  const fetchShips = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = {
        page: 1,
        pageSize: 100,
      };
      console.log('Fetching ships with filters:', filters);
      const data = await shipService.getAll(filters);
      console.log('Ships response:', data);
      setShips(data);
    } catch (err: any) {
      setError('Failed to fetch ships');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      if (editingShip) {
        await shipService.edit({ ...formData, shipId: editingShip.shipId });
      } else {
        const userId = parseInt(localStorage.getItem('userId') || '0');
        console.log('Adding ship with ownerId:', userId, 'formData:', formData);
        const result = await shipService.add({ ...formData, ownerId: userId });
        console.log('Ship add result:', result);
      }
      fetchShips();
      setShowForm(false);
      setEditingShip(null);
      resetForm();
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.response?.data || err.message || 'Failed to save ship';
      setError(errorMessage);
      console.error('Ship save error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (ship: Ship) => {
    setEditingShip(ship);
    setFormData(ship);
    setShowForm(true);
  };

  const handleDelete = async (shipId: number) => {
    if (window.confirm('Are you sure you want to delete this ship?')) {
      try {
        await shipService.delete(shipId);
        fetchShips();
      } catch (err: any) {
        setError('Failed to delete ship');
        console.error('Ship delete error:', err);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      externalMarking: '',
      name: '',
      internationalNumber: '',
      radioCallSign: '',
      length: 0,
      width: 0,
      grossTonnage: 0,
      draft: 0,
      mainEnginePower: 0,
      fuelType: '',
      fuelCapacity: 0,
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Ships Management</h1>
        <button onClick={() => { setShowForm(true); setEditingShip(null); resetForm(); }}>
          Add New Ship
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editingShip ? 'Edit Ship' : 'Add New Ship'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>External Marking *</label>
                <input
                  type="text"
                  value={formData.externalMarking}
                  onChange={(e) => setFormData({ ...formData, externalMarking: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Ship Name</label>
                <input
                  type="text"
                  value={formData.name || ''}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>International Number</label>
                <input
                  type="text"
                  value={formData.internationalNumber || ''}
                  onChange={(e) => setFormData({ ...formData, internationalNumber: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Radio Call Sign</label>
                <input
                  type="text"
                  value={formData.radioCallSign || ''}
                  onChange={(e) => setFormData({ ...formData, radioCallSign: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Length (m) *</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.length}
                  onChange={(e) => setFormData({ ...formData, length: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Width (m) *</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.width}
                  onChange={(e) => setFormData({ ...formData, width: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Gross Tonnage</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.grossTonnage || ''}
                  onChange={(e) => setFormData({ ...formData, grossTonnage: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label>Draft (m)</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.draft || ''}
                  onChange={(e) => setFormData({ ...formData, draft: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label>Main Engine Power (kW)</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.mainEnginePower || ''}
                  onChange={(e) => setFormData({ ...formData, mainEnginePower: Number(e.target.value) })}
                />
              </div>
              <div className="form-group">
                <label>Fuel Type</label>
                <input
                  type="text"
                  value={formData.fuelType || ''}
                  onChange={(e) => setFormData({ ...formData, fuelType: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Fuel Capacity (L)</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.fuelCapacity || ''}
                  onChange={(e) => setFormData({ ...formData, fuelCapacity: Number(e.target.value) })}
                />
              </div>
              <div className="form-actions">
                <button type="submit" disabled={loading}>Save</button>
                <button type="button" onClick={() => { setShowForm(false); setEditingShip(null); }}>
                  Cancel
                </button>
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
              <th>External Marking</th>
              <th>Ship Name</th>
              <th>International Number</th>
              <th>Call Sign</th>
              <th>Length (m)</th>
              <th>Tonnage</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {ships.map((ship) => (
              <tr key={ship.shipId}>
                <td>{ship.shipId}</td>
                <td>{ship.externalMarking}</td>
                <td>{ship.name || '-'}</td>
                <td>{ship.internationalNumber || '-'}</td>
                <td>{ship.radioCallSign || '-'}</td>
                <td>{ship.length}</td>
                <td>{ship.grossTonnage || '-'}</td>
                <td>
                  <button onClick={() => handleEdit(ship)}>Edit</button>
                  <button onClick={() => handleDelete(ship.shipId!)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
