import React, { useState } from 'react'
import { Box, IconButton, useTheme } from "@mui/material";
import { DataGrid, GridToolbar, GridValueGetterParams } from "@mui/x-data-grid";
import { categoryData } from '../../dataTest';
import Header from '../../app/components/Header';
import Loading from '../../app/components/Loading';
import { Delete, Edit } from '@mui/icons-material';
import ConfirmDialog from '../../app/components/ConfirmDialog';
import { toast } from 'react-toastify';
const Category = () => {
    const theme: any = useTheme();
    const [loading, setLoading] = useState(false);
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [data, setData] = useState(categoryData);
    const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
    const columns: any = [
        {
            field: "ordinal",
            headerName: "#",
            flex: 0.2,
            valueGetter: (params: GridValueGetterParams) => {
                const { row } = params;
                const index = data.findIndex((r) => r.id === row.id);
                return index + 1;
            }
        },
        {
            field: "id",
            headerName: "ID",
            flex: 1,
            minWidth: 50,
        },
        {
            field: "name",
            headerName: "Category Name",
            flex: 0.5,
            minWidth: 300,
        },
    ];
    const actionColumn = [
        {
            field: "action",
            headerName: "Action",
            width: 200,
            renderCell: (params: { row: { id: any; }; }) => {
                return (
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: "3px" }}>
                        <IconButton aria-label="edit" size="large" color="info">
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
    const handleDelete = (id: any) => {
        //Integrate BE to use this functionality
        // setLoading(true);
        // agent.User.deleteUser(id)
        //   .then(() => setData(data.filter((item: { _id: any; }) => item._id !== id)))
        //   .catch(error => toast.error(error.toString(), {style: { marginTop: '50px' }, position: toast.POSITION.TOP_RIGHT}))
        //   .finally(() => setLoading(false))
        setData(data.filter((item: { id: any; }) => item.id !== id))
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false
        })
        toast.success('Delete Record Success !', {
            style: { marginTop: '50px' },
            position: toast.POSITION.TOP_RIGHT
        });
    }
    if (!data) {
        setLoading(true);
    }
    return (
        <>
            {loading ? (<Loading />) : (
                <Box m="1.5rem 2.5rem">
                    <Header title="CATEGORIES" subtitle="List of Categories" />
                    <Box
                        mt="40px"
                        style={{ height: '55vh' }}
                        sx={{
                            "& .MuiDataGrid-root": {
                                border: "none",
                                [theme.breakpoints.up('sm')]: {
                                    width: '100%',
                                },
                                [theme.breakpoints.down('sm')]: {
                                    width: '130%',
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
                            getRowId={(row) => row.id}
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
            )
            }
            <ConfirmDialog
                confirmDialog={confirmDialog}
                setConfirmDialog={setConfirmDialog}
            />
        </>
    )
}

export default Category