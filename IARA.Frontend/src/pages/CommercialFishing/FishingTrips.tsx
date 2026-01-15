import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { fishingTripService } from '../../services/fishingTripService';
import { shipService } from '../../services/shipService';
import { fishingPermitService } from '../../services/fishingPermitService';
import type { FishingTrip, Ship, FishingPermit } from '../../types';
import './FishingTrips.css';

export const FishingTrips: React.FC = () => {
  const [trips, setTrips] = useState<FishingTrip[]>([]);
  const [ships, setShips] = useState<Ship[]>([]);
  const [permits, setPermits] = useState<FishingPermit[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [editingTrip, setEditingTrip] = useState<FishingTrip | null>(null);
  const [formData, setFormData] = useState<FishingTrip>({
    shipId: 0,
    permitId: 0,
    departureDate: '',
    arrivalDate: '',
    departurePort: '',
    arrivalPort: '',
    tripStatus: 'Planned',
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [tripsData, shipsData, permitsData] = await Promise.all([
        fishingTripService.getAll({ page: 1, pageSize: 100 }),
        shipService.getAll({ page: 1, pageSize: 100 }),
        fishingPermitService.getAll({ page: 1, pageSize: 100 }),
      ]);
      setTrips(tripsData);
      setShips(shipsData);
      setPermits(permitsData);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingTrip && editingTrip.id) {
        await fishingTripService.edit({ ...formData, id: editingTrip.id });
      } else {
        await fishingTripService.add(formData);
      }
      fetchData();
      resetForm();
      setShowForm(false);
    } catch (error) {
      console.error('Failed to save trip:', error);
    }
  };

  const handleEdit = (trip: FishingTrip) => {
    setEditingTrip(trip);
    setFormData(trip);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Are you sure you want to delete this fishing trip?')) {
      try {
        await fishingTripService.delete(id);
        fetchData();
      } catch (error) {
        console.error('Failed to delete trip:', error);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      shipId: 0,
      permitId: 0,
      departureDate: '',
      arrivalDate: '',
      departurePort: '',
      arrivalPort: '',
      tripStatus: 'Planned',
    });
    setEditingTrip(null);
  };

  const getShipName = (shipId: number) => {
    const ship = ships.find(s => s.shipId === shipId);
    return ship ? ship.name || ship.externalMarking : 'Unknown';
  };

  const getPermitNumber = (permitId: number) => {
    const permit = permits.find(p => p.id === permitId);
    return permit ? permit.permitNumber : 'Unknown';
  };

  const tripColumns = [
    { header: 'Ship', accessor: ((row: FishingTrip) => row.shipName || getShipName(row.shipId)) },
    { header: 'Permit', accessor: ((row: FishingTrip) => row.permitNumber || getPermitNumber(row.permitId)) },
    { header: 'Departure', accessor: ((row: FishingTrip) => new Date(row.departureDate).toLocaleDateString()) },
    { header: 'Departure Port', accessor: ((row: FishingTrip) => row.departurePort || '-') },
    { header: 'Arrival', accessor: ((row: FishingTrip) => row.arrivalDate ? new Date(row.arrivalDate).toLocaleDateString() : '-') },
    { header: 'Status', accessor: 'tripStatus' as keyof FishingTrip },
    { 
      header: 'Actions', 
      accessor: ((row: FishingTrip) => (
        <div className="action-buttons">
          <button className="btn-edit" onClick={() => handleEdit(row)}>Edit</button>
          <button className="btn-delete" onClick={() => row.id && handleDelete(row.id)}>Delete</button>
        </div>
      ))
    },
  ];

  if (loading) {
    return <div className="loading">Loading fishing trips...</div>;
  }

  return (
    <div className="trips-page">
      <div className="page-header">
        <h1>Fishing Trips</h1>
        <button className="btn-primary" onClick={() => setShowForm(true)}>
          ➕ Start New Trip
        </button>
      </div>

      {showForm && (
        <Card title={editingTrip ? 'Edit Fishing Trip' : 'Start New Fishing Trip'}>
          <form onSubmit={handleSubmit} className="trip-form">
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
                <label>Fishing Permit *</label>
                <select
                  value={formData.permitId}
                  onChange={(e) => setFormData({ ...formData, permitId: parseInt(e.target.value) })}
                  required
                >
                  <option value={0}>Select a permit</option>
                  {permits.filter(p => p.isActive).map((permit) => (
                    <option key={permit.id} value={permit.id}>
                      {permit.permitNumber}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Departure Date *</label>
                <input
                  type="datetime-local"
                  value={formData.departureDate.split('.')[0]}
                  onChange={(e) => setFormData({ ...formData, departureDate: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Departure Port *</label>
                <input
                  type="text"
                  value={formData.departurePort || ''}
                  onChange={(e) => setFormData({ ...formData, departurePort: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Arrival Date</label>
                <input
                  type="datetime-local"
                  value={formData.arrivalDate ? formData.arrivalDate.split('.')[0] : ''}
                  onChange={(e) => setFormData({ ...formData, arrivalDate: e.target.value })}
                />
              </div>

              <div className="form-group">
                <label>Arrival Port</label>
                <input
                  type="text"
                  value={formData.arrivalPort || ''}
                  onChange={(e) => setFormData({ ...formData, arrivalPort: e.target.value })}
                />
              </div>

              <div className="form-group">
                <label>Trip Status *</label>
                <select
                  value={formData.tripStatus}
                  onChange={(e) => setFormData({ ...formData, tripStatus: e.target.value })}
                  required
                >
                  <option value="Planned">Planned</option>
                  <option value="In Progress">In Progress</option>
                  <option value="Active">Active</option>
                  <option value="Completed">Completed</option>
                  <option value="Cancelled">Cancelled</option>
                </select>
              </div>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-primary">
                {editingTrip ? 'Update Trip' : 'Start Trip'}
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

      <Card title="All Fishing Trips">
        <DataTable data={trips} columns={tripColumns} emptyMessage="No fishing trips found." />
      </Card>
    </div>
  );
};
