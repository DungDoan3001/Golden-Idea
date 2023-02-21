import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../../App";
import RequireAuth from "./RequireAuth";
import Dashboard from "../../pages/dashboard";
import Exception from "../../pages/exception";

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
                element: <RequireAuth roles={['Admin']} />, children: [
                ]
            },
            { path: 'dashboard', element: <Dashboard /> },
            { path: 'exception', element: <Exception /> },
            { path: '*', element: <Navigate replace to='/not-found' /> }
        ]
    }
])