import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import ContributorChart from "../../app/components/ContributorChart";

const Contributors = () => {
    const theme = useTheme();
    return (
        <Box m="1.5rem 2.5rem">
            <Box sx={{
                [theme.breakpoints.down('sm')]: {
                    width: "150%"
                }
            }}>
                <Header title="CONTRIBUTOR" subtitle="Number of Contributors within each Department." />
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
                    <ContributorChart />
                </Box>
            </Box>
        </Box>
    );
}

export default Contributors