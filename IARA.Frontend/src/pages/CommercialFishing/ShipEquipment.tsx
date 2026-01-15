import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { DataTable } from '../../components/common/DataTable';
import { shipEquipmentService } from '../../services/shipEquipmentService';
import { shipService } from '../../services/shipService';
import type { ShipEquipment, Ship } from '../../types';
import './ShipEquipment.css';

export const ShipEquipmentPage: React.FC = () => {
  const [equipment, setEquipment] = useState<ShipEquipment[]>([]);
  const [ships, setShips] = useState<Ship[]>([]);
  const [loading, setLoading] = useState(true);
  const [showForm, setShowForm] = useState(false);
  const [editingEquipment, setEditingEquipment] = useState<ShipEquipment | null>(null);
  const [formData, setFormData] = useState<Partial<ShipEquipment>>({
    shipId: 0,
    equipmentType: 'Net',
    equipmentName: '',
    quantity: 1,
    length: undefined,
    meshSize: undefined,
    isActive: true,
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const shipsData = await shipService.getAll({ page: 1, pageSize: 100 });
      setShips(shipsData);
      
      // Try to load equipment, but handle if endpoint doesn't exist yet
      try {
        const equipmentData = await shipEquipmentService.getAll({ page: 1, pageSize: 100 });
        setEquipment(equipmentData);
      } catch (error) {
        console.warn('Ship equipment endpoint not available yet:', error);
        alert('Ship Equipment management is not yet available. Backend API endpoint needs to be implemented.');
      }
    } catch (error) {
      console.error('Failed to load ship equipment:', error);
      alert('Failed to load data. Please check your connection.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingEquipment) {
        await shipEquipmentService.edit({ ...formData, equipmentId: editingEquipment.equipmentId } as ShipEquipment);
      } else {
        await shipEquipmentService.add(formData as any);
      }
      await loadData();
      resetForm();
    } catch (error) {
      console.error('Failed to save equipment:', error);
      alert('Failed to save equipment');
    }
  };

  const handleEdit = (equip: ShipEquipment) => {
    setEditingEquipment(equip);
    setFormData({
      shipId: equip.shipId,
      equipmentType: equip.equipmentType,
      equipmentName: equip.equipmentName,
      quantity: equip.quantity,
      length: equip.length,
      meshSize: equip.meshSize,
      isActive: equip.isActive,
    });
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this equipment?')) return;
    try {
      await shipEquipmentService.delete(id);
      await loadData();
    } catch (error) {
      console.error('Failed to delete equipment:', error);
      alert('Failed to delete equipment');
    }
  };

  const resetForm = () => {
    setShowForm(false);
    setEditingEquipment(null);
    setFormData({
      shipId: 0,
      equipmentType: 'Net',
      equipmentName: '',
      quantity: 1,
      length: undefined,
      meshSize: undefined,
      isActive: true,
    });
  };

  const getShipName = (shipId: number) => {
    const ship = ships.find(s => s.shipId === shipId);
    return ship?.name || ship?.externalMarking || '-';
  };

  const equipmentColumns = [
    { header: 'Ship', accessor: ((row: ShipEquipment) => getShipName(row.shipId)) },
    { header: 'Type', accessor: 'equipmentType' as keyof ShipEquipment },
    { header: 'Name', accessor: ((row: ShipEquipment) => row.equipmentName || '-') },
    { header: 'Quantity', accessor: 'quantity' as keyof ShipEquipment },
    { header: 'Length (m)', accessor: ((row: ShipEquipment) => row.length ? `${row.length}` : '-') },
    { header: 'Mesh Size (mm)', accessor: ((row: ShipEquipment) => row.meshSize ? `${row.meshSize}` : '-') },
    { 
      header: 'Status', 
      accessor: ((row: ShipEquipment) => row.isActive ? '✅ Active' : '❌ Inactive')
    },
    {
      header: 'Actions',
      accessor: ((row: ShipEquipment) => (
        <div className="action-buttons">
          <button className="btn-edit" onClick={() => handleEdit(row)}>Edit</button>
          <button className="btn-delete" onClick={() => handleDelete(row.equipmentId!)}>Delete</button>
        </div>
      )),
    },
  ];

  if (loading) {
    return <div className="loading">Loading ship equipment...</div>;
  }

  return (
    <div className="ship-equipment-page">
      <div className="page-header">
        <h1>Ship Equipment Management</h1>
        <button className="btn-add" onClick={() => setShowForm(true)}>+ Add Equipment</button>
      </div>

      {showForm && (
        <Card title={editingEquipment ? 'Edit Equipment' : 'Add New Equipment'}>
          <form onSubmit={handleSubmit} className="equipment-form">
            <div className="form-row">
              <div className="form-group">
                <label>Ship *</label>
                <select
                  value={formData.shipId}
                  onChange={(e) => setFormData({ ...formData, shipId: parseInt(e.target.value) })}
                  required
                >
                  <option value={0}>Select Ship</option>
                  {ships.map((ship) => (
                    <option key={ship.shipId} value={ship.shipId}>
                      {ship.name || ship.externalMarking} ({ship.externalMarking})
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Equipment Type *</label>
                <select
                  value={formData.equipmentType}
                  onChange={(e) => setFormData({ ...formData, equipmentType: e.target.value })}
                  required
                >
                  <option value="Net">Net</option>
                  <option value="Trawl">Trawl</option>
                  <option value="Longline">Longline</option>
                  <option value="Trap">Trap</option>
                  <option value="Dredge">Dredge</option>
                  <option value="Other">Other</option>
                </select>
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Equipment Name</label>
                <input
                  type="text"
                  value={formData.equipmentName}
                  onChange={(e) => setFormData({ ...formData, equipmentName: e.target.value })}
                  placeholder="e.g., Gillnet A1"
                />
              </div>

              <div className="form-group">
                <label>Quantity *</label>
                <input
                  type="number"
                  value={formData.quantity}
                  onChange={(e) => setFormData({ ...formData, quantity: parseInt(e.target.value) })}
                  min="1"
                  required
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Length (meters)</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.length || ''}
                  onChange={(e) => setFormData({ ...formData, length: e.target.value ? parseFloat(e.target.value) : undefined })}
                  placeholder="Optional"
                />
              </div>

              <div className="form-group">
                <label>Mesh Size (mm)</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.meshSize || ''}
                  onChange={(e) => setFormData({ ...formData, meshSize: e.target.value ? parseFloat(e.target.value) : undefined })}
                  placeholder="Optional"
                />
              </div>
            </div>

            <div className="form-group">
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  checked={formData.isActive}
                  onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                />
                Active
              </label>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-submit">
                {editingEquipment ? 'Update' : 'Add'} Equipment
              </button>
              <button type="button" className="btn-cancel" onClick={resetForm}>
                Cancel
              </button>
            </div>
          </form>
        </Card>
      )}

      <Card title="All Ship Equipment">
        <DataTable data={equipment} columns={equipmentColumns} emptyMessage="No equipment registered yet." />
      </Card>
    </div>
  );
};
