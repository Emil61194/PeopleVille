import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [forecasts, setForecasts] = useState();

    useEffect(() => {
        populateWeatherData();
    }, []);

    function getMapItem(itemName) {
        return <div><p>{itemName}</p></div>;
    }

    const mapItems = [
        "hC",
        "wC",
        "mC"
    ];

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <div id="mapContainer">
            {mapItems.map((name) => (
                <div id="menuDiv" name={name} key={name}>
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                    {getMapItem(name)}
                </div>
            ))}
        </div>;

    return (
        <div>
            <h1 id="tableLabel">PeopleVille</h1>
            <p id="tableDescription">Map</p>
            {contents}
        </div>
    );
    
    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
}

export default App;