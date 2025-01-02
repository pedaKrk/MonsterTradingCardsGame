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
    Name VARCHAR(255) NOT NULL,
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
    CardId UUID NOT NULL,
    FOREIGN KEY (CardId) REFERENCES Cards(Id) ON DELETE CASCADE
);
