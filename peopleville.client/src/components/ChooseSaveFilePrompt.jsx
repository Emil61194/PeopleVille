import React, { useState, useEffect } from "react";
import { GetSaveFiles } from "../hooks/GetSaveFiles";

const ChooseSaveFilePrompt = ({ onClose }) => {
  const [saveFiles, setSaveFiles] = useState([]);

  useEffect(() => {
    GetSaveFiles()
      .then((files) =>
        setSaveFiles(
          files.toSorted(
            (firstFile, secondFile) =>
              new Date(secondFile.modifyDate) - new Date(firstFile.modifyDate),
          ),
        ),
      )
      .catch((error) => console.error(error));
  }, []);

  return (
    <div className="LoadSavesPromptOverlay" onClick={onClose}>
      <div
        className="LoadSavesPrompt"
        onClick={(event) => event.stopPropagation()}
      >
        <p>Choose a save file:</p>
        {saveFiles.map((file) => (
          <div className="SaveFile" key={file.filename}>
            <p>Name: {file.filename}</p>
            <p>Modified: {formatModifyDate(file.modifyDate)}</p>
            <p>Size: {formatFileSize(file.size)}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

function formatModifyDate(modifyDate) {
  const timestamp =
    typeof modifyDate === "number" && modifyDate < 1e12
      ? modifyDate * 1000
      : modifyDate;

  return new Date(timestamp).toLocaleString("en-GB", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
    hour12: false,
  });
}

function formatFileSize(size) {
  const sizeInBytes = Number(size);
  if (sizeInBytes < 1024) return `${sizeInBytes} bytes`;

  const units = ["KB", "MB", "GB", "TB"];
  let value = sizeInBytes;
  let unitIndex = -1;

  while (value >= 1024 && unitIndex < units.length - 1) {
    value /= 1024;
    unitIndex += 1;
  }

  return `${value.toFixed(value < 10 && unitIndex > 0 ? 1 : 0)} ${units[unitIndex]}`;
}

export default ChooseSaveFilePrompt;
