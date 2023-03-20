import { Box, Button, IconButton, useTheme } from "@mui/material";
import { Delete, Edit } from '@mui/icons-material';
import Header from "../../app/components/Header";
import { AddCircleOutline } from '@mui/icons-material';
import { DataGrid, GridToolbar, GridValueGetterParams } from "@mui/x-data-grid";
import React, { useEffect, useState } from 'react'
import { User } from "../../app/models/User";
import ConfirmDialog from "../../app/components/ConfirmDialog";
import Notification from "../../app/components/Notification";
import Popup from "../../app/components/Popup";
import StaffForm from "./StaffForm";
import { RootState, useAppDispatch } from "../../app/store/configureStore";
import { deleteUser, getRoles, getUsers } from "./userSlice";
import { toast } from "react-toastify";
import { useSelector } from "react-redux";
import Loading from "../../app/components/Loading";
import { getDepartments } from "../department/departmentSlice";

const Staffs = () => {
  const theme: any = useTheme();
  const [pageSize, setPageSize] = React.useState<number>(5);
  const [editMode, setEditMode] = useState(false);
  const [recordForEdit, setRecordForEdit] = useState<User | undefined>(undefined);
  const [notify, setNotify] = useState({ isOpen: false, message: '', type: '' })
  const [confirmDialog, setConfirmDialog] = useState({ isOpen: false, title: '', subTitle: '', onConfirm: () => { } })
  const { users, loading } = useSelector((state: RootState) => state.user);
  const { departments } = useSelector((state: RootState) => state.department);
  const { roles } = useSelector((state: RootState) => state.user);
  const dispatch = useAppDispatch();
  let fetchMount = true;
  useEffect(() => {
    if (fetchMount) {
      dispatch(getUsers());
      dispatch(getDepartments());
      dispatch(getRoles());

    }
    return () => {
      fetchMount = false;
    };
  }, []);
  const staffUsers = users.filter((user: { role: string; }) => user.role === "Staff");
  function handleSelect(user: User) {
    setRecordForEdit(user);
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
        const index = staffUsers.findIndex((r) => r.id === row.id);
        return index + 1;
      }
    },
    {
      field: "avatar",
      headerName: "Avatar Picture",
      minWidth: 70,
      renderCell: (params: { value: any; }) => <Box
        component="img"
        sx={{
          height: 42,
          width: 62,
          borderRadius: "10%",
          objectFit: "fix-content"
        }}
        alt="img"
        src={params.value}
      />
    },
    {
      field: "name",
      headerName: "Name",
      flex: 0.5,
      minWidth: 130,
    },
    {
      field: "username",
      headerName: "Username",
      flex: 0.5,
      minWidth: 130,
    },
    {
      field: "email",
      headerName: "Email",
      flex: 1,
      minWidth: 170
    },
    {
      field: "role",
      headerName: "Role",
      flex: 0.5,
      minWidth: 130,
    },
    {
      field: "phoneNumber",
      headerName: "Phone Number",
      flex: 0.5,
      renderCell: (params: { value: string; }) => {
        if (params.value !== null) {
          return params.value.replace(/^(\d{3})(\d{3})(\d{4})/, "($1)$2-$3");
        } else {
          return params.value;
        }
      },
      minWidth: 130,
    },
    {
      field: "address",
      headerName: "Address",
      flex: 0.4,
      minWidth: 250,
    },
  ];
  const actionColumn = [
    {
      field: "action",
      headerName: "Action",
      width: 200,
      renderCell: (params: { row: { id: any; name: any; username: any; password: any; email: any; avatar: any; departmentId: any; role: any }; }) => {
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
    dispatch(deleteUser(id))
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
              loading={loading || !staffUsers}
              getRowId={(row) => row.id ?? ''}
              rows={staffUsers || []}
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
      )}
      <Notification
        notify={notify}
        setNotify={setNotify}
      />
      <ConfirmDialog
        confirmDialog={confirmDialog}
        setConfirmDialog={setConfirmDialog}
      />
      <Popup
        title="Staff Details"
        openPopup={editMode}
        setOpenPopup={setEditMode}
        cancelEdit={cancelEdit}
      >
        <StaffForm user={recordForEdit} cancelEdit={cancelEdit} departments={departments} roles={roles} />
      </Popup>
    </>
  );
};

export default Staffs;