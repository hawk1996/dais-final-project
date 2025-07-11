CREATE DATABASE PaymentSystem;
GO

USE PaymentSystem;
GO

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    Password CHAR(64) NOT NULL
);

CREATE TABLE BankAccounts (
    BankAccountId INT IDENTITY(1,1) PRIMARY KEY,
    AccountNumber CHAR(22) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL CHECK (Balance >= 0)
);

CREATE TABLE Users_BankAccounts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BankAccountId INT NOT NULL,
    CONSTRAINT FK_UsersBank_User FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UsersBank_BankAccount FOREIGN KEY (BankAccountId) REFERENCES BankAccounts(BankAccountId) ON DELETE CASCADE,
    CONSTRAINT UQ_UsersBank UNIQUE (UserId, BankAccountId)  -- no duplicates
);

CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    FromBankAccountId INT NOT NULL,
    ToBankAccountId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL CHECK (Amount > 0),
    Timestamp DATETIME2 NOT NULL,
    Reason NVARCHAR(32) NOT NULL,
    Status INT NOT NULL CHECK (Status IN (0, 1, 2)), -- 0=Pending,1=Processed,2=Rejected

    CONSTRAINT FK_Payment_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Payment_FromBankAccount FOREIGN KEY (FromBankAccountId) REFERENCES BankAccounts(BankAccountId),
    CONSTRAINT FK_Payment_ToBankAccount FOREIGN KEY (ToBankAccountId) REFERENCES BankAccounts(BankAccountId)
);

INSERT INTO Users (Name, Username, Password) VALUES
('Alice Johnson', 'alice', 'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3'),
('Bob Smith', 'bob', 'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3'),
('Charlie Davis', 'charlie', 'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3'),
('Diana Evans', 'diana', 'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3'),
('Ethan Brown', 'ethan', 'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3');

INSERT INTO BankAccounts (AccountNumber, Balance) VALUES
('ACC0000000000000000001', 1000.00),
('ACC0000000000000000002', 1000.00),
('ACC0000000000000000003', 1000.00),
('ACC0000000000000000004', 1000.00),
('ACC0000000000000000005', 1000.00),
('ACC0000000000000000006', 1000.00),
('ACC0000000000000000007', 1000.00),
('ACC0000000000000000008', 1000.00),
('ACC0000000000000000009', 1000.00),
('ACC0000000000000000010', 1000.00);

INSERT INTO Users_BankAccounts (UserId, BankAccountId) VALUES
(1, 1),
(1, 2),
(1, 9),
(1, 10),

(2, 3),
(2, 4),

(3, 5),
(3, 6),

(4, 7),
(4, 8);
