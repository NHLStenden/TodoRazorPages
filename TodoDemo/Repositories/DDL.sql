DROP TABLE IF EXISTS User;
CREATE TABLE User (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    UserName VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL
);

INSERT INTO User (UserName, Password) VALUES ('Joris', 'Test@1234!');
INSERT INTO User (UserName, Password) VALUES ('Piet', 'Test@1234!');

DROP TABLE IF EXISTS Category;
CREATE TABLE Category (
    CategoryId INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) NOT NULL,
    UserId INT NOT NULL,
    UNIQUE (Name, UserId)
);

INSERT INTO Category (Name, UserId) VALUES ('Category 1', (SELECT UserId FROM User WHERE UserName = 'Joris'));
INSERT INTO Category (Name, UserId) VALUES ('Category 2', (SELECT UserId FROM User WHERE UserName = 'Joris'));

INSERT INTO Category (Name, UserId) VALUES ('Category 3', (SELECT UserId FROM User WHERE UserName = 'Piet'));
INSERT INTO Category (Name, UserId) VALUES ('Category 4', (SELECT UserId FROM User WHERE UserName = 'Piet'));

DROP TABLE IF EXISTS Todo;
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

SET @userId := (SELECT UserId FROM User WHERE UserName = 'Joris');
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 1', false, @userId, (SELECT CategoryId FROM Category WHERE Name = 'Category 1' AND UserId = @userId));
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 2', true, @userId, (SELECT CategoryId FROM Category WHERE Name = 'Category 2' AND UserId = @userId));

SET @userId := (SELECT UserId FROM User WHERE UserName = 'Piet');
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 3', false, @userId, (SELECT CategoryId FROM Category WHERE Name = 'Category 3' AND UserId = @userId));
INSERT INTO Todo (Description, Done, UserId, CategoryId) VALUES ('Todo 4', true, @userId, (SELECT CategoryId FROM Category WHERE Name = 'Category 4' AND UserId = @userId));

SELECT Description, Done, Name, UserName
FROM Todo t 
    JOIN Category c ON t.CategoryId = c.CategoryId
    JOIN User u ON t.UserId = u.UserId


