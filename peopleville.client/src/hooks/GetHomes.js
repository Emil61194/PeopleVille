import { connection } from "../services/WSService.js";

export function GetHomes() {
  if (connection?.state !== "Connected") {
    return Promise.reject(new Error("The game connection is not ready."));
  }

  let xx = connection.invoke("GetAllHomes");
  console.log(xx);
  return connection.invoke("GetAllHomes");
}
