import { useState } from "react";
import { IconHome, IconUser } from "@tabler/icons-react";
import { Modal } from "./Modal.jsx";

function HomeModal({ home, residents, logs, onClose }) {
  const filteredLogs = logs.filter((l) => l.message.includes(home.address));

  return (
    <Modal title={home.address} onClose={onClose}>
      <p className="modal-section-label">Statistik</p>
      <div className="modal-stats-grid">
        <div className="modal-stat">
          <span className="modal-stat__label">Mad</span>
          <span className="modal-stat__value badge-orange">{home.foodInventory ?? 0}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Vand</span>
          <span className="modal-stat__value badge-blue">{home.waterInventory ?? 0}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Kapacitet</span>
          <span className="modal-stat__value badge-green">{home.citizenCapacity ?? 0}</span>
        </div>
        <div className="modal-stat">
          <span className="modal-stat__label">Saldo</span>
          <span className="modal-stat__value">
            {home.bankAccount?.balance !== undefined
              ? Math.round(home.bankAccount.balance)
              : 0}{" "}
            kr
          </span>
        </div>
        {home.rent !== undefined && (
          <div className="modal-stat">
            <span className="modal-stat__label">Husleje</span>
            <span className="modal-stat__value">{home.rent} kr</span>
          </div>
        )}
        {home.floors !== undefined && (
          <div className="modal-stat">
            <span className="modal-stat__label">Etager</span>
            <span className="modal-stat__value">{home.floors}</span>
          </div>
        )}
      </div>

      <p className="modal-section-label">Beboere ({residents.length})</p>
      <div className="modal-list">
        {residents.length === 0 ? (
          <p className="modal-empty">Ingen beboere</p>
        ) : (
          residents.map((c, i) => (
            <div key={i} className="modal-row">
              <IconUser size={16} stroke={1.5} className="row-icon" />
              <span className="modal-row__name">{c.firstName} {c.lastName}</span>
              <span className="modal-row__sub">{c.currentLocation}</span>
            </div>
          ))
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

export function HomesCard({ homes, citizens, logs }) {
  const [selected, setSelected] = useState(null);

  const residents = selected
    ? citizens.filter((c) => c.homeAddress === selected.address)
    : [];

  return (
    <div className="card">
      <h2 className="card-title">Hjem</h2>
      <div className="card-list">
        {homes.length === 0 && <div className="card-empty">Ingen hjem endnu</div>}
        {homes.map((home, i) => (
          <div key={i} className="card-row card-row--clickable" onClick={() => setSelected(home)}>
            <IconHome size={20} stroke={1.5} className="row-icon" />
            <span className="row-name">{home.address}</span>
            <div className="row-badges">
              <span className="badge badge-orange" title="Mad">{home.foodInventory ?? 0}</span>
              <span className="badge badge-blue" title="Vand">{home.waterInventory ?? 0}</span>
              <span className="badge badge-green" title="Kapacitet">{home.citizenCapacity ?? 0}</span>
              <span className="badge badge-dark" title="Saldo">
                {home.bankAccount?.balance !== undefined ? Math.round(home.bankAccount.balance) : 0}
              </span>
            </div>
          </div>
        ))}
      </div>

      {selected && (
        <HomeModal
          home={selected}
          residents={residents}
          logs={logs}
          onClose={() => setSelected(null)}
        />
      )}
    </div>
  );
}
