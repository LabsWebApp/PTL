﻿// Абстрактная фабрика
// Когда использовать абстрактную фабрику?
// - Когда система не должна зависеть от способа создания и компоновки новых объектов
// - Когда создаваемые объекты должны использоваться вместе и являются взаимосвязанными

using AbstractFactory;

// «Мне нужен автомобиль!»
// «Отлично!» — говорим мы ему, — «вы обратились по адресу! Фабрика фабрик – это то, что вам нужно!»
CarsFactory factory;

// «Автомобили какой фирмы предпочитаете в данное время суток?», — спрашиваем мы.
// Допустим, покупатель хочет приобрести тойоту.
factory = new ToyotaFactory();

// «А какой тип кузова вы бы хотели?»
// Допустим – седан. «Прекрасный выбор!»
factory.CreateSedan();

Thread.Sleep(3000);

// «А в это время суток автомобили какой фирмы предпочитаете?», — спрашиваем мы.
// Теперь покупатель хочет приобрести форд.
// (если осталась тойота, то не нужно было пересоздавать factory!)
factory = new FordFactory();

// «А какой тип кузова вы бы хотели?»
// Теперь – купе. 
factory.CreateCoupe();


ReadLine();