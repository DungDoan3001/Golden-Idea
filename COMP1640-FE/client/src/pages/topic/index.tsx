import React, { useEffect, useState } from 'react'
import { Box, Button, IconButton, useTheme } from "@mui/material";
import { DataGrid, GridToolbar, GridValueGetterParams } from "@mui/x-data-grid";
import Header from '../../app/components/Header';
import Loading from '../../app/components/Loading';
import Moment from 'moment';
import { AddCircleOutline, Delete, Edit, GetApp } from '@mui/icons-material';
import ConfirmDialog from '../../app/components/ConfirmDialog';
import Popup from '../../app/components/Popup';
import TopicForm from './topicForm';
import { Topic } from '../../app/models/Topic';
import { toast } from 'react-toastify';
import { useSelector } from 'react-redux';
import { RootState, useAppDispatch, useAppSelector } from '../../app/store/configureStore';
import { deleteTopic, getTopics } from './topicSlice';
import agent from '../../app/api/agent';
import axios from 'axios';
const TopicPage = () => {
    const theme: any = useTheme();
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [editMode, setEditMode] = useState(false);
    const [recordForEdit, setRecordForEdit] = useState<Topic | undefined>(undefined);
    const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
    const { topics, loading } = useSelector((state: RootState) => state.topic);
    //Get user info here
    const { user } = useAppSelector(state => state.account);
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
    const handleDelete = (id: string) => {
        dispatch(deleteTopic(id))
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
    const handleDownload = async (id: any) => {
        try {
            const response = await axios({
                method: "get",
                url: `https://goldenidea.azurewebsites.net/api/ZipFiles/download-all-ideas-of-topic/${id}`,
                responseType: "blob", // set the response type to blob
            });

            const blob = new Blob([response.data], { type: "application/zip" }); // create a new Blob object from the response data
            const url = URL.createObjectURL(blob); // create a temporary URL for the Blob object
            const link = document.createElement("a"); // create a new <a> element
            link.href = url; // set the href attribute of the <a> element to the temporary URL
            link.download = "topic-report.zip"; // set the download attribute of the <a> element to the desired filename
            document.body.appendChild(link); // append the <a> element to the DOM
            link.click(); // simulate a click on the <a> element to trigger the download
            document.body.removeChild(link); // remove the <a> element from the DOM
            toast.success('Download Successfully', {
                position: toast.POSITION.TOP_RIGHT,
            });
        } catch (error) {
            console.error(error);
        }
    };
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
            minWidth: 600,
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
                const today = new Date()
                const isAfterFinalClosure = today > new Date(params.row.finalClosureDate)
                let isDownloadablePerson = false;
                if (user) {
                    isDownloadablePerson = user?.role[0] === 'Administrator' || user?.role[0] === 'QA Manager';
                }
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
                        {isAfterFinalClosure && isDownloadablePerson && (
                            <IconButton aria-label="download" size="large" color="success" onClick={() => handleDownload(params.row.id)}>
                                <GetApp fontSize="inherit" />
                            </IconButton>
                        )}
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

