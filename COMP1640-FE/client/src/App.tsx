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
import Category from "./pages/category";
import Department from "./pages/department";
import Exception from "./pages/exception";
import Contributors from "./pages/contributor";
import Overview from "./pages/overview";
import DailyReport from "./pages/daily";
import Topic from "./pages/topic";
import Comment from "./pages/comment";
import Login from "./pages/account/login";

const App = () => {
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
        <Route path='/login' element={<Login />} />
        <Route element={<Layout />}>
          <Route path="/" element={<Navigate to="/home" replace />} />
          <Route path='/home' element={<Home />} />
          <Route path='/dashboard' element={<Dashboard />} />
          <Route path='/staffs' element={<Staffs />} />
          <Route path='/categories' element={<Category />} />
          <Route path='/departments' element={<Department />} />
          <Route path='/topics' element={<Topic />} />
          <Route path='/comments' element={<Comment />} />
          <Route path='/overview' element={<Overview />} />
          <Route path='/breakdown' element={<Breakdown />} />
          <Route path='/exception' element={<Exception />} />
          <Route path='/contributors' element={<Contributors />} />
          <Route path='/dailyreport' element={<DailyReport />} />
        </Route>
      </Routes>
    </ThemeProvider>
  );
}

export default App;
