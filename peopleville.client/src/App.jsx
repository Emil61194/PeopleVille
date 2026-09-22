import { useEffect, useState } from "react";
import "./App.css";
import { MainGameButtons } from "./components/MainGameButtons.jsx";

function App() {
  function getMapItem(itemName) {
    return (
      <div>
        <p>{itemName}</p>
      </div>
    );
  }

  const mapItems = ["hC", "wC", "mC"];

  const contents = (
      <div id="mapContainer">
        {mapItems.map((name) => (
          <div id="menuDiv" name={name} key={name}>
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
            {getMapItem(name)}
          </div>
        ))}
      </div>
    );

  return (
    <div>
      <h1 id="tableLabel">PeopleVille</h1>
      <p id="tableDescription">Map</p>
      {contents}
      <MainGameButtons />
    </div>
  );
}

export default App;
