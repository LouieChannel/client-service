# ClientService
Данный сервис реализует взаимосвязь между оператором-логистом и водителем самосвалов. Этот сервис является частью программного комплекса, который отвечает за прогнозирование состояние самосвалов в данный момент.

В данном проекте у задачи могут быть всего 4 состояния:
```c#

/// <summary>
/// Информация о статусах задачи.
/// </summary>
public enum StatusType : short
{
  /// <summary>
  /// Задача создана.
  /// </summary>
  Created = 0,

  /// <summary>
  /// Задача в процесе выполнения.
  /// </summary>
  InProgress = 1,

  /// <summary>
  /// Задача выполнена.
  /// </summary>
  Done = 2,

  /// <summary>
  /// Задача отменена.
  /// </summary>
  Cancelled = 3,
 }
 
 ```
 У самосвала может быть 5 состояний:

1 - холостой ход

2 - перевоз груза

3 - погрузка груза

4 - выгрузка груза

5 - двигатель выключен

На данный момент в системе будет рассмотренно только 5 условий задания:

1) Перевоз груза
2) Заменить ковш
3) Надеть ковш
4) Снять ковш
5) Перемешать ковшом материалы

### Пример запуска системы.

Для того чтобы запустить проект локально нужно выполнить команду:
```docker 

docker-compose up

```

После того как завершили работу с програмным комлпексом нужно выполнить команду: 
```docker

docker-compose down

```

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219

## Как подключиться к Hub проекта.

## LogistHub

### Пример

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000/logist

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219/logist

### GetAllDrivers

Метод получения всех водителей из базы:

```typescript

connection.send("GetAllDrivers");

```

Данные прийдут только оператору который вызвал метод.

```typescript 

connection.on("GetAllDrivers", (tasks: string) => { ... });

```

Пример данных которые могут прийти оператору:

```json

[
  {
    "Id": 2,
    "Role": "Driver",
    "FullName": "DriverTest"
  }
]

```

### GetAllTasks

Метод получения всех задач созданых операторами:

```typescript

connection.send("GetAllTasks");

```

Данные прийдут только оператору который вызвал метод.

```typescript 

connection.on("GetAllTasks", (tasks: string) => { ... });

```

Пример данных которые могут прийти оператору:

```json

[
  {
    "Id": 1,
    "Driver": {
      "Id": 2,
      "FullName": "DriverTest"
    },
    "Logist": {
      "Id": 1,
      "FullName": "Мирко А.А."
    },
    "Description": "test",
    "StartLongitude": 50.1354,
    "StartLatitude": 30.4324,
    "EndLongitude": 1.4342,
    "EndLatitude": 43.1234,
    "Status": 1,
    "Entity": "dasfas",
    "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
  }
]

```

### CreateTask

Метод создания задачи.

```typescript 

connection.send("CreateTask", "createTaskCommand"); 

```

Пример json для параметра [createTaskCommand] :

```json

{
  "Driver": {
    "Id": 2,
    "FullName": "DriverTest"
  },
  "Description": "test",
  "StartLongitude": 50.1354,
  "StartLatitude": 30.4324,
  "EndLongitude": 1.4342,
  "EndLatitude": 43.1234,
  "Status": 1,
  "Entity": "dasfas"
}

```

Оповещение прийдёт всем операторам и водителю на котрого была назначена задача:

```typescript 

connection.on("CreateTask", (task: string) => { ... });

```

Пример данных которые могут прийти пользователям:

```json

{
  "Id": 1,
  "Driver": {
    "Id": 2,
    "FullName": "DriverTest"
  },
  "Logist": {
    "Id": 1,
    "FullName": "Мирко А.А."
  },
  "Description": "test",
  "StartLongitude": 50.1354,
  "StartLatitude": 30.4324,
  "EndLongitude": 1.4342,
  "EndLatitude": 43.1234,
  "Status": 1,
  "Entity": "dasfas",
  "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
}

```

### UpdateTask

Метод изменение задачи. Изменять можно задачи только тогда, когда она в статусе "Задача создана (0)"

```typescript 

connection.send("UpdateTask", "updateStatusCommand");

```

Пример json для параметра [updateStatusCommand] :

```json

{
  "Id": 1,
  "Logist": {
    "Id": 1,
    "FullName": "DriverTest"
  },
  "Driver": {
    "Id": 2,
    "FullName": "Мирко А.А."
  },
  "Description": "Here is normal text",
  "StartLongitude": 50.1354,
  "StartLatitude": 30.4324,
  "EndLongitude": 1.4342,
  "EndLatitude": 43.1234,
  "Status": 2,
  "Entity": "Normal text",
  "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
}

```


Оповещение прийдёт всем операторам и водителю на котрого была назначена задача:

```typescript

connection.on("UpdateTask", (task: string) => { ... });

```

Пример данных которые могут прийти пользователям:

```json

{
  "Id": 1,
  "Logist": {
    "Id": 1,
    "FullName": "DriverTest"
  },
  "Driver": {
    "Id": 2,
    "FullName": "Мирко А.А."
  },
  "Description": "Here is normal text",
  "StartLongitude": 50.1354,
  "StartLatitude": 30.4324,
  "EndLongitude": 1.4342,
  "EndLatitude": 43.1234,
  "Status": 2,
  "Entity": "Normal text",
  "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
}

```

### DumperStatus

Данные приходят только операторам.

```typescript

connection.on("DumperStatus", (dumperStatus: string) => { ... });

```

Пример данных которые могут прийти операторам:

```json

{
  "Id": 1,
  "State": 1
}

```
Где [Id] - идентификатор задачи для которой отсылается прогноз по статусу автомобиля,

[State] - прогноз в котором указанно состояние самосвала.

