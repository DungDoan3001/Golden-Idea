import { Button, Container, Divider, Paper, Typography } from "@mui/material";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import './style.scss'

export default function ServerError() {
    const { state } = useLocation();

    return (
        <div id="notfound">
            <div className="notfound">
                <img src="https://img.freepik.com/free-vector/500-internal-server-error-concept-illustration_114360-5572.jpg?w=300" alt="error" />
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
                    <><Typography gutterBottom variant='h1' color='secondary'>OOPS! Server Error</Typography>
                        <Typography sx={{ fontFamily: 'Montserrat', color: '#787878', fontWeight: 300 }}>Sorry but the page you are looking for does not exist, have been removed. name changed or is temporarily unavailable</Typography>

                        <Button variant="contained"
                            component={Link} to='/home'
                            sx={{ marginTop: '10px' }}>Back Home</Button></>

                )}
            </div>
        </div>
    )
}