import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../../App";
import RequireAuth from "./RequireAuth";
import Dashboard from "../../pages/dashboard";
import Exception from "../../pages/exception";
import Home from "../../pages/home";
import Staffs from "../../pages/staffs";
import Category from "../../pages/category";
import Layout from "../../pages/layout";
import Topic from "../../pages/topic";
import Department from "../../pages/department";
import Comment from "../../pages/comment";
import Overview from "../../pages/overview";
import Breakdown from "../../pages/breakdown";
import Contributors from "../../pages/contributor";
import DailyReport from "../../pages/daily";
import NotFound from "../errors/NotFound";
import ServerError from "../errors/ServerError";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            // user authenticated routes
            {
                element: <RequireAuth />, children: [
                ]
            },
            // admin routes
            {
                element: <RequireAuth roles={['Admin']} />, children: [
                ]
            },
            // QA manager routes
            {
                element: <RequireAuth roles={['QAManager']} />, children: [
                ]
            },
            {
                element: <Layout />, children: [
                    { path: 'home', element: <Home /> },
                    { path: 'dashboard', element: <Dashboard /> },
                    { path: 'staffs', element: <Staffs /> },
                    { path: 'categories', element: <Category /> },
                    { path: 'topics', element: <Topic /> },
                    { path: 'departments', element: <Department /> },
                    { path: 'comments', element: <Comment /> },
                    { path: 'overview', element: <Overview /> },
                    { path: 'breakdown', element: <Breakdown /> },
                    { path: 'contributors', element: <Contributors /> },
                    { path: 'dailyreport', element: <DailyReport /> },
                    { path: 'exception', element: <Exception /> },
                ]
            },
            { path: 'server-error', element: <ServerError /> },
            { path: 'not-found', element: <NotFound /> },
            { path: '*', element: <Navigate replace to='/not-found' /> }
        ]
    }
])