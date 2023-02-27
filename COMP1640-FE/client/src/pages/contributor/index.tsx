import { Box, FormControl, InputLabel, MenuItem, Select, SelectChangeEvent, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import ContributorChart from "../../app/components/ContributorChart";
import { useState } from "react";
import { dataContributor } from "../../dataTest";

const Contributors = () => {
    const theme = useTheme();
    const menuItems: Array<string> = [...new Set(dataContributor.map((Val: { department: any }) => Val.department))];
    const [selectedDepartment, setSelectedDepartment] = useState<number>(0);

    const handleChange = (event: SelectChangeEvent) => {
        setSelectedDepartment(event.target.value as unknown as number);
    };

    const filteredDataContributor = selectedDepartment === 0
        ? dataContributor
        : dataContributor.filter((item) => item.department === menuItems[selectedDepartment - 1]);
    return (
        <Box m="1.5rem 2.5rem">
            <Box sx={{
                [theme.breakpoints.down('sm')]: {
                    width: "150%"
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
                    {dataContributor.map((item, index) => (
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
                    <ContributorChart formatedData={filteredDataContributor} />
                </Box>
            </Box>
        </Box>
    );
}

export default Contributors