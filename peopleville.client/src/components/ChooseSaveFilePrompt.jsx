import React, { useState, useEffect } from "react";

const ChooseSaveFilePrompt = ({ onClose }) => {
  const [saveFiles, setSaveFiles] = useState([]);

  useEffect(() => {
    getAllFiles().then(setSaveFiles);
  }, []);

  return (
    <div className="LoadSavesPromptOverlay" onClick={onClose}>
      <div
        className="LoadSavesPrompt"
        onClick={(event) => event.stopPropagation()}
      >
        <p>Choose a save file:</p>
        {saveFiles.map((file) => (
          <div className="SaveFile" key={file.modifyDate}>
            <p>Name: {file.name}</p>
            <p>Modified: {formatModifyDate(file.modifyDate)}</p>
            <p>Size: {formatFileSize(file.size)}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

async function getAllFiles() {
  if (true) return getMockData();
  // Order by modifyDate
  return await GetSaveFiles().then((saveFiles) => {
    // Process the save files and return them
    return saveFiles.sort((a, b) => b.modifyDate - a.modifyDate);
  });
}

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
  if (size < 1024) return `${size} bytes`;

  const units = ["KB", "MB", "GB", "TB"];
  let value = size;
  let unitIndex = -1;

  while (value >= 1024 && unitIndex < units.length - 1) {
    value /= 1024;
    unitIndex += 1;
  }

  return `${value.toFixed(value < 10 && unitIndex > 0 ? 1 : 0)} ${units[unitIndex]}`;
}

function getMockData() {
  // 30 of these please
  return [
    { name: "SaveFile1", modifyDate: 1620000000, size: 1024 },
    { name: "SaveFile2", modifyDate: 1625000000, size: 2048 },
    { name: "SaveFile3", modifyDate: 1615000000, size: 4096 },
    { name: "SaveFile4", modifyDate: 1630000000, size: 8192 },
    { name: "SaveFile5", modifyDate: 1622000000, size: 1024 },
    { name: "SaveFile6", modifyDate: 1623000000, size: 2048 },
    { name: "SaveFile7", modifyDate: 1624000000, size: 4096 },
    { name: "SaveFile8", modifyDate: 1626000000, size: 8192 },
    { name: "SaveFile9", modifyDate: 1627000000, size: 1024 },
    { name: "SaveFile10", modifyDate: 1628000000, size: 2048 },
    { name: "SaveFile11", modifyDate: 1629000000, size: 4096 },
    { name: "SaveFile12", modifyDate: 1631000000, size: 8192 },
    { name: "SaveFile13", modifyDate: 1632000000, size: 1024 },
    { name: "SaveFile14", modifyDate: 1633000000, size: 2048 },
    { name: "SaveFile15", modifyDate: 1634000000, size: 4096 },
    { name: "SaveFile16", modifyDate: 1635000000, size: 8192 },
    { name: "SaveFile17", modifyDate: 1636000000, size: 1024 },
    { name: "SaveFile18", modifyDate: 1637000000, size: 2048 },
    { name: "SaveFile19", modifyDate: 1638000000, size: 4096 },
    { name: "SaveFile20", modifyDate: 1639000000, size: 8192 },
    { name: "SaveFile21", modifyDate: 1640000000, size: 1024 },
    { name: "SaveFile22", modifyDate: 1641000000, size: 2048 },
    { name: "SaveFile23", modifyDate: 1642000000, size: 4096 },
    { name: "SaveFile24", modifyDate: 1643000000, size: 8192 },
    { name: "SaveFile25", modifyDate: 1644000000, size: 1024 },
    { name: "SaveFile26", modifyDate: 1645000000, size: 2048 },
    { name: "SaveFile27", modifyDate: 1646000000, size: 4096 },
    { name: "SaveFile28", modifyDate: 1647000000, size: 8192 },
    { name: "SaveFile29", modifyDate: 1648000000, size: 1024 },
    { name: "SaveFile30", modifyDate: 1649000000, size: 2048 },
  ];
}

export default ChooseSaveFilePrompt;
