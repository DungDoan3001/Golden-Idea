import { CssBaseline, ThemeProvider, createTheme } from "@mui/material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useSelector } from "react-redux";
import { themeSettings } from "./theme";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useAppDispatch } from "./app/store/configureStore";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Loading from "./app/components/Loading";
import Login from "./pages/account/login";
import { fetchCurrentUser } from "./pages/account/accountSlice";

const App = () => {
  const mode = useSelector((state: any) => state.global.mode);
  const theme = useMemo(() => createTheme(themeSettings(mode)), [mode]);
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);
  const location = useLocation();
  const navigate = useNavigate();
  const initApp = useCallback(async () => {
    try {
      await dispatch(fetchCurrentUser());
      var token: any = sessionStorage.getItem('user');
      var tokenValue = JSON.parse(token).token;
      if (tokenValue) {
        navigate(location.state?.from || '/home');
      }
    } catch (error) {
      console.log(error);
    }
  }, [dispatch])

  useEffect(() => {
    initApp().then(() => setLoading(false));
  }, [initApp])

  if (loading) return <Loading message='Initializing app...' />

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <ToastContainer />
      {loading ? <Loading message="Initialising app..." />
        : location.pathname === '/' ? <Login />
          : <Outlet />
      }

    </ThemeProvider>
  );
}

export default App;
