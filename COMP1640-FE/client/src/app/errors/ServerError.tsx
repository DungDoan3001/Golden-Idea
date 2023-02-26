import { Button, Container, Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import './style.scss'
import Image500 from '../assets/500.svg'

export default function ServerError() {
    const { state } = useLocation();

    return (
        <div id="notfound">
            <div className="notfound">
                <img src={Image500} alt="error" className="img_error" />
                {state?.error ? (
                    <>
                        <Typography gutterBottom variant='h1' color='secondary'>
                            {state.error.title}
                        </Typography>
                        <Divider />
                        <Typography sx={{ fontFamily: 'Montserrat', color: '#787878', fontWeight: 300 }}>{state.error.detail || 'Internal server error'}</Typography>
                        <Button variant="contained"
                            component={Link} to='/home'
                            sx={{ marginTop: '10px' }}>Back Home</Button>
                    </>
                ) : (
                    <><Typography gutterBottom variant='h2' color='secondary'>OOPS! Server Error</Typography>
                        <Typography sx={{ fontFamily: 'Montserrat', color: '#787878', fontWeight: 300, textAlign: 'justify' }}>This error is caused by an issue with our server and our team is already working on resolving the problem. Please try again later, or contact our technical support team if the problem persists. We apologize for any inconvenience this may have caused.</Typography>

                        <Button variant="contained"
                            component={Link} to='/home'
                            sx={{ marginTop: '10px' }}>Back Home</Button></>

                )}
            </div>
        </div>
    )
}