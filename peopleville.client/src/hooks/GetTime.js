import { connection } from "../services/WSService.js";

export function getTime() {
  if (connection?.state !== "Connected") {
    return Promise.reject(new Error("The game connection is not ready."));
  }

  return connection.invoke("GetTime");
}
