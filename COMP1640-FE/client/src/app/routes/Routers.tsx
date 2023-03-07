import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../../App";
import RequireAuth from "./RequireAuth";
import Dashboard from "../../pages/dashboard";
import Exception from "../../pages/exception";
import Home from "../../pages/home";
import MyIdeas from "../../pages/myIdeas";
import IdeaDetail from "../../pages/ideaDetail";
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
import ForgotPassword from "../../pages/account/forgotpassword";
import ResetPass from "../../pages/account/resetpassword";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            // User authenticated routes
            {
                element: <RequireAuth />, children: [
                ]
            },
            // Admin routes
            {
                element: <RequireAuth roles={['Administrator']} />, children: [
                ]
            },
            // QA manager routes
            {
                element: <RequireAuth roles={['QA Manager']} />, children: [
                ]
            },
            // QA coordinator routes
            {
                element: <RequireAuth roles={[' QA Coordinator']} />, children: [
                ]
            },
            {
                element: <Layout />, children: [
                    { path: 'home', element: <Home /> },
                    { path: 'myIdeas', element: <MyIdeas /> },
                    { path: 'ideaDetail', element: <IdeaDetail /> },
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
            { path: 'forgot', element: <ForgotPassword /> },
            { path: 'test', element: <ForgotPassword /> },
            { path: 'change-password/:resetCode', element: <ResetPass /> },
            { path: '*', element: <Navigate replace to='/not-found' /> }
        ]
    }
])