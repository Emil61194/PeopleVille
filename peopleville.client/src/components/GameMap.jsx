import HomeContainer from "./HomeContainer.jsx";
import CitizenContainer from "./CitizenContainer.jsx";

function getMapItem(itemName, index) {
  return (
    <div key={`${itemName}-${index}`}>
      <p>{itemName}</p>
    </div>
  );
}

export function GameMap() {
  const mapItems = ["wC", "mC"];

  return (
    <div id="mapContainer">
      <HomeContainer />
      {mapItems.map((name) =>
        name === "wC" ? (
          <CitizenContainer key={name} />
        ) : (
          <div id="menuDiv" name={name} key={name}>
            {Array.from({ length: 8 }, (_, index) => getMapItem(name, index))}
          </div>
        ),
      )}
    </div>
  );
}
