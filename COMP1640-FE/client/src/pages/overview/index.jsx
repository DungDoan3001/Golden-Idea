import React from 'react'
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import OverviewChart from '../../app/components/OverviewChart';
const Overview = () => {
    const theme = useTheme();
    return (
      <Box m="1.5rem 2.5rem"
        sx={{
          [theme.breakpoints.down('sm')]: {
            width: '100%',
          },
        }} >
        <Header title="OVERVIEW" subtitle="Number of ideas made by each Department." />
        <Box mt="40px" height="75vh">
          <OverviewChart />
        </Box>
      </Box>
    );
}

export default Overview