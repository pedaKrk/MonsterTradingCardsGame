-- Create Users table with auto-increment for UserId
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Coins DOUBLE PRECISION DEFAULT 20,
    Role VARCHAR(50) DEFAULT 'User'
);

-- Create UserStats table
CREATE TABLE UserStats (
    Id SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    Elo INT DEFAULT 1000,
    Wins INT DEFAULT 0,
    Losses INT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create UserData table
CREATE TABLE UserData (
    Id SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Bio TEXT,
    Image TEXT,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create Cards table with UUID as a string, no default
CREATE TABLE Cards (
    Id UUID PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Damage DOUBLE PRECISION NOT NULL,
    Element VARCHAR(50) NOT NULL,
    CardType VARCHAR(50) NOT NULL
);

-- Create Stacks table
CREATE TABLE Stacks (
    Id SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    CardId UUID NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (CardId) REFERENCES Cards(Id) ON DELETE CASCADE
);

-- Create Decks table
CREATE TABLE Decks (
    Id SERIAL PRIMARY KEY,
    UserId INT NOT NULL,
    CardId UUID NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (CardId) REFERENCES Cards(Id) ON DELETE CASCADE
);

-- Create TradingDeals table with passed UUID for CardId and TradingDealId
CREATE TABLE TradingDeals (
    Id UUID PRIMARY KEY,
    CardId UUID NOT NULL,
    Price DOUBLE PRECISION NOT NULL,
    Username VARCHAR(255) NOT NULL,
    FOREIGN KEY (CardId) REFERENCES Cards(Id) ON DELETE CASCADE
);

-- Create Packages table
CREATE TABLE Packages (
    Id SERIAL PRIMARY KEY,
    Card1Id UUID NOT NULL,
    Card2Id UUID NOT NULL,
    Card3Id UUID NOT NULL,
    Card4Id UUID NOT NULL,
    Card5Id UUID NOT NULL,
    FOREIGN KEY (Card1Id) REFERENCES Cards(Id) ON DELETE CASCADE,
    FOREIGN KEY (Card2Id) REFERENCES Cards(Id) ON DELETE CASCADE,
    FOREIGN KEY (Card3Id) REFERENCES Cards(Id) ON DELETE CASCADE,
    FOREIGN KEY (Card4Id) REFERENCES Cards(Id) ON DELETE CASCADE,
    FOREIGN KEY (Card5Id) REFERENCES Cards(Id) ON DELETE CASCADE
);

-- Insert sample data into Users table
INSERT INTO Users (Username, Password, Coins, Role)
VALUES 
('player1', 'password1', 20, 'User'),
('player2', 'password2', 30, 'User'),
('admin', 'admin', 100, 'Admin');

-- Insert sample data into Cards table with UUID passed explicitly as a string
INSERT INTO Cards (Id, Name, Damage, Element, CardType)
VALUES 
('550e8400-e29b-41d4-a716-446655440000', 'Fire Dragon', 50, 'Fire', 'Monster'),
('550e8400-e29b-41d4-a716-446655440001', 'Water Wizard', 40, 'Water', 'Spell'),
('550e8400-e29b-41d4-a716-446655440002', 'Earth Golem', 60, 'Normal', 'Monster'),
('550e8400-e29b-41d4-a716-446655440003', 'Lightning Bolt', 30, 'Normal', 'Spell'),
('550e8400-e29b-41d4-a716-446655440004', 'Tsunami', 35, 'Water', 'Spell');

-- Insert sample data into UserStats table
INSERT INTO UserStats (UserId, Elo, Wins, Losses)
VALUES 
(1, 1200, 10, 5),
(2, 1100, 8, 7),
(3, 1500, 20, 2);

-- Insert sample data into UserData table
INSERT INTO UserData (UserId, Name, Bio, Image)
VALUES 
(1, 'Player One', 'I love trading cards!', NULL),
(2, 'Player Two', NULL, 'player2.png'),
(3, 'Admin', 'Game administrator.', 'admin.png');

-- Insert sample data into Stacks table
INSERT INTO Stacks (UserId, CardId)
VALUES 
(1, '550e8400-e29b-41d4-a716-446655440000'),
(2, '550e8400-e29b-41d4-a716-446655440001'),
(1, '550e8400-e29b-41d4-a716-446655440002');

-- Insert sample data into Decks table
INSERT INTO Decks (UserId, CardId)
VALUES 
(1, '550e8400-e29b-41d4-a716-446655440000'),
(2, '550e8400-e29b-41d4-a716-446655440001'),
(1, '550e8400-e29b-41d4-a716-446655440002');

-- Insert sample data into TradingDeals table with passed UUID for CardId
INSERT INTO TradingDeals (Id, CardId, Price, Username)
VALUES 
('550e8400-e29b-41d4-a716-446655440100', '550e8400-e29b-41d4-a716-446655440000', 15.5, 'player1'),
('550e8400-e29b-41d4-a716-446655440101', '550e8400-e29b-41d4-a716-446655440001', 12.0, 'player2');

-- Insert sample data into Packages table
INSERT INTO Packages (Card1Id, Card2Id, Card3Id, Card4Id, Card5Id)
VALUES 
('550e8400-e29b-41d4-a716-446655440000', '550e8400-e29b-41d4-a716-446655440001', 
 '550e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440003', 
 '550e8400-e29b-41d4-a716-446655440004');
