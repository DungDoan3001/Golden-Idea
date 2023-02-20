import React from 'react'
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import OverviewChart from '../../app/components/OverviewChart';
const Overview = () => {
  const theme = useTheme();
  return (
    <Box m="1.5rem 2.5rem">
      <Box sx={{
        [theme.breakpoints.down('sm')]: {
          width: "150%"
        }
      }}> <Header title="OVERVIEW" subtitle="Number of ideas made by each Department." />
      </Box>
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
          <OverviewChart />
        </Box>
      </Box>
    </Box>

  );
}

export default Overview