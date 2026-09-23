import { Fragment } from "react";
import HomeContainer from "./HomeContainer.jsx";
import CitizenContainer from "./CitizenContainer.jsx";
import WorkplaceContainer from "./WorkplaceContainer.jsx";

export function GameMap() {
  const mapItems = ["wC", "mC"];

  return (
    <div id="mapContainer">
      <HomeContainer />
      {mapItems.map((name) => (
        <Fragment key={name}>
          {name === "wC" && <CitizenContainer />}
          {name === "mC" && <WorkplaceContainer />}
        </Fragment>
      ))}
    </div>
  );
}
