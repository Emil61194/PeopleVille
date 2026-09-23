import { useEffect, useState } from "react";
import { GetCitizens } from "../hooks/GetCitizens";
import EntityPopup from "./EntityPopup.jsx";

function getCitizenItem(citizen, index, onSelect) {
  const name = [citizen?.firstName, citizen?.lastName]
    .filter(Boolean)
    .join(" ");

  return (
    <button
      type="button"
      className="mapItem"
      key={citizen?.id ?? index}
      onClick={() => onSelect(citizen)}
    >
      <p>{name || citizen?.id || JSON.stringify(citizen)}</p>
      {citizen?.currentLocation && <small>{citizen.currentLocation}</small>}
    </button>
  );
}

const CitizenContainer = () => {
  const [citizens, setCitizens] = useState([]);
  const [selectedCitizen, setSelectedCitizen] = useState(null);

  useEffect(() => {
    let cancelled = false;

    GetCitizens()
      .then((loadedCitizens) => {
        if (!cancelled) setCitizens(loadedCitizens ?? []);
      })
      .catch((error) => {
        if (!cancelled) console.error("Failed to load citizens", error);
      });

    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <>
      <div id="menuDiv" name="wC">
        {citizens.map((citizen, index) =>
          getCitizenItem(citizen, index, setSelectedCitizen),
        )}
      </div>
      {selectedCitizen && (
        <EntityPopup
          type="citizen"
          item={selectedCitizen}
          onClose={() => setSelectedCitizen(null)}
        />
      )}
    </>
  );
};

export default CitizenContainer;
