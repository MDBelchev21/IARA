import React, { useState, useEffect } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { fishingPermitService } from '../../services/fishingPermitService';
import { shipService } from '../../services/shipService';
import type { FishingPermit, Ship } from '../../types';
import './FishingPermits.css';

export const FishingPermits = () => {
  const [permits, setPermits] = useState<FishingPermit[]>([]);
  const [ships, setShips] = useState<Ship[]>([]);
  const [loading, setLoading] = useState(false);
  const [showForm, setShowForm] = useState(false);

  const [formData, setFormData] = useState({
    shipId: 0,
    permitNumber: '',
    issueDate: new Date().toISOString().split('T')[0],
    expiryDate: '',
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [permitsData, shipsData] = await Promise.all([
        fishingPermitService.getAll({ page: 1, pageSize: 100 }),
        shipService.getAll({ page: 1, pageSize: 100 }),
      ]);
      setPermits(permitsData);
      setShips(shipsData);
    } catch (err: any) {
      console.error('Failed to fetch data:', err);
      alert('Failed to load permits. Please check if you have permission to view permits.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      const permitData: FishingPermit = {
        shipId: formData.shipId,
        permitNumber: formData.permitNumber,
        issueDate: formData.issueDate,
        expiryDate: formData.expiryDate,
        isActive: true,
      };
      
      await fishingPermitService.add(permitData);
      await fetchData();
      setShowForm(false);
      resetForm();
      alert('Permit application submitted successfully!');
    } catch (err: any) {
      console.error('Failed to apply for permit:', err);
      alert('Failed to apply for permit. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleRevoke = async (id: number) => {
    if (!window.confirm('Are you sure you want to revoke this permit?')) return;
    
    try {
      await fishingPermitService.revoke(id);
      await fetchData();
      alert('Permit revoked successfully!');
    } catch (err: any) {
      console.error('Failed to revoke permit:', err);
      alert('Failed to revoke permit.');
    }
  };

  const resetForm = () => {
    setFormData({
      shipId: 0,
      permitNumber: '',
      issueDate: new Date().toISOString().split('T')[0],
      expiryDate: '',
    });
  };

  const getShipName = (shipId: number) => {
    const ship = ships.find(s => s.shipId === shipId);
    return ship ? ship.name || ship.externalMarking : 'Unknown';
  };

  const permitColumns = [
    { header: 'Permit #', accessor: 'permitNumber' as keyof FishingPermit },
    { header: 'Ship', accessor: ((row: FishingPermit) => getShipName(row.shipId)) },
    { header: 'Issue Date', accessor: ((row: FishingPermit) => new Date(row.issueDate).toLocaleDateString()) },
    { header: 'Expiry Date', accessor: ((row: FishingPermit) => new Date(row.expiryDate).toLocaleDateString()) },
    { 
      header: 'Status', 
      accessor: ((row: FishingPermit) => {
        const now = new Date();
        const expiry = new Date(row.expiryDate);
        if (!row.isActive) return '🔴 Revoked';
        if (expiry < now) return '⚠️ Expired';
        return '✅ Active';
      })
    },
    {
      header: 'Actions',
      accessor: ((row: FishingPermit) => (
        <div className="action-buttons">
          {row.isActive && (
            <button className="btn-revoke" onClick={() => handleRevoke(row.id!)}>
              Revoke
            </button>
          )}
        </div>
      )),
    },
  ];

  if (loading && permits.length === 0) {
    return <div className="loading">Loading fishing permits...</div>;
  }

  return (
    <div className="permits-page">
      <div className="page-header">
        <div>
          <h1>Fishing Permits</h1>
          <p>Manage your commercial fishing permits and applications</p>
        </div>
        <button className="btn-primary" onClick={() => setShowForm(true)}>
          ➕ Apply for New Permit
        </button>
      </div>

      {showForm && (
        <Card title="Apply for Fishing Permit">
          <form onSubmit={handleSubmit} className="permit-form">
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
                <label>Permit Number *</label>
                <input
                  type="text"
                  value={formData.permitNumber}
                  onChange={(e) => setFormData({ ...formData, permitNumber: e.target.value })}
                  placeholder="e.g., FP-2026-001"
                  required
                />
              </div>

              <div className="form-group">
                <label>Issue Date *</label>
                <input
                  type="date"
                  value={formData.issueDate}
                  onChange={(e) => setFormData({ ...formData, issueDate: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Expiry Date *</label>
                <input
                  type="date"
                  value={formData.expiryDate}
                  onChange={(e) => setFormData({ ...formData, expiryDate: e.target.value })}
                  min={formData.issueDate}
                  required
                />
              </div>
            </div>

            <div className="form-info">
              <p>ℹ️ <strong>Note:</strong> Your permit application will be reviewed by fisheries authorities. You will be notified once it's approved.</p>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-primary" disabled={loading}>
                {loading ? 'Submitting...' : 'Submit Application'}
              </button>
              <button 
                type="button" 
                className="btn-secondary" 
                onClick={() => { setShowForm(false); resetForm(); }}
              >
                Cancel
              </button>
            </div>
          </form>
        </Card>
      )}

      <Card title="My Fishing Permits">
        <DataTable 
          data={permits} 
          columns={permitColumns} 
          emptyMessage="No fishing permits found. Apply for your first permit!"
        />
      </Card>
    </div>
  );
};
