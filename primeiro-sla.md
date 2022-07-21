# Medições SLA 20/07/2022

### _Cauê Melo, Carolina Fantini & Thaís Brasil_

# 1. Serviço de notas

### 1.1. Operações

- Criação de nota
- Listagem de todas as notas
- Atualização de nota pelo id
- Exclusão de nota pelo id

### 1.2. Arquivos envolvidos

[Arquivo principal](https://github.com/cauesmelo/capybara-api/blob/master/Program.cs)

[Controlador](https://github.com/cauesmelo/capybara-api/blob/master/Controllers/NoteController.cs)

[Serviço](https://github.com/cauesmelo/capybara-api/blob/master/Services/NoteService.cs)

### 1.3. Código fonte de medição

```js
import http from "k6/http";
import { check, group, sleep } from "k6";

function randomString(length, charset = "") {
  if (!charset) charset = "abcdefghijklmnopqrstuvwxyz";
  let res = "";
  while (length--) res += charset[(Math.random() * charset.length) | 0];
  return res;
}

const BASE_URL = "http://localhost:5029/api";

export let options = {
  stages: [
    { target: 200, duration: "10s" },
    { target: 500, duration: "20s" },
    { target: 100, duration: "10s" },
  ],
};

const pl = (obj) => {
  return JSON.stringify(obj);
};

export default () => {
  const requestConfigWithTag = (tag) => ({
    headers: {
      "content-Type": "application/json",
    },
    tags: Object.assign(
      {},
      {
        name: "notes",
      },
      tag
    ),
  });

  group("Operações notas", () => {
    let URL = `${BASE_URL}/note/`;

    group("Busca notas", () => {
      const res = http.get(URL, requestConfigWithTag({ name: "Read" }));

      if (check(res, { "Notas recuperadas com sucesso": (r) => r.status === 200 })) {
      } else {
        console.log(`Erro ao criar nota ${res.status} ${res.body}`);
        return;
      }
    });

    group("Criacao de notas", () => {
      const payload = {
        content: randomString(10),
      };

      const res = http.post(URL, pl(payload), requestConfigWithTag({ name: "Create" }));

      if (check(res, { "Nota criada com sucesso": (r) => r.status === 200 })) {
        URL = `${URL}${res.json("id")}/`;
      } else {
        console.log(`Erro ao criar nota ${res.status} ${res.body}`);
        return;
      }
    });

    group("Atualizar nota", () => {
      const payload = { content: "Nota atualizada" };
      const res = http.put(URL, pl(payload), requestConfigWithTag({ name: "Update" }));
      const isSuccessfulUpdate = check(res, {
        "Nota atualizada": () => res.status === 200,
        "Conteúdo correto": () => res.json("content") === "Nota atualizada",
      });

      if (!isSuccessfulUpdate) {
        console.log(`Erro ao atualizar nota ${res.status} ${res.body}`);
        return;
      }
    });

    const delRes = http.del(URL, null, requestConfigWithTag({ name: "Delete" }));

    const isSuccessfulDelete = check(null, {
      "Nota deletada com sucesso": () => delRes.status === 200,
    });

    if (!isSuccessfulDelete) {
      console.log(`Erro ao deletar nota`);
      return;
    }
  });

  sleep(1);
};
```

### 1.4. Configurações de máquinas envolvidas

Foi utilizado um servidor local ASP.NET 6.
Para o banco de dados MySQL foi utilizado um container do Docker com sua imagem na tag latest.
Para o banco de dados Redis foi utilizado com container do Docker com sua imagem na tag latest.
Tudo isso rodando sob Windows 11 Enterprise com 16gb de RAM e Intel Core i7 de oitava geração.

### 1.5. Latência, vazão e concorrência

A latência média das requisições realizadas foi de 1,84s, com um valor máximo de 16,49s.
A vazão média das requisições feitas foi de em média 149,116 requisições por segundo. Ao calcular a vazão por minuto, temos uma média de aproximadamente 9 mil requisições por minuto.
Foi configurado um pool de usuários virtuais variáveis, começando com 200 usuários virtuais por 10 segundos, escalando até 500 e após 20segundos, escalando para baixo até 100 usuários virtuais e mantendo por 10 segundos.

### 1.6. Dados brutos

```sh

          /\      |‾‾| /‾‾/   /‾‾/
     /\  /  \     |  |/  /   /  /
    /  \/    \    |     (   /   ‾‾\
   /          \   |  |\  \ |  (‾)  |
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: teste-notas.js
     output: -

  scenarios: (100.00%) 1 scenario, 500 max VUs, 1m10s max duration (incl. graceful stop):
           * default: Up to 500 looping VUs for 40s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


running (0m56.5s), 000/500 VUs, 1054 complete and 0 interrupted iterations
default ✓ [======================================] 000/500 VUs  40s

     █ Operações notas

       ✓ Nota deletada com sucesso

       █ Busca notas

         ✓ Notas recuperadas com sucesso

       █ Criacao de notas

         ✓ Nota criada com sucesso

       █ Atualizar nota

         ✓ Nota atualizada
         ✓ Conteúdo correto

     checks.........................: 100.00% ✓ 5270       ✗ 0
     data_received..................: 33 MB   576 kB/s
     data_sent......................: 1.3 MB  23 kB/s
     group_duration.................: avg=6.89s    min=32.32ms med=4.42s  max=29.33s  p(90)=18.49s   p(95)=21.31s
     http_req_blocked...............: avg=100.06ms min=0s      med=0s     max=5.79s   p(90)=1.09ms   p(95)=217.98ms
     http_req_connecting............: avg=213.39µs min=0s      med=0s     max=54.15ms p(90)=568.95µs p(95)=1.29ms
     http_req_duration..............: avg=1.84s    min=0s      med=1.5s   max=16.49s  p(90)=3.37s    p(95)=4s
       { expected_response:true }...: avg=1.84s    min=0s      med=1.5s   max=16.49s  p(90)=3.37s    p(95)=4s
     http_req_failed................: 0.00%   ✓ 0          ✗ 8432
     http_req_receiving.............: avg=706.23µs min=0s      med=0s     max=1.14s   p(90)=1.28ms   p(95)=2.26ms
     http_req_sending...............: avg=159.83µs min=0s      med=0s     max=15.66ms p(90)=556.79µs p(95)=998.6µs
     http_req_tls_handshaking.......: avg=99.8ms   min=0s      med=0s     max=5.79s   p(90)=0s       p(95)=215.33ms
     http_req_waiting...............: avg=1.83s    min=0s      med=1.5s   max=16.49s  p(90)=3.37s    p(95)=4s
     http_reqs......................: 8432    149.116741/s
     iteration_duration.............: avg=16.53s   min=1.27s   med=17.66s max=30.33s  p(90)=27.26s   p(95)=28.67s
     iterations.....................: 1054    18.639593/s
     vus............................: 44      min=16       max=500
     vus_max........................: 500     min=500      max=500

```

### 1.7. Potenciais gargalos

Ao análisar os resultados, imaginamos que ao editar ou excluir um recurso no banco de dados, o cache é invalidado por completo, talvez executando uma lógica para invalidar parcialmente um recurso no cache, a performance possa melhorar.

# 2. Serviço de listas de tarefas

### 2.1. Operações

- Criação de lista de tarefa
- Listagem de todas as listas da tarefas
- Alternagem de tarefa feita/não-feita
- Exclusão lista de tarefa

### 2.2. Arquivos envolvidos

[Arquivo principal](https://github.com/cauesmelo/capybara-api/blob/master/Program.cs)

[Controlador Lista de tarefas](https://github.com/cauesmelo/capybara-api/blob/master/Controllers/TaskListController.cs)

[Controlador Tarefa](https://github.com/cauesmelo/capybara-api/blob/master/Controllers/TaskUnityController.cs)

[Serviço Lista de tarefas](https://github.com/cauesmelo/capybara-api/blob/master/Services/TasklistService.cs)

[Serviço tarefa](https://github.com/cauesmelo/capybara-api/blob/master/Services/TaskUnityService.cs)

### 2.3. Código fonte de medição

```js
import http from "k6/http";
import { check, group, sleep } from "k6";

function randomString(length, charset = "") {
  if (!charset) charset = "abcdefghijklmnopqrstuvwxyz";
  let res = "";
  while (length--) res += charset[(Math.random() * charset.length) | 0];
  return res;
}

const BASE_URL = "http://localhost:5029/api";

export let options = {
  stages: [
    { target: 200, duration: "20s" },
    { target: 500, duration: "30s" },
    { target: 0, duration: "10s" },
  ],
};

const pl = (obj) => {
  return JSON.stringify(obj);
};

export default () => {
  const requestConfigWithTag = (tag) => ({
    headers: {
      "content-Type": "application/json",
    },
    tags: Object.assign(
      {},
      {
        name: "tarefas",
      },
      tag
    ),
  });

  group("Operações lista de tarefas", () => {
    let URL = `${BASE_URL}/tasklist/`;
    let task;

    group("Busca lista de tarefas", () => {
      const res = http.get(URL, requestConfigWithTag({ name: "Read" }));

      if (check(res, { "Lista de tarefas recuperadas com sucesso": (r) => r.status === 200 })) {
      } else {
        console.log(`Erro ao criar lista de tarefas ${res.status} ${res.body}`);
        return;
      }
    });

    group("Criacao de lista de tarefa", () => {
      const payload = {
        title: randomString(10),
        tasks: ["correr", "brincar", "amar"],
      };

      const res = http.post(URL, pl(payload), requestConfigWithTag({ name: "Create" }));
      const tasks = res.json("tasks");
      if (res) task = tasks[0];

      if (check(res, { "Lista de tarefa criada com sucesso": (r) => r.status === 200 })) {
        URL = `${URL}${res.json("id")}/`;
      } else {
        console.log(`Erro ao criar lista de tarefa ${res.status} ${res.body}`);
        return;
      }
    });

    group("Atualizar tarefa", () => {
      task.isComplete = !task.isComplete;
      const payload = task;
      const res = http.patch(`${BASE_URL}/taskunity/${task.id}`, pl(payload), requestConfigWithTag({ name: "Update" }));
      const isSuccessfulUpdate = check(res, {
        "Tarefa atualizada": () => res.status === 200,
      });

      if (!isSuccessfulUpdate) {
        console.log(`Erro ao atualizar lista de tarefa ${res.status} ${res.body}`);
        return;
      }
    });

    const delRes = http.del(URL, null, requestConfigWithTag({ name: "Delete" }));

    const isSuccessfulDelete = check(null, {
      "Lista de tarefa deletada com sucesso": () => delRes.status === 200,
    });

    if (!isSuccessfulDelete) {
      console.log(`Erro ao deletar lista de tarefa`);
      return;
    }
  });

  sleep(1);
};
```

### 2.4. Configurações de máquinas envolvidas

Foi utilizado um servidor local ASP.NET 6.
Para o banco de dados MySQL foi utilizado um container do Docker com sua imagem na tag latest.
Para o banco de dados Redis foi utilizado com container do Docker com sua imagem na tag latest.
Tudo isso rodando sob Windows 11 Enterprise com 16gb de RAM e Intel Core i7 de oitava geração.

### 2.5. Latência, vazão e concorrência

A latência média das requisições realizadas foi de 1,08s, com um valor máximo de 8,05s.
A vazão média das requisições feitas foi de em média 231,474 requisições por segundo. Ao calcularmos a vazão por minuto, temos uma média de aproximademente 19mil requisiçoes por minuto.
Foi configurado um pool de usuários virtuais variáveis, começando com 200 usuários virtuais por 20 segundos, escalando até 500 e após 30 segundos, escalando para baixo até 100 usuários virtuais em 10 segundos.

### 2.6. Dados brutos

```sh
          /\      |‾‾| /‾‾/   /‾‾/
     /\  /  \     |  |/  /   /  /
    /  \/    \    |     (   /   ‾‾\
   /          \   |  |\  \ |  (‾)  |
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: teste-lista.js
     output: -

  scenarios: (100.00%) 1 scenario, 500 max VUs, 1m30s max duration (incl. graceful stop):
           * default: Up to 500 looping VUs for 1m0s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


running (1m06.0s), 000/500 VUs, 1911 complete and 0 interrupted iterations
default ✓ [======================================] 000/500 VUs  1m0s

     █ Operações lista de tarefas

       ✓ Lista de tarefa deletada com sucesso

       █ Busca lista de tarefas

         ✓ Lista de tarefas recuperadas com sucesso

       █ Criacao de lista de tarefa

         ✓ Lista de tarefa criada com sucesso

       █ Atualizar tarefa

         ✓ Tarefa atualizada

     checks.........................: 100.00% ✓ 7644       ✗ 0
     data_received..................: 307 MB  4.6 MB/s
     data_sent......................: 3.1 MB  47 kB/s
     group_duration.................: avg=3.93s    min=15.88ms med=2.57s    max=18.47s  p(90)=11.03s   p(95)=14.56s
     http_req_blocked...............: avg=16.62ms  min=0s      med=0s       max=2.37s   p(90)=0s       p(95)=1.22ms
     http_req_connecting............: avg=226.37µs min=0s      med=0s       max=145ms   p(90)=0s       p(95)=988.13µs
     http_req_duration..............: avg=1.08s    min=0s      med=762.07ms max=8.05s   p(90)=2.38s    p(95)=3.27s
       { expected_response:true }...: avg=1.08s    min=0s      med=762.07ms max=8.05s   p(90)=2.38s    p(95)=3.27s
     http_req_failed................: 0.00%   ✓ 0          ✗ 15288
     http_req_receiving.............: avg=156.35ms min=0s      med=0s       max=5.51s   p(90)=140.23ms p(95)=1.45s
     http_req_sending...............: avg=175.39µs min=0s      med=0s       max=29.79ms p(90)=784.68µs p(95)=1ms
     http_req_tls_handshaking.......: avg=16.36ms  min=0s      med=0s       max=2.37s   p(90)=0s       p(95)=0s
     http_req_waiting...............: avg=925.87ms min=0s      med=736.69ms max=3.94s   p(90)=2.02s    p(95)=2.38s
     http_reqs......................: 15288   231.474884/s
     iteration_duration.............: avg=9.79s    min=1.09s   med=10.09s   max=19.47s  p(90)=16.93s   p(95)=17.63s
     iterations.....................: 1911    28.93436/s
     vus............................: 28      min=10       max=500
     vus_max........................: 500     min=500      max=500

```

### 2.7. Potenciais gargalos

Semelhante ao primeiro SLA, ao análisar os resultados, imaginamos que ao editar ou excluir um recurso no banco de dados, o cache é invalidado por completo, talvez executando uma lógica para invalidar parcialmente um recurso no cache, a performance possa melhorar.
