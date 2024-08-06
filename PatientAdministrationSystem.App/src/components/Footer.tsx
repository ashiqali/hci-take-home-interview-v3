import React from 'react';
import { Box, Typography } from '@mui/material';

const Footer: React.FC = () => {
    return (
        <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', backgroundColor: '#f8f8f8', textAlign: 'center' }}>
            <Typography variant="body2" color="textSecondary">
                &copy; {new Date().getFullYear()} Patient Administration System. All rights reserved.
            </Typography>
        </Box>
    );
};

export default Footer;
