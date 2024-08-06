import React from 'react';
import { Modal, Box, Typography, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Button, CircularProgress, Paper } from '@mui/material';
import { format } from 'date-fns';
import { Patient, PatientVisit } from '../services/patient-interface';
import { Close, LocationOn } from '@mui/icons-material';

interface VisitsModalProps {
    openModal: boolean;
    selectedPatient: Patient | null;
    visits: PatientVisit[];
    loading: boolean;
    handleCloseModal: () => void;
}

const VisitsModal: React.FC<VisitsModalProps> = ({
    openModal,
    selectedPatient,
    visits,
    loading,
    handleCloseModal
}) => {
    return (
        <Modal
            open={openModal}
            onClose={handleCloseModal}
            aria-labelledby="modal-title"
            aria-describedby="modal-description"
        >
            <Box className="modal-box">
                <Typography variant="h6" id="modal-title">
                    <center> Visits for {selectedPatient?.firstName} {selectedPatient?.lastName}</center>
                </Typography>
                {loading ? (
                    <CircularProgress />
                ) : (
                    <TableContainer component={Paper}>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Date</TableCell>
                                    <TableCell>Hospital Name</TableCell>
                                    <TableCell>Hospital Address</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {visits.map(visit => (
                                    <TableRow key={visit.id}>
                                        <TableCell>{format(new Date(visit.date), 'dd MMM yyyy')}</TableCell>
                                        <TableCell>{visit.hospitalName}</TableCell>
                                        <TableCell>
                                            <LocationOn style={{ verticalAlign: 'middle', marginRight: 4 }} />
                                            {visit.hospitalAddress}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
                <Button
                    variant="contained"
                    color="secondary"
                    onClick={handleCloseModal}
                    className="close-button"
                    startIcon={<Close />}
                >
                    Close
                </Button>
            </Box>
        </Modal>
    );
};

export default VisitsModal;
