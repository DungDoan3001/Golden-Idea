
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";

import ExceptionBarChart from "../../app/components/ExceptionBarChart";

const Exception = () => {
    const theme = useTheme();
    return (
        <Box m="1.5rem 2.5rem"
            sx={{
                [theme.breakpoints.down('sm')]: {
                    width: '100%',
                },
            }} >
            <Header title="EXCEPTION REPORT" subtitle="Bar Chart for Anonymous & Non-comment Ideas" />
            <Box mt="40px" height="75vh">
                <ExceptionBarChart />
            </Box>
        </Box>
    );
}

export default Exception