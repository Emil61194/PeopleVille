import { HubConnectionBuilder } from "@microsoft/signalr";

let connection;

export async function connectToHub(onLog) {
  if (connection) return;

  const log = (message) => onLog?.(message);

  connection = new HubConnectionBuilder()
    .withUrl(`http://localhost:5045/hubs/game`)
    .withAutomaticReconnect()
    .build();

  connection.onclose((error) =>
    log(`Connection closed${error ? `: ${error.message}` : ""}`),
  );
  connection.onreconnecting((error) =>
    log(`Reconnecting${error ? `: ${error.message}` : ""}`),
  );
  connection.onreconnected(() => log("Connected"));

  try {
    await connection.start();
    log("Connected");
  } catch (error) {
    log(`Connection failed: ${error.message}`);
  }
}
