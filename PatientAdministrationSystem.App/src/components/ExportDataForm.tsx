import React from 'react';
import { TextField, Button } from '@mui/material';
import { FileDownload } from '@mui/icons-material';

interface ExportDataFormProps {
    dateRange: { start: string | null, end: string | null };
    handleDateRangeChange: (e: React.ChangeEvent<HTMLInputElement>, field: 'start' | 'end') => void;
    handleExportData: () => void;
}

const ExportDataForm: React.FC<ExportDataFormProps> = ({
    dateRange,
    handleDateRangeChange,
    handleExportData
}) => {
    return (
        <div className="date-export-container">
            <TextField
                label="Start Date"
                type="date"
                variant="outlined"
                margin="normal"
                InputLabelProps={{ shrink: true }}
                value={dateRange.start || ''}
                onChange={(e) => handleDateRangeChange(e as React.ChangeEvent<HTMLInputElement>, 'start')}
            />

            <TextField
                label="End Date"
                type="date"
                variant="outlined"
                margin="normal"
                InputLabelProps={{ shrink: true }}
                value={dateRange.end || ''}
                onChange={(e) => handleDateRangeChange(e as React.ChangeEvent<HTMLInputElement>, 'end')}
            />

            <Button
                variant="contained"
                color="primary"
                onClick={handleExportData}
                className="export-button"
                startIcon={<FileDownload />}
            >
                Export Data
            </Button>
        </div>
    );
};

export default ExportDataForm;
