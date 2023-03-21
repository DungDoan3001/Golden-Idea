import { Box, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import ContributorChart from "../../app/components/ContributorChart";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import Loading from "../../app/components/Loading";

const Contributors = () => {
    const theme = useTheme();
    const [dataContributor, setDataContributor] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedDepartment, setSelectedDepartment] = useState<number>(0);
    useEffect(() => {
        const fetchContributors = async () => {
            const response = await agent.Chart.contributorChart();
            setDataContributor(response);
            setIsLoading(false);
        };
        fetchContributors();
    }, []);

    const menuItems: Array<string> = [...new Set(dataContributor.map((Val: { departmentName: any }) => Val.departmentName))];


    const handleChange = (event: SelectChangeEvent) => {
        setSelectedDepartment(event.target.value as unknown as number);
    };

    const filteredDataContributor = selectedDepartment === 0
        ? dataContributor
        : dataContributor.filter((item: any) => item.departmentName === menuItems[selectedDepartment - 1]);
    return (
        <>
            <Box m="1.5rem 2.5rem">
                <Box sx={{
                    [theme.breakpoints.down('sm')]: {
                        width: "21rem"
                    }
                }}>
                    <Header title="CONTRIBUTOR" subtitle="Number of Contributors within each Department." />
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
                        {dataContributor.map((item: any, index) => (
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
                            <ContributorChart formatedData={filteredDataContributor} />
                        }
                    </Box>
                </Box>
            </Box>
        </>
    );
}

export default Contributors