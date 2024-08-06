import React from 'react';
import { Paper, Typography, TableContainer, Table, TableHead, TableRow, TableCell, TableSortLabel, TableBody, Button, Pagination } from '@mui/material';
import { format } from 'date-fns';
import { Patient } from '../services/patient-interface';
import { Visibility } from '@mui/icons-material';

interface PatientDetailsTableProps {
    patients: Patient[];
    totalRecords: number;
    sortColumn: string;
    sortDirection: 'asc' | 'desc';
    selectedRowId: string | null;
    page: number;
    rowsPerPage: number;
    handleSort: (column: string) => void;
    handlePatientSelect: (patient: Patient) => void;
    handlePageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
}

const PatientDetailsTable: React.FC<PatientDetailsTableProps> = ({
    patients,
    totalRecords,
    sortColumn,
    sortDirection,
    selectedRowId,
    page,
    rowsPerPage,
    handleSort,
    handlePatientSelect,
    handlePageChange
}) => {
    return (
        <Paper style={{ padding: '16px', margin: '16px' }}>
            <center><Typography variant="h5">Patients Details</Typography></center>
            <Typography variant="body1" style={{ margin: '16px 0' }}>
                Total Records: {totalRecords}
            </Typography>
            <TableContainer>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <TableSortLabel
                                    active={sortColumn === 'firstName'}
                                    direction={sortDirection}
                                    onClick={() => handleSort('firstName')}
                                >
                                    First Name
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortColumn === 'lastName'}
                                    direction={sortDirection}
                                    onClick={() => handleSort('lastName')}
                                >
                                    Last Name
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Date of Birth</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {patients.map(patient => (
                            <TableRow key={patient.id} selected={selectedRowId === patient.id}>
                                <TableCell>{patient.firstName}</TableCell>
                                <TableCell>{patient.lastName}</TableCell>
                                <TableCell>{patient.email}</TableCell>
                                <TableCell>{format(new Date(patient.dateOfBirth), 'dd/MM/yyyy')}</TableCell>
                                <TableCell>
                                    <Button
                                        variant="contained"
                                        color="primary"
                                        startIcon={<Visibility />}
                                        onClick={() => handlePatientSelect(patient)}
                                    >
                                        View Visits
                                    </Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <Pagination
                count={Math.ceil(totalRecords / rowsPerPage)}
                page={page}
                onChange={handlePageChange}
                style={{ marginTop: '16px', display: 'flex', justifyContent: 'center' }}
            />
        </Paper>
    );
};

export default PatientDetailsTable;
