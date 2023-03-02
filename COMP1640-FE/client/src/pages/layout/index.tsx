import { Box, useMediaQuery, Zoom } from "@mui/material";
import { Outlet } from "react-router-dom";
import Navbar from "../../app/components/Navbar";
import Sidebar from "../../app/components/Sidebar";

const Layout = () => {
  const isNonMobile = useMediaQuery("(min-width: 600px)");

  return (
    <Box display={isNonMobile ? "flex" : "block"} width="100%" height="100%">
      <Sidebar
        isNonMobile={isNonMobile}
        drawerWidth="250px"
      />
      <Box flexGrow={1}>
        <Navbar
        />
        <Zoom style={{ zoom: isNonMobile ? '100%' : '70%' }}><Outlet /></Zoom>
      </Box>
    </Box>
  );
};

export default Layout;