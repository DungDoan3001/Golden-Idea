import React from 'react'
import { Dialog, DialogTitle, DialogContent, DialogActions, Typography, IconButton, Button } from '@mui/material'
import { NotListedLocation } from '@mui/icons-material';
import { makeStyles } from '@mui/styles';


const useStyles = makeStyles((theme: { spacing: (arg0: number) => any; palette: { secondary: { light: any; main: any; }; }; }) => ({
    dialog: {
        padding: theme.spacing(2),
        position: 'absolute',
        top: theme.spacing(5)
    },
    dialogTitle: {
        textAlign: 'center'
    },
    dialogContent: {
        textAlign: 'center'
    },
    dialogAction: {
        justifyContent: 'center'
    },
    dialogConfirm: {
        justifyContent: "space-around"
    },
    titleIcon: {
        backgroundColor: theme.palette.secondary.light,
        color: theme.palette.secondary.main,
        '&:hover': {
            backgroundColor: theme.palette.secondary.light,
            cursor: 'default'
        },
        '& .MuiSvgIcon-root': {
            fontSize: '8rem',
        }
    }
}))
interface Props {
    confirmDialog: any
    setConfirmDialog: (confirmDialog: any) => any,
}
export default function ConfirmDialog({ confirmDialog, setConfirmDialog }: Props) {
    const classes = useStyles()
    return (
        <Dialog open={confirmDialog.isOpen} classes={{ paper: classes.dialog }}>
            <DialogTitle className={classes.dialogTitle}>
                <IconButton disableRipple className={classes.titleIcon}>
                    <NotListedLocation />
                </IconButton>
            </DialogTitle>
            <DialogContent className={classes.dialogContent}>
                <Typography variant="h6">
                    {confirmDialog.title}
                </Typography>
                <Typography variant="subtitle2">
                    {confirmDialog.subTitle}
                </Typography>
            </DialogContent>
            <DialogActions className={classes.dialogConfirm}>
                <Button variant="outlined"
                    color="inherit"
                    onClick={() => setConfirmDialog({ ...confirmDialog, isOpen: false })}>No
                </Button>
                <Button variant="outlined"
                    color="error"
                    onClick={confirmDialog.onConfirm}>Yes
                </Button>
            </DialogActions>
        </Dialog>
    )
}