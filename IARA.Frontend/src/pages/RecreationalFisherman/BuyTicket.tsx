import React, { useEffect, useState } from 'react';
import { Card } from '../../components/common/Card';
import { recreationalTicketService } from '../../services/recreationalTicketService';
import { recreationalTicketTypeService } from '../../services/recreationalTicketTypeService';
import { apiClient } from '../../config/api';
import type { RecreationalTicketType } from '../../types';
import './BuyTicket.css';

export const BuyTicket: React.FC = () => {
  const [ticketTypes, setTicketTypes] = useState<RecreationalTicketType[]>([]);
  const [selectedType, setSelectedType] = useState<RecreationalTicketType | null>(null);
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadTicketTypes();
  }, []);

  const loadTicketTypes = async () => {
    try {
      const types = await recreationalTicketTypeService.getAll({ page: 1, pageSize: 100 });
      setTicketTypes(types);
    } catch (err) {
      console.error('Failed to load ticket types:', err);
      setError('Failed to load ticket types');
    }
  };

  const handlePurchase = async () => {
    if (!selectedType) return;

    setLoading(true);
    setError('');
    setSuccess(false);

    try {
      const userId = parseInt(localStorage.getItem('userId') || '0');
      
      // Get the recreational fisherman record for the current user
      const response = await apiClient.get(`/RecreationalFisherman/GetByPersonId/${userId}`);
      const recFishermanId = response.data;
      
      console.log('RecFishermanId:', recFishermanId);
      
      if (!recFishermanId) {
        setError('Recreational fisherman profile not found. Please contact support.');
        setLoading(false);
        return;
      }

      const now = new Date();
      const validUntil = new Date(now);
      validUntil.setDate(validUntil.getDate() + (selectedType.validDays || 365));

      const ticketData = {
        recFishermanId: recFishermanId,
        ticketTypeId: selectedType.ticketTypeId || 0,
        validFrom: now.toISOString(),
        validUntil: validUntil.toISOString(),
        purchaseChannel: 'Online',
      };
      
      console.log('Sending ticket data:', ticketData);

      await recreationalTicketService.add(ticketData as any);

      setSuccess(true);
      setSelectedType(null);
    } catch (err: any) {
      console.error('Failed to purchase ticket:', err);
      console.error('Error response:', err.response?.data);
      const errorMsg = err.response?.data?.errors 
        ? Object.values(err.response.data.errors).flat().join(', ')
        : err.response?.data?.title || err.message || 'Failed to purchase ticket';
      setError(errorMsg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="buy-ticket-page">
      <div className="page-header">
        <h1>Purchase Fishing Ticket</h1>
        <p>Select a ticket type to begin fishing legally</p>
      </div>

      {success && (
        <div className="alert alert-success">
          ✓ Ticket purchased successfully! You can now start fishing.
        </div>
      )}

      {error && (
        <div className="alert alert-error">
          ✗ {error}
        </div>
      )}

      <div className="ticket-types-grid">
        {ticketTypes.map((type) => (
          <Card
            key={type.ticketTypeId}
            className={`ticket-type-card ${selectedType?.ticketTypeId === type.ticketTypeId ? 'selected' : ''}`}
          >
            <div className="ticket-type-content">
              <h3>{type.name}</h3>
              <p className="ticket-description">{type.description || 'Standard fishing permit'}</p>
              <div className="ticket-details">
                <div className="detail-item">
                  <span className="detail-label">Duration:</span>
                  <span className="detail-value">{type.validDays} days</span>
                </div>
                <div className="detail-item">
                  <span className="detail-label">Price:</span>
                  <span className="detail-value">€{type.price}</span>
                </div>
              </div>
              <button
                className={`select-button ${selectedType?.ticketTypeId === type.ticketTypeId ? 'selected' : ''}`}
                onClick={() => setSelectedType(type)}
              >
                {selectedType?.ticketTypeId === type.ticketTypeId ? 'Selected' : 'Select'}
              </button>
            </div>
          </Card>
        ))}
      </div>

      {selectedType && (
        <div className="purchase-section">
          <Card title="Confirm Purchase">
            <div className="purchase-summary">
              <h4>You are purchasing: {selectedType.name}</h4>
              <p>Duration: {selectedType.validDays} days</p>
              <p className="total-price">Total: €{selectedType.price}</p>
              <button
                className="purchase-button"
                onClick={handlePurchase}
                disabled={loading}
              >
                {loading ? 'Processing...' : 'Confirm Purchase'}
              </button>
            </div>
          </Card>
        </div>
      )}
    </div>
  );
};
