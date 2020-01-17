import React, { useState } from "react";
import "./App.css";
let socket;

function enviarComando(comando) {
  if (!!socket) socket.send(comando);
}

function App() {
  const [ip, setIp] = useState("192.168.1.");
  const [porta, setPorta] = useState("15345");
  const [ativo, setAtivo] = useState(false);

  function conectar() {
    const server = "ws://" + ip + ":" + porta;
    socket = new WebSocket(server);
    socket.onopen = function(response) {
      console.log("conectado no server: " + server);
      setAtivo(true);
    };

    socket.onclose = function() {
      setAtivo(false);
    };
  }

  function desconectar() {
    setAtivo(false);
  }

  return (
    <div className="App">
      <div className="input-block">
        <label htmlFor="ip">IP: </label>
        <input
          type="text"
          name="ip"
          id="ip"
          value={ip}
          disabled={ativo}
          onChange={e => setIp(e.target.value)}
        ></input>
      </div>
      <div className="input-block">
        <label htmlFor="porta">Porta: </label>
        <input
          type="text"
          name="porta"
          id="porta"
          disabled={ativo}
          value={porta}
          onChange={e => setPorta(e.target.value)}
        ></input>
      </div>

      <div className="input-block">
        <button disabled={ativo} onClick={conectar}>
          Conectar
        </button>
        <button disabled={!ativo} onClick={desconectar}>
          Desconectar
        </button>
      </div>

      <div className="actions">
        <button
          onClickCapture={() => {
            enviarComando("hide");
          }}
        >
          Barra de tarefa
        </button>
        <button
          onClickCapture={() => {
            enviarComando("swap");
          }}
        >
          Inverter botão mouse
        </button>
      </div>

      <div className="actions">
        <button
          onClickCapture={() => {
            enviarComando("movecursor");
          }}
        >
          Mover cursor
        </button>
        <button
          onClickCapture={() => {
            enviarComando("opencddriver");
          }}
        >
          Abre CD
        </button>
        <button
          onClickCapture={() => {
            enviarComando("lock");
          }}
        >
          Bloqueia tela
        </button>
      </div>

      <div className="actions">
        <button
          onClickCapture={() => {
            enviarComando("wallpaper");
          }}
        >
          Wallpaper
        </button>
        <button
          onClickCapture={() => {
            enviarComando("grita");
          }}
        >
          Grita !!
        </button>
      </div>
    </div>
  );
}

export default App;
