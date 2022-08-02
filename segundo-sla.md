
# Medições SLA 01/09/2022

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

[Serviço (ALTERADO)](https://github.com/cauesmelo/capybara-api/blob/master/Services/NoteService.cs)

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

A latência média das requisições realizadas foi de 1,13s, com um valor máximo de 3,84s.
A vazão média das requisições feitas foi de em média 240,63 requisições por segundo. Ao calcular a vazão por minuto, temos uma média de aproximadamente 14 mil requisições por minuto.
Foi configurado um pool de usuários virtuais variáveis, começando com 200 usuários virtuais por 10 segundos, escalando até 500 e após 20segundos, escalando para baixo até 100 usuários virtuais e mantendo por 10 segundos.

### 1.6. Dados brutos

```sh

          /\      |‾‾| /‾‾/   /‾‾/
     /\  /  \     |  |/  /   /  /
    /  \/    \    |     (   /   ‾‾\
   /          \   |  |\  \ |  (‾)  |
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: nota.js
     output: -

  scenarios: (100.00%) 1 scenario, 500 max VUs, 1m10s max duration (incl. graceful stop):
           * default: Up to 500 looping VUs for 40s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


running (0m47.1s), 000/500 VUs, 1417 complete and 0 interrupted iterations
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

     checks.........................: 100.00% ✓ 7085      ✗ 0
     data_received..................: 35 MB   749 kB/s
     data_sent......................: 1.6 MB  35 kB/s
     group_duration.................: avg=4.05s    min=7.68ms med=2.87s    max=17.93s   p(90)=11.8s   p(95)=13.84s
     http_req_blocked...............: avg=25.47ms  min=0s     med=0s       max=2.19s    p(90)=0s      p(95)=5.73ms
     http_req_connecting............: avg=349.79µs min=0s     med=0s       max=142.99ms p(90)=0s      p(95)=1.52ms
     http_req_duration..............: avg=1.13s    min=0s     med=952.24ms max=3.84s    p(90)=2.43s   p(95)=2.88s
       { expected_response:true }...: avg=1.13s    min=0s     med=952.24ms max=3.84s    p(90)=2.43s   p(95)=2.88s
     http_req_failed................: 0.00%   ✓ 0         ✗ 11336
     http_req_receiving.............: avg=1.26ms   min=0s     med=0s       max=713.53ms p(90)=3.01ms  p(95)=5.01ms
     http_req_sending...............: avg=234.64µs min=0s     med=0s       max=71.01ms  p(90)=995.2µs p(95)=1ms
     http_req_tls_handshaking.......: avg=25.02ms  min=0s     med=0s       max=2.19s    p(90)=0s      p(95)=0s
     http_req_waiting...............: avg=1.13s    min=0s     med=951.57ms max=3.84s    p(90)=2.43s   p(95)=2.87s
     http_reqs......................: 11336   240.63204/s
     iteration_duration.............: avg=10.28s   min=1.18s  med=11.14s   max=18.95s   p(90)=15.81s  p(95)=16.58s
     iterations.....................: 1417    30.079005/s
     vus............................: 49      min=18      max=500
     vus_max........................: 500     min=500     max=500
```

### 1.7. Comparação com medição anterior
Ao analisarmos os resultados, podemos ver um ganho de peformance substancial na capacidade de vazão e tempo de resposta das requisições.

### 1.8. Alterações feitas
Ao invés de invalidarmos o cache ao realizarmos modificações, optamos por uma estratégia de atualizar o cache e o banco de dados simultaneamente nas operações do sistema, mantendo assim um cache sempre válido e disponível para ser resgatado. Para isso, modificamos o arquivo de serviço das notas.


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

[Serviço Lista de tarefas (ALTERADO)](https://github.com/cauesmelo/capybara-api/blob/master/Services/TasklistService.cs)

[Serviço tarefa (ALTERADO)](https://github.com/cauesmelo/capybara-api/blob/master/Services/TaskUnityService.cs)

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

A latência média das requisições realizadas foi de 0,760s, com um valor máximo de 6,74s.
A vazão média das requisições feitas foi de em média 310,61 requisições por segundo. Ao calcularmos a vazão por minuto, temos uma média de aproximademente 19mil requisiçoes por minuto.
Foi configurado um pool de usuários virtuais variáveis, começando com 200 usuários virtuais por 20 segundos, escalando até 500 e após 30 segundos, escalando para baixo até 100 usuários virtuais em 10 segundos.

### 2.6. Dados brutos

```sh

          /\      |‾‾| /‾‾/   /‾‾/
     /\  /  \     |  |/  /   /  /
    /  \/    \    |     (   /   ‾‾\
   /          \   |  |\  \ |  (‾)  |
  / __________ \  |__| \__\ \_____/ .io

  execution: local
     script: lista.js
     output: -

  scenarios: (100.00%) 1 scenario, 500 max VUs, 1m30s max duration (incl. graceful stop):
           * default: Up to 500 looping VUs for 1m0s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)


running (1m04.1s), 000/500 VUs, 2488 complete and 0 interrupted iterations
default ✓ [======================================] 000/500 VUs  1m0s

     █ Operações lista de tarefas

       ✓ Lista de tarefa deletada com sucesso

       █ Busca lista de tarefas

         ✓ Lista de tarefas recuperadas com sucesso

       █ Criacao de lista de tarefa

         ✓ Lista de tarefa criada com sucesso

       █ Atualizar tarefa

         ✓ Tarefa atualizada

     checks.........................: 100.00% ✓ 9952       ✗ 0
     data_received..................: 188 MB  2.9 MB/s
     data_sent......................: 3.7 MB  58 kB/s
     group_duration.................: avg=2.72s    min=1.66ms med=1.56s    max=17.32s  p(90)=7.4s     p(95)=11.3s
     http_req_blocked...............: avg=9.63ms   min=0s     med=0s       max=2.66s   p(90)=0s       p(95)=984.69µs
     http_req_connecting............: avg=145.66µs min=0s     med=0s       max=43.98ms p(90)=0s       p(95)=0s
     http_req_duration..............: avg=760.88ms min=0s     med=447.42ms max=6.74s   p(90)=1.86s    p(95)=2.53s
       { expected_response:true }...: avg=760.88ms min=0s     med=447.42ms max=6.74s   p(90)=1.86s    p(95)=2.53s
     http_req_failed................: 0.00%   ✓ 0          ✗ 19904
     http_req_receiving.............: avg=51.44ms  min=0s     med=0s       max=3.73s   p(90)=3.01ms   p(95)=12.99ms
     http_req_sending...............: avg=152.6µs  min=0s     med=0s       max=45.99ms p(90)=534.51µs p(95)=1ms
     http_req_tls_handshaking.......: avg=9.46ms   min=0s     med=0s       max=2.64s   p(90)=0s       p(95)=0s
     http_req_waiting...............: avg=709.29ms min=0s     med=444.46ms max=4.43s   p(90)=1.79s    p(95)=2.24s
     http_reqs......................: 19904   310.610445/s
     iteration_duration.............: avg=7.17s    min=1.14s  med=6.11s    max=18.32s  p(90)=14.5s    p(95)=15.71s
     iterations.....................: 2488    38.826306/s
     vus............................: 25      min=9        max=500
     vus_max........................: 500     min=500      max=500

```


### 2.7. Comparação com medição anterior
Ao analisarmos os resultados, podemos ver um ganho de peformance razoável na capacidade de vazão e tempo de resposta das requisições, esperávamos que por ter mais relacionamentos envolvidos, o ganho de performance nesse serviço fosse superior ao de notas, mas aconteceu justamente o contrário. O ganho de performance foi inferior ao serviço de notas.

### 2.8. Alterações feitas
Ao invés de invalidarmos o cache ao realizarmos modificações, optamos por uma estratégia de atualizar o cache e o banco de dados simultaneamente nas operações do sistema, mantendo assim um cache sempre válido e disponível para ser resgatado. Para isso, modificamos o arquivo de serviço das listas de tarefas.

## Gráficos e dados antigos
[Os gráficos de comparação entre os dois SLAs podem ser vistos clicando aqui](https://github.com/cauesmelo/capybara-api/blob/master/graficos.pdf)

[Os dados completos do SLA anterior pode ser visto clicando aqui](https://github.com/cauesmelo/capybara-api/edit/master/primeiro-sla.md)
