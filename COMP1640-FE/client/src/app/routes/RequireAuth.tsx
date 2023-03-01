import { Navigate, Outlet, useLocation } from "react-router-dom";
import { toast } from "react-toastify";
import { useAppSelector } from "../store/configureStore";
import { useEffect } from "react";

interface Props {
    roles?: string[];
}

export default function RequireAuth({ roles }: Props) {
    const { user } = useAppSelector(state => state.account);
    const location = useLocation();
    useEffect(() => {
        if (!user) toast.error('Login to access this area');
    }, []);
    if (!user) {
        return <Navigate to='/login' state={{ from: location }} />
    }

    if (roles && !roles.some(r => user.role?.includes(r))) {
        toast.error('Not authorized to access this area');
        return <Navigate to='/login' />
    }
    return <Outlet />
}