export function LogCard({ logs }) {
    const sortedLogs = [...logs].sort((a, b) => new Date(b.time) - new Date(a.time)).slice(0, 300);

  return (
    <div className="card">
      <h2 className="card-title">Log</h2>
      <div className="log-list">
      {sortedLogs.length === 0 && (
          <div className="card-empty">Ingen begivenheder endnu</div>
        )}
      {sortedLogs.map((entry, i) => (
          <div key={i} className="log-row">
              <span className="log-time">
                  {entry.time.toLocaleString("da-DK", {
                      year: "numeric",
                      month: "2-digit",
                      day: "2-digit",
                      hour: "2-digit",
                      minute: "2-digit",
                  })}
              </span>
            <span className="log-msg">{entry.message}</span>
          </div>
        ))}
      </div>
    </div>
  );
}
