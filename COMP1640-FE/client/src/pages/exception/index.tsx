
import { Box, FormControl, InputLabel, MenuItem, Select, useTheme } from "@mui/material";
import Header from "../../app/components/Header";

import ExceptionBarChart from "../../app/components/ExceptionBarChart";
import { useEffect, useState } from "react";
import { dataComment } from '../../dataTest'
import agent from "../../app/api/agent";
import Loading from "../../app/components/Loading";

const Exception = () => {
    const theme = useTheme();
    const [dataExceptions, setDataExceptions] = useState([]);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const fetchExceptions = async () => {
            const response = await agent.Chart.exceptionChart();
            setDataExceptions(response);
            setLoading(false);
        };
        fetchExceptions();
    }, []);
    const [view, setView] = useState("ideas");
    return (
        <Box m="1.5rem 2.5rem" >
            <Box sx={{
                [theme.breakpoints.down('sm')]: {
                    width: "150%"
                }
            }}>
                <Header title="EXCEPTION REPORT" subtitle={view === 'ideas' ? "Non Comment & Anonymous Ideas Bar Chart" : "Comments by Department Bar Chart"} />
            </Box>
            <Box height="2vh">
                <FormControl sx={{ mt: "1rem" }}>
                    <InputLabel>View</InputLabel>
                    <Select
                        value={view}
                        label="View"
                        onChange={(e) => setView(e.target.value)}
                        defaultValue="{{ideas}}"
                    >
                        <MenuItem value="ideas">Ideas</MenuItem>
                        <MenuItem value="comments">Comments</MenuItem>
                    </Select>
                </FormControl>
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
                    {loading ? <Loading /> : (<>
                        {view === "comments" ? <ExceptionBarChart isComment={true} dataChart={dataComment} /> : <ExceptionBarChart isComment={false} dataChart={dataExceptions} />}
                    </>
                    )}

                </Box>
            </Box>
        </Box>
    );
}

export default Exception