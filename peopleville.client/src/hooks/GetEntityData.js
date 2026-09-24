import { waitForConnection } from "../services/WSService.js";

function invoke(method, ...args) {
  return waitForConnection().then((connection) => connection.invoke(method, ...args));
}

export function GetHouseData(address) {
  return invoke("GetHouseData", address);
}

export function GetApartmentData(address) {
  return invoke("GetApartmentData", address);
}

export function GetCitizenData(id) {
  return invoke("GetCitizenData", id);
}

export function GetWorkplaceData(address) {
  return invoke("GetWorkplaceData", address);
}
