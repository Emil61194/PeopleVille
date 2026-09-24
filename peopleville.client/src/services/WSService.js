import { HubConnectionBuilder } from "@microsoft/signalr";

export let connection;
const eventListeners = new Set();
const world = { homes: [], citizens: [], workplaces: [] };
let onWorldUpdate;

export function subscribeToEvents(listener) {
  eventListeners.add(listener);
  return () => eventListeners.delete(listener);
}

function notifyWorldUpdate() {
  onWorldUpdate?.({
    homes: world.homes,
    citizens: world.citizens,
    workplaces: world.workplaces,
  });
}

export async function connectToHub(onLog, worldUpdateHandler) {
  if (connection) return;

  const log = (message) => onLog?.(message);
  onWorldUpdate = worldUpdateHandler;

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
    fetchWorldData();
  });
  connection.on("Event", (message) => {
    eventListeners.forEach((listener) => listener(message));
    const value =
      typeof message === "string"
        ? message
        : (message?.message ?? JSON.stringify(message));
    log(value);
    applyEvent(message);
  });

  try {
    await connection.start();
    log("Connected");
    await fetchWorldData();
  } catch (error) {
    log(`Connection failed: ${error.message}`);
  }
}

async function fetchWorldData() {
  if (!connection) return;
  try {
    const [homes, citizens, workplaces] = await Promise.all([
      connection.invoke("GetAllHomes"),
      connection.invoke("GetAllCitizens"),
      connection.invoke("GetAllWorkplaces"),
    ]);
    world.homes = homes ?? [];
    world.citizens = citizens ?? [];
    world.workplaces = workplaces ?? [];
    notifyWorldUpdate();
  } catch (error) {
    console.error("Failed to fetch world data:", error);
  }
}

function applyEvent(message) {
  if (message && typeof message === "object") {
    if (Number.isInteger(message.citizenId)) {
      applyCitizenEvent(message);
    } else if (
      Number.isInteger(message.foodInventory) &&
      typeof message.address === "string"
    ) {
      applyHomeEvent(message);
    }
  }
}

function applyCitizenEvent(event) {
  world.citizens = world.citizens.map((citizen) =>
    citizen.id === event.citizenId
      ? { ...citizen, currentLocation: event.currentLocation }
      : citizen,
  );
  notifyWorldUpdate();
}

function applyHomeEvent(event) {
  world.homes = world.homes.map((home) => {
    if (home.address !== event.address) return home;
    return {
      ...home,
      foodInventory: event.foodInventory,
      waterInventory: event.waterInventory,
      bankAccount: {
        ...(home.bankAccount ?? {}),
        ...(event.bankAccountBalance !== undefined
          ? { balance: event.bankAccountBalance }
          : {}),
      },
    };
  });
  notifyWorldUpdate();
}
