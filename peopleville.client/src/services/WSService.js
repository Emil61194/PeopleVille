import { HubConnectionBuilder } from "@microsoft/signalr";

export let connection;
let connectionStartPromise;
const eventListeners = new Set();

export function subscribeToEvents(listener) {
  eventListeners.add(listener);
  return () => eventListeners.delete(listener);
}

export async function connectToHub(onLog) {
  if (connection?.state === "Connected") return true;
  if (connectionStartPromise) return connectionStartPromise;

  const log = (message) => onLog?.(message);

  connection = new HubConnectionBuilder()
    .withUrl("/hubs/game")
    .withAutomaticReconnect()
    .build();

  connection.onclose((error) =>
    log(`Connection closed${error ? `: ${error.message}` : ""}`),
  );
  connection.onreconnecting((error) =>
    log(`Reconnecting${error ? `: ${error.message}` : ""}`),
  );
  connection.onreconnected(() => log("Connected"));
  connection.on("Event", (message) => {
    eventListeners.forEach((listener) => listener(message));
    const value =
      typeof message === "string" ? message : JSON.stringify(message);
    log(`Event: ${value}`);
  });

  connectionStartPromise = connection
    .start()
    .then(() => {
      log("Connected");
      return true;
    })
    .catch((error) => {
      log(`Connection failed: ${error.message}`);
      connection = undefined;
      return false;
    })
    .finally(() => {
      connectionStartPromise = undefined;
    });

  return connectionStartPromise;
}
