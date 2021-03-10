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
DROP TABLE IF EXISTS FINISHED_CHALLENGES
GO
DROP TABLE IF EXISTS FINISHED_CHALLENGE_PARTICIPANTS
GO

--One friendship is one entry
CREATE TABLE FRIENDS(
        UserID INT NOT NULL,
        FriendID INT NOT NULL,
        UserName nvarchar(500) NOT NULL,
        FriendName nvarchar(500) NOT NULL,
        Pending BIT NOT NULL
);

CREATE TABLE USERS(
        ID INT IDENTITY(1,1) NOT NULL,
        Username nvarchar(500) UNIQUE NOT NULL,
        PasswordHash varchar(500) NOT NULL
);

CREATE TABLE RECORDS(
        UserID INT NOT NULL,
        RecordName varchar(500) NOT NULL,
        SelectedImage varchar(250) NOT NULL,
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
        EndDate DATE --Nullable
);

CREATE TABLE CHALLENGES(
        ChallengeID INT IDENTITY(1,1) NOT NULL,
        RecordName varchar(500) NOT NULL,
        GoalScore FLOAT NOT NULL,
        SuccessInfo INT NOT NULL,
        EndDate DATE, --Nullable
        InProgress BIT NOT NULL
);

CREATE TABLE CHALLENGE_PARTICIPANTS(
        ChallengeID INT NOT NULL,
        ParticipantID INT NOT NULL,
        ParticipantName nvarchar(500) NOT NULL,
        Pending BIT NOT NULL
);

CREATE TABLE NOTIFICATIONS(
        SenderID INT NOT NULL,
        ReceiverID INT NOT NULL,
        MessageType INT NOT NULL,
        ChallengeID INT --Nullable
);

CREATE TABLE FINISHED_CHALLENGES
(
	ChallengeID INT NOT NULL,
	RecordName varchar(500) NOT NULL,
	GoalScore FLOAT NOT NULL,
	WinnerID INT NOT NULL,
	DateFinished DATE NOT NULL
);

CREATE TABLE FINISHED_CHALLENGE_PARTICIPANTS
(
	ChallengeID INT NOT NULL,
	ParticipantID INT NOT NULL,
	ParticipantName nvarchar(500) NOT NULL
);
