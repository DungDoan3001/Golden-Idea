import React from 'react';
import { Box, Button, Container, Typography } from '@mui/material';
import Grid from '@mui/material/Grid';
import { Link } from "react-router-dom";
import Image404 from '../assets/404.svg';

export default function NotFound() {
    return (
        <div id="notfound">
            <div className="notfound">
                <img src={Image404} alt="error" className='img_error' />
                <><Typography gutterBottom variant='h1' color='secondary'>OOPS! Not Found</Typography>
                    <Typography sx={{ fontFamily: 'Montserrat', color: '#787878', fontWeight: 300, }}>Sorry but the page you are looking for does not exist, have been removed. name changed or is temporarily unavailable</Typography>

                    <Button variant="contained"
                        component={Link} to='/home'
                        sx={{ marginTop: '10px' }}>Back Home</Button></>
            </div>
        </div>
    );
}