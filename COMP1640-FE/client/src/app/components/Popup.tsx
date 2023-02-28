import React from 'react'
import { makeStyles } from '@mui/styles';
import { Close } from '@mui/icons-material';
import { Dialog, DialogContent, DialogTitle, IconButton, Typography } from '@mui/material';

const useStyles = makeStyles((theme: { spacing: (arg0: number) => any; }) => ({
    dialogWrapper: {
        padding: theme.spacing(2),
        position: 'absolute',
        top: theme.spacing(5)
    },
    dialogTitle: {
        paddingRight: '0px'
    }
}))

interface Props {
    title: any,
    children: any,
    openPopup: any,
    setOpenPopup: (popup: any) => any,
}
export default function Popup({ title, children, openPopup, setOpenPopup }: Props) {
    const classes = useStyles();

    return (
        <Dialog open={openPopup} maxWidth="md" classes={{ paper: classes.dialogWrapper }}>
            <DialogTitle className={classes.dialogTitle}>
                <div style={{ display: 'flex' }}>
                    <Typography variant="h6" component="div" style={{ flexGrow: 1 }}>
                        {title}
                    </Typography>
                    <IconButton
                        color="secondary"
                        onClick={() => { setOpenPopup(false) }}>
                        <Close />
                    </IconButton>
                </div>
            </DialogTitle>
            <DialogContent dividers>
                {children}
            </DialogContent>
        </Dialog>
    )
}