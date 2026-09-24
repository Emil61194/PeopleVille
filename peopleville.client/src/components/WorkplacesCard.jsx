import { useState } from "react";
import { IconBuildingStore } from "../icons.js";
import { Modal } from "./Modal.jsx";

function padTime(h) {
  return String(h).padStart(2, "0") + ":00";
}

function WorkplaceModal({ wp, logs, onClose }) {
  const filteredLogs = logs.filter((l) => l.message.includes(wp.address));

  return (
    <Modal title={wp.address} onClose={onClose}>
      <p className="modal-section-label">Detaljer</p>
      <div className="modal-stats-grid">
        <div className="modal-stat">
          <span className="modal-stat__label">Åbningstid</span>
          <span className="modal-stat__value">
            {padTime(wp.workStartTime)} – {padTime(wp.workEndTime)}
          </span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Løn</span>
          <span className="modal-stat__value">{wp.salary} kr</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Jobkapacitet</span>
          <span className="modal-stat__value">{wp.jobCapacity}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Madpris</span>
          <span className="modal-stat__value badge-orange">{wp.foodPrice} kr</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Vandpris</span>
          <span className="modal-stat__value badge-blue">{wp.waterPrice} kr</span>
        </div>
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

export function WorkplacesCard({ workplaces, logs }) {
  const [selected, setSelected] = useState(null);

  return (
    <div className="card">
      <h2 className="card-title">Butikker</h2>
      <div className="card-list">
        {workplaces.length === 0 && (
          <div className="card-empty">Ingen butikker endnu</div>
        )}
        {workplaces.map((wp, i) => (
          <div
            key={i}
            className="card-row card-row--store card-row--clickable"
            onClick={() => setSelected(wp)}
          >
            <IconBuildingStore size={28} stroke={1.5} className="row-icon row-icon--store" />
            <div className="row-store-info">
              <span className="row-name">{wp.address}</span>
              <span className="row-address">
                {padTime(wp.workStartTime)} – {padTime(wp.workEndTime)}
              </span>
            </div>
            <div className="row-badges">
              <span className="price-chip price-chip--food">
                <span className="price-chip__label">Mad</span>
                <span className="price-chip__value">{wp.foodPrice} kr</span>
              </span>
              <span className="price-chip price-chip--water">
                <span className="price-chip__label">Vand</span>
                <span className="price-chip__value">{wp.waterPrice} kr</span>
              </span>
            </div>
          </div>
        ))}
      </div>

      {selected && (
        <WorkplaceModal
          wp={selected}
          logs={logs}
          onClose={() => setSelected(null)}
        />
      )}
    </div>
  );
}
