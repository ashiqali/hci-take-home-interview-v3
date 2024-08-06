/* eslint-disable @typescript-eslint/no-explicit-any */
import apiClient from '../api/api-client';
import axios from 'axios';
import { Patient, PatientVisit } from '../services/patient-interface';

export const fetchPatients = async (): Promise<Patient[]> => {
    try {
        const response = await apiClient.get<{ $values: Patient[] }>('/patients');
        return response.data.$values;
    } catch (error) {
        console.error('Error fetching patients:', error);
        throw error;
    }
};

export const getPatientById = async (id: string): Promise<Patient> => {
    try {
        const response = await apiClient.get<Patient>(`/patients/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching patient with ID ${id}:`, error);
        throw error; 
    }
};

export const fetchPatientVisits = async (patientId: string): Promise<PatientVisit[] | 404> => {
    try {
        const response = await apiClient.get<{ $values: PatientVisit[] }>(`/patients/${patientId}/visits`);
        return response.data.$values;
    } catch (error) {
        // Check if the error is an AxiosError and has a response property
        if (axios.isAxiosError(error) && error.response?.status === 404) {
            return 404; // Return 404 to indicate that no visits were found
        }
        console.error(`Error fetching visits for patient ${patientId}:`, error);
        throw error;
    }
};