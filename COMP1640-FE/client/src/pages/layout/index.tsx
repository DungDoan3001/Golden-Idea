import { Box, useMediaQuery } from "@mui/material";
import { Outlet, useNavigate } from "react-router-dom";
import Navbar from "../../app/components/Navbar";
import Sidebar from "../../app/components/Sidebar";
import alanBtn from "@alan-ai/alan-sdk-web";
import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { setMode } from "../../app/utils/AppSlice";

const Layout = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  useEffect(() => {
    alanBtn({
      key: 'a30779b9b719acd29bc0cfbf2c32f36f2e956eca572e1d8b807a3e2338fdd0dc/stage',
      onCommand: (commandData: any) => {
        if (commandData.command === 'search') {
          // Call the client code that will react to the received command
          navigate(`/search/${commandData.searchString}`);
        }
        else if (commandData.command === 'navigate') {
          if (commandData.page !== "") navigate(`/${commandData.page}`);
        }
        else if (commandData.command === 'toggleColorMode') {
          dispatch(setMode());
        }
      }
    });
  }, []);
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
        <Outlet />
      </Box>
    </Box>
  );
};

export default Layout;