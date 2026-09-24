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
            ? date.toLocaleString("da-DK", {
                year: "numeric",
                month: "2-digit",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
            })
            : "Loading...";

    return (
        <div className="timeShower">
            <span className="timeShowerLabel">Time: </span>
            <time dateTime={time ?? undefined}>{formattedTime}</time>
        </div>
    );
};

export default TimeShower;
