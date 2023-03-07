import React, { useEffect, useState } from 'react'
import { Box, Button, IconButton, useTheme } from "@mui/material";
import { DataGrid, GridToolbar, GridValueGetterParams } from "@mui/x-data-grid";
import Header from '../../app/components/Header';
import Loading from '../../app/components/Loading';
import Moment from 'moment';
import { AddCircleOutline, Delete, Edit } from '@mui/icons-material';
import ConfirmDialog from '../../app/components/ConfirmDialog';
import Popup from '../../app/components/Popup';
import TopicForm from './topicForm';
import { Topic } from '../../app/models/Topic';
import { toast } from 'react-toastify';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch } from '../../app/store/configureStore';
import { getTopics } from './topicSlice';
const TopicPage = () => {
    const theme: any = useTheme();
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<Topic | undefined>(undefined);
    const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
    const { topics, loading } = useSelector((state: RootState) => state.topic);
    const dispatch = useAppDispatch();
    let fetchMount = true;
    useEffect(() => {
        if (fetchMount) {
            dispatch(getTopics());
        }
        return () => {
            fetchMount = false;
        };
    }, []);

    function handleSelect(topic: Topic) {
        setRecordForEdit(topic);
        setEditMode(true);
    }

    function cancelEdit() {
        if (recordForEdit) setRecordForEdit(undefined);
        setEditMode(false);
    }
    const handleDelete = (id: any) => {
        //Integrate BE to use this functionality
        // setLoading(true);
        // agent.User.deleteUser(id)
        //   .then(() => setData(data.filter((item: { _id: any; }) => item._id !== id)))
        //   .catch(error => toast.error(error.toString(), {style: { marginTop: '50px' }, position: toast.POSITION.TOP_RIGHT}))
        //   .finally(() => setLoading(false))
        setConfirmDialog({
            ...confirmDialog,
            isOpen: false
        })
        toast.success('Delete Record Success !', {
            style: { marginTop: '50px' },
            position: toast.POSITION.TOP_RIGHT
        });
    }
    const columns: any = [
        {
            field: "ordinal",
            headerName: "#",
            flex: 0.2,
            valueGetter: (params: GridValueGetterParams) => {
                const { row } = params;
                const index = topics.findIndex((r) => r.id === row.id);
                return index + 1;
            }
        },
        {
            field: "name",
            headerName: "Topic Name",
            flex: 0.5,
            minWidth: 300,
        },
        {
            field: "closureDate",
            headerName: "Closure Date",
            flex: 1,
            minWidth: 50,
            renderCell: (params: { value: string; }) => {
                return Moment(params.value).format('DD-MM-YYYY');
            },
        },
        {
            field: "finalClosureDate",
            headerName: "Final Closure Date",
            flex: 1,
            minWidth: 50,
            renderCell: (params: { value: string; }) => {
                return Moment(params.value).format('DD-MM-YYYY');
            },
        },
        {
            field: "username",
            headerName: "User Name",
            flex: 1,
            minWidth: 50,
        },
    ];
    const actionColumn = [
        {
            field: "action",
            headerName: "Action",
            width: 200,
            renderCell: (params: { row: { id: any; name: any, username: any, closureDate: any, finalClosureDate: any, }; }) => {
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
    return (
        <>
            {loading ? (<Loading />) : (
                <Box m="1.5rem 2.5rem">
                    <Header title="TOPICS" subtitle="List of Topics" />
                    <Button variant="contained" size="medium" color="success" onClick={() => setEditMode(true)} style={{ marginTop: 15 }}
                        startIcon={<AddCircleOutline />}>
                        Create a new Topic
                    </Button>
                    <Box
                        mt="40px"
                        style={{ height: '55vh' }}
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
                            loading={loading || !topics}
                            getRowId={(row) => row.id ?? ''}
                            rows={topics || []}
                            pageSize={pageSize}
                            onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
                            rowsPerPageOptions={[5, 10, 20]}
                            columns={columns.concat(actionColumn)}
                            components={{ Toolbar: GridToolbar }}
                            componentsProps={{
                                toolbar: {
                                    showQuickFilter: true,
                                    quickFilterProps: { fields: ["name", "userName"], debounceMs: 500 },
                                },
                            }}
                        />
                    </Box>
                    <ConfirmDialog
                        confirmDialog={confirmDialog}
                        setConfirmDialog={setConfirmDialog}
                    />
                    <Popup
                        title="Topic Details"
                        openPopup={editMode}
                        setOpenPopup={setEditMode}
                        cancelEdit={cancelEdit}
                    >
                        <TopicForm topic={recordForEdit} cancelEdit={cancelEdit} />
                    </Popup>
                </Box>
            )
            }

        </>
    )
}

export default TopicPage

