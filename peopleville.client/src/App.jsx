import "./App.css";
import { useEffect, useState } from "react";
import { MainGameButtons } from "./components/MainGameButtons.jsx";
import { GameMap } from "./components/GameMap.jsx";
import { WSConsole } from "./components/WSConsole.jsx";
import { connectToHub } from "./services/WSService.js";
import TimeShower from "./components/TimeShower.jsx";

function App() {
  const [gameStarted, setGameStarted] = useState(false);
  const [connectionReady, setConnectionReady] = useState(false);
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    if (!gameStarted) return;

    let cancelled = false;

    connectToHub((message) =>
      setLogs((currentLogs) => [...currentLogs, message]),
    ).then((connected) => {
      if (!cancelled) setConnectionReady(connected);
    });

    return () => {
      cancelled = true;
    };
  }, [gameStarted]);

  return (
    <div>
      <h1 id="tableLabel">PeopleVille</h1>
      {!gameStarted && (
        <MainGameButtons onGameStarted={() => setGameStarted(true)} />
      )}
      {gameStarted && connectionReady && (
        <>
          <TimeShower />
          <GameMap />
          <WSConsole logs={logs} />
        </>
      )}
    </div>
  );
}

export default App;
