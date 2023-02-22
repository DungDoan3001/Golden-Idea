import React from 'react';
import { Box, Button, Container, Typography } from '@mui/material';
import Grid from '@mui/material/Grid';
import { Link } from "react-router-dom";

export default function NotFound() {
    return (
        <Box
            sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                minHeight: '100vh'
            }}
        >
            <Container maxWidth="md">
                <Grid container spacing={2}>
                    <Grid xs={6}>
                        <Typography variant="h1" color='secondary' sx={{ fontFamily: 'Montserrat', fontWeight: '700', fontSize: 100 }}>
                            404
                        </Typography>
                        <Typography variant="h1" color='secondary'>
                            OOPS! Page not found
                        </Typography>
                        <Typography variant="h6">
                            The page you’re looking for doesn’t exist.
                        </Typography>
                        <Button variant="contained" color='info'
                            component={Link} to='/home'
                            sx={{ marginTop: '10px', fontFamily: 'Montserrat', fontWeight: '700', padding: '10px 25px', backgroundColor: 'rgb(204 167 82)', color: '#fff', borderRadius: '40px', textDecoration: 'none', transition: '0.2s all', '&:hover': { opacity: 0.8 } }}>Back Home</Button>
                    </Grid>
                    <Grid xs={6}>
                        <img
                            src="https://cdn.pixabay.com/photo/2017/03/09/12/31/error-2129569__340.jpg"
                            alt=""
                            width={500} height={250}
                        />
                    </Grid>
                </Grid>
            </Container>
        </Box>
    );
}