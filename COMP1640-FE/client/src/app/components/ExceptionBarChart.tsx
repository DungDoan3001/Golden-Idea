import { ResponsiveBar } from '@nivo/bar'
import { Box, useTheme } from "@mui/material";
interface Props {
    isComment: boolean,
    dataChart: any
}
const ExceptionBarChart = ({ isComment, dataChart }: Props) => {
    const theme: any = useTheme();

    return (
        <Box height={"100%"}
            width={"73rem"}
            minHeight={"325px"}
            minWidth={"325px"}
            position="relative">
            <ResponsiveBar
                data={dataChart}
                theme={{
                    // added
                    axis: {
                        domain: {
                            line: {
                                stroke: theme.palette.secondary[100],
                            },
                        },
                        legend: {
                            text: {
                                fill: theme.palette.secondary[100],
                            },
                        },
                        ticks: {
                            line: {
                                stroke: theme.palette.secondary[100],
                                strokeWidth: 1,
                            },
                            text: {
                                fill: theme.palette.secondary[100],
                            },
                        },
                    },
                    legends: {
                        text: {
                            fill: theme.palette.secondary[100],
                        },
                    },
                    tooltip: {
                        container: {
                            color: theme.palette.modify.default,
                        },
                    },
                }}
                keys={isComment ? ["total comments"] : ["no comment", "anonymous",]}
                indexBy="department"
                margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
                padding={0.3}
                valueScale={{ type: "linear" }}
                indexScale={{ type: "band", round: true }}
                motionConfig={"stiff"}
                colors={{ scheme: isComment ? 'nivo' : 'set2' }}
                defs={[
                    {
                        id: "dots",
                        type: "patternDots",
                        background: "inherit",
                        color: "#38bcb2",
                        size: 4,
                        padding: 1,
                        stagger: true,
                    },
                    {
                        id: "lines",
                        type: "patternLines",
                        background: "inherit",
                        color: "#eed312",
                        rotation: -45,
                        lineWidth: 6,
                        spacing: 10,
                    },
                ]}
                borderColor={{
                    from: "color",
                    modifiers: [["darker", 1.6]],
                }}
                axisTop={null}
                axisRight={null}
                axisBottom={{
                    tickSize: 5,
                    tickPadding: 10,
                    tickRotation: 0,
                    legend: "Department", // changed
                    legendPosition: "middle",
                    legendOffset: 32,
                }}
                axisLeft={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: isComment ? "Comments" : "Ideas", // changed
                    legendPosition: "middle",
                    legendOffset: -40,
                }}
                enableLabel={true}
                labelSkipWidth={12}
                labelSkipHeight={12}
                labelTextColor={{
                    from: "color",
                    modifiers: [["darker", 10]],
                }}
                legends={[
                    {
                        dataFrom: "keys",
                        anchor: "bottom-right",
                        direction: "column",
                        justify: false,
                        translateX: 120,
                        translateY: 0,
                        itemsSpacing: 2,
                        itemWidth: 100,
                        itemHeight: 20,
                        itemDirection: "left-to-right",
                        itemOpacity: 0.85,
                        symbolSize: 20,
                        effects: [
                            {
                                on: "hover",
                                style: {
                                    itemOpacity: 1,
                                },
                            },
                        ],
                    },
                ]}
                role="application"
                barAriaLabel={function (e) {
                    return e.id + ": " + e.formattedValue + " in country: " + e.indexValue;
                }}
            />
        </Box>

    );
};
export default ExceptionBarChart