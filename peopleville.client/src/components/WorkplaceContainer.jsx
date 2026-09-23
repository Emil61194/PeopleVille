import { useEffect, useState } from "react";
import { GetWorkplaces } from "../hooks/GetWorkplaces";
import EntityPopup from "./EntityPopup.jsx";

function getWorkplaceItem(workplace, index, onSelect) {
  const address = workplace?.address;

  return (
    <button
      type="button"
      className="mapItem"
      key={address ?? index}
      onClick={() => onSelect(workplace)}
    >
      <p>{workplace?.name ?? "Shopping center"}</p>
      {address && <small>{address}</small>}
    </button>
  );
}

const WorkplaceContainer = () => {
  const [workplaces, setWorkplaces] = useState([]);
  const [selectedWorkplace, setSelectedWorkplace] = useState(null);

  useEffect(() => {
    let cancelled = false;

    GetWorkplaces()
      .then((loadedWorkplaces) => {
        if (!cancelled) setWorkplaces(loadedWorkplaces ?? []);
      })
      .catch((error) => {
        if (!cancelled) console.error("Failed to load workplaces", error);
      });

    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <>
      <div id="menuDiv" name="mC">
        {workplaces.map((workplace, index) =>
          getWorkplaceItem(workplace, index, setSelectedWorkplace),
        )}
      </div>
      {selectedWorkplace && (
        <EntityPopup
          type="workplace"
          item={selectedWorkplace}
          onClose={() => setSelectedWorkplace(null)}
        />
      )}
    </>
  );
};

export default WorkplaceContainer;
