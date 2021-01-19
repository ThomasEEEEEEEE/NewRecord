USE NewRecordDB
GO

DROP TABLE IF EXISTS FRIENDS
GO
DROP TABLE IF EXISTS USERS
GO
DROP TABLE IF EXISTS RECORDS
GO
DROP TABLE IF EXISTS RECORDHISTORY
GO
DROP TABLE IF EXISTS GOALS
GO
DROP TABLE IF EXISTS CHALLENGES
GO
DROP TABLE IF EXISTS NOTIFICATIONS
GO

--Unfinished
CREATE TABLE FRIENDS(
        UserID INT NOT NULL,
        FriendID INT NOT NULL,
        UserName nvarchar(500) NOT NULL,
        FriendName nvarchar(500) NOT NULL
);

CREATE TABLE USERS(
        ID INT IDENTITY(1,1) NOT NULL,
        Username nvarchar(500) UNIQUE NOT NULL,
        PasswordHash varchar(500) NOT NULL
);

CREATE TABLE RECORDS(
        UserID INT NOT NULL,
        RecordName varchar(500) NOT NULL,
        SelectedImage INT NOT NULL,
        SuccessInfo INT NOT NULL, --0 larger, 1 smaller
        Privacy INT NOT NULL --0 public, 1 private, 2 friends only
);

CREATE TABLE RECORD_HISTORY(
        UserID INT NOT NULL,
        RecordName varchar(500) NOT NULL,
        Score FLOAT NOT NULL,
        DateAchieved DATE NOT NULL
);

CREATE TABLE GOALS(
        UserID INT NOT NULL,
        RecordName varchar(500) NOT NULL,
        GoalScore FLOAT NOT NULL,
        StartDate DATE, --Nullable
        EndDate DATE --Nullable
);

--Unfinished
CREATE TABLE CHALLENGES(
        Participants varchar(500) NOT NULL, --In the form id,id,id
        RecordName varchar(500) NOT NULL,
        GoalScore FLOAT NOT NULL,
        StartDate DATE, --Nullable
        EndDate DATE, --Nullable
        WinnerID INT --Nullable
);

--Very Unfinished
CREATE TABLE NOTIFICATIONS(
        SenderUserID INT NOT NULL,
        ReceiverUserID INT NOT NULL,
        MessageType varchar(100) NOT NULL,
        MessageInfo varchar(1000) NOT NULL
);
