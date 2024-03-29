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
import Overview from "../../pages/overview";
import Breakdown from "../../pages/breakdown";
import Contributors from "../../pages/contributor";
import DailyReport from "../../pages/daily";
import NotFound from "../errors/NotFound";
import ServerError from "../errors/ServerError";
import ForgotPassword from "../../pages/account/forgotpassword";
import ResetPass from "../../pages/account/resetpassword";
import Login from "../../pages/account/login";
import AdminPage from "../../pages/staffs/index.admin";
import ListIdeas from "../../pages/myIdeas/index.ideas";
import ListMyIdeas from "../../pages/myIdeas/index.myideas";
import IdeaForm from "../../pages/ideaDetail/ideaForm";
import SearchPage from "../../pages/home/index.search";
import ListCatagoryIdeas from "../../pages/myIdeas/index.categoryideas";

export const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      // User authenticated routes
      {
        element: <RequireAuth />, children: [
          {
            element: <Layout />, children: [
              { path: 'home', element: <Home /> },
              { path: 'topic/:id/:name', element: <ListIdeas /> },
              { path: 'myTopic/:id/:name', element: <ListMyIdeas /> },
              { path: 'myIdeas', element: <MyIdeas /> },
              { path: 'ideaDetail/:slug', element: <IdeaDetail /> },
              { path: 'catagoryTopic/:name', element: <ListCatagoryIdeas /> },
              { path: 'search/:filter', element: <SearchPage /> },
              { path: 'ideaform/:topicId/', element: <IdeaForm /> },
            ]
          },
        ]
      },
      // Admin, QA Manager, QA Coordinator routes
      {
        element: <RequireAuth roles={['Administrator', 'QA Manager', 'QA Coordinator']} />, children: [
          {
            element: <Layout />, children: [
              { path: 'topics', element: <Topic /> },
              {
                element: <RequireAuth roles={['Administrator', 'QA Manager']} />, children: [
                  { path: 'dashboard', element: <Dashboard /> },
                  { path: 'categories', element: <Category /> },
                  { path: 'departments', element: <Department /> },
                  { path: 'overview', element: <Overview /> },
                  { path: 'breakdown', element: <Breakdown /> },
                  { path: 'contributors', element: <Contributors /> },
                  { path: 'dailyreport', element: <DailyReport /> },
                  { path: 'exception', element: <Exception /> },
                  {
                    element: <RequireAuth roles={['Administrator']} />, children: [
                      { path: 'staffs', element: <Staffs /> },
                      { path: 'admin', element: <AdminPage /> },
                    ]
                  }
                ]
              }

            ]
          },
        ]
      },
      { path: 'server-error', element: <ServerError /> },
      { path: 'login', element: <Login /> },
      { path: 'not-found', element: <NotFound /> },
      { path: 'forgot', element: <ForgotPassword /> },
      { path: 'change-password/:resetCode', element: <ResetPass /> },
      { path: '*', element: <Navigate replace to='/not-found' /> }
    ]
  }
])