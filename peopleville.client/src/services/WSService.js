import { HubConnectionBuilder } from "@microsoft/signalr";

export let connection;
const eventListeners = new Set();
const world = { homes: [], citizens: [], workplaces: [] };
let onWorldUpdate;
let onLog;
let sessionActive = false;
let retryDelay = 1000;
let retryTimer;

const MAX_RETRY_DELAY = 15000;

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

const log = (message) => onLog?.(message);

  const log = (message, worldTime) => onLog?.(message, worldTime);
  onWorldUpdate = worldUpdateHandler;

  const conn = new HubConnectionBuilder()
    .withUrl("/hubs/game")
    .withAutomaticReconnect()
    .build();

  conn.onclose((error) => {
    log(`Connection lost${error ? `: ${error.message}` : ""}. Retrying...`);
    if (sessionActive) scheduleRetry();
  });
  conn.onreconnecting((error) => {
    log(`Reconnecting${error ? `: ${error.message}` : ""}`);
  });
  conn.onreconnected(() => {
    log("Reconnected");
    loadInitialWorld();
  });
  conn.on("Event", (message) => {
    eventListeners.forEach((listener) => listener(message));
    const value =
      typeof message === "string"
        ? message
        : (message?.message ?? JSON.stringify(message));
    log(value, message?.worldTime);
    applyEvent(message);
  });

  connection = conn;
  return conn;
}

async function connect() {
  if (!sessionActive) return;

  const conn = buildConnection();

  try {
    await conn.start();
    retryDelay = 1000;
    log("Connected");
    await loadInitialWorld();
  } catch (error) {
    log(`Connection failed: ${error.message}. Retrying in ${retryDelay}ms...`);
    scheduleRetry();
  }
}

export function connectToHub(onLogHandler, worldUpdateHandler) {
  onLog = onLogHandler;
  onWorldUpdate = worldUpdateHandler;

  if (sessionActive) return;

  sessionActive = true;
  retryDelay = 1000;
  clearTimeout(retryTimer);

  if (connection?.state === "Connected") {
    loadInitialWorld();
  } else {
    connect();
  }
}

export function resetConnection() {
  sessionActive = false;
  clearTimeout(retryTimer);
  if (connection) {
    connection.off("Event");
    connection.stop().catch(() => {});
    connection = undefined;
  }
  world.homes = [];
  world.citizens = [];
  world.workplaces = [];
  notifyWorldUpdate();
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