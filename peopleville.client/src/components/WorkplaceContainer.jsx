import { useEffect, useState } from "react";
import { GetWorkplaces } from "../hooks/GetWorkplaces";
import EntityPopup from "./EntityPopup.jsx";

function getWorkplaceItem(workplace, index, onSelect) {
  const title = workplace?.jobTitle ?? "Workplace";
  const address = workplace?.address;

  return (
    <button
      type="button"
      className="mapItem"
      key={address ?? index}
      onClick={() => onSelect(workplace)}
    >
      <p>{title}</p>
      {address && <small>{address}</small>}
      <small>Capacity: {workplace?.jobCapacity ?? "-"}</small>
      <small>Salary: {workplace?.salary ?? "-"}</small>
      <small>
        Hours: {workplace?.workStartTime ?? "-"}:00 -{" "}
        {workplace?.workEndTime ?? "-"}:00
      </small>
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
