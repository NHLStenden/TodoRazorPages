DROP TABLE IF EXISTS Todo, User, Category, TodoUser;
CREATE TABLE User (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL
);

INSERT INTO User (Email, Password) VALUES ('joris@test.com', 'Test@1234!');
SET @userIdJoris := (SELECT LAST_INSERT_ID());

INSERT INTO User (Email, Password) VALUES ('piet@test.com', 'Test@1234!');
SET @userIdPiet := (SELECT LAST_INSERT_ID());

# DROP TABLE IF EXISTS Category;
CREATE TABLE Category (
    CategoryId INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    UserId INT NOT NULL,
    UNIQUE (Name, UserId)
);

INSERT INTO Category (Name, UserId) VALUES ('Category 1', @userIdJoris);
INSERT INTO Category (Name, UserId) VALUES ('Category 2', @userIdJoris);

INSERT INTO Category (Name, UserId) VALUES ('Category 3', @userIdPiet);
INSERT INTO Category (Name, UserId) VALUES ('Category 4', @userIdPiet);

# DROP TABLE IF EXISTS Todo;
CREATE TABLE Todo
(
    TodoId INT AUTO_INCREMENT
        PRIMARY KEY,
    Description VARCHAR(100) NOT NULL,
    Done TINYINT(1) DEFAULT 0 NOT NULL,
    UserId INT NOT NULL,
    CategoryId INT NOT NULL,
    
    FOREIGN KEY (UserId) REFERENCES User(UserId),
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId) 
);

INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 1', false, @userIdJoris, (SELECT CategoryId FROM Category WHERE Name = 'Category 1' AND UserId = @userIdJoris));
SET @todo1Id := (SELECT LAST_INSERT_ID());
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 2', true, @userIdJoris, (SELECT CategoryId FROM Category WHERE Name = 'Category 2' AND UserId = @userIdJoris));

INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 3', false, @userIdPiet, (SELECT CategoryId FROM Category WHERE Name = 'Category 3' AND UserId = @userIdPiet));
SET @todo3Id := (SELECT LAST_INSERT_ID());
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 4', true, @userIdPiet, (SELECT CategoryId FROM Category WHERE Name = 'Category 4' AND UserId = @userIdPiet));


CREATE TABLE TodoUser (
    TodoId INT NOT NULL,
    UserId Int NOT NULL,
    PRIMARY KEY (TodoId, UserId),
    FOREIGN KEY (TodoId) REFERENCES Todo(TodoId),
    FOREIGN KEY (UserId) REFERENCES User(UserId)
);

INSERT INTO TodoUser (TodoId, UserId) VALUES (@todo1Id, @userIdPiet);
INSERT INTO TodoUser (TodoId, UserId) VALUES (@todo1Id, @userIdJoris);

INSERT INTO TodoUser (TodoId, UserId) VALUES (@todo3Id, @userIdJoris);
INSERT INTO TodoUser (TodoId, UserId) VALUES (@todo3Id, @userIdPiet);

SELECT Description, Done, Name, Email
FROM Todo t 
    JOIN Category c ON t.CategoryId = c.CategoryId
    JOIN User u ON t.UserId = u.UserId;

SELECT *
FROM Todo T 
    JOIN TodoUser TU ON T.TodoId = TU.TodoId
        JOIN User U on TU.UserId = U.UserId
    JOIN Category C on T.CategoryId = C.CategoryId
