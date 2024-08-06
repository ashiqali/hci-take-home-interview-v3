export interface Patient {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    createdDate: string;
}

export interface PatientVisit {
    id: number;
    date: string;
    hospitalName: string;
    hospitalAddress: string;
}