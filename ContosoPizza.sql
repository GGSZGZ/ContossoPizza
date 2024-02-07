-- Crear la Base de datos de ContosoPizza
CREATE DATABASE ContosoPizzaDB;

USE ContosoPizzaDB;

CREATE TABLE Ingredients(
	Name NVARCHAR(80) PRIMARY KEY,
	Calories DECIMAL(7, 2),
	Origin NVARCHAR(30),
	IsGlutenFree bit
);


CREATE TABLE Pizza(
	Id INT PRIMARY KEY,
	Name NVARCHAR(30),
	Veggie BIT
);

CREATE TABLE PizzaIngredients
(
    Pizza_Id INT,
    Ingredient_Name NVARCHAR(80),
    FOREIGN KEY (Pizza_Id) REFERENCES Pizza(Id),
    FOREIGN KEY (Ingredient_Name) REFERENCES Ingredients(Name),
    PRIMARY KEY (Pizza_Id, Ingredient_Name)
);

CREATE TABLE Orders
(
    Id INT PRIMARY KEY,
    Price DECIMAL(7, 2),
    OrderDate DATETIME,
    EstimatedDelivery DATETIME
);

CREATE TABLE OrderPizzaDetails
(
    Order_Id INT,
    Pizza_Id INT,
    PRIMARY KEY (Order_Id, Pizza_Id),
    FOREIGN KEY (Order_Id) REFERENCES Orders(Id),
    FOREIGN KEY (Pizza_Id) REFERENCES Pizza(Id)
);

CREATE TABLE Users
(
    Id INT PRIMARY KEY,
    Name NVARCHAR(50),
    Surname NVARCHAR(50),
    Direction NVARCHAR(100),
    Order_Id INT,
    FOREIGN KEY (Order_Id) REFERENCES Orders(Id)
);


INSERT INTO Ingredients(Name, Calories, Origin, isGlutenFree)
  VALUES ('Tomato Sauce', 29.00, 'Spain', 0),
			('Basil', 22.00,'Italy', 0),
			('Cheese', 402.00, 'France', 1),
			('Flour', 358.00, 'Italy', 1);
			
SELECT * FROM Ingredients;

INSERT INTO Pizza(Id, Name, Veggie)
  VALUES (1, 'Barbacue', 0),
  			(2, 'Veggie', 1),
  			(3, 'Barbacue', 0);
  			
SELECT * FROM Pizza;

INSERT INTO PizzaIngredients(Pizza_Id, Ingredient_Name)
  VALUES (2, 'Tomato Sauce'),
  			(2, 'Basil'),
  			(2, 'Cheese'),
  			(2, 'Flour');
  			
SELECT * FROM PizzaIngredients;

INSERT INTO Orders (Id, Price, OrderDate, EstimatedDelivery)
VALUES
    (1, 25.99, '2024-01-31 12:00:00', '2024-02-01 18:00:00'),
    (2, 18.50, '2024-01-31 15:30:00', '2024-02-02 17:30:00');

SELECT * FROM Orders;

INSERT INTO OrderPizzaDetails (Order_Id, Pizza_Id)
VALUES
    (1, 1),
    (1, 2),
    (2, 3);
    
SELECT * FROM OrderPizzaDetails;



INSERT INTO Users (Id, Name, Surname, Direction, Order_Id)
VALUES
    (1, 'Jeffrey', 'Epstein', 'C/Alfonso IV Meridio, 14, 4ºG', 1),
    (2, 'Stephen', 'Hawkins', 'C/Santa Madona, 13, 33ºE', 2);


SELECT * FROM Users;