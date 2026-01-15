import { useState, useEffect } from 'react';
import { violationService } from '../../services/violationService';
import type { Violation, BaseFilter } from '../../types';

export const Violations = () => {
  const [violations, setViolations] = useState<Violation[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [showFineModal, setShowFineModal] = useState(false);
  const [selectedViolation, setSelectedViolation] = useState<number | null>(null);
  const [fineAmount, setFineAmount] = useState<number>(0);

  const [formData, setFormData] = useState<Violation>({
    inspectionId: 0,
    violationType: '',
    description: '',
  });

  useEffect(() => {
    fetchViolations();
  }, []);

  const fetchViolations = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await violationService.getAll(filters);
      setViolations(data);
    } catch (err: any) {
      setError('Failed to fetch violations');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await violationService.add(formData);
      fetchViolations();
      setShowForm(false);
      resetForm();
    } catch (err: any) {
      setError('Failed to save violation');
    } finally {
      setLoading(false);
    }
  };

  const handleIssueFine = async () => {
    if (selectedViolation && fineAmount > 0) {
      try {
        await violationService.issueFine(selectedViolation, fineAmount);
        fetchViolations();
        setShowFineModal(false);
        setSelectedViolation(null);
        setFineAmount(0);
      } catch (err: any) {
        setError('Failed to issue fine');
      }
    }
  };

  const resetForm = () => {
    setFormData({
      inspectionId: 0,
      violationType: '',
      description: '',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Violations</h1>
        <button onClick={() => { setShowForm(true); resetForm(); }}>
          Add New Violation
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>Add New Violation</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Inspection ID</label>
                <input
                  type="number"
                  value={formData.inspectionId}
                  onChange={(e) => setFormData({ ...formData, inspectionId: Number(e.target.value) })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Violation Type</label>
                <input
                  type="text"
                  value={formData.violationType}
                  onChange={(e) => setFormData({ ...formData, violationType: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>Description</label>
                <textarea
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
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

      {showFineModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>Issue Fine</h2>
            <div className="form-group">
              <label>Fine Amount</label>
              <input
                type="number"
                value={fineAmount}
                onChange={(e) => setFineAmount(Number(e.target.value))}
                min="0"
                step="0.01"
              />
            </div>
            <div className="form-actions">
              <button onClick={handleIssueFine}>Issue Fine</button>
              <button onClick={() => { setShowFineModal(false); setSelectedViolation(null); }}>
                Cancel
              </button>
            </div>
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
              <th>Inspection ID</th>
              <th>Type</th>
              <th>Description</th>
              <th>Fine Amount</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {violations.map((violation) => (
              <tr key={violation.id}>
                <td>{violation.id}</td>
                <td>{violation.inspectionId}</td>
                <td>{violation.violationType}</td>
                <td>{violation.description}</td>
                <td>{violation.fineAmount ? `$${violation.fineAmount}` : '-'}</td>
                <td>{violation.status}</td>
                <td>
                  {!violation.fineAmount && (
                    <button onClick={() => { setSelectedViolation(violation.id!); setShowFineModal(true); }}>
                      Issue Fine
                    </button>
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
