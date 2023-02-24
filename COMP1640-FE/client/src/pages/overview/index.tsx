import React, { useState } from 'react'
import { Box, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import OverviewChart from '../../app/components/OverviewChart';
import { dataOverview } from '../../dataTest';
const Overview = () => {
  const theme = useTheme();

  const menuItems: Array<string> = [...new Set(dataOverview.map((Val: { department: any }) => Val.department))];
  const [selectedDepartment, setSelectedDepartment] = useState<number>(0);

  const handleChange = (event: SelectChangeEvent) => {
    setSelectedDepartment(event.target.value as unknown as number);
  };

  const filteredDataOverview = selectedDepartment === 0
    ? dataOverview
    : dataOverview.filter((item) => item.department === menuItems[selectedDepartment - 1]);

  return (
    <Box m="1.5rem 2.5rem">
      <Box sx={{
        [theme.breakpoints.down('sm')]: {
          width: "150%"
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
          {dataOverview.map((item, index) => (
            <MenuItem key={index} value={index + 1}>{item.department}</MenuItem>
          ))}
        </Select>
      </FormControl>
      <Box
        sx={{
          width: '100%',
          [theme.breakpoints.down('sm')]: {
            width: '130%',
            overflow: 'auto'
          },
        }} >
        <Box mt="40px" height="75vh" sx={{
          [theme.breakpoints.down('sm')]: {
            width: '180%',
            overflow: 'auto'
          },
        }}>
          <OverviewChart isDashboard={false} formatedData={filteredDataOverview} />
        </Box>
      </Box>
    </Box>

  );
}

export default Overview