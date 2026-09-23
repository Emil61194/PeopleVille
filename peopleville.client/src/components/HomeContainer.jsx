import { useEffect, useState } from "react";
import { GetHomes } from "../hooks/GetHomes";
import EntityPopup from "./EntityPopup.jsx";

function getHomeItem(home, index, onSelect) {
  return (
    <button
      type="button"
      className="mapItem"
      key={home?.id ?? home?.address ?? index}
      onClick={() => onSelect(home)}
    >
      <p>{home?.name ?? home?.address ?? JSON.stringify(home)}</p>
    </button>
  );
}

const HomeContainer = () => {
  const [homes, setHomes] = useState([]);
  const [selectedHome, setSelectedHome] = useState(null);

  useEffect(() => {
    let cancelled = false;

    GetHomes()
      .then((loadedHomes) => {
        if (!cancelled) setHomes(loadedHomes ?? []);
      })
      .catch((error) => {
        if (!cancelled) console.error("Failed to load homes", error);
      });

    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <>
      <div id="menuDiv" name="hC">
        {homes.map((home, index) =>
          getHomeItem(home, index, setSelectedHome),
        )}
      </div>
      {selectedHome && (
        <EntityPopup
          type="home"
          item={selectedHome}
          onClose={() => setSelectedHome(null)}
        />
      )}
    </>
  );
};

export default HomeContainer;
