import FlexBetween from "../../app/components/FlexBetween";
import Header from "../../app/components/Header";
import {
  DownloadOutlined,
  PostAdd,
  CalendarMonth,
  PersonAdd,
  DynamicFeed,
} from "@mui/icons-material";
import {
  Box,
  Button,
  Typography,
  useTheme,
  useMediaQuery,
} from "@mui/material";
import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import BreakdownChart from "../../app/components/BreakdownChart";
import StatBox from "../../app/components/StatBox";
import OverviewChart from "../../app/components/OverviewChart";
import { dataIdeas } from "../../dataTest";
import Loading from "../../app/components/Loading";
import React from "react";

// import OverviewChart from "components/OverviewChart";
// import { useGetDashboardQuery } from "state/api";
// import StatBox from "components/StatBox";
const Dashboard = () => {
  const theme: any = useTheme();
  const isNonMediumScreens = useMediaQuery("(min-width: 1200px)");
  const [pageSize, setPageSize] = React.useState<number>(5);
  const columns: any = [
    {
      field: "id",
      headerName: "ID",
      minWidth: 130,
      flex: 1,
    },
    {
      field: "image",
      headerName: "Image",
      minWidth: 130,
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
      field: "title",
      headerName: "Title",
      minWidth: 130,
      flex: 1,
    },
    {
      field: "lastUpdate",
      headerName: "CreatedAt",
      minWidth: 130,
      flex: 1,
    },
    // {
    //   field: "cost",
    //   headerName: "Cost",
    //   flex: 1,
    //   renderCell: (params: { value: any; }) => `$${Number(params.value).toFixed(2)}`,
    // },
  ];
  if (!dataIdeas) return <Loading />;
  return (
    <Box m="1.5rem 2.5rem">
      <FlexBetween>
        <Header title="DASHBOARD" subtitle="Welcome to your dashboard" />
        <Box>
          <Button
            sx={{
              backgroundColor: theme.palette.secondary.light,
              color: theme.palette.background.alt,
              fontSize: "14px",
              fontWeight: "bold",
              padding: "10px 20px",
            }}
          >
            <DownloadOutlined sx={{ mr: "10px" }} />
            Download Reports
          </Button>
        </Box>
      </FlexBetween>

      <Box
        mt="20px"
        display="grid"
        gridTemplateColumns="repeat(12, 1fr)"
        gridAutoRows="160px"
        gap="20px"
        sx={{
          "& > div": { gridColumn: isNonMediumScreens ? undefined : "span 12" },
        }}
      >
        {/* ROW 1 */}
        <StatBox
          title="Total Staffs"
          value={1000}
          increase="+14%"
          description="Since last month"
          icon={
            <PersonAdd
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <StatBox
          title="Ideas Today"
          value={100}
          increase="+8%"
          description="Since yesterday"
          icon={
            <PostAdd
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <Box sx={{
          gridColumn: "span 8", gridRow: "span 2", backgroundColor: theme.palette.background.alt, p: "1rem", borderRadius: "0.55rem", [theme.breakpoints.down('sm')]: {
            width: '100vw',
          },
        }}
        >
          <Typography
            variant="h5"
            fontWeight="600"
            sx={{ padding: "5px 5px 0 5px" }}
          >
            Ideas by department
          </Typography>
          <OverviewChart isDashboard={true} />
        </Box>
        <StatBox
          title="Monthly Ideas"
          value={100}
          increase="+5%"
          description="Since last month"
          icon={
            <CalendarMonth
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <StatBox
          title="Yearly Ideas"
          value={12353}
          increase="+43%"
          description="Since last month"
          icon={
            < DynamicFeed
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />

        {/* ROW 2 */}
        <Box
          gridColumn="span 8"
          gridRow="span 3"
          sx={{
            "& .MuiDataGrid-root": {
              border: "none",
              borderRadius: "5rem",
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
              backgroundColor: theme.palette.background.alt,
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
            pageSize={pageSize}
            onPageSizeChange={(newPageSize) => setPageSize(newPageSize)}
            rowsPerPageOptions={[5, 10, 20]}
            getRowId={(row) => row.id}
            rows={(dataIdeas) || []}
            columns={columns}
            components={{ Toolbar: GridToolbar }}
            componentsProps={{
              toolbar: {
                showQuickFilter: true,
                quickFilterProps: { debounceMs: 500 },
              },
            }}
          />
        </Box>
        <Box sx={{
          gridColumn: "span 4", gridRow: "span 3", backgroundColor: theme.palette.background.alt, p: "1.5rem", borderRadius: "0.55rem", [theme.breakpoints.down('sm')]: {
            width: '100vw',
          },
        }}
        >
          <Typography variant="h6" sx={{ color: theme.palette.secondary[100] }}>
            Ideas By Category
          </Typography>
          <BreakdownChart isDashboard={true} />
          <Typography
            p="0 0.6rem"
            fontSize="0.8rem"
            align="justify"
            sx={{ color: theme.palette.secondary[200] }}
          >
            Breakdown of real states and information via department for ideas
            made for this year and total ideas.
          </Typography>
        </Box>
      </Box>
    </Box>
  );
};

export default Dashboard;