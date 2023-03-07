import React from "react";
import { Box, Pagination } from "@mui/material";
import { useEffect, useState } from "react";
import Service from "../utils/Service";

// import { postData } from "../../dataTest.js"

const AppPagination = ({ setItem, data, size }: any) => {
  const pageSize = size;
  const [pagination, setPagination] = useState({
    count: 0,
    from: 0,
    to: pageSize,
  });

  useEffect(() => {
    Service.getData(data, pagination.from, pagination.to).then((response: any) => {
      setPagination({ ...pagination, count: response.count })

      setItem(response.data);
    })
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pagination.from, pagination.to]);

  const HandlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
    const from = (value - 1) * pageSize;
    const to = (value - 1) * pageSize + pageSize;

    setPagination({ ...pagination, from: from, to: to })
  }

  return (
    <Box mt="3rem" display='flex' justifyContent='center' alignItems='center' sx={{ marginBottom: 3 }}>
      <Pagination
        color='secondary'
        size='large'
        count={Math.ceil(pagination.count / pageSize)}
        onChange={HandlePageChange}
      />
    </Box>
  )
}

export default AppPagination