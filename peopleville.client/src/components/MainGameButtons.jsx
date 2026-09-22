import React, { useState } from "react";
import ChooseSaveFilePrompt from "./ChooseSaveFilePrompt.jsx";
import { GetNewGame } from "../hooks/GetNewGame.js";

export const MainGameButtons = () => {
  const [filePrompt, setFilePrompt] = useState(false);

  return (
    <div style={{ marginTop: "100px" }}>
      {filePrompt && (
        <ChooseSaveFilePrompt onClose={() => setFilePrompt(false)} />
      )}
      <button
        text="Load Save"
        className="LoadSaveButton"
        onClick={() => setFilePrompt(true)}
      >
        Load Save
      </button>
      <button className="LoadSaveButton" onClick={handleNewGameClick}>
        New Game
      </button>
    </div>
  );
};

function handleNewGameClick() {
  let success = GetNewGame();
  if (success) {
  }
}
