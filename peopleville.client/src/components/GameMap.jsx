function getMapItem(itemName) {
  return (
    <div>
      <p>{itemName}</p>
    </div>
  );
}

export function GameMap() {
  const mapItems = ["hC", "wC", "mC"];

  return (
    <div id="mapContainer">
      {mapItems.map((name) => (
        <div id="menuDiv" name={name} key={name}>
          {Array.from({ length: 8 }, () => getMapItem(name))}
        </div>
      ))}
    </div>
  );
}
