import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { shipCrewService } from '../../services/shipCrewService';
import { shipService } from '../../services/shipService';
import type { ShipCrew, Ship } from '../../types';
import './Crew.css';

export const Crew: React.FC = () => {
  const [crew, setCrew] = useState<ShipCrew[]>([]);
  const [ships, setShips] = useState<Ship[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [editingCrew, setEditingCrew] = useState<ShipCrew | null>(null);
  const [formData, setFormData] = useState<ShipCrew>({
    shipId: 0,
    personId: 0,
    position: '',
    startDate: '',
    endDate: '',
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [crewData, shipsData] = await Promise.all([
        shipCrewService.getAll({ page: 1, pageSize: 100 }),
        shipService.getAll({ page: 1, pageSize: 100 }),
      ]);
      setCrew(crewData);
      setShips(shipsData);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingCrew && editingCrew.id) {
        await shipCrewService.edit({ ...formData, id: editingCrew.id });
      } else {
        await shipCrewService.add(formData);
      }
      fetchData();
      resetForm();
      setShowForm(false);
    } catch (error) {
      console.error('Failed to save crew member:', error);
    }
  };

  const handleEdit = (crewMember: ShipCrew) => {
    setEditingCrew(crewMember);
    setFormData(crewMember);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to remove this crew member?')) {
      try {
        await shipCrewService.delete(id);
        fetchData();
      } catch (error) {
        console.error('Failed to delete crew member:', error);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      shipId: 0,
      personId: 0,
      position: '',
      startDate: '',
      endDate: '',
    });
    setEditingCrew(null);
  };

  const getShipName = (shipId: number) => {
    const ship = ships.find(s => s.shipId === shipId);
    return ship ? ship.name || ship.externalMarking : 'Unknown';
  };

  const crewColumns = [
    { header: 'Ship', accessor: ((row: ShipCrew) => getShipName(row.shipId)) },
    { header: 'Person ID', accessor: 'personId' as keyof ShipCrew },
    { header: 'Position', accessor: 'position' as keyof ShipCrew },
    { header: 'Start Date', accessor: ((row: ShipCrew) => new Date(row.startDate).toLocaleDateString()) },
    { header: 'End Date', accessor: ((row: ShipCrew) => row.endDate ? new Date(row.endDate).toLocaleDateString() : 'Active') },
    { 
      header: 'Actions', 
      accessor: ((row: ShipCrew) => (
        <div className="action-buttons">
          <button className="btn-edit" onClick={() => handleEdit(row)}>Edit</button>
          <button className="btn-delete" onClick={() => row.id && handleDelete(row.id)}>Remove</button>
        </div>
      ))
    },
  ];

  if (loading) {
    return <div className="loading">Loading crew members...</div>;
  }

  return (
    <div className="crew-page">
      <div className="page-header">
        <h1>Crew Management</h1>
        <button className="btn-primary" onClick={() => setShowForm(true)}>
          ➕ Add Crew Member
        </button>
      </div>

      {showForm && (
        <Card title={editingCrew ? 'Edit Crew Member' : 'Add New Crew Member'}>
          <form onSubmit={handleSubmit} className="crew-form">
            <div className="form-grid">
              <div className="form-group">
                <label>Ship *</label>
                <select
                  value={formData.shipId}
                  onChange={(e) => setFormData({ ...formData, shipId: parseInt(e.target.value) })}
                  required
                >
                  <option value={0}>Select a ship</option>
                  {ships.map((ship) => (
                    <option key={ship.shipId} value={ship.shipId}>
                      {ship.name || ship.externalMarking} ({ship.externalMarking})
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Person ID *</label>
                <input
                  type="number"
                  value={formData.personId || ''}
                  onChange={(e) => setFormData({ ...formData, personId: parseInt(e.target.value) })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Position *</label>
                <select
                  value={formData.position}
                  onChange={(e) => setFormData({ ...formData, position: e.target.value })}
                  required
                >
                  <option value="">Select position</option>
                  <option value="Captain">Captain</option>
                  <option value="First Mate">First Mate</option>
                  <option value="Engineer">Engineer</option>
                  <option value="Deckhand">Deckhand</option>
                  <option value="Cook">Cook</option>
                  <option value="Other">Other</option>
                </select>
              </div>

              <div className="form-group">
                <label>Start Date *</label>
                <input
                  type="date"
                  value={formData.startDate.split('T')[0]}
                  onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>End Date</label>
                <input
                  type="date"
                  value={formData.endDate ? formData.endDate.split('T')[0] : ''}
                  onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                />
              </div>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-primary">
                {editingCrew ? 'Update Crew Member' : 'Add Crew Member'}
              </button>
              <button 
                type="button" 
                className="btn-secondary" 
                onClick={() => { resetForm(); setShowForm(false); }}
              >
                Cancel
              </button>
            </div>
          </form>
        </Card>
      )}

      <Card title="All Crew Members">
        <DataTable data={crew} columns={crewColumns} emptyMessage="No crew members found." />
      </Card>
    </div>
  );
};
