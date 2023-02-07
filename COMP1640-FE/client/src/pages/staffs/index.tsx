import React, {useState} from "react";
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import { DataGrid } from "@mui/x-data-grid";
import { userData } from "../../dataTest";
import { Link } from "react-router-dom";
import "./style.scss"

const Staffs = () => {
  const theme:any = useTheme();
  const [data, setData] = useState(userData);

//   const { data, isLoading } = useGetCustomersQuery();

  console.log("data", userData);
  const [pageSize, setPageSize] = React.useState<number>(5);
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
      field: "country",
      headerName: "Country",
      flex: 0.4,
      minWidth: 130,
    },
    {
      field: "occupation",
      headerName: "Occupation",
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
  const handleDelete = (id: any) => {
    setData(data.filter((item) => item._id !== id));
}
  const actionColumn = [
    {
      field: "action",
      headerName: "Action",
      width: 200,
      renderCell: (params: { row: { _id: any; }; }) => {
        return (
          <div className="cellAction">
            <Link to="/users/test" style={{ textDecoration: "none" }}>
              <div className="viewButton">View</div>
            </Link>
            <button
              className="deleteButton"
              onClick={() => handleDelete(params.row._id)}
            >
              Delete
            </button>
          </div>
        );
      },
    },
  ];
  return (
    <Box m="1.5rem 2.5rem">
      <Header title="STAFFS" subtitle="List of Staffs" />
      <Box
        mt="40px"
        style={{ height: '50vh', width: '100%' }}
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
        //   loading={isLoading || !data}
          loading={!data}
          getRowId={(row) => row._id}
          rows={data || []}
          pageSize={pageSize}
          onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
          rowsPerPageOptions={[5, 10, 20]}
          columns={columns.concat(actionColumn)}
        />
      </Box>
    </Box>
  );
};

export default Staffs;