## DriverHub

### Пример

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000/driver

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219/driver

### GetDriverTasks

Метод получения всех задач созданых операторами и назначеных на водителя который вызвал метод:

```typescript

connection.send("GetDriverTasks");

```

Данные прийдут только водителю который вызвал метод.

```typescript

connection.on("GetDriverTasks", (tasks: string) => { ... });

```

Пример данных которые могут прийти оператору:

```json

[
  {
    "Id": 1,
    "Driver": {
      "Id": 2,
      "FullName": "DriverTest"
    },
    "Logist": {
      "Id": 1,
      "FullName": "Мирко А.А."
    },
    "Description": "test",
    "StartLongitude": 50.1354,
    "StartLatitude": 30.4324,
    "EndLongitude": 1.4342,
    "EndLatitude": 43.1234,
    "Status": 1,
    "Entity": "dasfas",
    "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
  }
]

```

### UpdateStatus

Метод изменения статуса у задачи:

```typescript 

connection.send("UpdateStatus", "updateStatusCommand");

```


Пример json для параметра [updateStatusCommand] :

```json 

{
  "Id": 1,
  "Logist": {
    "Id": 1,
    "FullName": "DriverTest"
  },
  "Driver": {
    "Id": 2,
    "FullName": "Мирко А.А."
  },
  "Description": "Here is normal text",
  "StartLongitude": 50.1354,
  "StartLatitude": 30.4324,
  "EndLongitude": 1.4342,
  "EndLatitude": 43.1234,
  "Status": 2,
  "Entity": "Normal text",
  "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
}

```

Данные прийдут водителю который вызвал метод и всем логистам-операторам.

```typescript

connection.on("UpdateStatus", (tasks: string) => { ... });

```

Пример данных которые могут прийти оператору:

```json

[
  {
    "Id": 1,
    "Driver": {
      "Id": 2,
      "FullName": "DriverTest"
    },
    "Logist": {
      "Id": 1,
      "FullName": "Мирко А.А."
    },
    "Description": "test",
    "StartLongitude": 50.1354,
    "StartLatitude": 30.4324,
    "EndLongitude": 1.4342,
    "EndLatitude": 43.1234,
    "Status": 1,
    "Entity": "dasfas",
    "CreatedAt": "2020-04-12T19:07:26.939418+03:00"
  }
]

```

## AuthController

### Login

Для авторизации пользователя нужно обратиться к сервису по ссылке:

```c#

[HttpPost()]

```

Пример body запроса:

```json
{
 "login": "test@gmail.com",
 "password": "654321"
}

```

В результате будет получен JWT токен и дополнительные поля для авторизации.

### Пример

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000/auth

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219/auth

### Результат

Вернётся обьект: 
```js

{
  config: Object { url: "http://localhost:5000/auth", method: "post", data: "{\"login\":\"test@gmail.com\",\"password\":\"654321\"}", … },
  data: Object { access_token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRHJpdmVyVGVzdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRyaXZlciIsIklkIjoiMiIsIlJvbGUiOiJEcml2ZXIiLCJuYmYiOjE1ODY5OTgwMDEsImV4cCI6MTU5MDU5ODAwMSwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZW IifQ.fFaVmm8Qee68JisX8jpVItamh5pId9qD9Xy5ebx8eoA", username: "DriverTest", role: "Driver" },
  headers: Object { "content-type": "application/json; charset=utf-8" },
  request: XMLHttpRequest { readyState: 4, timeout: 0, withCredentials: false, … },
  status: 200,
  statusText: "OK"
}

```

где data:

```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRHJpdmVyVGVzdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRyaXZlciIsIklkIjoiMiIsIlJvbGUiOiJEcml2ZXIiLCJuYmYiOjE1ODY5OTgwMDEsImV4cCI6MTU5MDU5ODAwMSwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.fFaVmm8Qee68JisX8jpVItamh5pId9qD9Xy5ebx8eoA",
  "role": "Driver",
  "username": "DriverTest"
}
```

## AccountController

### Create

Для создания нового пользователя нужно обратиться к сервису по ссылке:

```c#

[HttpPost()]

```

Пример body запроса:

```json

{
	"Login":"test2@test.ru",
	"Password":"qwerty",
	"FullName":"Test test",
	"Role":1
}

```

В результате будет получен JWT токен и дополнительные поля для авторизации.

### Пример

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000/account

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219/account

### Результат

Вернётся обьект: 
```js

{
  config: Object { url: "http://localhost:5000/auth", method: "post", data: "{\"login\":\"test@gmail.com\",\"password\":\"654321\"}", … },
  data: Object { access_token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRHJpdmVyVGVzdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRyaXZlciIsIklkIjoiMiIsIlJvbGUiOiJEcml2ZXIiLCJuYmYiOjE1ODY5OTgwMDEsImV4cCI6MTU5MDU5ODAwMSwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZW IifQ.fFaVmm8Qee68JisX8jpVItamh5pId9qD9Xy5ebx8eoA", username: "DriverTest", role: "Driver" },
  headers: Object { "content-type": "application/json; charset=utf-8" },
  request: XMLHttpRequest { readyState: 4, timeout: 0, withCredentials: false, … },
  status: 200,
  statusText: "OK"
}

```

где data:

```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRHJpdmVyVGVzdCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRyaXZlciIsIklkIjoiMiIsIlJvbGUiOiJEcml2ZXIiLCJuYmYiOjE1ODY5OTgwMDEsImV4cCI6MTU5MDU5ODAwMSwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.fFaVmm8Qee68JisX8jpVItamh5pId9qD9Xy5ebx8eoA",
  "role": "Driver",
  "username": "DriverTest"
}
```
