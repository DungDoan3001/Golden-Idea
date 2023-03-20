import { Navigate, Outlet, useLocation, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useAppSelector } from "../store/configureStore";
import { useEffect } from "react";

interface Props {
    roles?: string[];
}
export default function RequireAuth({ roles }: Props) {
    const { user } = useAppSelector(state => state.account);
    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        if (!user) {
            toast.error('Login to access this area');
            navigate('/login', { state: { from: location } });
        } else if (roles && !roles.some(r => user.role?.includes(r))) {
            toast.error('Not authorized to access this area');
            navigate('/home');
        }
    }, [user, navigate, location, roles]);

    if (!user) {
        return null;
    }

    return <Outlet />;
}