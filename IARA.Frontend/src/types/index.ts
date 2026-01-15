// Auth Types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  middleName: string;
  lastName: string;
  email: string;
  password: string;
  phone: string;
  egn: string;
  address: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  userName: string;
  email: string;
  role: string;
  userId: number;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface ValidateTokenResponse {
  valid: boolean;
  userId: string;
  email: string;
  role: string;
}

// Base Filter
export interface BaseFilter<T> {
  freeTextSearch?: string;
  filters?: T;
  page: number;
  pageSize: number;
}

// Commercial Fishing Types
export interface Ship {
  shipId?: number;
  internationalNumber?: string;
  radioCallSign?: string;
  externalMarking: string;
  name?: string;
  length: number;
  width: number;
  grossTonnage?: number;
  draft?: number;
  mainEnginePower?: number;
  fuelType?: string;
  fuelCapacity?: number;
  ownerId?: number;
  ownerName?: string;
  activePermitsCount?: number;
}

export interface ShipEquipment {
  equipmentId?: number;
  shipId: number;
  equipmentType: string;
  equipmentName?: string;
  quantity: number;
  length?: number;
  meshSize?: number;
  isActive: boolean;
  shipName?: string;
  externalMarking?: string;
}

export interface FishingPermit {
  id?: number;
  permitNumber: string;
  shipId: number;
  issueDate: string;
  expiryDate: string;
  status?: string;
  isActive: boolean;
}

export interface FishingTrip {
  id?: number;
  shipId: number;
  permitId: number;
  departureDate: string;
  arrivalDate?: string;
  departurePort?: string;
  arrivalPort?: string;
  tripStatus: string;
  shipName?: string;
  internationalNumber?: string;
  permitNumber?: string;
}

export interface FishingOperation {
  id?: number;
  operationId?: number;
  tripId: number;
  equipmentId: number;
  startDate: string;
  endDate?: string;
  location?: string;
  latitude?: number;
  longitude?: number;
  durationHours?: number;
  equipmentName?: string;
}

export interface Catch {
  id?: number;
  catchId?: number;
  operationId: number;
  speciesName: string;
  weightKg: number;
}

export interface Landing {
  id?: number;
  fishingTripId: number;
  landingDate: string;
  port: string;
  totalWeight?: number;
  landingNumber?: string;
  shipName?: string;
}

export interface ShipCrew {
  id?: number;
  shipId: number;
  personId: number;
  position: string;
  startDate: string;
  endDate?: string;
}

export interface TransportDocument {
  id?: number;
  documentNumber: string;
  issueDate: string;
  originPort?: string;
  destinationPort?: string;
}

export interface TransportLine {
  id?: number;
  transportDocumentId: number;
  fishSpecies: string;
  weight: number;
  price?: number;
}

export interface LandingLine {
  id?: number;
  landingId: number;
  fishSpecies: string;
  weight: number;
  price?: number;
}

// Inspections Types
export interface Inspection {
  id?: number;
  inspectorId: number;
  inspectionDate: string;
  inspectionType: string;
  shipId?: number;
  location?: string;
  notes?: string;
  status?: string;
  result?: string;
}

export interface Violation {
  id?: number;
  inspectionId: number;
  violationType: string;
  description: string;
  fineAmount?: number;
  status?: string;
  issuedOn?: string;
  severity?: string;
}

// Recreational Fishing Types
export interface RecreationalFisherman {
  id?: number;
  personId?: number;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  address?: string;
}

export interface RecreationalTicket {
  ticketId: number;
  ticketNumber: string;
  fishermanName?: string;
  fishermanEGN?: string;
  ticketTypeName: string;
  issuedOn: string;
  validFrom: string;
  validUntil: string;
  price: number;
  purchaseChannel: string;
  qrCode?: string;
  isActive: boolean;
  // For request DTOs
  recFishermanId?: number;
  ticketTypeId?: number;
}

export interface RecreationalTicketType {
  ticketTypeId?: number;
  name: string;
  validDays: number;
  price: number;
  description?: string;
}

export interface RecreationalCatch {
  recCatchId?: number;
  ticketId: number;
  catchDate: string;
  speciesName: string;
  weightKg?: number;
  location?: string;
  quantity: number;
  registeredVia?: string;
}

// Registry Types
export interface Person {
  id?: number;
  firstName: string;
  lastName: string;
  egn?: string;
  email?: string;
  phone?: string;
  address?: string;
}

export interface LegalEntity {
  id?: number;
  name: string;
  eik?: string;
  email?: string;
  phone?: string;
  address?: string;
}

export interface Inspector {
  id?: number;
  personId: number;
  badgeNumber?: string;
  department?: string;
  fullName?: string;
  territory?: string;
  isActive?: boolean;
}

export interface ShipOwner {
  id?: number;
  personId?: number;
  legalEntityId?: number;
  registrationDate?: string;
}

export interface Qualification {
  id?: number;
  personId: number;
  qualificationType: string;
  issueDate: string;
  expiryDate?: string;
  certificateNumber?: string;
}
