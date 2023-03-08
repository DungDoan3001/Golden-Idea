import { Box, Button, IconButton, useTheme } from "@mui/material";
import { Delete, Edit } from '@mui/icons-material';
import Header from "../../app/components/Header";
import { userData } from "../../dataTest";
//import agent from "../../app/api/agent";
import { AddCircleOutline } from '@mui/icons-material';
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import React, { useState } from 'react'
import agent from "../../app/api/agent";
import { User } from "../../app/models/User";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import Notification from "../../app/components/Notification";
import Popup from "../../app/components/Popup";
import StaffForm from "./StaffForm";

const AdminPage = () => {
    const theme: any = useTheme();
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState(userData);
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<User | undefined>(undefined);
    const [openPopup, setOpenPopup] = useState(false)
    const [notify, setNotify] = useState({ isOpen: false, message: '', type: '' })
    const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
    const columns: any = [
        {
            field: "_id",
            headerName: "ID",
            flex: 1,
            minWidth: 70,
        },
        {
            field: "name",
            headerName: "Name",
            flex: 0.5,
            minWidth: 130,
        },
        {
            field: "email",
            headerName: "Email",
            flex: 1,
            minWidth: 130
        },
        {
            field: "phoneNumber",
            headerName: "Phone Number",
            flex: 0.5,
            renderCell: (params: { value: string; }) => {
                return params.value.replace(/^(\d{3})(\d{3})(\d{4})/, "($1)$2-$3");
            },
            minWidth: 130,
        },
        {
            field: "address",
            headerName: "Address",
            flex: 0.4,
            minWidth: 130,
        },
        {
            field: "department",
            headerName: "Department",
            flex: 1,
            minWidth: 130,
        },
        {
            field: "role",
            headerName: "Role",
            flex: 0.5,
            minWidth: 130,
        },
    ];
    function cancelEdit() {
        if (recordForEdit) setRecordForEdit(undefined);
        setEditMode(false);
    }
    const actionColumn = [
        {
            field: "action",
            headerName: "Action",
            width: 200,
            renderCell: (params: { row: { _id: any; }; }) => {
                return (
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: "3px" }}>
                        <IconButton aria-label="edit" size="large" color="info">
                            <Edit fontSize="inherit" />
                        </IconButton>
                        <IconButton aria-label="delete" size="large" color="error" onClick={() => setConfirmDialog({
                            isOpen: true,
                            title: 'Are you sure to delete this record?',
                            subTitle: "You can't undo this operation",
                            onConfirm: () => { handleDelete(params.row._id) }
                        })}>
                            <Delete fontSize="inherit" />
                        </IconButton>
                    </Box>
                );
            },
        },
    ];
    const handleDelete = (id: any) => {
        //Integrate BE to use this functionality
        // setLoading(true);
        // agent.User.deleteUser(id)
        //   .then(() => setData(data.filter((item: { _id: any; }) => item._id !== id)))
        //   .catch(error => console.log(error))
        //   .finally(() => setLoading(false))
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false
        })
        setNotify({
            isOpen: true,
            message: 'Deleted Successfully',
            type: 'error'
        })
        setData(data.filter((item: { _id: any; }) => item._id !== id))
    }
    const addOrEdit = async (user: { id: number; }, resetForm: () => void) => {
        if (user.id === 0)
            agent.User.createUser(user)
        else
            // agent.User.updateUser(user)
            resetForm()
        setRecordForEdit(undefined)
        setOpenPopup(false)
        setNotify({
            isOpen: true,
            message: 'Submitted Successfully',
            type: 'success'
        })
    }
    const openInPopup = (user: User) => {
        setRecordForEdit(user)
        setOpenPopup(true)
    }

    return (
        <>
            <Box m="1.5rem 2.5rem">
                <Header title="STAFFS" subtitle="List of Staffs" />
                <Button variant="contained" size="medium" color="success" onClick={() => setEditMode(true)} style={{ marginTop: 15 }}
                    startIcon={<AddCircleOutline />}>
                    Create a new Staff
                </Button>
                <Box
                    mt="40px"
                    style={{
                        height: '55vh',
                    }}
                    sx={{
                        "& .MuiDataGrid-root": {
                            border: "none",
                            [theme.breakpoints.up('md')]: {
                                width: '100%',
                                height: '60vh'
                            },
                            [theme.breakpoints.up('sm')]: {
                                width: '100%',
                                height: '40vh'
                            },
                            [theme.breakpoints.down('sm')]: {
                                width: '130%',
                                height: '50vh'
                            },
                        },
                        "& .MuiDataGrid-cell": {
                            borderBottom: "none",
                        },
                        "& .MuiDataGrid-columnHeaders": {
                            backgroundColor: theme.palette.background.alt,
                            color: theme.palette.secondary[100],
                            borderBottom: "none",
                        },
                        "& .MuiDataGrid-virtualScroller": {
                            backgroundColor: theme.palette.primary.light,
                        },
                        "& .MuiDataGrid-footerContainer": {
                            backgroundColor: theme.palette.background.alt,
                            color: theme.palette.secondary[100],
                            borderTop: "none",
                        },
                        "& .MuiDataGrid-toolbarContainer .MuiButton-text": {
                            color: `${theme.palette.secondary[200]} !important`,
                        },
                    }}
                >

                    <DataGrid
                        loading={loading || !data}
                        getRowId={(row) => row._id}
                        rows={data || []}
                        pageSize={pageSize}
                        onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
                        rowsPerPageOptions={[5, 10, 20]}
                        columns={columns.concat(actionColumn)}
                        components={{ Toolbar: GridToolbar }}
                        componentsProps={{
                            toolbar: {
                                showQuickFilter: true,
                                quickFilterProps: { debounceMs: 500 },
                            },
                        }}
                    />
                </Box>
            </Box>
            <Notification
                notify={notify}
                setNotify={setNotify}
            />
            <ConfirmDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
            {/* <Popup
                title="Staff Details"
                openPopup={editMode}
                setOpenPopup={setEditMode}
                cancelEdit={cancelEdit}
            >
                <StaffForm user={recordForEdit} cancelEdit={cancelEdit} />
            </Popup> */}
        </>
    );
};

export default AdminPage;