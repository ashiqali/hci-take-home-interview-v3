import React, { useEffect, useState } from 'react';
import { Container, CircularProgress, Snackbar, TextField, Autocomplete } from '@mui/material';
import { Alert } from '@mui/material';
import { Patient, PatientVisit } from './services/patient-interface';
import { fetchPatients, fetchPatientVisits,} from './services/patient-service';
import { format } from 'date-fns';
import Header from './components/Header';
import Footer from './components/Footer';
import PatientDetailsTable from './components/PatientDetailsTable';
import VisitsModal from './components/VisitsModal';
import ExportDataForm from './components/ExportDataForm';
import './App.css';
import * as XLSX from 'xlsx';

const App: React.FC = () => {
    const [patients, setPatients] = useState<Patient[]>([]);
    const [selectedPatient, setSelectedPatient] = useState<Patient | null>(null);
    const [selectedRowId, setSelectedRowId] = useState<string | null>(null);
    const [visits, setVisits] = useState<PatientVisit[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [page, setPage] = useState<number>(1);
    const [rowsPerPage] = useState<number>(5);
    const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');
    const [sortColumn, setSortColumn] = useState<string>('firstName');
    const [dateRange, setDateRange] = useState<{ start: string | null, end: string | null }>({ start: null, end: null });
    const [openModal, setOpenModal] = useState<boolean>(false);

    useEffect(() => {
        const loadPatients = async () => {
            setLoading(true);
            setError(null);
            try {
                const patientsData = await fetchPatients();
                if (Array.isArray(patientsData)) {
                    setPatients(patientsData);
                } else {
                    setPatients([]);
                    console.error('Error: Expected an array of patients, but received:', patientsData);
                }
            } catch (error) {
                console.error('Failed to fetch patients:', error);
                setError('Failed to fetch patients');
                setPatients([]);
            } finally {
                setLoading(false);
            }
        };

        loadPatients();
    }, []);

    const handlePatientSelect = async (patient: Patient) => {
        setSelectedPatient(patient);
        setSelectedRowId(patient.id);
        setOpenModal(false);
        setLoading(false);
        setError(null);
        try {
            const response = await fetchPatientVisits(patient.id);
            if (response === 404) {
                setVisits([]);
                setError('No visits found for the selected patient.');
            } else if (Array.isArray(response)) {
                const sortedVisits = response.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
                setLoading(true);
                setVisits(sortedVisits);
                setOpenModal(true);
            } else {
                setVisits([]);
                console.error('Error: Expected an array of visits, but received:', response);
                setError('Unexpected response format from server.');
            }
        } catch (error) {
            console.error('Failed to fetch patient visits:', error);
            setError('Failed to fetch patient visits');
            setVisits([]);
        } finally {
            setLoading(false);
        }
    };

    const handleCloseModal = () => setOpenModal(false);

    const handlePageChange = (_event: React.ChangeEvent<unknown>, value: number) => setPage(value);

    const handleSort = (column: string) => {
        const isAsc = sortColumn === column && sortDirection === 'asc';
        setSortDirection(isAsc ? 'desc' : 'asc');
        setSortColumn(column);
    };

    const handleDateRangeChange = (e: React.ChangeEvent<HTMLInputElement>, field: 'start' | 'end') => {
        setDateRange({ ...dateRange, [field]: e.target.value });
    };

    const handleExportData = () => {
        const filteredPatients = patients.filter(patient => {
            const patientCreatedDate = new Date(patient.createdDate).getTime();
            const startDate = dateRange.start ? new Date(dateRange.start).getTime() : null;
            const endDate = dateRange.end ? new Date(dateRange.end).getTime() : null;
    
            if (startDate && endDate) {
                return patientCreatedDate >= startDate && patientCreatedDate <= endDate;
            } else if (startDate) {
                return patientCreatedDate >= startDate;
            } else if (endDate) {
                return patientCreatedDate <= endDate;
            } else {
                return true;
            }
        });
    
        const exportData = filteredPatients.map(patient => ({
            FirstName: patient.firstName,
            LastName: patient.lastName,
            Email: patient.email,
            DateOfBirth: format(new Date(patient.dateOfBirth), 'dd/MM/yyyy'),
            CreatedDate: format(new Date(patient.createdDate), 'dd/MM/yyyy')
        }));
    
        const worksheet = XLSX.utils.json_to_sheet(exportData);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Patients');
    
        XLSX.writeFile(workbook, 'PatientsData.xlsx');
    };
    

    const filteredPatients = patients
        .filter(patient =>
            `${patient.firstName} ${patient.lastName}`.toLowerCase().includes(searchTerm.toLowerCase())
        )
        .sort((a, b) => {
            if (sortColumn === 'firstName') {
                return sortDirection === 'asc'
                    ? a.firstName.localeCompare(b.firstName)
                    : b.firstName.localeCompare(a.firstName);
            }
            return 0;
        });

    const paginatedPatients = filteredPatients.slice((page - 1) * rowsPerPage, page * rowsPerPage);

    return (
        <>
            <Header />
            <Container className="main-container">
                <Autocomplete
                    freeSolo
                    options={patients.map(patient => `${patient.firstName} ${patient.lastName}`)}
                    renderInput={(params) => (
                        <TextField
                            {...params}
                            label="Search Patients"
                            margin="normal"
                            variant="outlined"
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    )}
                    onInputChange={(_e, value) => setSearchTerm(value)}
                />

                <ExportDataForm
                    dateRange={dateRange}
                    handleDateRangeChange={handleDateRangeChange}
                    handleExportData={handleExportData}
                />

                {loading && <CircularProgress />}
                {error && <Snackbar open={true} autoHideDuration={6000} onClose={() => setError(null)} anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}>
                    <Alert onClose={() => setError(null)} severity="error">{error}</Alert>
                </Snackbar>}

                <PatientDetailsTable
                    patients={paginatedPatients}
                    totalRecords={filteredPatients.length}
                    sortColumn={sortColumn}
                    sortDirection={sortDirection}
                    selectedRowId={selectedRowId}
                    page={page}
                    rowsPerPage={rowsPerPage}
                    handleSort={handleSort}
                    handlePatientSelect={handlePatientSelect}
                    handlePageChange={handlePageChange}
                />

                <VisitsModal
                    openModal={openModal}
                    selectedPatient={selectedPatient}
                    visits={visits}
                    loading={loading}
                    handleCloseModal={handleCloseModal}
                />
            </Container>
            <Footer />
        </>
    );
};

export default App;
