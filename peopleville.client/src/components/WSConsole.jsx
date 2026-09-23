function getMessage(message) {
  if (message && typeof message === "object") {
    return typeof message.message === "string" && message.message.trim()
      ? message.message
      : null;
  }

  if (typeof message === "string") {
    const eventPayload = message.startsWith("Event:")
      ? message.slice("Event:".length).trim()
      : message;

    try {
      const parsed = JSON.parse(eventPayload);
      if (parsed && typeof parsed === "object") {
        return typeof parsed.message === "string" && parsed.message.trim()
          ? parsed.message
          : null;
      }
    } catch {
      // Display non-JSON messages as-is.
    }

    if (message.startsWith("Event:")) return null;
  }

  return message;
}

export function WSConsole({ logs }) {
  return (
    <div className="wsConsole">
      <strong>WS Console</strong>
      {logs.map((message, index) => {
        const renderedMessage = getMessage(message);
        return renderedMessage ? (
          <div key={index}>{renderedMessage}</div>
        ) : null;
      })}
    </div>
  );
}
