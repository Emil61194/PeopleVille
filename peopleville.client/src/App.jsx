import "./App.css";
import { useEffect, useRef, useState } from "react";
import { MainGameButtons } from "./components/MainGameButtons.jsx";
import { HomesCard } from "./components/HomesCard.jsx";
import { CitizensCard } from "./components/CitizensCard.jsx";
import { WorkplacesCard } from "./components/WorkplacesCard.jsx";
import { LogCard } from "./components/LogCard.jsx";
import { KontrolpanelCard } from "./components/KontrolpanelCard.jsx";
import { connectToHub, resetConnection } from "./services/WSService.js";
import { getTime } from "./hooks/GetTime.js";
import TimeShower from "./components/TimeShower.jsx";

function App() {
  const [gameStarted, setGameStarted] = useState(false);
  const [logs, setLogs] = useState([]);
  const [homes, setHomes] = useState([]);
  const [citizens, setCitizens] = useState([]);
  const [workplaces, setWorkplaces] = useState([]);
  const currentGameTime = useRef(null);

  useEffect(() => {
    if (!gameStarted) return;

    let cancelled = false;

    const updateGameTime = () =>
      getTime()
        .then((currentTime) => {
          if (!cancelled) currentGameTime.current = currentTime;
        })
        .catch(() => {});

    updateGameTime();
    const interval = setInterval(updateGameTime, 1000);

    return () => {
      cancelled = true;
      clearInterval(interval);
    };
  }, [gameStarted]);

  useEffect(() => {
    if (!gameStarted) return;

    resetConnection();

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
