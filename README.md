# ClientService
Данный сервис реализует взаимосвязь между оператором-логистом и водителем самосвалов. Данный комплекс является частью общей системы.

В данном проекте у задачи могут быть всего 4 состояния:
```
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
 

### Пример запуска системы.

Для того чтобы запустить проект локально нужно выполнить команду ```docker-compose up```.

### Обращение к сервису локально.

Сервис работает по адресу: http://localhost:5000

### Обращение к сервису в облаке.

Сервис работает по адресу: http://34.77.137.219

## Как подключиться к Hub проекта.

## LogistHub

### GetAllTasks

Метод получения всех задач созданых операторами:

``` connection.send("GetAllTasks"); ```

Данные прийдут только оператору который вызвал метод.

``` connection.on("GetAllTasks", (tasks: string) => { ... }); ```

Пример данных которые могут прийти оператору:

```
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

``` connection.send("CreateTask", "createTaskCommand"); ```

Пример json для параметра [createTaskCommand] :

``` 
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

``` connection.on("CreateTask", (task: string) => { ... }); ```

Пример данных которые могут прийти пользователям:

```
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

``` connection.send("UpdateTask", "updateStatusCommand"); ```

Пример json для параметра [updateStatusCommand] :

``` 
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

``` connection.on("UpdateTask", (task: string) => { ... }); ```

Пример данных которые могут прийти пользователям:

```
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

``` connection.on("DumperStatus", (dumperStatus: string) => { ... }); ```

Пример данных которые могут прийти операторам:

```
{
  "Id": 1,
  "State": 1
}
```
Где [Id] - идентификатор задачи для которой отсылается прогноз по статусу автомобиля,

[State] - прогноз в котором указанно состояние автомобиля.

## DriverHub

### GetDriverTasks

Метод получения всех задач созданых операторами и назначеных на водителя который вызвал метод:

``` connection.send("GetDriverTasks"); ```

Данные прийдут только водителю который вызвал метод.

``` connection.on("GetDriverTasks", (tasks: string) => { ... }); ```

Пример данных которые могут прийти оператору:

```
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

``` connection.send("UpdateStatus", "updateStatusCommand"); ```


Пример json для параметра [updateStatusCommand] :

``` 
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

``` connection.on("UpdateStatus", (tasks: string) => { ... }); ```

Пример данных которые могут прийти оператору:

```
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
