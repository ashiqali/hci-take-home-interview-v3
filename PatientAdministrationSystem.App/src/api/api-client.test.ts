import axios, { AxiosResponse }  from 'axios';
import { Patient, PatientVisit } from '../services/patient-interface';
import { fetchPatients, getPatientById, fetchPatientVisits } from '../services/patient-service';
import apiClient from './api-client';

jest.mock('../api/api-client');
const mockedApiClient = apiClient as jest.Mocked<typeof apiClient>;

describe('Patient Service', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });

  describe('fetchPatients', () => {
    it('should fetch patients successfully', async () => {
      const mockPatients: Patient[] = [
        {
            id: '1', firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01',
            createdDate: ''
        },
        {
            id: '2', firstName: 'Jane', lastName: 'Doe', email: 'jane@example.com', dateOfBirth: '1992-02-02',
            createdDate: ''
        },
      ];

      mockedApiClient.get.mockResolvedValue({ data: { $values: mockPatients } });

      const result = await fetchPatients();

      expect(result).toEqual(mockPatients);
      expect(mockedApiClient.get).toHaveBeenCalledWith('/patients');
    });

    it('should throw an error when fetching patients fails', async () => {
      const errorMessage = 'Network Error';
      mockedApiClient.get.mockRejectedValue(new Error(errorMessage));

      await expect(fetchPatients()).rejects.toThrow(errorMessage);
    });
  });

  describe('getPatientById', () => {
    it('should fetch a patient by ID successfully', async () => {
      const mockPatient: Patient = { id: '1', firstName: 'John', lastName: 'Doe', email: 'john@example.com', dateOfBirth: '1990-01-01', createdDate:'2024-01-15' };

      mockedApiClient.get.mockResolvedValue({ data: mockPatient });

      const result = await getPatientById('1');

      expect(result).toEqual(mockPatient);
      expect(mockedApiClient.get).toHaveBeenCalledWith('/patients/1');
    });

    it('should throw an error when fetching a patient by ID fails', async () => {
      const errorMessage = 'Patient not found';
      mockedApiClient.get.mockRejectedValue(new Error(errorMessage));

      await expect(getPatientById('999')).rejects.toThrow(errorMessage);
    });
  });

  describe('fetchPatientVisits', () => {
    it('should fetch patient visits successfully', async () => {
      const mockVisits: PatientVisit[] = [
        { id: 1, date: '2023-01-01', hospitalName: 'Hospital A', hospitalAddress: 'Address A' },
        { id: 2, date: '2023-02-02', hospitalName: 'Hospital B', hospitalAddress: 'Address B' },
      ];

      mockedApiClient.get.mockResolvedValue({ data: { $values: mockVisits } });

      const result = await fetchPatientVisits('1');

      expect(result).toEqual(mockVisits);
      expect(mockedApiClient.get).toHaveBeenCalledWith('/patients/1/visits');
    });

    it('should return 404 when no visits are found', async () => {
      const axiosError = new axios.AxiosError();
      axiosError.response = { status: 404 } as AxiosResponse;
      mockedApiClient.get.mockRejectedValue(axiosError);

      const result = await fetchPatientVisits('1');

      expect(result).toBe(404);
    });

    it('should throw an error for non-404 errors', async () => {
      const errorMessage = 'Server Error';
      mockedApiClient.get.mockRejectedValue(new Error(errorMessage));

      await expect(fetchPatientVisits('1')).rejects.toThrow(errorMessage);
    });
  });
});