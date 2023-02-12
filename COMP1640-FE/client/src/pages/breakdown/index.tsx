
import { Box, styled, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import BreakdownChart from "../../app/components/BreakdownChart";


const Breakdown = () => {
  const theme = useTheme();
  return (
    <Box m="1.5rem 2.5rem"
      sx={{
        [theme.breakpoints.down('sm')]: {
          width: '100%',
        },
      }} >
      <Header title="BREAKDOWN" subtitle="Breakdown of Ideas By Department" />
      <Box mt="40px" height="75vh">
        <BreakdownChart />
      </Box>
    </Box>
  );
};

export default Breakdown;