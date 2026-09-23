import { useEffect, useState } from "react";
import { WSConsole } from "./WSConsole.jsx";
import {
  GetApartmentData,
  GetCitizenData,
  GetHouseData,
  GetWorkplaceData,
} from "../hooks/GetEntityData.js";
import { subscribeToEvents } from "../services/WSService.js";
import {
  getEntityFields,
  getEnumLabel,
  getFieldLabel,
  getGenderLabel,
} from "./EntityFields.jsx";

const detailLoaders = {
  home: (item) =>
    item?.rent !== undefined
      ? GetApartmentData(item.address)
      : GetApartmentData(item.address).catch(() => GetHouseData(item.address)),
  citizen: (item) => GetCitizenData(item.id),
  workplace: (item) => GetWorkplaceData(item.address),
};

function normalizeEvent(event) {
  if (typeof event !== "string") return event;

  const payload = event.startsWith("Event:")
    ? event.slice("Event:".length).trim()
    : event;

  try {
    return JSON.parse(payload);
  } catch {
    return { message: event };
  }
}

function getEventValue(event, key) {
  return event?.[key] ?? event?.[key[0].toUpperCase() + key.slice(1)];
}

function eventMatchesItem(event, type, item) {
  event = normalizeEvent(event);
  const message = getEventValue(event, "message");
  const text = typeof message === "string" ? message.toLowerCase() : "";
  const address = getEventValue(item, "address");

  if (type === "citizen") {
    const eventCitizenId = getEventValue(event, "citizenId");
    const citizenId = getEventValue(item, "id");

    if (eventCitizenId !== undefined && eventCitizenId !== null) {
      return String(eventCitizenId) === String(citizenId);
    }

    const citizenName = [
      getEventValue(item, "firstName"),
      getEventValue(item, "lastName"),
    ]
      .filter(Boolean)
      .join(" ")
      .toLowerCase();

    return citizenName.length > 0 && text.includes(citizenName);
  }

  return (
    getEventValue(event, "address") === address ||
    getEventValue(event, "homeAddress") === address ||
    getEventValue(event, "currentLocation") === address ||
    (typeof address === "string" && text.includes(address.toLowerCase()))
  );
}

function getLinkTarget(key, value, type, item) {
  if (key === "job" && value && typeof value === "object") {
    return { type: "workplace", item: value };
  }

  if (typeof value !== "string") return null;
  if (key === "homeAddress") return { type: "home", item: { address: value } };
  if (key === "currentLocation") {
    const workplaceAddress = item?.job?.workplace?.address;
    return {
      type: workplaceAddress === value ? "workplace" : "home",
      item: { address: value },
    };
  }
  if (key === "address" && (type === "home" || type === "workplace")) {
    return { type, item: { address: value } };
  }

  return null;
}

function getDetailValue(value, key, type, item, onNavigate, depth = 0) {
  if (value === null || value === undefined) return "-";
  if (typeof value === "boolean") return value ? "Yes" : "No";
  if (value instanceof Date) return value.toLocaleString();
  if (typeof value === "object") {
    if (depth > 1) return "Available";

    const nestedType = key === "job" ? "workplace" : type;
    return (
      <span className="entityNestedValue">
        {Object.entries(value)
      .filter(([key]) => key !== "owner")
          .map(([nestedKey, nestedValue]) => (
            <span key={nestedKey}>
              {getFieldLabel(nestedKey)}: {getDetailValue(
                nestedValue,
                nestedKey,
                nestedType,
                item,
                onNavigate,
                depth + 1,
              )}
            </span>
          ))}
      </span>
    );
  }

  if (key === "jobTitle") return getEnumLabel(value);
  if (key === "gender") return getGenderLabel(value);

  const linkTarget = getLinkTarget(key, value, type, item);
  if (linkTarget) {
    return (
      <button
        type="button"
        className="entityLink"
        onClick={() => onNavigate(linkTarget.type, linkTarget.item)}
      >
        {value}
      </button>
    );
  }

  return String(value);
}

function EntityDetails({ type, item, onNavigate }) {
  return (
    <dl className="entityDetails">
      {getEntityFields(type, item).map(([key, value]) => (
        <div key={key}>
          <dt>{getFieldLabel(key)}</dt>
          <dd>{getDetailValue(value, key, type, item, onNavigate)}</dd>
        </div>
      ))}
    </dl>
  );
}

export default function EntityPopup({ type, item, onClose }) {
  const [activeEntity, setActiveEntity] = useState({ type, item });
  const [details, setDetails] = useState(item);
  const [logs, setLogs] = useState([]);
  const activeType = activeEntity.type;
  const activeItem = activeEntity.item;
  const loader = detailLoaders[activeType];
  const title =
    activeType === "citizen"
      ? `${activeItem.firstName ?? "Citizen"} ${activeItem.lastName ?? ""}`.trim()
      : (activeItem.address ?? activeType);

  const navigateToEntity = (nextType, nextItem) => {
    setActiveEntity({ type: nextType, item: nextItem });
    setDetails(nextItem);
    setLogs([]);
  };

  useEffect(() => {
    let cancelled = false;

    const refresh = () =>
      loader(activeItem)
        .then((updatedItem) => {
          if (!cancelled) setDetails(updatedItem);
        })
        .catch((error) => {
          if (!cancelled) console.error(`Failed to load ${activeType}`, error);
        });

    refresh();
    const unsubscribe = subscribeToEvents((event) => {
      if (!eventMatchesItem(event, activeType, activeItem)) return;
      setLogs((currentLogs) => [...currentLogs, event]);
      refresh();
    });

    return () => {
      cancelled = true;
      unsubscribe();
    };
  }, [activeItem, activeType, loader]);

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
        <EntityDetails
          type={activeType}
          item={details}
          onNavigate={navigateToEntity}
        />
        <WSConsole logs={logs} />
      </section>
    </div>
  );
}
