import { useState, useEffect } from 'react';
import { inspectionService } from '../../services/inspectionService';
import type { Inspection, BaseFilter } from '../../types';

export const Inspections = () => {
  const [inspections, setInspections] = useState<Inspection[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);

  const [formData, setFormData] = useState<Inspection>({
    inspectorId: 0,
    inspectionDate: '',
    inspectionType: '',
    location: '',
    notes: '',
  });

  useEffect(() => {
    fetchInspections();
  }, []);

  const fetchInspections = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await inspectionService.getAll(filters);
      setInspections(data);
    } catch (err: any) {
      setError('Failed to fetch inspections');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await inspectionService.add(formData);
      fetchInspections();
      setShowForm(false);
      resetForm();
    } catch (err: any) {
      setError('Failed to save inspection');
    } finally {
      setLoading(false);
    }
  };

  const handleComplete = async (id: number) => {
    try {
      await inspectionService.complete(id);
      fetchInspections();
    } catch (err: any) {
      setError('Failed to complete inspection');
    }
  };

  const resetForm = () => {
    setFormData({
      inspectorId: 0,
      inspectionDate: '',
      inspectionType: '',
      location: '',
      notes: '',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Inspections</h1>
        <button onClick={() => { setShowForm(true); resetForm(); }}>
          Add New Inspection
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>Add New Inspection</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Inspector ID</label>
                <input
                  type="number"
                  value={formData.inspectorId}
                  onChange={(e) => setFormData({ ...formData, inspectorId: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Inspection Date</label>
                <input
                  type="datetime-local"
                  value={formData.inspectionDate}
                  onChange={(e) => setFormData({ ...formData, inspectionDate: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Inspection Type</label>
                <input
                  type="text"
                  value={formData.inspectionType}
                  onChange={(e) => setFormData({ ...formData, inspectionType: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Location</label>
                <input
                  type="text"
                  value={formData.location}
                  onChange={(e) => setFormData({ ...formData, location: e.target.value })}
                />
              </div>
              <div className="form-group">
                <label>Notes</label>
                <textarea
                  value={formData.notes}
                  onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
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
              <th>Inspector ID</th>
              <th>Date</th>
              <th>Type</th>
              <th>Location</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {inspections.map((inspection) => (
              <tr key={inspection.id}>
                <td>{inspection.id}</td>
                <td>{inspection.inspectorId}</td>
                <td>{new Date(inspection.inspectionDate).toLocaleString()}</td>
                <td>{inspection.inspectionType}</td>
                <td>{inspection.location}</td>
                <td>{inspection.status}</td>
                <td>
                  {inspection.status !== 'Completed' && (
                    <button onClick={() => handleComplete(inspection.id!)}>Complete</button>
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
