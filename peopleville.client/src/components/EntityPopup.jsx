import { useEffect, useState } from "react";
import { WSConsole } from "./WSConsole.jsx";
import {
  GetApartmentData,
  GetCitizenData,
  GetHouseData,
  GetWorkplaceData,
} from "../hooks/GetEntityData.js";
import { subscribeToEvents } from "../services/WSService.js";

const detailLoaders = {
  home: (item) =>
    item?.rent !== undefined
      ? GetApartmentData(item.address)
      : GetHouseData(item.address),
  citizen: (item) => GetCitizenData(item.id),
  workplace: (item) => GetWorkplaceData(item.address),
};

function getEventValue(event, key) {
  return event?.[key] ?? event?.[key[0].toUpperCase() + key.slice(1)];
}

function eventMatchesItem(event, type, item) {
  const message = getEventValue(event, "message");
  const text = typeof message === "string" ? message.toLowerCase() : "";
  const address = getEventValue(item, "address");

  if (type === "citizen") {
    return (
      getEventValue(event, "citizenId") === getEventValue(item, "id") ||
      text.includes(
        `${getEventValue(item, "firstName")} ${getEventValue(item, "lastName")}`.toLowerCase(),
      )
    );
  }

  return (
    getEventValue(event, "address") === address ||
    getEventValue(event, "homeAddress") === address ||
    getEventValue(event, "currentLocation") === address ||
    (typeof address === "string" && text.includes(address.toLowerCase()))
  );
}

function getDetailValue(value) {
  if (value === null || value === undefined) return "-";
  if (typeof value === "object") return JSON.stringify(value);
  return String(value);
}

function EntityDetails({ item }) {
  return (
    <dl className="entityDetails">
      {Object.entries(item ?? {}).map(([key, value]) => (
        <div key={key}>
          <dt>{key}</dt>
          <dd>{getDetailValue(value)}</dd>
        </div>
      ))}
    </dl>
  );
}

export default function EntityPopup({ type, item, onClose }) {
  const [details, setDetails] = useState(item);
  const [logs, setLogs] = useState([]);
  const loader = detailLoaders[type];
  const title =
    type === "citizen"
      ? `${item.firstName ?? "Citizen"} ${item.lastName ?? ""}`.trim()
      : item.address ?? type;

  useEffect(() => {
    let cancelled = false;

    const refresh = () =>
      loader(item)
        .then((updatedItem) => {
          if (!cancelled) setDetails(updatedItem);
        })
        .catch((error) => {
          if (!cancelled) console.error(`Failed to load ${type}`, error);
        });

    refresh();
    const unsubscribe = subscribeToEvents((event) => {
      if (!eventMatchesItem(event, type, item)) return;
      setLogs((currentLogs) => [...currentLogs, event]);
      refresh();
    });

    return () => {
      cancelled = true;
      unsubscribe();
    };
  }, [item, loader, type]);

  return (
    <div className="entityPopupOverlay" onClick={onClose}>
      <section
        className="entityPopup"
        role="dialog"
        aria-modal="true"
        aria-labelledby="entityPopupTitle"
        onClick={(event) => event.stopPropagation()}
      >
        <div className="entityPopupHeader">
          <h2 id="entityPopupTitle">{title}</h2>
          <button type="button" onClick={onClose} aria-label="Close popup">
            Close
          </button>
        </div>
        <EntityDetails item={details} />
        <WSConsole logs={logs} />
      </section>
    </div>
  );
}
