import { HubConnectionBuilder } from "@microsoft/signalr";

export let connection;
let connectionStartPromise;

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
  connection.onreconnected(() => {
    log("Connected");
    fetchWorldData(onWorldUpdate);
  });
  connection.on("Event", (message) => {
    const value =
      typeof message === "string" ? message : JSON.stringify(message);
    log(`Event: ${value}`);
    fetchWorldData(onWorldUpdate);
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

async function fetchWorldData(onWorldUpdate) {
  if (!connection || !onWorldUpdate) return;
  try {
    const [homes, citizens, workplaces] = await Promise.all([
      connection.invoke("GetAllHomes"),
      connection.invoke("GetAllCitizens"),
      connection.invoke("GetAllWorkplaces"),
    ]);
    onWorldUpdate({ homes, citizens, workplaces });
  } catch (error) {
    console.error("Failed to fetch world data:", error);
  }
}
