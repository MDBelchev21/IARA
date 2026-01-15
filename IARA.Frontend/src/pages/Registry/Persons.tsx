import { useState, useEffect } from 'react';
import { personService } from '../../services/personService';
import type { Person, BaseFilter } from '../../types';

export const Persons = () => {
  const [persons, setPersons] = useState<Person[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editingPerson, setEditingPerson] = useState<Person | null>(null);

  const [formData, setFormData] = useState<Person>({
    firstName: '',
    lastName: '',
    egn: '',
    email: '',
    phone: '',
    address: '',
  });

  useEffect(() => {
    fetchPersons();
  }, []);

  const fetchPersons = async () => {
    setLoading(true);
    try {
      const filters: BaseFilter<any> = { page: 1, pageSize: 100 };
      const data = await personService.getAll(filters);
      setPersons(data);
    } catch (err: any) {
      setError('Failed to fetch persons');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (editingPerson) {
        await personService.edit({ ...formData, id: editingPerson.id });
      } else {
        await personService.add(formData);
      }
      fetchPersons();
      setShowForm(false);
      setEditingPerson(null);
      resetForm();
    } catch (err: any) {
      setError('Failed to save person');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (person: Person) => {
    setEditingPerson(person);
    setFormData(person);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this person?')) {
      try {
        await personService.delete(id);
        fetchPersons();
      } catch (err: any) {
        setError('Failed to delete person');
      }
    }
  };

  const resetForm = () => {
    setFormData({
      firstName: '',
      lastName: '',
      egn: '',
      email: '',
      phone: '',
      address: '',
    });
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Persons Registry</h1>
        <button onClick={() => { setShowForm(true); setEditingPerson(null); resetForm(); }}>
          Add New Person
        </button>
      </div>

      {error && <div className="error-message">{error}</div>}

      {showForm && (
        <div className="modal">
          <div className="modal-content">
            <h2>{editingPerson ? 'Edit Person' : 'Add New Person'}</h2>
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
                <label>EGN</label>
                <input
                  type="text"
                  value={formData.egn}
                  onChange={(e) => setFormData({ ...formData, egn: e.target.value })}
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
                <button type="button" onClick={() => { setShowForm(false); setEditingPerson(null); }}>
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
              <th>EGN</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {persons.map((person) => (
              <tr key={person.id}>
                <td>{person.id}</td>
                <td>{person.firstName} {person.lastName}</td>
                <td>{person.egn}</td>
                <td>{person.email}</td>
                <td>{person.phone}</td>
                <td>
                  <button onClick={() => handleEdit(person)}>Edit</button>
                  <button onClick={() => handleDelete(person.id!)}>Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
