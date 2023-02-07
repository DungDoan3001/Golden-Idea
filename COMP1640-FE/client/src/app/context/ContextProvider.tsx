import { createContext, PropsWithChildren, useContext, useState } from "react";

interface StoreContextValue {
    screenSize: any;
    setScreenSize: (screenSize:any) => void;
    isSidebarOpen: boolean;
    setIsSidebarOpen: (params:boolean) => void;
}
export const StoreContext = createContext<StoreContextValue | undefined>(undefined);

export function useStoreContext() {
    const context = useContext(StoreContext);

    if (context === undefined) {
        throw Error('Oops - we do not seem to be inside the provider');
    }

    return context;
}

export function ContextProvider({children}: PropsWithChildren<any>) {
    const [screenSize, setScreenSize] = useState(undefined);
    const [ isSidebarOpen,setIsSidebarOpen] = useState(true);
      return (
        // eslint-disable-next-line react/jsx-no-constructed-context-values
        <StoreContext.Provider value={{screenSize, setScreenSize, setIsSidebarOpen, isSidebarOpen}}>
          {children}
        </StoreContext.Provider>
    );
}