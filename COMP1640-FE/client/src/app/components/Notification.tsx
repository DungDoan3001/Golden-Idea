import React from 'react'
import { Snackbar, styled, Alert} from "@mui/material";
import { makeStyles } from '@mui/styles';

const Root = styled('div')(({ theme }) => ({
    top: theme.spacing(1),
  }));
interface props{
    notify:any, 
    setNotify: (notify:any) => void,
}
export default function Notification({notify,setNotify}:props) {

    const handleClose = (event: any, reason: string) => {
        if (reason === 'clickaway') {
            return;
        }
        setNotify({
            ...notify,
            isOpen: false
        })
    }

    return (
        <Root> 
        <Snackbar
        open={notify.isOpen}
        autoHideDuration={3000}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        onClose={handleClose}>
        <Alert
            severity={notify.type}
            onClose={()=>handleClose}>
            {notify.message}
        </Alert>
    </Snackbar>
    </Root>
       
    )
}