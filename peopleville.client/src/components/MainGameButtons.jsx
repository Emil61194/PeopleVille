import React, { useState } from "react";
import ChooseSaveFilePrompt from "./ChooseSaveFilePrompt.jsx";
import { GetNewGame } from "../hooks/GetNewGame.js";

export const MainGameButtons = ({ onGameStarted }) => {
  const [filePrompt, setFilePrompt] = useState(false);
  const [creating, setCreating] = useState(false);

  return (
    <div style={{ marginTop: "100px" }}>
      {filePrompt && (
        <ChooseSaveFilePrompt onClose={() => setFilePrompt(false)} />
      )}
      <button
        text="Load Save"
        className="LoadSaveButton"
        disabled={creating}
        onClick={() => setFilePrompt(true)}
      >
        Load Save
      </button>
      <button
        className="LoadSaveButton"
        disabled={creating}
        onClick={() => handleNewGameClick(onGameStarted, setCreating)}
      >
        {creating ? "Creating..." : "New Game"}
      </button>
    </div>
  );
};

async function handleNewGameClick(onGameStarted, setCreating) {
  setCreating(true);
  try {
    const success = await GetNewGame();
    if (success) {
      onGameStarted();
      console.log("New game created");
    }
  } catch (error) {
    console.error(error);
  } finally {
    setCreating(false);
  }
}
