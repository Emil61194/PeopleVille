import { useEffect, useState } from "react";
import { GetHomes } from "../hooks/GetHomes";

function getHomeItem(home, index) {
  return (
    <div key={home?.id ?? index}>
      <p>{home?.name ?? home?.address ?? JSON.stringify(home)}</p>
    </div>
  );
}

const HomeContainer = () => {
  const [homes, setHomes] = useState([]);

  useEffect(() => {
    let cancelled = false;

    GetHomes()
      .then((loadedHomes) => {
        if (!cancelled) setHomes(loadedHomes ?? []);
      })
      .catch((error) => {
        if (!cancelled) console.error("Failed to load homes", error);
      });

    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <div id="menuDiv" name="hC">
      {homes.map(getHomeItem)}
    </div>
  );
};

export default HomeContainer;
