
const Service = {
  getData: (data:any, from: any, to: any) => {
    return new Promise((resolve, rejects) => {

      const sdata = data.slice(from, to);

      resolve({
        count: data.length,
        data: sdata,
      });
    })
  }
}

export default Service;