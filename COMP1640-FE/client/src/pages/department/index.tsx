import React, { useEffect, useState } from 'react'
import { Box, Button, IconButton, useTheme } from "@mui/material";
import { DataGrid, GridToolbar, GridValueGetterParams } from "@mui/x-data-grid";
import Header from '../../app/components/Header';
import Loading from '../../app/components/Loading';
import { AddCircleOutline, Delete, Edit } from '@mui/icons-material';
import ConfirmDialog from '../../app/components/ConfirmDialog';
import { toast } from 'react-toastify';
import { Department } from '../../app/models/Department';
import Popup from '../../app/components/Popup';
import DepartmentForm from './departmentForm';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { deleteDepartment, getDepartments } from './departmentSlice';

const DepartmentPage = () => {
    const theme: any = useTheme();
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<Department | undefined>(undefined);
    const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
    const { departments, loading } = useSelector((state: RootState) => state.department);
    const dispatch = useAppDispatch();
    let fetchMount = true;
    useEffect(() => {
        if (fetchMount) {
            dispatch(getDepartments());
        }
        return () => {
            fetchMount = false;
        };
    }, []);

    function handleSelect(department: Department) {
        setRecordForEdit(department);
        setEditMode(true);
    }

    function cancelEdit() {
        if (recordForEdit) setRecordForEdit(undefined);
        setEditMode(false);
    }
    const columns: any = [
        {
            field: "ordinal",
            headerName: "#",
            flex: 0.2,
            valueGetter: (params: GridValueGetterParams) => {
                const { row } = params;
                const index = departments.findIndex((r) => r.id === row.id);
                return index + 1;
            }
        },
        {
            field: "id",
            headerName: "ID",
            flex: 1,
            minWidth: 300,
        },
        {
            field: "name",
            headerName: "Department Name",
            flex: 0.5,
            minWidth: 300,
        },
    ];
    const actionColumn = [
        {
            field: "action",
            headerName: "Action",
            width: 200,
            renderCell: (params: { row: { id: any; name: any }; }) => {
                return (
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: "3px" }}>
                        <IconButton aria-label="edit" size="large" color="info" onClick={() => handleSelect(params.row)} >
                            <Edit fontSize="inherit" />
                        </IconButton>
                        <IconButton aria-label="delete" size="large" color="error" onClick={() => setConfirmDialog({
                            isOpen: true,
                            title: 'Are you sure to delete this record?',
                            subTitle: "You can't undo this operation",
                            onConfirm: () => { handleDelete(params.row.id) }
                        })}>
                            <Delete fontSize="inherit" />
                        </IconButton>
                    </Box>
                );
            },
        },
    ];
    const handleDelete = (id: string) => {
        dispatch(deleteDepartment(id))
            .catch((error: any) => {
                // handle error
                toast.error(error.toString(), {
                    style: { marginTop: '50px' },
                    position: toast.POSITION.TOP_RIGHT
                });
            });

        setConfirmDialog({
            ...confirmDialog,
            isOpen: false
        });
    };
    return (
        <>
            {loading ? (<Loading />) : (
                <Box m="1.5rem 2.5rem">
                    <Header title="DEPARTMENT" subtitle="List of Departments" />
                    <Button variant="contained" size="medium" color="success" onClick={() => setEditMode(true)} style={{ marginTop: 15 }}
                        startIcon={<AddCircleOutline />}>
                        Create a new department
                    </Button>
                    <Box
                        mt="12px"
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
                                    width: '450px',
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
                            loading={loading || !departments}
                            getRowId={(row) => row.id ?? ''}
                            rows={departments || []}
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
            )
            }
            <ConfirmDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
            <Popup
                title="Department Details"
                openPopup={editMode}
                setOpenPopup={setEditMode}
                cancelEdit={cancelEdit}
            >
                <DepartmentForm department={recordForEdit} cancelEdit={cancelEdit} />
            </Popup>
        </>
    )
}

export default DepartmentPage;