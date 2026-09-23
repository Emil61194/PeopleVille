import { useEffect, useState } from "react";
import { GetCitizens } from "../hooks/GetCitizens";

function getCitizenItem(citizen, index) {
  const name = [citizen?.firstName, citizen?.lastName]
    .filter(Boolean)
    .join(" ");

  return (
    <div key={citizen?.id ?? index}>
      <p>{name || citizen?.id || JSON.stringify(citizen)}</p>
      {citizen?.currentLocation && <small>{citizen.currentLocation}</small>}
    </div>
  );
}

const CitizenContainer = () => {
  const [citizens, setCitizens] = useState([]);

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
    <div id="menuDiv" name="wC">
      {citizens.map(getCitizenItem)}
    </div>
  );
};

export default CitizenContainer;
