import React from "react";
import {
  Box,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
  useTheme,
} from "@mui/material";
import {
  Topic,
  Forum,
  ChevronLeft,
  ChevronRightOutlined,
  HomeOutlined,
  LightbulbCircle,
  Groups2Outlined,
  Category,
  PointOfSaleOutlined,
  SwitchAccount,
  AssignmentLate,
  AdminPanelSettingsOutlined,
  TrendingUpOutlined,
  PieChartOutlined,
  TipsAndUpdates,
  RoomPreferences,
} from "@mui/icons-material";
import DashboardIcon from '@mui/icons-material/Dashboard';
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import FlexBetween from "./FlexBetween";
import { useStoreContext } from "../context/ContextProvider";
import { useAppSelector } from "../store/configureStore";

const navItems = [
  {
    text: "Home",
    icon: <HomeOutlined />,
    roles: ["Staff", "QA Coordinator", "Administrator", "QA Manager"],
  },
  {
    text: "My Ideas",
    icon: <LightbulbCircle />,
    roles: ["Staff", "QA Coordinator", "Administrator", "QA Manager"],
  },
  {
    text: "Dashboard",
    icon: <DashboardIcon />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Management",
    icon: null,
    roles: ["Administrator", "QA Coordinator", "QA Manager"],
  },
  {
    text: "Staffs",
    icon: <Groups2Outlined />,
    roles: ["Administrator"],
  },
  {
    text: "Admin",
    icon: <AdminPanelSettingsOutlined />,
    roles: ["Administrator"],
  },
  {
    text: "Departments",
    icon: <RoomPreferences />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Categories",
    icon: <Category />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Topics",
    icon: <Topic />,
    roles: ["Administrator", "QA Manager", "QA Coordinator"],
  },
  {
    text: "Comments",
    icon: <Forum />,
    roles: ["Administrator"],
  },
  {
    text: "Data visualization",
    icon: null,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Overview",
    icon: <PointOfSaleOutlined />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Contributors",
    icon: < SwitchAccount />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Exception",
    icon: <AssignmentLate />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Breakdown",
    icon: <PieChartOutlined />,
    roles: ["Administrator", "QA Manager"],
  },
  {
    text: "Daily Report",
    icon: <TrendingUpOutlined />,
    roles: ["Administrator", "QA Manager"],
  },
];
interface sideBarProps {
  drawerWidth: any;
  isNonMobile: any;
}
const Sidebar = ({
  drawerWidth,
  isNonMobile,
}: sideBarProps) => {
  //Get user info here
  const { user } = useAppSelector(state => state.account);
  const { pathname } = useLocation();
  const [active, setActive] = useState("");
  const navigate = useNavigate();
  const theme: any = useTheme();
  const { isSidebarOpen, setIsSidebarOpen, screenSize } = useStoreContext();
  useEffect(() => {
    setActive(pathname.substring(1));
  }, [pathname]);
  const handleCloseSidebar = () => {
    if (isSidebarOpen && screenSize <= 900) {
      setIsSidebarOpen(false);
    }
  }
  // Filter the navItems array based on the user's role
  const filteredNavItems = navItems.filter((item) =>
    item.roles.includes(`${user?.role[0]}`)
  );
  console.log(filteredNavItems)
  return (
    <Box component="nav">
      {isSidebarOpen && (
        <Drawer
          open={isSidebarOpen}
          onClose={() => setIsSidebarOpen(false)}
          variant="persistent"
          anchor="left"
          sx={{
            width: drawerWidth,
            "& .MuiDrawer-paper": {
              color: theme.palette.secondary[200],
              backgroundColor: theme.palette.background.alt,
              boxSixing: "border-box",
              borderWidth: isNonMobile ? 0 : "2px",
              width: drawerWidth,
            },
          }}
        >
          <Box width="100%">
            <Box m="1.5rem 1.5rem 2rem 3rem">
              <FlexBetween color={theme.palette.secondary.main}>
                <Box display="flex" alignItems="center" gap="0.5rem">
                  <Typography variant="h4" align='center' fontWeight="bold">
                    <TipsAndUpdates />GOLDEN IDEA
                  </Typography>
                </Box>
                {!isNonMobile && (
                  <IconButton onClick={() => setIsSidebarOpen(!isSidebarOpen)}>
                    <ChevronLeft />
                  </IconButton>
                )}
              </FlexBetween>
            </Box>
            <List>
              {filteredNavItems.map(({ text, icon }) => {
                if (!icon) {
                  return (
                    <Typography key={text} sx={{ m: "2.25rem 0 1rem 3rem" }}>
                      {text}
                    </Typography>
                  );
                }
                const lcText = text.toLowerCase().replaceAll(' ', '');

                return (
                  <ListItem key={text} disablePadding>
                    <ListItemButton
                      onClick={() => {
                        navigate(`/${lcText}`);
                        setActive(lcText);
                        handleCloseSidebar();
                      }}
                      sx={{
                        backgroundColor:
                          active === lcText
                            ? theme.palette.secondary[300]
                            : "transparent",
                        color:
                          active === lcText
                            ? theme.palette.primary[600]
                            : theme.palette.secondary[100],
                      }}
                    >
                      <ListItemIcon
                        sx={{
                          ml: "2rem",
                          color:
                            active === lcText
                              ? theme.palette.primary[600]
                              : theme.palette.secondary[200],
                        }}
                      >
                        {icon}
                      </ListItemIcon>
                      <ListItemText primary={text} />
                      {active === lcText && (
                        <ChevronRightOutlined sx={{ ml: "auto" }} />
                      )}
                    </ListItemButton>
                  </ListItem>
                );
              })}
            </List>
          </Box>
        </Drawer>
      )}
    </Box>
  );
};

export default Sidebar;