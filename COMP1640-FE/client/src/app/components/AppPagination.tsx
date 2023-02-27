import React from "react";
import { Box, Pagination } from "@mui/material";
import { useEffect, useState } from "react";
import Service from "../utils/Service";

const AppPagination = ({setPost}:any) => {
  const pageSize = 6;
  const [pagination, setPagination] = useState({
    count: 0,
    from: 0,
    to: pageSize,
  });

  useEffect(() => {
    Service.getData(pagination.from,pagination.to).then((response: any) => {
      setPagination({ ...pagination, count: response.count })

      setPost(response.data);
      console.log(response);
    })
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