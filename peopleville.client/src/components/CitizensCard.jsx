import { useState } from "react";
import { IconUser, IconHome, IconBriefcase, IconSchool } from "@tabler/icons-react";
import { Modal } from "./Modal.jsx";

function age(birth) {
  if (!birth) return "?";
  const b = new Date(birth);
  const now = new Date();
  let y = now.getFullYear() - b.getFullYear();
  if (now < new Date(now.getFullYear(), b.getMonth(), b.getDate())) y--;
  return y;
}

function CitizenModal({ citizen, logs, onClose }) {
  const filteredLogs = logs.filter(
    (l) =>
      l.message.includes(citizen.firstName) &&
      l.message.includes(citizen.lastName),
  );

  return (
    <Modal title={`${citizen.firstName} ${citizen.lastName}`} onClose={onClose}>
      <p className="modal-section-label">Info</p>
      <div className="modal-stats-grid">
        <div className="modal-stat">
          <span className="modal-stat__label">Køn</span>
          <span className="modal-stat__value">{citizen.gender === 0 ? "Mand" : "Kvinde"}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Alder</span>
          <span className="modal-stat__value">{age(citizen.birth)} år</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Beskæftigelse</span>
          <span className="modal-stat__value">{citizen.job ? "Ansat" : "Ledig"}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Skole</span>
          <span className="modal-stat__value">{citizen.school ? citizen.school.address : "Ingen"}</span>
        </div>
      </div>

      <p className="modal-section-label">Lokationer</p>
      <div className="modal-list">
        <div className="modal-row">
          <IconHome size={16} stroke={1.5} className="row-icon" />
          <span className="modal-row__sub">Hjem</span>
          <span className="modal-row__name">{citizen.homeAddress}</span>
        </div>
        <div className="modal-row">
          <IconUser size={16} stroke={1.5} className="row-icon" />
          <span className="modal-row__sub">Nu</span>
          <span className="modal-row__name">{citizen.currentLocation}</span>
        </div>
        {citizen.job && (
          <div className="modal-row">
            <IconBriefcase size={16} stroke={1.5} className="row-icon" />
            <span className="modal-row__sub">Arbejde</span>
            <span className="modal-row__name">{citizen.job.workplace?.address ?? "?"}</span>
          </div>
        )}
      </div>

      <p className="modal-section-label">Log ({filteredLogs.length})</p>
      <div className="modal-log">
        {filteredLogs.length === 0 ? (
          <p className="modal-empty">Ingen begivenheder endnu</p>
        ) : (
          filteredLogs.map((l, i) => (
            <div key={i} className="log-row">
              <span className="log-time">{l.time}</span>
              <span className="log-msg">{l.message}</span>
            </div>
          ))
        )}
      </div>
    </Modal>
  );
}

export function CitizensCard({ citizens, logs }) {
  const [selected, setSelected] = useState(null);

  return (
    <div className="card">
      <h2 className="card-title">Personer</h2>
      <div className="card-list">
        {citizens.length === 0 && <div className="card-empty">Ingen borgere endnu</div>}
        {citizens.map((citizen, i) => (
          <div
            key={i}
            className="card-row card-row--clickable"
            onClick={() => setSelected(citizen)}
          >
            <IconUser size={20} stroke={1.5} className="row-icon" />
            <span className="row-name">
              {citizen.firstName} {citizen.lastName}
            </span>
            <span className="row-location">{citizen.currentLocation}</span>
          </div>
        ))}
      </div>

      {selected && (
        <CitizenModal
          citizen={selected}
          logs={logs}
          onClose={() => setSelected(null)}
        />
      )}
    </div>
  );
}
