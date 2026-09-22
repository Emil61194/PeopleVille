import "./App.css";
import { useEffect, useState } from "react";
import { MainGameButtons } from "./components/MainGameButtons.jsx";
import { WSConsole } from "./components/WSConsole.jsx";
import { connectToHub } from "./services/WSService.js";

function App() {
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    connectToHub((message) =>
      setLogs((currentLogs) => [...currentLogs, message]),
    );
  }, []);

  return (
    <div>
      <h1 id="tableLabel">PeopleVille</h1>
      <MainGameButtons />
      <WSConsole logs={logs} />
    </div>
  );
}

export default App;
