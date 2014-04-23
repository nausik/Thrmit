Thrmit
============================================================

Система интеллектуального анализа сбора и анализа температурных данных
----------------------------------
Во время, когда автоматизированный контроль всего давно перестал быть мечтой стоит задача улучшения средств. Это, например, минимизация цены и максимизация надежности.

Большинство известных решений нацелены на профессионалов и большие компании, у которых есть достаточно средств и людей для тяжелых, огромных средств.

Главной задачей при разработке системы Thrmit является простота. Установить и включить систему контроля сможет даже школьник-шестиклассник Вася, который только закончил решать домашнее задание по математике.

Структура системы
----------------------------------
Система состоит из двух компонентов: "железо" (различные датчики) и программная часть. Программная часть, в свою очередь, делится на две части.

1. Десктопное приложение, которое отвечает за работу с устройствами - оптимизирует их работу (например, строит наиболее оптимальный путь обхода устройств), меряет температуру, напряжение и т.д.

2. Веб-сайт, который находится на удаленном сервере. У каждого пользователя есть свой аккаунт, который привязан к десктопному приложению. Как только оно загружает данные в БД, сайт обрабатывает полученную информацию, отображает все в благоприятном виде и производит анализ собранных данных.

Диаграмма десктопного приложения
----------------------------------
![class diagram](http://i.imgur.com/HKd6faK.jpg "Class diagramm")

Авторы
----------------------------------
[Артём Барбинягра, НТУУ "КПИ", ФПМ, КМ-11](https://github.com/nausik)

[Александр Гончар, НТУУ "КПИ", ФПМ, КМ-12](https://github.com/rachnog)

[Владислав Борисенко, НТУУ "КПИ", ФПМ, КМ-12](https://github.com/flyingpirate)