import { HubConnectionBuilder } from "@microsoft/signalr";

let connection;

export async function connectToHub(onLog, onWorldUpdate) {
  if (connection) return;

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

  try {
    await connection.start();
    log("Connected");
    await fetchWorldData(onWorldUpdate);
  } catch (error) {
    log(`Connection failed: ${error.message}`);
  }
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
