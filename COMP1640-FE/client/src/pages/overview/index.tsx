import React, { useEffect, useState } from 'react'
import { Box, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import OverviewChart from '../../app/components/OverviewChart';
import agent from '../../app/api/agent';
import Loading from '../../app/components/Loading';
const Overview = () => {
  const theme = useTheme();

  const [dataOverview, setDataOverview] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedDepartment, setSelectedDepartment] = useState<number>(0);
  useEffect(() => {
    const fetchContributors = async () => {
      const response = await agent.Chart.overviewChart();
      setDataOverview(response);
      setIsLoading(false);
    };
    fetchContributors();
  }, []);

  const menuItems: Array<string> = [...new Set(dataOverview.map((Val: { departmentName: any }) => Val.departmentName))];


  const handleChange = (event: SelectChangeEvent) => {
    setSelectedDepartment(event.target.value as unknown as number);
  };

  const filteredDataOverview = selectedDepartment === 0
    ? dataOverview
    : dataOverview.filter((item: any) => item.departmentName === menuItems[selectedDepartment - 1]);

  return (
    <Box m="1.5rem 2.5rem">
      <Box sx={{
        [theme.breakpoints.up('sm')]: {
          width: '50%',
          maxWidth: '50%',
        },
        [theme.breakpoints.down('sm')]: {
          width: '21rem',
        }
      }}> <Header title="OVERVIEW" subtitle="Number of ideas made by each Department." />
      </Box>
      <FormControl sx={{ width: '120px', marginTop: '1rem' }}>
        <InputLabel id="demo-simple-select-label">Department</InputLabel>
        <Select
          labelId="demo-simple-select-label"
          id="demo-simple-select"
          value={selectedDepartment.toString()}
          label="Department"
          onChange={handleChange}
        >
          <MenuItem value={0}>All</MenuItem>
          {dataOverview.map((item: any, index) => (
            <MenuItem key={index} value={index + 1}>{item.departmentName}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <Box
        sx={{
          width: '100%',
          [theme.breakpoints.down('sm')]: {
            width: '100%',
            overflow: 'auto'
          },
        }} >
        <Box mt="40px" height="75vh" sx={{
          [theme.breakpoints.down('sm')]: {
            width: '180%',
            overflow: 'auto'
          },
        }}>
          {isLoading ? <Loading /> :
            <OverviewChart isDashboard={false} formatedData={filteredDataOverview} />
          }
        </Box>
      </Box>
    </Box>

  );
}

export default Overview