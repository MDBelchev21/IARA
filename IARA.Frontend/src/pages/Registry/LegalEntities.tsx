import { useState, useEffect } from 'react';
import { legalEntityService } from '../../services/legalEntityService';
import type { LegalEntity, BaseFilter } from '../../types';

export const LegalEntities = () => {
  const [entities, setEntities] = useState<LegalEntity[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editingEntity, setEditingEntity] = useState<LegalEntity | null>(null);

  const [formData, setFormData] = useState<LegalEntity>({
    name: '',
    eik: '',
    email: '',
    phone: '',
    address: '',
  });

  useEffect(() => {
    fetchEntities();
  }, []);

  const fetchEntities = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await legalEntityService.getAll(filters);
      setEntities(data);
    } catch (err: any) {
      setError('Failed to fetch legal entities');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (editingEntity) {
        await legalEntityService.edit({ ...formData, id: editingEntity.id });
      } else {
        await legalEntityService.add(formData);
      }
      fetchEntities();
      setShowForm(false);
      setEditingEntity(null);
      resetForm();
    } catch (err: any) {
      setError('Failed to save legal entity');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (entity: LegalEntity) => {
    setEditingEntity(entity);
    setFormData(entity);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this legal entity?')) {
      try {
        await legalEntityService.delete(id);
        fetchEntities();
      } catch (err: any) {
        setError('Failed to delete legal entity');
      }
    }
  };

  const resetForm = () => {
    setFormData({
      name: '',
      eik: '',
      email: '',
      phone: '',
      address: '',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Legal Entities Registry</h1>
        <button onClick={() => { setShowForm(true); setEditingEntity(null); resetForm(); }}>
          Add New Legal Entity
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editingEntity ? 'Edit Legal Entity' : 'Add New Legal Entity'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Name</label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  required
                />
              </div>
              <div className="form-group">
                <label>EIK</label>
                <input
                  type="text"
                  value={formData.eik}
                  onChange={(e) => setFormData({ ...formData, eik: e.target.value })}
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
                <button type="button" onClick={() => { setShowForm(false); setEditingEntity(null); }}>
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
              <th>Name</th>
              <th>EIK</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {entities.map((entity) => (
              <tr key={entity.id}>
                <td>{entity.id}</td>
                <td>{entity.name}</td>
                <td>{entity.eik}</td>
                <td>{entity.email}</td>
                <td>{entity.phone}</td>
                <td>
                  <button onClick={() => handleEdit(entity)}>Edit</button>
                  <button onClick={() => handleDelete(entity.id!)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
