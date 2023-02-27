import { postData } from "../../dataTest.js"

const Service = {
  getData: (from: any, to: any) => {
    return new Promise((resolve, rejects) => {

      const data = postData.slice(from, to);

      resolve({
        count: postData.length,
        data: data,
      });
    })
  }
}

export default Service;