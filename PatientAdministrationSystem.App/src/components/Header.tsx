import React from 'react';
import { AppBar, Toolbar, Typography } from '@mui/material';
import { HealingOutlined } from '@mui/icons-material';

const Header: React.FC = () => {
    return (
        <AppBar position="static">
            <Toolbar>
            <HealingOutlined /> <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Patient Administration System
                </Typography>
            </Toolbar>
        </AppBar>
    );
};

export default Header;
