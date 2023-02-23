import React, { useState } from 'react'
import { Box, useTheme } from "@mui/material";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import { commentData } from '../../dataTest';
import Header from '../../app/components/Header';
import Loading from '../../app/components/Loading';

const Comment = () => {
    const theme: any = useTheme();
    const [loading, setLoading] = useState(false);
    const [pageSize, setPageSize] = React.useState<number>(5);
    const [data, setData] = useState(commentData);
    const columns: any = [
        {
            field: "id",
            headerName: "ID",
            flex: 1,
            minWidth: 50,
        },
        {
            field: "content",
            headerName: "Content",
            flex: 0.5,
            minWidth: 300,
        },
        {
            field: "createdDate",
            headerName: "Created Date",
            flex: 1,
            minWidth: 50,
        },
        {
            field: "userID",
            headerName: "User ID",
            flex: 0.5,
            minWidth: 130,
        },
        {
            field: "ideaID",
            headerName: "Idea ID",
            flex: 0.4,
            minWidth: 130,
        },
    ];
    if (!data) {
        setLoading(true);
    }
    return (
        <>
            {loading ? (<Loading />) : (
                <Box m="1.5rem 2.5rem">
                    <Header title="COMMENTS" subtitle="List of Comments" />
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
                            columns={columns}
                            components={{ Toolbar: GridToolbar }}
                            componentsProps={{
                                toolbar: {
                                    showQuickFilter: true,
                                    quickFilterProps: { field: "content", debounceMs: 500 },
                                },
                            }}
                        />
                    </Box>
                </Box>
            )
            }

        </>
    )
}

export default Comment