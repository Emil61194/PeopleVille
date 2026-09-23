import { useEffect, useState } from "react";
import { getTime } from "../hooks/GetTime";

const TimeShower = () => {
  const [time, setTime] = useState(null);

  useEffect(() => {
    let cancelled = false;

    const updateTime = () => {
      getTime()
        .then((currentTime) => {
          if (!cancelled) setTime(currentTime);
        })
        .catch((error) => {
          if (!cancelled) console.error("Failed to load time", error);
        });
    };

    updateTime();
    const interval = setInterval(updateTime, 1 * 1000);

    return () => {
      cancelled = true;
      clearInterval(interval);
    };
  }, []);

  const date = time ? new Date(time) : null;
  const formattedTime =
    date && !Number.isNaN(date.valueOf())
      ? new Intl.DateTimeFormat(undefined, {
          weekday: "short",
          month: "short",
          day: "numeric",
          year: "numeric",
          hour: "numeric",
          minute: "2-digit",
          second: "2-digit",
        }).format(date)
      : "Loading...";

  return (
    <div className="timeShower">
      <span className="timeShowerLabel">PeopleVille time</span>
      <time dateTime={time ?? undefined}>{formattedTime}</time>
    </div>
  );
};

export default TimeShower;
