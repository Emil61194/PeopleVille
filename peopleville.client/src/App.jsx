import "./App.css";
import { useEffect, useState } from "react";
import { MainGameButtons } from "./components/MainGameButtons.jsx";
import { WSConsole } from "./components/WSConsole.jsx";
import { connectToHub } from "./services/WSService.js";

function App() {
  const [gameStarted, setGameStarted] = useState(false);
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    if (!gameStarted) return;

    connectToHub((message) =>
      setLogs((currentLogs) => [...currentLogs, message]),
    );
  }, [gameStarted]);

  return (
    <div>
      <h1 id="tableLabel">PeopleVille</h1>
      {!gameStarted && (
        <MainGameButtons onGameStarted={() => setGameStarted(true)} />
      )}
      {gameStarted && <WSConsole logs={logs} />}
    </div>
  );
}

export default App;
