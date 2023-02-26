import { useMemo, useState } from "react";
import { Box, useTheme } from "@mui/material";
import Header from "../../app/components/Header";
import { ResponsiveLine } from "@nivo/line";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { dataOverall } from "../../dataTest";
import Loading from "../../app/components/Loading";
const DailyReport = () => {
    const [startDate, setStartDate] = useState(new Date("2023-02-01"));
    const [endDate, setEndDate] = useState(new Date("2023-03-01"));
    //const { data } = useGetIdeasQuery();
    const data = dataOverall[0].dailyData;
    const theme: any = useTheme();

    const [formattedData] = useMemo(() => {
        if (!data) return [];

        const totalIdeasLine: any = {
            id: "totalIdeas",
            color: theme.palette.secondary.main,
            data: [],
        };
        const totalCommentsLine: any = {
            id: "totalComments",
            color: theme.palette.secondary[600],
            data: [],
        };

        Object.values(data).forEach(({ date, totalIdeas, totalComments }: any) => {
            const dateFormatted = new Date(date);
            if (dateFormatted >= startDate && dateFormatted <= endDate) {
                const splitDate = date.substring(date.indexOf("-") + 1);

                totalIdeasLine.data = [
                    ...totalIdeasLine.data,
                    { x: splitDate, y: totalIdeas },
                ];
                totalCommentsLine.data = [
                    ...totalCommentsLine.data,
                    { x: splitDate, y: totalComments },
                ];
            }
        });

        const formattedData = [totalIdeasLine, totalCommentsLine];
        return [formattedData];
    }, [data, startDate, endDate]); // eslint-disable-line react-hooks/exhaustive-deps

    return (
        <Box m="1.5rem 2.5rem" >
            <Box sx={{
                [theme.breakpoints.down('sm')]: {
                    width: "150%"
                }
            }}>
                <Header title="DAILY REPORT" subtitle="Chart of Daily Ideas and Comments" />
            </Box>
            <Box display="flex" justifyContent="flex-start" flexDirection="row" alignItems='center' sx={{
                marginTop: "1rem",
                [theme.breakpoints.down('sm')]: {
                    marginTop: "3rem",
                },
            }}>
                <Box>
                    <DatePicker
                        selected={startDate}
                        onChange={date => date && setStartDate(date)}
                        selectsStart
                        startDate={startDate}
                        endDate={endDate}
                    />
                </Box>
                <Box>
                    <DatePicker
                        selected={startDate}
                        onChange={date => date && setEndDate(date)}
                        selectsEnd
                        startDate={startDate}
                        endDate={endDate}
                        minDate={startDate}

                    />
                </Box>
            </Box>
            <Box
                sx={{
                    [theme.breakpoints.down('sm')]: {
                        maxWidth: '140%',
                        width: '180%',
                        overflow: 'auto'
                    },
                }} >
                <Box mt="40px" height="75vh" sx={{
                    [theme.breakpoints.down('sm')]: {
                        width: '200%',
                        overflow: 'auto'
                    },
                }}>
                    {formattedData ? (

                        <Box height={"100%"}
                            width={"73rem"}
                            minHeight={"325px"}
                            minWidth={"325px"}
                            position="relative"
                            sx={{
                                [theme.breakpoints.down('sm')]: {
                                    width: '100%',
                                    maxWidth: '250%',
                                    overflow: 'auto'
                                },
                            }}>
                            <ResponsiveLine
                                data={formattedData}
                                theme={{
                                    axis: {
                                        domain: {
                                            line: {
                                                stroke: theme.palette.secondary[200],
                                            },
                                        },
                                        legend: {
                                            text: {
                                                fill: theme.palette.secondary[200],
                                            },
                                        },
                                        ticks: {
                                            line: {
                                                stroke: theme.palette.secondary[200],
                                                strokeWidth: 1,
                                            },
                                            text: {
                                                fill: theme.palette.secondary[200],
                                            },
                                        },
                                    },
                                    legends: {
                                        text: {
                                            fill: theme.palette.secondary[200],
                                        },
                                    },
                                    tooltip: {
                                        container: {
                                            color: theme.palette.primary.main,
                                        },
                                    },
                                }}
                                colors={{ scheme: 'set2' }}
                                margin={{ top: 50, right: 50, bottom: 70, left: 60 }}
                                xScale={{ type: "point" }}
                                yScale={{
                                    type: "linear",
                                    min: "auto",
                                    max: "auto",
                                    stacked: false,
                                    reverse: false,
                                }}
                                yFormat=" >-.2f"
                                curve="catmullRom"
                                axisTop={null}
                                axisRight={null}
                                axisBottom={{
                                    tickSize: 5,
                                    tickPadding: 5,
                                    tickRotation: 90,
                                    legend: "Month",
                                    legendOffset: 60,
                                    legendPosition: "middle",
                                }}
                                axisLeft={{
                                    tickSize: 5,
                                    tickPadding: 5,
                                    tickRotation: 0,
                                    legend: "Total",
                                    legendOffset: -50,
                                    legendPosition: "middle",
                                }}
                                enableGridX={false}
                                enableGridY={false}
                                pointSize={10}
                                pointColor={{ theme: "background" }}
                                pointBorderWidth={2}
                                pointBorderColor={{ from: "serieColor" }}
                                pointLabelYOffset={-12}
                                useMesh={true}
                                legends={[
                                    {
                                        anchor: "top-right",
                                        direction: "column",
                                        justify: false,
                                        translateX: 50,
                                        translateY: 0,
                                        itemsSpacing: 0,
                                        itemDirection: "left-to-right",
                                        itemWidth: 95,
                                        itemHeight: 20,
                                        itemOpacity: 0.75,
                                        symbolSize: 12,
                                        symbolShape: "circle",
                                        symbolBorderColor: "rgba(0, 0, 0, .5)",
                                        effects: [
                                            {
                                                on: "hover",
                                                style: {
                                                    itemBackground: "rgba(0, 0, 0, .03)",
                                                    itemOpacity: 1,
                                                },
                                            },
                                        ],
                                    },
                                ]}
                            />
                        </Box>
                    ) : (
                        <Loading />
                    )}
                </Box>
            </Box>
        </Box>
    );
};

export default DailyReport