
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import BreakdownChart from "../../app/components/BreakdownChart";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import Loading from "../../app/components/Loading";


const Breakdown = () => {
  const [dataBreakdown, setDataBreakdown] = useState([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    const fetchData = async () => {
      const response = await agent.Chart.breakdownChart();
      setDataBreakdown(response);
      setLoading(false);
    };
    fetchData();
  }, []);
  const theme = useTheme();
  return (
    <> {loading ? <Loading /> :
      <Box m="1.5rem 2.5rem"
        sx={{
          [theme.breakpoints.up('sm')]: {
            width: '90%',
          },
          [theme.breakpoints.down('sm')]: {
            width: '21rem',
          },
        }}  >
        <Header title="BREAKDOWN" subtitle="Breakdown of Ideas By Department" />
        <Box mt="40px" height="75vh">
          <BreakdownChart isDashboard={false} data={dataBreakdown} />
        </Box>
      </Box>}
    </>
  );
};

export default Breakdown;