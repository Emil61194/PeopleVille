import "./App.css";
import { useEffect, useState } from "react";
import { MainGameButtons } from "./components/MainGameButtons.jsx";
import { HomesCard } from "./components/HomesCard.jsx";
import { CitizensCard } from "./components/CitizensCard.jsx";
import { WorkplacesCard } from "./components/WorkplacesCard.jsx";
import { LogCard } from "./components/LogCard.jsx";
import { KontrolpanelCard } from "./components/KontrolpanelCard.jsx";
import { connectToHub } from "./services/WSService.js";
import TimeShower from "./components/TimeShower.jsx";

function App() {
  const [gameStarted, setGameStarted] = useState(false);
  const [connectionReady, setConnectionReady] = useState(false);
  const [logs, setLogs] = useState([]);
  const [homes, setHomes] = useState([]);
  const [citizens, setCitizens] = useState([]);
  const [workplaces, setWorkplaces] = useState([]);

  useEffect(() => {
    if (!gameStarted) return;

    let cancelled = false;

    connectToHub(
      (message, worldTime) =>
        setLogs((prev) => [
            { time: worldTime ? worldTime : new Date(), message },
          ...prev,
        ]),
      ({ homes: h, citizens: c, workplaces: w }) => {
        setHomes(h ?? []);
        setCitizens(c ?? []);
        setWorkplaces(w ?? []);
      },
    );
  }, [gameStarted]);

  return (
    <div className="app">
      <h1 className="app-title">Peopleville</h1>
      {!gameStarted && (
        <MainGameButtons onGameStarted={() => setGameStarted(true)} />
      )}

          {gameStarted && (
          <div className="dashboard">
          <TimeShower />
          <div className="dashboard-top">
            <HomesCard homes={homes} citizens={citizens} logs={logs} />
            <CitizensCard citizens={citizens} logs={logs} />
            <WorkplacesCard workplaces={workplaces} logs={logs} />
          </div>
          <div className="dashboard-bottom">
            <KontrolpanelCard />
            <LogCard logs={logs} />
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
