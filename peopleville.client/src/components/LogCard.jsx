export function LogCard({ logs }) {
  return (
    <div className="card">
      <h2 className="card-title">Log</h2>
      <div className="log-list">
        {logs.length === 0 && (
          <div className="card-empty">Ingen begivenheder endnu</div>
        )}
        {logs.map((entry, i) => (
          <div key={i} className="log-row">
            <span className="log-time">{entry.time}</span>
            <span className="log-msg">{entry.message}</span>
          </div>
        ))}
      </div>
    </div>
  );
}
