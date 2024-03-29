import FlexBetween from "../../app/components/FlexBetween";
import Header from "../../app/components/Header";
import axios from "axios";
import {
  DownloadOutlined,
  PostAdd,
  InsertComment,
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
import { DataGrid, GridValueGetterParams } from "@mui/x-data-grid";
import BreakdownChart from "../../app/components/BreakdownChart";
import StatBox from "../../app/components/StatBox";
import OverviewChart from "../../app/components/OverviewChart";
import Loading from "../../app/components/Loading";
import React, { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import { useSelector } from "react-redux";
import { RootState, useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { getDashboardIdeas } from "../myIdeas/ideasSlice";
import moment from "moment";
import { toast } from "react-toastify";

interface IData {
  totalStaff: number;
  totalIdea: number;
  totalComment: number;
  totalTopic: number;
}
const Dashboard = () => {
  const theme: any = useTheme();
  const isNonMediumScreens = useMediaQuery("(min-width: 1200px)");
  //Get user info here
  const { user } = useAppSelector(state => state.account);
  const dispatch = useAppDispatch();
  const { ideas_dashboard } = useSelector((state: RootState) => state.idea);
  const [pageSize, setPageSize] = React.useState<number>(10);
  const [dataBreakdown, setDataBreakdown] = useState([]);
  const [dataOverview, setDataOverview] = useState([]);
  const [dataOverall, setDataOverall] = useState<IData>({
    totalStaff: 0,
    totalIdea: 0,
    totalComment: 0,
    totalTopic: 0,
  });
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    const fetchData = async () => {
      const response = await agent.Chart.breakdownChart();
      const res = await agent.Chart.overallChart();
      const data = await agent.Chart.overviewChart();
      await dispatch(getDashboardIdeas());
      setDataBreakdown(response);
      setDataOverall(res);
      setDataOverview(data);
      setLoading(false);
    };
    fetchData();
  }, []);

  const handleDownload = async () => {
    try {
      const response = await axios({
        method: "get",
        url: "https://goldenidea.azurewebsites.net/api/ZipFiles/download-data-dashboard",
        responseType: "blob", // set the response type to blob
      });

      const blob = new Blob([response.data], { type: "application/zip" }); // create a new Blob object from the response data
      const url = URL.createObjectURL(blob); // create a temporary URL for the Blob object
      const link = document.createElement("a"); // create a new <a> element
      link.href = url; // set the href attribute of the <a> element to the temporary URL
      link.download = "dashboard.zip"; // set the download attribute of the <a> element to the desired filename
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
  let isDownloadablePerson = false;
  if (user) {
    isDownloadablePerson = user?.role[0] === 'QA Manager';
  }
  const columns: any = [
    {
      field: "ordinal",
      headerName: "#",
      flex: 0.2,
      valueGetter: (params: GridValueGetterParams) => {
        const { row } = params;
        const index = ideas_dashboard.findIndex((r) => r.id === row.id);
        return index + 1;
      }
    },
    {
      field: "image",
      headerName: "Image",
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
      field: "title",
      headerName: "Title",
      minWidth: 300,
      flex: 1,
    },
    {
      field: "createdAt",
      headerName: "CreatedAt",
      minWidth: 150,
      flex: 1,
      renderCell: (params: { value: string; }) => {
        return moment(params.value).format('DD-MM-YYYY');
      },
    },
    {
      field: "lastUpdate",
      headerName: "Last Update",
      minWidth: 150,
      flex: 1,
      renderCell: (params: { value: string; }) => {
        return moment(params.value).format('DD-MM-YYYY');
      },
    },
    {
      field: "category",
      headerName: "Category",
      minWidth: 250,
      flex: 1,
      renderCell: (params: { value: any; }) => params.value.name
    },
    {
      field: "topic",
      headerName: "Topic",
      minWidth: 350,
      flex: 1,
      renderCell: (params: { value: any; }) => params.value.name
    },
    {
      field: "user",
      headerName: "Creator",
      minWidth: 250,
      flex: 1,
      renderCell: (params: { value: any; }) => params.value.userName
    },
  ];
  if (loading) return <Loading />;
  return (
    <Box m="1.5rem 2.5rem" sx={{
      [theme.breakpoints.down('sm')]: {
        width: '22.5rem',
        margin: '1.5rem 1rem',
      },
    }}>
      <FlexBetween>
        <Header title="DASHBOARD" subtitle="Welcome to Summary of Golden Idea Statistic" />
        {isDownloadablePerson && (
          <Box>
            <Button
              sx={{
                backgroundColor: theme.palette.secondary[300],
                color: theme.palette.background.alt,
                fontSize: "14px",
                fontWeight: "bold",
                padding: "10px 20px",
                [theme.breakpoints.down('sm')]: {
                  marginLeft: "3.5rem",
                },
                '&:hover': {
                  backgroundColor: theme.palette.secondary[300],
                  opacity: 0.8,
                  boxShadow: 'none',
                },
              }}
              onClick={handleDownload}
            >
              <DownloadOutlined sx={{ mr: "10px" }} />
              Download Reports
            </Button>
          </Box>
        )}
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
          value={dataOverall.totalStaff}
          increase={`${2}%`}
          description="Since last month"
          icon={
            <PersonAdd
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <StatBox
          title="Total Ideas"
          value={dataOverall.totalIdea}
          increase={`${10}%`}
          description="Since last month"
          icon={
            <PostAdd
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <Box sx={{
          gridColumn: "span 8", gridRow: "span 2", backgroundColor: theme.palette.background.alt, p: "1rem", borderRadius: "0.55rem", [theme.breakpoints.down('sm')]: {
            width: '100%',
            overflow: 'auto'
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
          <Box sx={{
            [theme.breakpoints.up('sm')]: {
              width: '100%',
              height: '100%',
            },
            [theme.breakpoints.down('sm')]: {
              width: '210%',
              height: '100%',
              overflow: 'auto'
            },
          }}>
            <OverviewChart isDashboard={true} formatedData={dataOverview} /></Box>
        </Box>
        <StatBox
          title="Total comments"
          value={dataOverall.totalComment}
          increase={`${5}%`}
          description="Since last month"
          icon={
            <InsertComment
              sx={{ color: theme.palette.secondary[300], fontSize: "26px" }}
            />
          }
        />
        <StatBox
          title="Total topics"
          value={dataOverall.totalTopic}
          increase={`${3}%`}
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
            marginBottom: "8px",
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
            rowsPerPageOptions={[10, 50, 100]}
            getRowId={(row) => row.id ?? ''}
            rows={(ideas_dashboard) || []}
            columns={columns}
          />
        </Box>
        <Box sx={{
          gridColumn: "span 4", gridRow: "span 3", backgroundColor: theme.palette.background.alt, p: "1.5rem", borderRadius: "0.55rem", [theme.breakpoints.down('sm')]: {
            width: '100%',
          },
          marginBottom: "8px",
        }}
        >
          <Typography variant="h6" sx={{ color: theme.palette.secondary[100] }}>
            Ideas By Category
          </Typography>
          <BreakdownChart isDashboard={true} data={dataBreakdown} />
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