import React, { useState } from "react";
import ChooseSaveFilePrompt from "./ChooseSaveFilePrompt.jsx";

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
    </div>
  );
};
