export function WSConsole({ logs }) {
  return (
    <div className="wsConsole">
      <strong>WS Console</strong>
      {logs.map((message, index) => (
        <div key={`${message}-${index}`}>{message}</div>
      ))}
    </div>
  );
}
