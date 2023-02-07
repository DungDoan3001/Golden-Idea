import * as React from "react";
import { Router, Navigator } from "react-router-dom";

type Props = {
  basename?: string;
  children: React.ReactNode;
  history: any;
}

const CustomRouter = ({ basename, children, history }: Props) => {
  const [state, setState] = React.useState({
    action: history.action,
    location: history.location,
  });
  
  React.useLayoutEffect(() => history.listen(setState),[history])

  return (
    <Router
      basename={basename}
      location={state.location}
      navigator={history}
      navigationType={state.action}
    >
      {children}
    </Router>
  );
};
export default CustomRouter;