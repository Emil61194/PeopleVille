import { waitForConnection } from "../services/WSService.js";

export function getTime() {
  return waitForConnection().then((connection) => connection.invoke("GetTime"));
}
