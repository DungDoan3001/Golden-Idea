import { Container, CssBaseline, ThemeProvider, createTheme } from "@mui/material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useSelector } from "react-redux";
import { themeSettings } from "./theme";
import { Navigate, Route, Routes } from "react-router-dom";
import Home from "./pages/home";
import Dashboard from "./pages/dashboard";
import { useAppDispatch } from "./app/store/configureStore";
import Loading from "./app/components/Loading";
import Layout from "./pages/layout";
import Breakdown from "./pages/breakdown";
import Staffs from "./pages/staffs";

const App =() => {
  const mode = useSelector((state: any) => state.global.mode);
  const theme = useMemo(() => createTheme(themeSettings(mode)), [mode]);
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);

  const initApp = useCallback(async () => {
    try {
      // await dispatch(fetchCurrentUser());
      // await dispatch(fetchBasketAsync());
      console.log('dispatch Current User');
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
      <Routes>
      <Route element={<Layout />}>
      <Route path="/" element={<Navigate to="/home" replace />} />
      <Route path='/home' element={<Home/>} />
      <Route path='/dashboard' element={<Dashboard/>} />
      <Route path='/staffs' element={<Staffs/>} />
      <Route path='/breakdown' element={<Breakdown/>} />
      </Route>
      </Routes>
    </ThemeProvider>
  );
}

export default App;
