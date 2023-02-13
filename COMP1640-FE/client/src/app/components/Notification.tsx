import React from 'react'
import { Snackbar, styled, Alert } from "@mui/material";
import { makeStyles } from '@mui/styles';

const useStyles = makeStyles((theme: { spacing: (arg0: number) => any; }) => ({
    root: {
        top: theme.spacing(9)
    }
}))
interface Props {
    notify: any
    setNotify: (notify: any) => any,
}
export default function Notification({ notify, setNotify }: Props) {

    const classes = useStyles()

    const handleClose = (event: React.SyntheticEvent | Event, reason?: string) => {
        if (reason === 'clickaway') {
            return;
        }
        setNotify({
            ...notify,
            isOpen: false
        })
    };

    return (
        <Snackbar
            className={classes.root}
            open={notify.isOpen}
            autoHideDuration={3000}
            anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
            onClose={handleClose}>
            <Alert
                severity={notify.type}
                onClose={handleClose}>
                {notify.message}
            </Alert>
        </Snackbar>
    )
}
