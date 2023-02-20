import React, { useState } from 'react'
import { Box, IconButton, useTheme } from "@mui/material";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import { Delete, Edit } from '@mui/icons-material';
import agent from '../api/agent';
import { useAppDispatch } from '../store/configureStore';
interface PropsDataDatable {
  dataProps: any;
  columns: any;
  type: string;
}
const Datatable = ({ dataProps, columns, type }: PropsDataDatable) => {
  const [editMode, setEditMode] = useState(false);
  const [recordForEdit, setRecordForEdit] = useState(null);
  const [openPopup, setOpenPopup] = useState(false)
  const [notify, setNotify] = useState({ isOpen: false, message: '', type: '' })
  const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '' })
  const [data, setData] = useState(dataProps);
  const [pageSize, setPageSize] = React.useState<number>(5);
  const theme: any = useTheme();
  const [loading, setLoading] = useState(false);
  const handleDelete = (id: any) => {
    setLoading(true);
    agent.User.deleteUser(id)
      .then(() => setData(data.filter((item: { _id: any; }) => item._id !== id)))
      .catch(error => console.log(error))
      .finally(() => setLoading(false))

  }
  const addOrEdit = (user: { id: number; }, resetForm: () => void) => {
    switch (type) {
      case 'user':
        if (user.id == 0)
          agent.User.createUser(user)
        else
          agent.User.updateUser(user)
        resetForm()
        setRecordForEdit(null)
        setOpenPopup(false)
        setData(agent.User.listUser())
        setNotify({
          isOpen: true,
          message: 'Submitted Successfully',
          type: 'success'
        })
    }
  }
  const openInPopup = (item: React.SetStateAction<null>) => {
    setRecordForEdit(item)
    setOpenPopup(true)
  }
  const actionColumn = [
    {
      field: "action",
      headerName: "Action",
      width: 200,
      renderCell: (params: { row: { _id: any; }; }) => {
        return (
          <div className="cellAction">
            <IconButton aria-label="edit" size="large" color="info">
              <Edit fontSize="inherit" />
            </IconButton>
            <IconButton aria-label="delete" size="large" color="error" onClick={() => handleDelete(params.row._id)}>
              <Delete fontSize="inherit" />
            </IconButton>
          </div>
        );
      },
    },
  ];
  return (
    <Box
      mt="40px"
      style={{ height: '55vh', width: '100%' }}
      sx={{
        "& .MuiDataGrid-root": {
          border: "none",
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
  )
}

export default Datatable