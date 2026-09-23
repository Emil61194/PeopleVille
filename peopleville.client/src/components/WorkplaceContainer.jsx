import { useEffect, useState } from "react";
import { GetWorkplaces } from "../hooks/GetWorkplaces";

function getWorkplaceItem(workplace, index) {
  const title = workplace?.jobTitle ?? "Workplace";
  const address = workplace?.address;

  return (
    <div key={address ?? index}>
      <p>{title}</p>
      {address && <small>{address}</small>}
      <small>Capacity: {workplace?.jobCapacity ?? "-"}</small>
      <small>Salary: {workplace?.salary ?? "-"}</small>
      <small>
        Hours: {workplace?.workStartTime ?? "-"}:00 - {workplace?.workEndTime ?? "-"}:00
      </small>
    </div>
  );
}

const WorkplaceContainer = () => {
  const [workplaces, setWorkplaces] = useState([]);

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
    <div id="menuDiv" name="mC">
      {workplaces.map(getWorkplaceItem)}
    </div>
  );
};

export default WorkplaceContainer;
